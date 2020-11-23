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
            this.Name = name;
            this.Color = color;
            this.Price = price;
            this.Type = type;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }

        //protected bool Equals(Pet pet, Pet other)
        //{
        //    if (object.GetType() != this.GetType())
        //    {
        //        return false;
        //    }

        //    return Equals((Pet) object);
        //}

        public override bool Equals(object other)
        {
            var otherpet = (Pet)other;
            return Name == otherpet.Name && Type == otherpet.Type && Color == otherpet.Color && Price == otherpet.Price;
        }
    }
}
