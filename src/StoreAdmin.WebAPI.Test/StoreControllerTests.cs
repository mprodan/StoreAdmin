using Newtonsoft.Json;
using System.Net;
using Xunit;
using StoreAdmin.Core.Models;
using System.Text;

namespace StoreAdmin.WebAPI.Test
{
    [Collection("SharedFileCollection")]
    public class StoreControllerTests: BaseTest
    {

        public StoreControllerTests()
        {
        }

        [Fact]
        public async Task GetStores()
        {
            var response = await _httpClient.GetAsync("/api/stores");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var stores = JsonConvert.DeserializeObject<List<Store>>(content);
            Assert.Empty(stores);
        }

        [Fact]
        public async Task CRUDStore()
        {
            // Create Store
            var requestContent = new StringContent(
                $"{{ \"name\": \"test\", \"location\": \"loc\"}}",
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PostAsync("/api/stores", requestContent);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var store = JsonConvert.DeserializeObject<Store>(content);
            Assert.Equal("test", store.Name);
            Assert.NotEqual(0,store.Id);

            // Get Store
            var responseGet = await _httpClient.GetAsync($"/api/stores/{store.Id}");
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            content = await responseGet.Content.ReadAsStringAsync();
            var storeGet = JsonConvert.DeserializeObject<Store>(content);
            Assert.Equal(store.Id, storeGet.Id);
            Assert.Equal(store.Name, storeGet.Name);
            Assert.Equal(store.Location, storeGet.Location);

            //Update store
            var requestContentPut = new StringContent(
                $"{{ \"name\": \"test2\", \"location\": \"loc2\"}}",
                Encoding.UTF8,
                "application/json"
            );
            var responsePut = await _httpClient.PutAsync($"/api/stores/{store.Id}", requestContentPut);
            Assert.Equal(HttpStatusCode.NoContent, responsePut.StatusCode);

            // Get Store
            responseGet = await _httpClient.GetAsync($"/api/stores/{store.Id}");
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            content = await responseGet.Content.ReadAsStringAsync();
            storeGet = JsonConvert.DeserializeObject<Store>(content);
            Assert.Equal("test2", storeGet.Name);

            // GetAll Store
            responseGet = await _httpClient.GetAsync($"/api/stores");
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            content = await responseGet.Content.ReadAsStringAsync();
            var stores = JsonConvert.DeserializeObject<List<Store>>(content);
            Assert.Contains(stores, x=> x.Name=="test2");

            //Delete
            var responseDel = await _httpClient.DeleteAsync($"/api/stores/{store.Id}");
            Assert.Equal(HttpStatusCode.NoContent, responseDel.StatusCode);

            // GetAll Store
            responseGet = await _httpClient.GetAsync($"/api/stores");
            Assert.Equal(HttpStatusCode.OK, responseGet.StatusCode);
            content = await responseGet.Content.ReadAsStringAsync();
            stores = JsonConvert.DeserializeObject<List<Store>>(content);
            Assert.DoesNotContain(stores, x => x.Name == "test2");

        }

    }
}


