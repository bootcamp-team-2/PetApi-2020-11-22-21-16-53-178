﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PetStoreController : ControllerBase
    {
        static private IList<Pet> pets = new List<Pet>();
        [HttpPost("addNewPet")]
        public Pet AddPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("pets")]
        public IList<Pet> GetAllPets()
        {
            return pets;
        }

        [HttpGet("getPetByName")]
        public IList<Pet> GetPetByName(string name)
        {
            return pets.Where(pet => pet.Name == name).ToList();
        }

        [HttpGet("getPetByType")]
        public IList<Pet> GetPetByType(string type)
        {
            return pets.Where(pet => pet.Type == type).ToList();
        }

        [HttpGet("getPetByColor")]
        public IList<Pet> GetPetByColor(string color)
        {
            return pets.Where(pet => pet.Color == color).ToList();
        }

        [HttpGet("getPetByPriceRange")]
        public IList<Pet> GetPetByPriceRange(int minPrice, int maxPrice)
        {
            return pets.Where(pet => pet.Price >= minPrice && pet.Price <= maxPrice).ToList();
        }

        [HttpDelete("clear")]
        public void Clear()
        {
            pets.Clear();
        }

        [HttpDelete("deletePet")]
        public void DeletePet(string name)
        {
            pets = pets.Where(pet => pet.Name != name).ToList();
        }

        [HttpPut("updatePet")]
        public Pet UpdatePetPrice(string name, Pet newpet)
        {
            pets = pets.Select(pet =>
            {
                if (pet.Name == name)
                {
                    pet = newpet;
                }

                return pet;
            }).ToList();
            return pets.First(pet => pet.Name == name);
        }
    }
}
