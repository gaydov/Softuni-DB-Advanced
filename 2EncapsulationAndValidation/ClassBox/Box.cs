namespace ClassBox
{
    public class Box
    {
        private readonly double length;
        private readonly double width;
        private readonly double height;

        public Box(double length, double width, double height)
        {
            this.length = length;
            this.width = width;
            this.height = height;
        }

        public double CalcSurfaceArea() 
        {
            return (2 * this.length * this.width) + (2 * this.length * this.height) + (2 * this.width * this.height);
        }

        public double CalcLaterialSurfaceArea()
        {
            return (2 * this.length * this.height) + (2 * this.width * this.height);
        }

        public double CalcVolume()
        {
            return this.length * this.width * this.height;
        }
    }
}