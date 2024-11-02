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

    public Result PurgeSessionState(string id) => _authenticationStateCollection.Remove(id) ? Result.Ok() : Result.Failed("Failed to purge session state from cache! Or the session was not cached...");

    public void CacheSessionState(AuthenticationStateModel state) => _authenticationStateCollection.Insert(new CacheNode<AuthenticationStateModel>(state));

    public Result<AuthenticationStateModel> RequestSessionState(string id)
    {
        if (!_authenticationStateCollection.TryGetValue(id, out var node))
            return Result<AuthenticationStateModel>.Failed("No cached object found!");
        string failedMsg;
        if (node.Object != null)
        {
            if (node.IsValidLifespan(_configuration.CachedAuthSessionLifespan))
                return Result<AuthenticationStateModel>.Ok(node.Object);
            failedMsg = $"Session has invalid lifespan, removing entry: [{id}] from cache!";
        }
        else
            failedMsg = $"Returned object is null, removing entry: [{id}] from cache!";
        _authenticationStateCollection.Remove(id);
        return Result<AuthenticationStateModel>.Failed(failedMsg);
    }
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

    public new CacheNode<TItem>? this[string id]
    {
        get => TryGetValue(id, out CacheNode<TItem>? nodeValue) ? nodeValue : null;
        set
        {
            if (value == null)
                return;
            if (TryGetValue(id, out CacheNode<TItem>? nodeValue))
                Remove(nodeValue);
            Add(value);
        }
    }

    public void Insert(CacheNode<TItem> node)
    {
        if (Contains(node))
            Remove(node);
        Add(node);
    }
}