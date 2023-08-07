using StoreAdmin.Core.BusinessInterfaces;
using StoreAdmin.Core.Models;
using StoreAdmin.Core.RepositoryInterfaces;

namespace StoreAdmin.Business
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public User CreateUser(User user)
        {
            if(user == null) throw new ArgumentNullException("User null");
            ValidateUser(user);
            return _userRepository.CreateUser(user);
        }

        private void ValidateUser(User user)
        {
            var validator = new UserValidator();

            var result = validator.Validate(user);

            if (!result.IsValid)
            {
                throw new ArgumentException(result.Errors.FirstOrDefault()?.ErrorMessage);
            }

            //this is better do a call to the database
            if (_userRepository.GetAllUsers().Any(x => x.Username == user.Username && user.Id != x.Id))
            {
                throw new ArgumentException("User exists");
            }
        }

        public void UpdateUser(int id, User user)
        {
            ValidateUser(user);
            var existingUser = _userRepository.GetUserById(id);

            if (existingUser == null)
            {
                throw new ArgumentException("User not found.");
            }
            user.Id = existingUser.Id;
            _userRepository.UpdateUser(user);
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            _userRepository.DeleteUser(user);
        }


    }
}

