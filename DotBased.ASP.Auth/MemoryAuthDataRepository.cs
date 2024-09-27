using System.Diagnostics.CodeAnalysis;
using DotBased.ASP.Auth.Domains.Auth;
using DotBased.ASP.Auth.Domains.Identity;

namespace DotBased.ASP.Auth;
/// <summary>
/// In memory data provider, for testing only!
/// </summary>
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
public class MemoryAuthDataRepository : IAuthDataRepository
{
    private readonly List<UserModel> _userList = [];
    private readonly List<GroupModel> _groupList = [];
    private readonly List<AuthenticationStateModel> _authenticationStateList = [];
    
    public async Task<Result> CreateUserAsync(UserModel user)
    {
        if (_userList.Any(x => x.Id == user.Id || x.Email == user.Email))
            return Result.Failed("User already exists.");
        _userList.Add(user);
        return Result.Ok();
    }

    public async Task<Result> UpdateUserAsync(UserModel user)
    {
        if (_userList.All(x => x.Id != user.Id))
            return Result.Failed("User does not exist!");
        
        return Result.Ok();
    }

    public Task<Result> DeleteUserAsync(UserModel user)
    {
        throw new NotImplementedException();
    }

    public Task<Result<UserModel>> GetUserAsync(string id, string email, string username)
    {
        throw new NotImplementedException();
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

    public Task<Result> CreateAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthenticationStateModel>> GetAuthenticationStateAsync(string id)
    {
        throw new NotImplementedException();
    }
}