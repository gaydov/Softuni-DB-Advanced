using System.Collections.Generic;
using RawData.Enums;
using RawData.Models;

namespace RawData.CommandsModels
{
    public abstract class Command
    {
        private readonly CargoType cargoType;
        private readonly IList<Car> cars;

        protected Command(CargoType cargoType, IList<Car> cars)
        {
            this.cargoType = cargoType;
            this.cars = cars;
        }

        protected CargoType CargoType
        {
            get
            {
                return this.cargoType;
            }
        }

        protected IList<Car> Cars
        {
            get
            {
                return this.cars;
            }
        }

        public abstract string GetCarsModels();
    }
}