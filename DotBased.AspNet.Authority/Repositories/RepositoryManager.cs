namespace DotBased.AspNet.Authority.Repositories;

public class RepositoryManager<TUser, TGroup> where TUser : class where TGroup : class
{
    public RepositoryManager(IUserRepository<TUser> userRepository, IGroupRepository<TGroup> groupRepository)
    {
        UserRepository = userRepository;
        GroupRepository = groupRepository;
    }

    public IUserRepository<TUser> UserRepository { get; set; }
    public IGroupRepository<TGroup> GroupRepository { get; set; }
}