using System.Collections.ObjectModel;
using DotBased.ASP.Auth.Domains.Auth;

namespace DotBased.ASP.Auth;

public class AuthDataCache
{
    public AuthDataCache(BasedAuthConfiguration configuration)
    {
        _configuration = configuration;
    }

    private readonly BasedAuthConfiguration _configuration;

    private readonly CacheNodeCollection<AuthenticationStateModel> _authenticationStateCollection = [];

    public Result PurgeSessionFromCache(string id) => _authenticationStateCollection.Remove(id) ? Result.Ok() : Result.Failed("Failed to purge session state from cache! Or the session was not cached...");

    public async Task<Result<AuthenticationStateModel>> RequestAuthStateAsync(IAuthDataRepository dataRepository, string id)
    {
        if (_authenticationStateCollection.TryGetValue(id, out var node))
        {
            if (node.Object == null)
            {
                _authenticationStateCollection.Remove(id);
                return Result<AuthenticationStateModel>.Failed($"Returned object is null, removing entry [{id}] from cache!");
            }

            if (node.IsValidLifespan(_configuration.CachedAuthSessionLifespan))
                return Result<AuthenticationStateModel>.Ok(node.Object);
        }
        
        var dbResult = await dataRepository.GetAuthenticationStateAsync(id);
        if (!dbResult.Success || dbResult.Value == null)
        {
            _authenticationStateCollection.Remove(id);
            return Result<AuthenticationStateModel>.Failed("Unknown session state!");
        }

        if (node == null)
            node = new CacheNode<AuthenticationStateModel>(dbResult.Value);
        else
            node.UpdateObject(dbResult.Value);
        if (node.Object != null)
            return Result<AuthenticationStateModel>.Ok(node.Object);
        return node.Object != null ? Result<AuthenticationStateModel>.Ok(node.Object) : Result<AuthenticationStateModel>.Failed("Failed to get db object!");
    }

    /*
     *
     */
}

public class CacheNode<T> where T : class
{
    public CacheNode(T obj)
    {
        Object = obj;
    }
    public T? Object { get; private set; }
    public DateTime DateCached { get; private set; } = DateTime.Now;

    public void UpdateObject(T obj)
    {
        Object = obj;
        DateCached = DateTime.Now;
    }

    /// <summary>
    /// Checks if the cached object is within the given lifespan.
    /// </summary>
    /// <param name="lifespan">The max. lifespan</param>
    public bool IsValidLifespan(TimeSpan lifespan) => DateCached.Add(lifespan) < DateTime.Now;

    public override bool Equals(object? obj)
    {
        if (obj is CacheNode<T> cacheObj)
            return typeof(T).Equals(cacheObj.Object);
        return false;
    }

    public override int GetHashCode() => typeof(T).GetHashCode();
    public override string ToString() => typeof(T).ToString();
}

public class CacheNodeCollection<TItem> : KeyedCollection<string, CacheNode<TItem>> where TItem : class
{
    protected override string GetKeyForItem(CacheNode<TItem> item) => item.Object?.ToString() ?? string.Empty;
}