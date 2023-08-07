using Xunit;
using StoreAdmin.Core.Models;

namespace StoreAdmin.Data.Test
{

    [Collection("SharedFileCollection")]
    public class StoreRepositoryTests
    {

        public StoreRepositoryTests()
        {
            
        }

        [Fact]
        public void StoreRepository_CRUD()
        {
            using (var repository = new StoreRepository(new DbContext($"Data Source={TestDataFixture.TestPath}")))
            {
                // Create
                var storeCreate = repository.CreateStore(new Store { Name = "name", Location = "location" });

                // Assert
                Assert.NotNull(storeCreate);

                //Get
                var storeGet = repository.GetStoreById(storeCreate.Id);

                // Assert
                Assert.NotNull(storeGet);
                Assert.Equal(storeCreate.Id, storeGet.Id);
                Assert.Equal(storeCreate.Name, storeGet.Name);
                Assert.Equal(storeCreate.Location, storeGet.Location);

                //Get All
                var stores = repository.GetAllStores();
                Assert.Single(stores);

                //Update
                repository.UpdateStore(new Store { Id = storeCreate.Id, Name = "Update", Location = "Loc" });
                storeGet = repository.GetStoreById(storeCreate.Id);

                // Assert
                Assert.NotNull(storeGet);
                Assert.Equal("Update", storeGet.Name);

                //Delete
                repository.DeleteStore(new Store { Id = storeCreate.Id });

                //Get All
                stores = repository.GetAllStores();
                Assert.Empty(stores);

            }

        }

    }
}
