namespace ShoeStore.Data.Repositories.Interfaces;
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByLoginAsync(string login);
    Task<User?> GetByIdWithRoleAsync(int id);
}