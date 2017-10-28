namespace SpeedRacing
{
    public class Car
    {
        public Car(string model, double fuelAmount, double consumptionPerKm)
        {
            this.Model = model;
            this.FuelAmount = fuelAmount;
            this.ConsumptionPerKm = consumptionPerKm;
        }

        public string Model { get; set; }

        public double FuelAmount { get; set; }

        public double ConsumptionPerKm { get; set; }

        public double DistanceTraveled { get; set; }

        public bool IsFuelEnough(double distance)
        {
            if (this.FuelAmount / this.ConsumptionPerKm >= distance)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{this.Model} {this.FuelAmount:F2} {this.DistanceTraveled}";
        }
    }
}
