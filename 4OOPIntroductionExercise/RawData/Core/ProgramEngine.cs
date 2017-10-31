using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RawData.CommandsModels;
using RawData.Enums;
using RawData.Models;

namespace RawData.Core
{
    public class ProgramEngine
    {
        private const string CommandClassSuffix = "CargosCommand";
        private readonly IList<Car> cars;

        public ProgramEngine()
        {
            this.cars = new List<Car>();
        }

        public void Run()
        {
            this.ReadInputAndAddCars(this.cars);
            string commandName = Console.ReadLine();
            Command command = this.GenerateCommand(commandName, this.cars);
            Console.WriteLine(command.GetCarsModels());
        }

        private void ReadInputAndAddCars(IList<Car> carsCollection)
        {
            int carsCount = int.Parse(Console.ReadLine());

            for (int i = 0; i < carsCount; i++)
            {
                string[] carInfo = Console.ReadLine().Split();
                string model = carInfo[0];
                int engineSpeed = int.Parse(carInfo[1]);
                int enginePower = int.Parse(carInfo[2]);
                Models.Engine engine = new Models.Engine(engineSpeed, enginePower);

                int cargoWeight = int.Parse(carInfo[3]);
                CargoType cargoType = (CargoType)Enum.Parse(typeof(CargoType), carInfo[4]);
                Cargo cargo = new Cargo(cargoWeight, cargoType);

                IList<Tyre> tyres = new List<Tyre>();

                double firstTyrePressure = double.Parse(carInfo[5]);
                int firstTyreAge = int.Parse(carInfo[6]);
                Tyre firstTyre = new Tyre(firstTyrePressure, firstTyreAge);
                tyres.Add(firstTyre);

                double secondTyrePressure = double.Parse(carInfo[7]);
                int secondTyreAge = int.Parse(carInfo[8]);
                Tyre secondTyre = new Tyre(secondTyrePressure, secondTyreAge);
                tyres.Add(secondTyre);

                double thirdTyrePressure = double.Parse(carInfo[9]);
                int thirdTyreAge = int.Parse(carInfo[10]);
                Tyre thirdTyre = new Tyre(thirdTyrePressure, thirdTyreAge);
                tyres.Add(thirdTyre);

                double fourthTyrePressure = double.Parse(carInfo[11]);
                int fourthTyreAge = int.Parse(carInfo[12]);
                Tyre fourthTyre = new Tyre(fourthTyrePressure, fourthTyreAge);
                tyres.Add(fourthTyre);

                Car currentCar = new Car(model, engine, cargo, tyres);
                carsCollection.Add(currentCar);
            }
        }

        private Command GenerateCommand(string commandName, IList<Car> carsCollection)
        {
            string fullCommandName = char.ToUpper(commandName[0]) + commandName.Substring(1) + CommandClassSuffix;
            Type commandNameType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name.Equals(fullCommandName));

            Command command = (Command)Activator.CreateInstance(commandNameType, new object[] { carsCollection });
            return command;
        }
    }
}