using StoreAdmin.Core.Models;

namespace StoreAdmin.Core.BusinessInterfaces
{
    public interface IStoreService
    {
        List<Store> GetAllStores();
        Store GetStoreById(int id);
        Store CreateStore(Store store);
        Store UpdateStore(int id, Store updatedStore);
        void DeleteStore(int id);
    }
}