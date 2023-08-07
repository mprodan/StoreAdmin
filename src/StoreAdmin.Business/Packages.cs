using Microsoft.Extensions.DependencyInjection;
using StoreAdmin.Core.BusinessInterfaces;

namespace StoreAdmin.Business
{
    public static class Packages
    {
        public static void AddServices(
            this IServiceCollection services
        )
        {
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAuthService, UserAuthService>();
        }
    }
}