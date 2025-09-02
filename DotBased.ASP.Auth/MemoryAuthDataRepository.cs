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
    public async Task<ResultOld> CreateUserAsync(UserModel user)
    {
        if (MemoryData.users.Any(x => x.Id == user.Id || x.Email == user.Email))
            return ResultOld.Failed("User already exists.");
        MemoryData.users.Add(user);
        return ResultOld.Ok();
    }

    public async Task<ResultOld> UpdateUserAsync(UserModel user)
    {
        if (MemoryData.users.All(x => x.Id != user.Id))
            return ResultOld.Failed("User does not exist!");
        
        return ResultOld.Ok();
    }

    public Task<ResultOld> DeleteUserAsync(UserModel user)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultOld<UserModel>> GetUserAsync(string id, string email, string username)
    {
        UserModel? userModel = null;
        if (!id.IsNullOrEmpty())
            userModel = MemoryData.users.FirstOrDefault(u => u.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (!email.IsNullOrEmpty())
            userModel = MemoryData.users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (!username.IsNullOrEmpty())
            userModel = MemoryData.users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        return userModel != null ? ResultOld<UserModel>.Ok(userModel) : ResultOld<UserModel>.Failed("No user found!");
    }

    public Task<ListResultOld<UserItemModel>> GetUsersAsync(int start = 0, int amount = 30, string search = "")
    {
        throw new NotImplementedException();
    }

    public Task<ResultOld> CreateGroupAsync(GroupModel group)
    {
        throw new NotImplementedException();
    }

    public Task<ResultOld> UpdateGroupAsync(GroupModel group)
    {
        throw new NotImplementedException();
    }

    public Task<ResultOld> DeleteGroupAsync(GroupModel group)
    {
        throw new NotImplementedException();
    }

    public Task<ResultOld<GroupModel>> GetGroupAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ListResultOld<GroupItemModel>> GetGroupsAsync(int start = 0, int amount = 30, string search = "")
    {
        throw new NotImplementedException();
    }

    public async Task<ResultOld> CreateAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        if (MemoryData.AuthenticationStates.Contains(authenticationState)) return ResultOld.Failed("Item already exists!");
        MemoryData.AuthenticationStates.Add(authenticationState);
        return ResultOld.Ok();
    }

    public Task<ResultOld> UpdateAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultOld> DeleteAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        MemoryData.AuthenticationStates.Remove(authenticationState);
        return ResultOld.Ok();
    }

    public async Task<ResultOld<AuthenticationStateModel>> GetAuthenticationStateAsync(string id)
    {
        var item = MemoryData.AuthenticationStates.FirstOrDefault(x => x.Id == id);
        if (item == null) return ResultOld<AuthenticationStateModel>.Failed("Could not get the session state!");
        return ResultOld<AuthenticationStateModel>.Ok(item);
    }
}

internal static class MemoryData
{
    public static readonly List<UserModel> users = [];
    public static readonly List<GroupModel> Groups = [];
    public static readonly List<AuthenticationStateModel> AuthenticationStates = [];
}