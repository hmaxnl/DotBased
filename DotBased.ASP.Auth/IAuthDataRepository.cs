using DotBased.ASP.Auth.Domains.Auth;
using DotBased.ASP.Auth.Domains.Identity;

namespace DotBased.ASP.Auth;

public interface IAuthDataRepository
{
    public Task<ResultOld> CreateUserAsync(UserModel user);
    public Task<ResultOld> UpdateUserAsync(UserModel user);
    public Task<ResultOld> DeleteUserAsync(UserModel user);
    public Task<ResultOld<UserModel>> GetUserAsync(string id, string email, string username);
    public Task<ListResultOld<UserItemModel>> GetUsersAsync(int start = 0, int amount = 30, string search = "");
    public Task<ResultOld> CreateGroupAsync(GroupModel group);
    public Task<ResultOld> UpdateGroupAsync(GroupModel group);
    public Task<ResultOld> DeleteGroupAsync(GroupModel group);
    public Task<ResultOld<GroupModel>> GetGroupAsync(string id);
    public Task<ListResultOld<GroupItemModel>> GetGroupsAsync(int start = 0, int amount = 30, string search = "");
    public Task<ResultOld> CreateAuthenticationStateAsync(AuthenticationStateModel authenticationState);
    public Task<ResultOld> UpdateAuthenticationStateAsync(AuthenticationStateModel authenticationState);
    public Task<ResultOld> DeleteAuthenticationStateAsync(AuthenticationStateModel authenticationState);
    public Task<ResultOld<AuthenticationStateModel>> GetAuthenticationStateAsync(string id);
}