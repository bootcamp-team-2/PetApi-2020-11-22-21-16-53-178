using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetApi.Models;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStoreController : ControllerBase
    {
        private static IList<Pet> pets = new List<Pet>();

        [HttpPost("addNewPet")]
        public async Task<Pet> AddPetAsync(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("pet")]
        public async Task<Pet> GetPet(string name)
        {
            return pets.FirstOrDefault(pet => pet.Name == name);
        }

        [HttpGet("pets")]
        public async Task<IEnumerable<Pet>> GetPets(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return pets;
            }

            return pets.Where(pet => pet.Type == type);
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }

        [HttpDelete("pet")]
        public async Task DeletePet(string name)
        {
            pets.Remove(pets.FirstOrDefault(pet => pet.Name == name));
        }
    }
}
