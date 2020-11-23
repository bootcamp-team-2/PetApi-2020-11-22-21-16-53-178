using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetApi.Models
{
    public class PetToUpdate
    {
        public PetToUpdate(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; }
        public int Price { get; }
    }
}
