namespace PetApiTest
{
    public class Pet
    {
        public Pet()
        {
        }

        public Pet(string name, string type, string color, double price)
        {
            this.Name = name;
            this.Type = type;
            this.Color = color;
            this.Price = price;
        }

        public string Type { get; set;  }
        public string Name { get; set; }
        public string Color { get; set; }
        public double Price { get; set;  }

        public override bool Equals(object? obj)
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected bool Equals(Pet other)
        {
            return Name == other.Name && Type == other.Type && Color == other.Color && Price == other.Price;
        }

        //public Pet AddPet(Pet pet)
        //{
        //    return pet;
        //}
    }
}