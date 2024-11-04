using System.Collections.ObjectModel;
using DotBased.ASP.Auth.Domains.Auth;
using Microsoft.AspNetCore.Components.Authorization;

namespace DotBased.ASP.Auth;

public class AuthDataCache
{
    public AuthDataCache(BasedAuthConfiguration configuration)
    {
        _configuration = configuration;
    }

    private readonly BasedAuthConfiguration _configuration;

    private readonly AuthStateCacheCollection<AuthenticationStateModel, AuthenticationState> _authenticationStateCollection = [];

    public Result PurgeSessionState(string id) => _authenticationStateCollection.Remove(id) ? Result.Ok() : Result.Failed("Failed to purge session state from cache! Or the session was not cached...");

    public void CacheSessionState(AuthenticationStateModel stateModel, AuthenticationState? state = null) => _authenticationStateCollection[stateModel.Id] =
        new AuthStateCacheNode<AuthenticationStateModel, AuthenticationState>(stateModel, state);

    public Result<Tuple<AuthenticationStateModel, AuthenticationState?>> RequestSessionState(string id)
    {
        if (!_authenticationStateCollection.TryGetValue(id, out var node))
            return Result<Tuple<AuthenticationStateModel, AuthenticationState?>>.Failed("No cached object found!");
        string failedMsg;
        if (node.StateModel != null)
        {
            if (node.IsValidLifespan(_configuration.CachedAuthSessionLifespan))
                return Result<Tuple<AuthenticationStateModel, AuthenticationState?>>.Ok(new Tuple<AuthenticationStateModel, AuthenticationState?>(node.StateModel, node.State));
            failedMsg = $"Session has invalid lifespan, removing entry: [{id}] from cache!";
        }
        else
            failedMsg = $"Returned object is null, removing entry: [{id}] from cache!";
        _authenticationStateCollection.Remove(id);
        return Result<Tuple<AuthenticationStateModel, AuthenticationState?>>.Failed(failedMsg);
    }
}

public class AuthStateCacheNode<TStateModel, TState> where TStateModel : class where TState : class
{
    public AuthStateCacheNode(TStateModel stateModel, TState? state)
    {
        StateModel = stateModel;
        State = state;
    }
    public TStateModel? StateModel { get; private set; }
    public TState? State { get; private set; }
    public DateTime DateCached { get; private set; } = DateTime.Now;

    public void UpdateObject(TStateModel obj)
    {
        StateModel = obj;
        DateCached = DateTime.Now;
    }

    /// <summary>
    /// Checks if the cached object is within the given lifespan.
    /// </summary>
    /// <param name="lifespan">The max. lifespan</param>
    public bool IsValidLifespan(TimeSpan lifespan) => DateCached.Add(lifespan) > DateTime.Now;

    public override bool Equals(object? obj)
    {
        if (obj is AuthStateCacheNode<TStateModel, TState> cacheObj)
            return StateModel != null && StateModel.Equals(cacheObj.StateModel);
        return false;
    }

    public override int GetHashCode() => typeof(TStateModel).GetHashCode();
    public override string ToString() => typeof(TStateModel).ToString();
}

public class AuthStateCacheCollection<TStateModel, TState> : KeyedCollection<string, AuthStateCacheNode<TStateModel, TState>> where TStateModel : class where TState : class
{
    protected override string GetKeyForItem(AuthStateCacheNode<TStateModel, TState> item) => item.StateModel?.ToString() ?? string.Empty;

    public new AuthStateCacheNode<TStateModel, TState>? this[string id]
    {
        get => TryGetValue(id, out AuthStateCacheNode<TStateModel, TState>? nodeValue) ? nodeValue : null;
        set
        {
            if (value == null)
                return;
            if (TryGetValue(id, out AuthStateCacheNode<TStateModel, TState>? nodeValue))
                Remove(nodeValue);
            Add(value);
        }
    }

    public void Insert(AuthStateCacheNode<TStateModel, TState> node)
    {
        if (Contains(node))
            Remove(node);
        Add(node);
    }
}