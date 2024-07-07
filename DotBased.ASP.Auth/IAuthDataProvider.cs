using DotBased.ASP.Auth.Domains.Auth;
using DotBased.ASP.Auth.Domains.Identity;

namespace DotBased.ASP.Auth;

public interface IAuthDataProvider
{
    /*
     * Identity
     */
    
    // User
    public Task<Result> CreateUserAsync(UserModel user);
    public Task<Result> UpdateUserAsync(UserModel user);
    public Task<Result> DeleteUserAsync(UserModel user);
    public Task<Result<UserModel>> GetUserAsync(string id, string email, string username);
    public Task<ListResult<UserItemModel>> GetUsersAsync(int start = 0, int amount = 30, string search = "");

    // Group
    public Task<Result> CreateGroupAsync(GroupModel group);
    public Task<Result> UpdateGroupAsync(GroupModel group);
    public Task<Result> DeleteGroupAsync(GroupModel group);
    public Task<Result<GroupModel>> GetGroupAsync(string id);
    public Task<ListResult<GroupItemModel>> GetGroupsAsync(int start = 0, int amount = 30, string search = "");
    
    /*
     * Auth
     */
    
    // AuthenticationState
    public Task<Result> CreateAuthenticationStateAsync(AuthenticationStateModel authenticationState);
    public Task<Result> UpdateAuthenticationStateAsync(AuthenticationStateModel authenticationState);
    public Task<Result> DeleteAuthenticationStateAsync(AuthenticationStateModel authenticationState);
    public Task<Result<AuthenticationStateModel>> GetAuthenticationStateAsync(string id);
    
    // Authorization
    
}