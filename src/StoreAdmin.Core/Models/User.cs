namespace StoreAdmin.Core.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
