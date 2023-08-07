using Newtonsoft.Json;
using System.Net;
using Xunit;
using StoreAdmin.Core.Models;
using System.Text;

namespace StoreAdmin.WebAPI.Test
{
    [Collection("SharedFileCollection")]
    public class UsersControllerTests : BaseTest
    {

        public UsersControllerTests()
        {
        }

        [Fact]
        public async Task GetUsers_ReturnsUserAdmin()
        {
            var response = await _httpClient.GetAsync("/api/users");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(content);
            Assert.Contains(users, x => x.Username == "admin");
        }

        [Fact]
        public async Task GetUserByID_ReturnsUserAdmin()
        {
            var response = await _httpClient.GetAsync("/api/users/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(content);
            Assert.Equal("admin", user.Username);
        }

        [Fact]
        public async Task CRUDUser()
        {
            // Create
            var requestContent = new StringContent(
                $"{{ \"username\": \"test\", \"PasswordHash\": \"test1234\" , \"email\": \"1@1.com\"}}",
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PostAsync("/api/users", requestContent);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(content);
            Assert.Equal("test", user.Username);
            Assert.NotEqual(0, user.Id);

            // Get 
            var responseGet = await _httpClient.GetAsync($"/api/users/{user.Id}");
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            content = await responseGet.Content.ReadAsStringAsync();
            var userGet = JsonConvert.DeserializeObject<User>(content);
            Assert.Equal(user.Id, userGet.Id);
            Assert.Equal(user.Username, userGet.Username);
            Assert.Equal(user.Email, userGet.Email);

            //Update user
            var requestContentPut = new StringContent(
                $"{{ \"username\": \"test2\", \"email\": \"1@1.com\",\"PasswordHash\": \"test1234\"}}",
                Encoding.UTF8,
                "application/json"
            );
            var responsePut = await _httpClient.PutAsync($"/api/users/{user.Id}", requestContentPut);
            Assert.Equal(HttpStatusCode.NoContent, responsePut.StatusCode);

            // Get Store
            responseGet = await _httpClient.GetAsync($"/api/users/{user.Id}");
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            content = await responseGet.Content.ReadAsStringAsync();
            userGet = JsonConvert.DeserializeObject<User>(content);
            Assert.Equal("test2", userGet.Username);

            // GetAll Store
            responseGet = await _httpClient.GetAsync($"/api/users");
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            content = await responseGet.Content.ReadAsStringAsync();
            var stores = JsonConvert.DeserializeObject<List<User>>(content);
            Assert.Contains(stores, x => x.Username == "test2");

            //Delete
            var responseDel = await _httpClient.DeleteAsync($"/api/users/{user.Id}");
            Assert.Equal(HttpStatusCode.NoContent, responseDel.StatusCode);

            // GetAll Store
            responseGet = await _httpClient.GetAsync($"/api/users");
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            content = await responseGet.Content.ReadAsStringAsync();
            stores = JsonConvert.DeserializeObject<List<User>>(content);
            Assert.DoesNotContain(stores, x => x.Username == "test2");
        }

    }
}


