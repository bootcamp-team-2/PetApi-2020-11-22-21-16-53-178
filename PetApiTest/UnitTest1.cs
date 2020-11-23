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
        //petStore/addNewPet
        [Fact]
        public async Task Should_Add_Pet_When_Add_Pet()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            Pet pet = new Pet("huahua", "dog", "black", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            //when
            var response = await client.PostAsync("PetStore/addNewPet", requestBody);

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actrualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actrualPet);
        }

        [Fact]
        public async Task Should_Return_All_Pets_When_Get_All_Pets()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("PetStore/Clear");

            Pet pet = new Pet("huahua", "dog", "black", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            //when
            await client.PostAsync("PetStore/addNewPet", requestBody);
            var response = await client.GetAsync("PetStore/Pets");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actrualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet> { pet }, actrualPet);
        }

        [Fact]
        public async Task Should_Return_Specific_Pet_When_Providing_Its_Name()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("PetStore/Clear");

            Pet pet = new Pet("huahua", "dog", "black", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("PetStore/addNewPet", requestBody);

            //when
            var response = await client.GetAsync($"PetStore/GetByName?name={pet.Name}");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actrualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actrualPet);
        }

        [Fact]
        public async Task Should_Delete_Specific_Pet_When_Buy_Name()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("PetStore/Clear");

            Pet pet = new Pet("huahua", "dog", "black", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("PetStore/addNewPet", requestBody);

            //when
            var response = await client.DeleteAsync($"PetStore/DeleteByName?name={pet.Name}");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actrualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Null(actrualPet);
        }

        [Fact]
        public async Task Should_Delete_Specific_Pet_When_Buy_item_Buy_Name_With_Two_Items()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("PetStore/Clear");

            Pet pet = new Pet("huahua", "dog", "black", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            await client.PostAsync("PetStore/addNewPet", requestBody);

            Pet pet2 = new Pet("hua2", "dog", "black", 5000);
            string request2 = JsonConvert.SerializeObject(pet2);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            await client.PostAsync("PetStore/addNewPet", requestBody2);

            //when
            var response = await client.DeleteAsync($"PetStore/DeleteByName?name={pet.Name}");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actrualPet = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(new List<Pet> { pet2 }, actrualPet);
        }
    }
}
