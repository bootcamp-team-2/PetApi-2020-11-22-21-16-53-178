using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetStoreController : ControllerBase
    {
        private static List<Pet> pets = new List<Pet>();
        [HttpPost("addNewPet")]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpPost("addNewPets")]
        public void AddPet(List<Pet> newPets)
        {
            foreach (var newPet in newPets)
            {
                pets.Add(newPet);
            }
        }

        [HttpGet("{name}")]
        public Pet GetPetByName(string name)
        {
            return pets.FirstOrDefault(pet => pet.Name == name);
        }

        [HttpGet("type/{type}")]
        public List<Pet> GetPetByType(string type)
        {
            return pets.Where(pet => pet.Type == type).Select(x => x).ToList();
        }

        [HttpDelete("RemoveSoldPet/{name}")]
        public List<Pet> RemoveOnePet(string name)
        {
            return pets.Where(pet => pet.Name != name).ToList();
        }

        [HttpGet("pets")]
        public List<Pet> Getall()
        {
            return pets;
        }

        [HttpDelete("removePets")]
        public void ClearPets()
        {
            pets.Clear();
        }

        [HttpPatch("ModifyPrice/{name}")]
        public List<Pet> ModifyPrice(string name, UpdatePet newPet)
        {
            return pets.Where(pet => pet.Name == name).Select(pet => 
            { 
                pet.Price = newPet.Price;
                return pet;
            }).ToList();
        }
    }
}
