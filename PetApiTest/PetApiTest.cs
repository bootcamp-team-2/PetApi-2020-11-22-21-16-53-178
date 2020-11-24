using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PetApi;
using PetApi.Models;
using Xunit;

namespace PetApiTest
{
    public class PetApiTest
    {
        private readonly HttpClient client;
        public PetApiTest()
        {
            TestServer testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            this.client = testServer.CreateClient();
            client.DeleteAsync("petStore/clear");
        }

        [Fact]
        public async Task Should_add_pet_when_add_pet()
        {
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("petStore/addNewPet", requestBody);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_return_correct_pets_when_get_all_pets()
        {
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

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
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

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
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            await client.PostAsync("petStore/addNewPet", requestBody);

            await client.DeleteAsync("petStore/pet?name=BayMax");

            var response = await client.GetAsync("petStore/pet?name=BayMax");
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }

        [Fact]
        public async Task Should_return_pets_with_same_type_when_given_type()
        {
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            await client.PostAsync("petStore/addNewPet", requestBody);

            var response = await client.GetAsync("petStore/pets?type=dog");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<IList<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet }, actualPets);
        }

        [Fact]
        public async Task Should_return_pets_with_same_color_when_given_color()
        {
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            await client.PostAsync("petStore/addNewPet", requestBody);

            var response = await client.GetAsync("petStore/pets?color=white");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<IList<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet }, actualPets);
        }

        [Fact]
        public async Task Should_return_pets_in_price_range_when_given_price_range()
        {
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            await client.PostAsync("petStore/addNewPet", requestBody);

            var response = await client.GetAsync("petStore/pets?minPrice=1000&maxPrice=6000");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPets = JsonConvert.DeserializeObject<IList<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet }, actualPets);
        }

        [Fact]
        public async Task Should_return_pet_with_price_updated_when_update_price()
        {
            Pet pet = new Pet("BayMax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);
            var petToUpdate = new PetToUpdate("BayMax", 4000);

            string requestUpdate = JsonConvert.SerializeObject(petToUpdate);
            StringContent requestBodyUpdate = new StringContent(requestUpdate, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync("petStore/pets", requestBodyUpdate);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(petToUpdate.Price, actualPet.Price);
        }
    }
}
