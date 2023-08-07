namespace StoreAdmin.Data
{
    public class DbContext
    {
        private string _connectionString;
        public DbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString { get { return _connectionString; } }
    }
}
