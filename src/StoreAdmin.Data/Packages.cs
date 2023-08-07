using Microsoft.Extensions.DependencyInjection;
using StoreAdmin.Core.RepositoryInterfaces;

namespace StoreAdmin.Data
{
    public static class Packages
    {
        public static void AddRepositories(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddSingleton(new DbContext(connectionString));

            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}