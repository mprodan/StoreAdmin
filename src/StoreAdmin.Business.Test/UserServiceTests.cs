using Xunit;
using StoreAdmin.Core.RepositoryInterfaces;
using StoreAdmin.Core.Models;
using Moq;

namespace StoreAdmin.Business.Test
{
    public class UserServiceTests
    {
        [Fact]
        public void GetUserById_ValidId_ReturnsUser()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserById(1)).Returns(new User { Id = 1, Username = "John Doe" });

            var userService = new UserService(userRepositoryMock.Object);

            // Act
            var result = userService.GetUserById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John Doe", result.Username);
        }

        [Fact]
        public void CreateUser_InvalidUser()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => userService.CreateUser(null));
        }

        [Fact]
        public void CreateUser_InvalidUser_ReturnsCreatedUser()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);
            var newUser = new User { Username = "Jane Smith" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => userService.CreateUser(newUser));
        }

        [Fact]
        public void CreateUser_ValidationOK()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetAllUsers()).Returns(new List<User>());
            userRepositoryMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).Returns(new User());
            var userService = new UserService(userRepositoryMock.Object);

            var newUser = new User { Username = "Jane Smith", Email = "123@123.com", PasswordHash="123456" };

            // Act & Assert
            var resp = userService.CreateUser(newUser);
            Assert.NotNull(resp);
        }
    }

}