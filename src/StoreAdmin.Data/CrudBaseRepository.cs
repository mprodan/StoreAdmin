using Microsoft.Data.Sqlite;
using StoreAdmin.Core.Models;
using System.Data;

namespace StoreAdmin.Data
{
    public abstract class CrudBaseRepository<T> : IDisposable where T : BaseModel
    {
        private readonly SqliteConnection _connection;

        protected CrudBaseRepository(DbContext dbContext)
        {
            _connection = new SqliteConnection(dbContext.ConnectionString);
            _connection.Open();
            InitializeDatabase().Wait();
        }

        private async Task InitializeDatabase()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = GetInitializationSql();
                await command.ExecuteNonQueryAsync();
            }
        }

        protected abstract string GetInitializationSql();

        protected async Task ExecuteNonQueryAsync(string commandText, params object[] parameters)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = commandText;
                AddParameters(command, parameters);
                await command.ExecuteNonQueryAsync();
            }
        }

        protected async Task<T?> ExecuteQueryFirstOrDefaultAsync(string commandText, Func<IDataReader, T> mapFunc, params object[] parameters)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = commandText;
                AddParameters(command, parameters);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return mapFunc(reader);
                    }
                    else 
                    {
                        return null;
                    }
                }
            }
        }

        protected async Task<List<T>> ExecuteQueryToListAsync(string commandText, Func<IDataReader, T> mapFunc, params object[] parameters)
        {
            var entities = new List<T>();
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = commandText;
                AddParameters(command, parameters);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        entities.Add(mapFunc(reader));
                    }
                }
            }
            return entities;
        }

        protected abstract T MapDataReaderToEntity(IDataReader reader);

        protected void AddParameters(IDbCommand command, object[] parameters)
        {
            for (int i = 0; i < parameters.Length; i += 2)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = parameters[i].ToString();
                parameter.Value = parameters[i + 1];
                command.Parameters.Add(parameter);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM " + typeof(T).Name + " WHERE Id = @id";
            return await ExecuteQueryFirstOrDefaultAsync(query, MapDataReaderToEntity, "@id", id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            var query = "SELECT * FROM " + typeof(T).Name;
            return await ExecuteQueryToListAsync(query, MapDataReaderToEntity);
        }

        public async Task<T> CreateAsync(T entity)
        {
            var query = $"INSERT INTO {typeof(T).Name} {GetInsertClause()}";
            var parameterValues = GetParameterValues(entity);
            await ExecuteNonQueryAsync(query, parameterValues.ToArray());
            entity.Id = GetLastInsertRowId();
            return entity;
        }

        private int GetLastInsertRowId()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT last_insert_rowid()";

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }

            return 0;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var query = "UPDATE " + typeof(T).Name + " SET " + GetUpdateSetClause() + " WHERE Id = @id";
            var parameterValues = GetParameterValues(entity);
            parameterValues.Add("@id");
            parameterValues.Add(entity.Id);
            await ExecuteNonQueryAsync(query, parameterValues.ToArray());
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM " + typeof(T).Name + " WHERE Id = @id";
            await ExecuteNonQueryAsync(query, "@id", id);
        }

        protected abstract List<object> GetParameterValues(T entity);
        protected abstract string GetUpdateSetClause();
        protected abstract string GetInsertClause();

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
