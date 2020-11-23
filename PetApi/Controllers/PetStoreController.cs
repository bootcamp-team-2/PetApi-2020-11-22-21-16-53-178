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

        [HttpPatch("pets")]
        public async Task<Pet> UpdatePet(Pet petToUpdate)
        {
            if (!pets.Any(pet => pet.Name == petToUpdate.Name))
            {
                return null;
            }

            var pet = pets.First(pet => pet.Name == petToUpdate.Name);
            pet.Price = petToUpdate.Price;
            return pet;
        }

        [HttpGet("pets")]
        public async Task<IEnumerable<Pet>> GetPets(string type, string color, string minPrice, string maxPrice)
        {
            return pets.Where(pet => GetTypeCondition(type, pet) 
                                     && GetColorCondition(color, pet)
                                     && GetMinPriceCondition(minPrice, pet)
                                     && GetMaxPriceCondition(maxPrice, pet));
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

        private bool GetTypeCondition(string type, Pet pet)
        {
            if (string.IsNullOrEmpty(type))
            {
                return true;
            }

            return pet.Type == type;
        }

        private bool GetColorCondition(string color, Pet pet)
        {
            if (string.IsNullOrEmpty(color))
            {
                return true;
            }

            return pet.Color == color;
        }

        private bool GetMinPriceCondition(string minPrice, Pet pet)
        {
            if (string.IsNullOrEmpty(minPrice))
            {
                return true;
            }

            return pet.Price >= int.Parse(minPrice);
        }

        private bool GetMaxPriceCondition(string maxPrice, Pet pet)
        {
            if (string.IsNullOrEmpty(maxPrice))
            {
                return true;
            }

            return pet.Price <= int.Parse(maxPrice);
        }
    }
}
