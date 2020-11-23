namespace PetApiTest
{
    public class UpdatedPet
    {
        public UpdatedPet()
        {
        }

        public UpdatedPet(string name, double price)
        {
            this.Name = name;
            this.Price = price;
        }

        public string Name { get; set; }
        public double Price { get; set; }
    }
}