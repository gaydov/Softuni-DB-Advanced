using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRacing
{
    public class Launcher
    {
        public static void Main()
        {
            int carsCount = int.Parse(Console.ReadLine());
            IList<Car> cars = new List<Car>();

            for (int i = 0; i < carsCount; i++)
            {
                string[] input = Console.ReadLine().Split();
                string currentCarModel = input[0];
                double currentCarFuelAmount = double.Parse(input[1]);
                double currentCarFuelConsumption = double.Parse(input[2]);

                Car currentCar = new Car(currentCarModel, currentCarFuelAmount, currentCarFuelConsumption);
                cars.Add(currentCar);
            }

            string command = Console.ReadLine();

            while (!command.Equals("End"))
            {
                string[] args = command.Split();
                string model = args[1];
                double distance = double.Parse(args[2]);

                Car currentCar = cars.FirstOrDefault(c => c.Model.Equals(model));

                if (currentCar.IsFuelEnough(distance))
                {
                    currentCar.FuelAmount -= distance * currentCar.ConsumptionPerKm;
                    currentCar.DistanceTraveled += distance;
                }
                else
                {
                    Console.WriteLine("Insufficient fuel for the drive");
                }

                command = Console.ReadLine();
            }

            foreach (Car car in cars)
            {
                Console.WriteLine(car);
            }
        }
    }
}