using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using PetApi;
using Xunit;

namespace PetApiTest
{
    public class PetAPITest
    {
        [Fact]
        public async void Should_Creat_When_Add_pet()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            Pet pet = new Pet("huanhuan", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet); //该方法强制调用空构造函数，再拿属性
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            //when
            var response = await client.PostAsync("petStore/addNewPet", requestBody);
            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet, actualPet);
        }

        [Fact]
        public async void Should_get_right_info_When_get_all_pet()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/removePets");
            Pet pet = new Pet("huanhuan", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            //when
            await client.PostAsync("petStore/addNewPet", requestBody);
            var response = await client.GetAsync("petStore/pets");
            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Contains(pet, actualPets);
        }

        [Fact]
        public async void Should_get_right_pet_When_search_by_name()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/removePets");
            Pet pet1 = new Pet("HuanHuan", "dog", "white", 5000);
            Pet pet2 = new Pet("DaHuang", "dog", "white", 5000);
            Pet pet3 = new Pet("WangCai", "dog", "white", 5000);
            List<Pet> pets = new List<Pet>() { pet1, pet2, pet3 };
            string request = JsonConvert.SerializeObject(pets);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            //when
            await client.PostAsync("petStore/addNewPets", requestBody);
            var response = await client.GetAsync("PetStore/WangCai");
            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
            Assert.Equal(pet3, actualPet);
        }

        [Fact]
        public async void Should_remove_pet_after_sale()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/removePets");
            Pet pet1 = new Pet("HuanHuan", "dog", "white", 5000);
            Pet pet2 = new Pet("DaHuang", "dog", "white", 5000);
            Pet pet3 = new Pet("WangCai", "dog", "white", 5000);
            List<Pet> pets = new List<Pet>() { pet1, pet2, pet3 };
            string request = JsonConvert.SerializeObject(pets);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            //when
            await client.PostAsync("petStore/addNewPets", requestBody);
            var response = await client.DeleteAsync("petStore/RemoveSoldPet/WangCai");
            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Contains(pet1, actualPets);
            Assert.Contains(pet2, actualPets);
            Assert.DoesNotContain(pet3, actualPets);
        }

        [Fact]
        public async void Should_be_able_to_modify_the_price_of_a_pet()
        {
            //given
            TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("petStore/removePets");
            Pet pet = new Pet("huanhuan", "dog", "white", 5000);
            string request = JsonConvert.SerializeObject(pet);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            //when
            await client.PostAsync("petStore/addNewPet", requestBody);

            UpdatePet newpet = new UpdatePet("huanhuan", 100);
            string request2 = JsonConvert.SerializeObject(newpet);
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            //when
            var response = await client.PatchAsync("petStore/ModifyPrice/huanhuan", requestBody2);
            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Pet> actualPets = JsonConvert.DeserializeObject<List<Pet>>(responseString);
            Assert.Equal(100, actualPets[0].Price);
        }
    }
}
