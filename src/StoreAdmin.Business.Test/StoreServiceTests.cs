using Xunit;
using StoreAdmin.Core.RepositoryInterfaces;
using StoreAdmin.Core.Models;
using Moq;
using StoreAdmin.Core.BusinessInterfaces;

namespace StoreAdmin.Business.Test
{
    public class StoreServiceTests
    {
        [Fact]
        public void CreateStore_ValidationOK()
        {
            // Arrange
            var repositoryMock = new Mock<IStoreRepository>();
            repositoryMock.Setup(repo => repo.GetAllStores()).Returns(new List<Store>());
            repositoryMock.Setup(repo => repo.CreateStore(It.IsAny<Store>())).Returns(new Store());
            var service = new StoreService(repositoryMock.Object);

            var store = new Store { Name = "store1", Location = "loc1"};

            // Act & Assert
            var resp = service.CreateStore(store);
            Assert.NotNull(resp);
        }

        [Fact]
        public void CreateStore_ValidationError()
        {
            // Arrange
            var repositoryMock = new Mock<IStoreRepository>();
            repositoryMock.Setup(repo => repo.GetAllStores()).Returns(new List<Store>());
            repositoryMock.Setup(repo => repo.CreateStore(It.IsAny<Store>())).Returns(new Store());
            var service = new StoreService(repositoryMock.Object);

            var store = new Store { Name = "", Location = "loc1" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.CreateStore(store));
        }

        [Fact]
        public void UpdateStore_ValidationError()
        {
            // Arrange
            var repositoryMock = new Mock<IStoreRepository>();
            repositoryMock.Setup(repo => repo.GetAllStores()).Returns(new List<Store>());
            var service = new StoreService(repositoryMock.Object);

            var store = new Store { Name = "", Location = "loc1" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.UpdateStore(1, store));
        }
    }

}