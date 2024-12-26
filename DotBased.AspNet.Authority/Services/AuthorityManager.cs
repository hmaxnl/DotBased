using System.Reflection;
using DotBased.AspNet.Authority.Attributes;
using DotBased.AspNet.Authority.Crypto;
using DotBased.AspNet.Authority.Models.Options;
using DotBased.AspNet.Authority.Repositories;
using DotBased.Logging;
using Microsoft.Extensions.Options;

namespace DotBased.AspNet.Authority.Services;

public class AuthorityManager
{
    public AuthorityManager(
        IOptions<AuthorityOptions> options,
        IServiceProvider services,
        IAuthorityRepository repository,
        ICryptographer cryptographer)
    {
        _logger = LogService.RegisterLogger<AuthorityManager>();
        Options = options.Value ?? new AuthorityOptions();
        Services = services;
        Repository = repository;
        Cryptographer = cryptographer;
    }

    private readonly ILogger _logger;

    public IServiceProvider Services { get; }
    public AuthorityOptions Options { get; }
    public IAuthorityRepository Repository { get; }
    public ICryptographer Cryptographer { get; }


    public long GenerateVersion() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    

    /// <summary>
    /// Protect or unprotect the properties with the <see cref="ProtectAttribute"/>
    /// </summary>
    /// <param name="data">The data model</param>
    /// <param name="protection">True for protection false for unprotection.</param>
    /// <typeparam name="TModel">The class with the properties to protect.</typeparam>
    public async Task HandlePropertyProtection<TModel>(TModel data, bool protection)
    {
        var props = GetProtectedPropertiesValues<TModel>(data);
        if (Cryptographer == null)
        {
            _logger.Warning("No cryptographer specified! Cannot encrypt/decrypt properties.");
            return;
        }
        if (props.Count == 0)
        {
            return;
        }

        var handledProperties = 0;
        foreach (var property in props)
        {
            if (property.PropertyType != typeof(string))
            {
                _logger.Warning("Property({PropName}) with type: {PropType} detected, encrypting only supports strings! Skipping property!", property.Name, property.PropertyType);
                continue;
            }

            string? cryptString;
            if (protection)
            {
                cryptString = await Cryptographer.EncryptAsync(property.GetValue(data)?.ToString() ?? string.Empty);
            }
            else
            {
                cryptString = await Cryptographer.DecryptAsync(property.GetValue(data)?.ToString() ?? string.Empty);
            }
            
            if (cryptString == null)
            {
                _logger.Warning("{Protection} failed for property {PropName}", protection ? "Encyption" : "Decyption", property.Name);
                continue;
            }
            property.SetValue(data, cryptString);
            handledProperties++;
        }
        _logger.Debug("{HandledPropCount}/{TotalPropCount} protection properties handled!", handledProperties, props.Count);
    }

    public bool IsPropertieProtected<TModel>(string propertieName)
    {
        var protectedProperties = GetProtectedProperties<TModel>();
        var propertieFound = protectedProperties.Where(propInfo => propInfo.Name == propertieName);
        return propertieFound.Any();
    }

    public List<PropertyInfo> GetProtectedPropertiesValues<TModel>(TModel model)
    {
        var protectedProperties = GetProtectedProperties<TModel>();
        return protectedProperties.Count != 0 ? protectedProperties : [];
    }

    public List<PropertyInfo> GetProtectedProperties<TModel>()
        => typeof(TModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(ProtectAttribute))).ToList();
}