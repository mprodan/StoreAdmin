using System.Data;
using StoreAdmin.Core.Models;
using StoreAdmin.Core.RepositoryInterfaces;

namespace StoreAdmin.Data
{
    public class StoreRepository : CrudBaseRepository<Store>, IStoreRepository
    {
        public StoreRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override string GetInitializationSql()
        {
            return @"CREATE TABLE IF NOT EXISTS Store (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Location TEXT NOT NULL
                    );";
        }

        protected override Store MapDataReaderToEntity(IDataReader reader)
        {
            var entity = new Store();
            entity.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            entity.Name = reader.GetString(reader.GetOrdinal("Name"));
            entity.Location = reader.GetString(reader.GetOrdinal("Location"));
            return entity;
        }

        protected override List<object> GetParameterValues(Store entity)
        {
            return new List<object> { "@name", entity.Name, "@location", entity.Location };
        }

        protected override string GetUpdateSetClause()
        {
            return "Name = @name, Location = @location";
        }

        protected override string GetInsertClause()
        {
            return "(Name, Location) VALUES (@name, @location)";
        }


        //OLD 

        public List<Store> GetAllStores()
        {
            return GetAllAsync().GetAwaiter().GetResult();
        }

        public Store GetStoreById(int id)
        {
            return GetByIdAsync(id).GetAwaiter().GetResult();
        }

        public Store CreateStore(Store store)
        {
            return CreateAsync(store).GetAwaiter().GetResult();
        }

        public Store UpdateStore(Store store)
        {
            return UpdateAsync(store).GetAwaiter().GetResult();
        }

        public void DeleteStore(Store store)
        {
            DeleteAsync(store.Id).GetAwaiter().GetResult();
        }
    }
}
