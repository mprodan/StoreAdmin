using Xunit;
using StoreAdmin.Core.Models;

namespace StoreAdmin.Data.Test
{
    [Collection("SharedFileCollection")]
    public class UserRepositoryTests
    {
        public UserRepositoryTests()
        {
        }

        [Fact]
        public void UserRepository_CRUD()
        {
            using (var repository = new UserRepository(new DbContext($"Data Source={TestDataFixture.TestPath}"))) 
            {                
                // Create
                var UserCreate = repository.CreateUser(new User { Username = "name", Email = "m@m.com", PasswordHash = "123456" });

                // Assert
                Assert.NotNull(UserCreate);

                //Get
                var UserGet = repository.GetUserById(UserCreate.Id);

                // Assert
                Assert.NotNull(UserGet);
                Assert.Equal(UserCreate.Id, UserGet.Id);
                Assert.Equal(UserCreate.Username, UserGet.Username);
                Assert.Equal(UserCreate.Email, UserGet.Email);

                //Get All
                var Users = repository.GetAllUsers();
                Assert.Equal(2, Users.Count);

                //Update
                repository.UpdateUser(new User { Id = UserCreate.Id, Username = "Update", Email = "Loc" });
                UserGet = repository.GetUserById(UserCreate.Id);

                // Assert
                Assert.NotNull(UserGet);
                Assert.Equal("Update", UserGet.Username);

                //Delete
                repository.DeleteUser(new User { Id = UserCreate.Id });

                //Get All
                Users = repository.GetAllUsers();
                Assert.Single(Users);
            }
        }
    }
}
