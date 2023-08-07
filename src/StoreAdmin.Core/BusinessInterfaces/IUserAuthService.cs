namespace StoreAdmin.Core.BusinessInterfaces
{
    public interface IUserAuthService
    {
        static readonly string JwtSecretKey = "storeAdminstoreAdminstoreAdminstoreAdminstoreAdminstoreAdminstoreAdminstoreAdmin";
        string AuthenticateUser(string username, string password);
    }
}