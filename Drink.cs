using SOA_CA1.Interfaces;

namespace SOA_CA1
{
    public abstract class Drink : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Instructions { get; set; }
        public string Image { get; set; }

        public abstract void DisplayDrinkInfo();
    }
}
