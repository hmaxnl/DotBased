using DotBased.ASP.Auth.Domains.Auth;
using DotBased.ASP.Auth.Domains.Identity;

namespace DotBased.ASP.Auth;
/// <summary>
/// In memory data provider, for testing only!
/// </summary>
public class MemoryAuthDataProvider : IAuthDataProvider
{
    private Dictionary<string, UserModel> _userDict = [];
    private Dictionary<string, GroupModel> _groupDict = [];
    private Dictionary<string, AuthenticationStateModel> _authenticationDict = [];

    public async Task<Result> CreateUserAsync(UserModel user)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateUserAsync(UserModel user)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteUserAsync(UserModel user)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<UserModel>> GetUserAsync(string id, string email, string username)
    {
        throw new NotImplementedException();
    }

    public async Task<ListResult<UserItemModel>> GetUsersAsync(int start = 0, int amount = 30, string search = "")
    {
        throw new NotImplementedException();
    }

    public async Task<Result> CreateGroupAsync(GroupModel group)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateGroupAsync(GroupModel group)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteGroupAsync(GroupModel group)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<GroupModel>> GetGroupAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<ListResult<GroupItemModel>> GetGroupsAsync(int start = 0, int amount = 30, string search = "")
    {
        throw new NotImplementedException();
    }

    public async Task<Result> CreateAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteAuthenticationStateAsync(AuthenticationStateModel authenticationState)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<AuthenticationStateModel>> GetAuthenticationStateAsync(string id)
    {
        throw new NotImplementedException();
    }
}