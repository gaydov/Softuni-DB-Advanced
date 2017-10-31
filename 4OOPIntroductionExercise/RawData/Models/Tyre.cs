namespace RawData.Models
{
    public class Tyre
    {
        private double pressure;
        private int age;

        public Tyre(double pressure, int age)
        {
            this.pressure = pressure;
            this.age = age;
        }

        public double Pressure
        {
            get { return this.pressure; }
            private set { this.pressure = value; }
        }
    }
}