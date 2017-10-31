namespace RawData.Models
{
    public class Engine
    {
        private int speed;
        private int power;

        public Engine(int speed, int power)
        {
            this.speed = speed;
            this.power = power;
        }

        public int Power
        {
            get { return this.power; }
            private set { this.power = value; }
        }
    }
}