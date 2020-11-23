using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetApi.Models
{
    public class Pet
    {
        public Pet()
        {
        }

        public Pet(string name, string type, string color, int price)
        {
            Name = name;
            Type = type;
            Color = color;
            Price = price;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals((Pet)obj);
        }

        protected bool Equals(Pet other)
        {
            return this.Name == other.Name
                   && this.Type == other.Type
                   && this.Color == other.Color
                   && this.Price == other.Price;
        }
    }
}