using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetApi
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Pet)obj);
        }

        protected bool Equals(Pet otherPet)
        {
            return otherPet.Name == this.Name && otherPet.Type == this.Type && otherPet.Color == this.Color &&
                   otherPet.Price == this.Price;
        }
    }
}
