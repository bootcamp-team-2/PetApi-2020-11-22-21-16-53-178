using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PetApi;
using PetApi.Models;
using Xunit;

namespace PetApiTest
{
    public class PetApiTest
    {
        [Fact]
        public async Task Should_add_pet_when_add_pet()
        {
            TestServer testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = testServer.CreateClient();
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            await client.DeleteAsync("petStore/clear");
            var response = await client.PostAsync("petStore/addNewPet", requestBody);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_return_correct_pets_when_get_all_pets()
        {
            TestServer testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = testServer.CreateClient();
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            await client.DeleteAsync("petStore/clear");
            await client.PostAsync("petStore/addNewPet", requestBody);

            var response = await client.GetAsync("petStore/pets");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<IList<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet }, actualPets);
        }

        [Fact]
        public async Task Should_return_pet_with_correct_name_when_given_name()
        {
            TestServer testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = testServer.CreateClient();
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            await client.DeleteAsync("petStore/clear");
            await client.PostAsync("petStore/addNewPet", requestBody);

            var response = await client.GetAsync("petStore/pet?name=BayMax");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_delete_pet_when_delete()
        {
            TestServer testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = testServer.CreateClient();
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            await client.DeleteAsync("petStore/clear");
            await client.PostAsync("petStore/addNewPet", requestBody);

            await client.DeleteAsync("petStore/pet?name=BayMax");

            var response = await client.GetAsync("petStore/pet?name=BayMax");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Empty(responseString);
        }

        [Fact]
        public async Task Should_return_pets_with_same_type_when_given_type()
        {
            TestServer testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = testServer.CreateClient();
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            await client.DeleteAsync("petStore/clear");
            await client.PostAsync("petStore/addNewPet", requestBody);

            var response = await client.GetAsync("petStore/pets?type=dog");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<IList<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet }, actualPets);
        }
    }
}
