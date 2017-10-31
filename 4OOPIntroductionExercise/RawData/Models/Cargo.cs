using RawData.Enums;

namespace RawData.Models
{
    public class Cargo
    {
        private int weight;
        private CargoType type;

        public Cargo(int weight, CargoType type)
        {
            this.weight = weight;
            this.type = type;
        }

        public CargoType Type
        {
            get { return this.type; }
            private set { this.type = value; }
        }
    }
}