using Microsoft.Data.Sqlite;
using StoreAdmin.Core.Models;
using StoreAdmin.Core.RepositoryInterfaces;

namespace StoreAdmin.Data
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly SqliteConnection _connection;

        public UserRepository(DbContext dbContext)
        {
            _connection = new SqliteConnection(dbContext.ConnectionString);
            _connection.Open();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Users (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Username TEXT NOT NULL,
                                        Email TEXT NOT NULL,
                                        PasswordHash TEXT NOT NULL
                                        );";

                command.ExecuteNonQuery();
            }

            if(GetAllUsers().Count == 0) 
            {
                using (var insertCommand = _connection.CreateCommand())
                {
                    insertCommand.CommandText = @"INSERT INTO Users (Username, Email, PasswordHash)
                                  VALUES (@username, @email, @password);";

                    insertCommand.Parameters.AddWithValue("@username", "admin");
                    insertCommand.Parameters.AddWithValue("@email", "admin@admin.com");
                    insertCommand.Parameters.AddWithValue("@password", "$2a$11$AHhP5JRgHYe5KJJBfXjz8uwJX1hKwl5CVcEEpcjsBPkEpa/B2AiJm");

                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT Id, Username, Email FROM Users";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2)
                        });
                    }
                }
            }

            return users;
        }

        public User GetUserById(int id)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT Id, Username, Email FROM Users WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2)
                        };
                    }
                }
            }

            return null;
        }
        public User CreateUser(User user)
        {
            // Hash the password before storing it in the database.
            var hashedPassword = HashPassword(user.PasswordHash);
            user.PasswordHash = hashedPassword;

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Users (Username, Email, PasswordHash) VALUES (@username, @email, @password)";
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@password", user.PasswordHash);

                command.ExecuteNonQuery();
            }

            user.Id = GetLastInsertRowId();
            user.PasswordHash = null;
            return user;
        }

        public User UpdateUser(User user)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "UPDATE Users SET Username = @username, Email = @email";
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    var hashedPassword = HashPassword(user.PasswordHash);
                    user.PasswordHash = hashedPassword;
                    command.CommandText += ", PasswordHash = @password";
                    command.Parameters.AddWithValue("@password", user.PasswordHash);
                }
                command.CommandText += " WHERE Id = @id";
                command.Parameters.AddWithValue("@id", user.Id);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@email", user.Email);

                command.ExecuteNonQuery();
            }

            return user;
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

        public void DeleteUser(User user)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Users WHERE Id = @id";
                command.Parameters.AddWithValue("@id", user.Id);

                command.ExecuteNonQuery();
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public User GetUserByusername(string username)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT Id, Username, Email, PasswordHash FROM Users " +
                                      "WHERE Username = @username";
                command.Parameters.AddWithValue("@username", username);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2),
                            PasswordHash = reader.GetString(3)
                        };
                    }
                }
            }

            return null;
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
