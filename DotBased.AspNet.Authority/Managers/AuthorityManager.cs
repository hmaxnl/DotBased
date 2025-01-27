using System.Reflection;
using DotBased.AspNet.Authority.Attributes;
using DotBased.AspNet.Authority.Crypto;
using DotBased.AspNet.Authority.Models.Options;
using DotBased.AspNet.Authority.Repositories;
using DotBased.AspNet.Authority.Validators;
using DotBased.Logging;
using Microsoft.Extensions.Options;

namespace DotBased.AspNet.Authority.Managers;

public partial class AuthorityManager(
    IOptions<AuthorityOptions> options,
    IServiceProvider services,
    ICryptographer cryptographer,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPasswordHasher passwordHasher)
{
    private readonly ILogger _logger = LogService.RegisterLogger<AuthorityManager>();

    public IServiceProvider Services { get; } = services;
    public AuthorityOptions Options { get; } = options.Value;
    public ICryptographer Cryptographer { get; } = cryptographer;

    public IUserRepository UserRepository { get; } = userRepository;
    public IRoleRepository RoleRepository { get; } = roleRepository;

    public IPasswordHasher PasswordHasher { get; } = passwordHasher;

    public IEnumerable<IPasswordValidator> PasswordValidators { get; } = [];
    public IEnumerable<IUserValidator> UserValidators { get; } = [];


    public long GenerateVersion() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    

    /// <summary>
    /// Protect or unprotect the properties with the <see cref="ProtectAttribute"/>
    /// </summary>
    /// <param name="data">The data model</param>
    /// <param name="protection">True for protect false for unprotect.</param>
    /// <typeparam name="TModel">The class with the properties to protect.</typeparam>
    public async Task HandlePropertyProtection<TModel>(TModel data, bool protection)
    {
        var props = GetProtectedPropertiesValues(data);
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
                _logger.Warning("{Protection} failed for property {PropName}", protection ? "Encryption" : "Decryption", property.Name);
                continue;
            }
            property.SetValue(data, cryptString);
            handledProperties++;
        }
        _logger.Debug("{HandledPropCount}/{TotalPropCount} protection properties handled!", handledProperties, props.Count);
    }

    public bool IsPropertyProtected<TModel>(string propertyName)
    {
        var protectedProperties = GetProtectedProperties<TModel>();
        var propertyFound = protectedProperties.Where(propInfo => propInfo.Name == propertyName);
        return propertyFound.Any();
    }

    public List<PropertyInfo> GetProtectedPropertiesValues<TModel>(TModel model)
    {
        var protectedProperties = GetProtectedProperties<TModel>();
        return protectedProperties.Count != 0 ? protectedProperties : [];
    }

    public List<PropertyInfo> GetProtectedProperties<TModel>()
        => typeof(TModel).GetProperties().Where(p => Attribute.IsDefined(p, typeof(ProtectAttribute))).ToList();
}