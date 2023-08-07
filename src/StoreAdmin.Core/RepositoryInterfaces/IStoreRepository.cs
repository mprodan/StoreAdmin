using StoreAdmin.Core.Models;

namespace StoreAdmin.Core.RepositoryInterfaces
{
    public interface IStoreRepository
    {
        List<Store> GetAllStores();
        Store GetStoreById(int id);
        Store CreateStore(Store store);
        Store UpdateStore(Store store);
        void DeleteStore(Store store);
    }
}
