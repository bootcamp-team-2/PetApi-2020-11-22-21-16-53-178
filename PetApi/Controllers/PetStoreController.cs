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

        [HttpGet("pets")]
        public List<Pet> Getall()
        {
            return pets;
        }

        [HttpDelete("removePets")]
        public void Delete()
        {
            pets.Clear();
        }
    }
}
