using System.Collections.Generic;
using System.Linq;
using System.Text;
using RawData.Enums;
using RawData.Models;

namespace RawData.CommandsModels
{
    public class FlammableCargosCommand : Command
    {
        private const int EnginePowerConst = 250;
        private static CargoType cargoTypeConst = CargoType.flammable;

        public FlammableCargosCommand(IList<Car> cars)
            : base(cargoTypeConst, cars)
        {
        }

        public override string GetCarsModels()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Car car in this.Cars.Where(c => c.Cargo.Type.Equals(this.CargoType) && c.Engine.Power > EnginePowerConst))
            {
                sb.AppendLine(car.Model);
            }

            return sb.ToString();
        }
    }
}