using StoreAdmin.Core.Models;

namespace StoreAdmin.Core.RepositoryInterfaces
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        User CreateUser(User user);
        User UpdateUser(User user);
        void DeleteUser(User user);
        User GetUserByusername(string username);
        bool VerifyPassword(string hashedPassword, string password);
    }
}
