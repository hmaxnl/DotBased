using System.Diagnostics.CodeAnalysis;
using DotBased.ASP.Auth.Domains.Auth;
using DotBased.ASP.Auth.Domains.Identity;
using DotBased.Extensions;

namespace DotBased.ASP.Auth;
/// <summary>
/// In memory data provider, for testing only!
/// </summary>
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
public class MemoryAuthDataRepository : IAuthDataRepository
{
    public async Task<Result> CreateUserAsync(UserModel user)
    {
        if (MemoryData.users.Any(x => x.Id == user.Id || x.Email == user.Email))
            return Result.Failed("User already exists.");
        MemoryData.users.Add(user);
        return Result.Ok();
    }

    public async Task<Result> UpdateUserAsync(UserModel user)
    {
        if (MemoryData.users.All(x => x.Id != user.Id))
            return Result.Failed("User does not exist!");
        
        return Result.Ok();
    }

    public Task<Result> DeleteUserAsync(UserModel user)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<UserModel>> GetUserAsync(string id, string email, string username)
    {
        UserModel? userModel = null;
        if (!id.IsNullOrEmpty())
            userModel = MemoryData.users.FirstOrDefault(u => u.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (!email.IsNullOrEmpty())
            userModel = MemoryData.users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (!username.IsNullOrEmpty())
            userModel = MemoryData.users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        return userModel != null ? Result<UserModel>.Ok(userModel) : Result<UserModel>.Failed("No user found!");
    }

    public Task<ListResult<UserItemModel>> GetUsersAsync(int start = 0, int amount = 30, string search = "")
    {
        throw new NotImplementedException();
    }

    public Task<Result> CreateGroupAsync(GroupModel group)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateGroupAsync(GroupModel group)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteGroupAsync(GroupModel group)
    {
        throw new NotImplementedException();
    }

    public Task<Result<GroupModel>> GetGroupAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ListResult<GroupItemModel>> GetGroupsAsync(int start = 0, int amount = 30, string search = "")
    {
        throw new NotImplementedException();
    }

    public async Task<Result> CreateAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        if (MemoryData.AuthenticationStates.Contains(authenticationState)) return Result.Failed("Item already exists!");
        MemoryData.AuthenticationStates.Add(authenticationState);
        return Result.Ok();
    }

    public Task<Result> UpdateAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        MemoryData.AuthenticationStates.Remove(authenticationState);
        return Result.Ok();
    }

    public async Task<Result<AuthenticationStateModel>> GetAuthenticationStateAsync(string id)
    {
        var item = MemoryData.AuthenticationStates.FirstOrDefault(x => x.Id == id);
        if (item == null) return Result<AuthenticationStateModel>.Failed("Could not get the session state!");
        return Result<AuthenticationStateModel>.Ok(item);
    }
}

internal static class MemoryData
{
    public static readonly List<UserModel> users = [];
    public static readonly List<GroupModel> Groups = [];
    public static readonly List<AuthenticationStateModel> AuthenticationStates = [];
}