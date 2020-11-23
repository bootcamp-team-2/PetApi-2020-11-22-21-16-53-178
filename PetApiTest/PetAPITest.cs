using System;
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

        //[Fact]
        //public async void Should_get_right_info_When_get_all_pet()
        //{
        //    //given
        //    TestServer server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        //    HttpClient client = server.CreateClient();
        //    await client.DeleteAsync("petStore/removePets");
        //    Pet pet = new Pet("huanhuan", "dog", "white", 5000);
        //    string request = JsonConvert.SerializeObject(pet); //该方法强制调用空构造函数，再拿属性
        //    StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
        //    //when
        //    await client.PostAsync("petStore/addNewPet", requestBody);
        //    var response = await client.GetAsync("petStore/pets");
        //    //then
        //    response.EnsureSuccessStatusCode();
        //    var responseString = await response.Content.ReadAsStringAsync();
        //    Pet actualPet = JsonConvert.DeserializeObject<Pet>(responseString);
        //    Assert.Equal(pet, actualPet);
        //}
    }
}
