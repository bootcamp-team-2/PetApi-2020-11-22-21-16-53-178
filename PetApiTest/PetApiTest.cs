using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PetApi;
using Xunit;

namespace PetApiTest
{
    public class PetApiTest
    {
        // petStore/addNewPet
        [Fact]
        public async Task Should_When_Add_Pet()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            Pet pet = new Pet("Baymax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            // when
            var response = await client.PostAsync("petStore/addNewPet", requestBody);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async Task Should_Return_Correct_Pets_When_Get_All_Pets()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/clear");

            Pet pet = new Pet("Baymax", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            // when
            var response = await client.GetAsync("petStore/pets");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet }, actualPet);
        }

        [Fact]
        public async Task Should_Return_Correct_Pet_When_Given_Name()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/clear");

            Pet pet_1 = new Pet("Baymax", "dog", "white", 5000);
            Pet pet_2 = new Pet("Tom", "cat", "black", 1300);
            string request = JsonConvert.SerializeObject(pet_1);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            request = JsonConvert.SerializeObject(pet_2);
            requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            string query_name = "Tom";

            // when
            var response = await client.GetAsync($"petStore/getPetByName?name={query_name}");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet_2 }, actualPet);
        }

        [Fact]
        public async Task Should_Delete_Pet_Given_Pet()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/clear");

            string petName = "Baymax";
            Pet pet_1 = new Pet(petName, "dog", "white", 5000);
            Pet pet_2 = new Pet("Tom", "cat", "black", 1300);
            string request_1 = JsonConvert.SerializeObject(pet_1);
            StringContent requestBody_1 = new StringContent(request_1, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody_1);

            string request_2 = JsonConvert.SerializeObject(pet_2);
            StringContent requestBody_2 = new StringContent(request_2, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody_2);

            // when
            await client.DeleteAsync($"petStore/deletePet?name={petName}");
            var response = await client.GetAsync("petStore/pets");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet_2 }, actualPet);
        }

        [Fact]
        public async Task Should_Update_Pet_Given_Name_And_Price()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/clear");

            string petName = "Baymax";
            Pet pet_1 = new Pet(petName, "dog", "white", 5000);
            string request_1 = JsonConvert.SerializeObject(pet_1);
            StringContent requestBody_1 = new StringContent(request_1, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody_1);
            int newPrice = 800;
            pet_1.Price = newPrice;

            string request = JsonConvert.SerializeObject(pet_1);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            // when
            var response = await client.PutAsync($"petStore/updatePet?name={petName}", requestBody);

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet_1, actualPet);
        }

        [Fact]
        public async Task Should_Return_Correct_Pet_When_Given_Type()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/clear");

            Pet pet_1 = new Pet("Baymax", "dog", "white", 5000);
            Pet pet_2 = new Pet("Tom", "cat", "black", 1300);
            Pet pet_3 = new Pet("Hans", "cat", "yellow", 500);
            string request = JsonConvert.SerializeObject(pet_1);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            request = JsonConvert.SerializeObject(pet_2);
            requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            request = JsonConvert.SerializeObject(pet_3);
            requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            string query_Type = "cat";

            // when
            var response = await client.GetAsync($"petStore/getPetByType?type={query_Type}");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet_2, pet_3 }, actualPet);
        }

        [Fact]
        public async Task Should_Return_Correct_Pet_When_Given_Color()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/clear");

            Pet pet_1 = new Pet("Baymax", "dog", "white", 5000);
            Pet pet_2 = new Pet("Tom", "cat", "black", 1300);
            Pet pet_3 = new Pet("Hans", "cat", "white", 500);
            string request = JsonConvert.SerializeObject(pet_1);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            request = JsonConvert.SerializeObject(pet_2);
            requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            request = JsonConvert.SerializeObject(pet_3);
            requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("petStore/addNewPet", requestBody);

            string query_Color = "white";

            // when
            var response = await client.GetAsync($"petStore/getPetByColor?color={query_Color}");

            // then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet>() { pet_1, pet_3 }, actualPet);
        }
    }
}
