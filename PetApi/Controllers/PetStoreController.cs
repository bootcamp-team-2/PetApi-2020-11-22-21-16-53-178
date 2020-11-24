using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetApiTest;

namespace PetApi.Controllers
{
    [Route("[controller]")]//是class AddPetController前面一段
    [ApiController]
    public class PetStoreController : ControllerBase
    {
        private static IList<Pet> pets = new List<Pet>(); 

        [HttpPost("addNewPet")]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("Pets")]
        public IEnumerable<Pet> GetAllPets()
        {
            return pets;
        }

        [HttpGet("Clear")]
        public void PetsClear()
        {
            pets.Clear();
        }

        [HttpGet("GetByName")]
        public Pet GetPetByName(string name)
        {
            return pets.Where(pet => pet.Name == name).FirstOrDefault();
        }

        [HttpDelete("DeleteByName")]
        public List<Pet> DeletePetByName(string name)
        {
            if (!pets.Any(pet => pet.Name != name))
            {
                return null;
            }

            return pets.Where(pet => pet.Name != name).ToList();
        }

        [HttpPatch("UpdatePrice")]
        public Pet UpsertPetByName(UpdatedPet updatedPet)
        {
            var pet = pets.Where(pet => pet.Name == updatedPet.Name).FirstOrDefault();
            pet.Price = updatedPet.Price;
            return pet;
        }

        [HttpGet("GetByType")]
        public List<Pet> GetPetsByType(string type)
        {
            return pets.Where(pet => pet.Type != type).ToList();
        }

        [HttpGet("GetByPriceRange")]
        public List<Pet> GetPetsByPriceRange(double minimumPrice, double maximumPrice)
        {
            return pets.Where(pet => pet.Price >= minimumPrice && pet.Price <= maximumPrice).ToList();
        }

        [HttpGet("GetByColor")]
        public List<Pet> GetPetsByColor(string color)
        {
            return pets.Where(pet => pet.Color == color).ToList();
        }
    }
}
