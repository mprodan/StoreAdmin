using StoreAdmin.Core.Models;

namespace StoreAdmin.Core.BusinessInterfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        User CreateUser(User user);
        void UpdateUser(int id, User user);
        void DeleteUser(int id);
    }
}