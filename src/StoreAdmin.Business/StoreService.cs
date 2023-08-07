using StoreAdmin.Core.BusinessInterfaces;
using StoreAdmin.Core.Models;
using StoreAdmin.Core.RepositoryInterfaces;
using StoreAdmin.Data;
using static System.Formats.Asn1.AsnWriter;

namespace StoreAdmin.Business
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _repository;

        public StoreService(IStoreRepository repository)
        {
            _repository = repository;
        }

        public List<Store> GetAllStores()
        {
            return _repository.GetAllStores();
        }

        public Store GetStoreById(int id)
        {
            return _repository.GetStoreById(id);
        }

        public Store CreateStore(Store store)
        {
            ValidateStore(store);
            return _repository.CreateStore(store);
        }

        public Store UpdateStore(int id, Store updatedStore)
        {
            ValidateStore(updatedStore);
            var existingStore = _repository.GetStoreById(id);
            if (existingStore == null)
            {
                throw new ArgumentException("Store not found.");
            }

            existingStore.Name = updatedStore.Name;
            existingStore.Location = updatedStore.Location;

            return _repository.UpdateStore(existingStore);
        }

        public void DeleteStore(int id)
        {
            var store = _repository.GetStoreById(id);
            if (store == null)
            {
                throw new ArgumentException("Store not found.");
            }

            _repository.DeleteStore(store);
        }

        private void ValidateStore(Store store)
        {
            var validator = new StoreValidator();

            var result = validator.Validate(store);

            if (!result.IsValid)
            {
                throw new ArgumentException(result.Errors.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}


