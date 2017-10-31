using System.Collections.Generic;
using System.Linq;
using System.Text;
using RawData.Enums;
using RawData.Models;

namespace RawData.CommandsModels
{
    public class FragileCargosCommand : Command
    {
        private const double TyrePressureConst = 1;
        private static CargoType cargoTypeConst = CargoType.fragile;

        public FragileCargosCommand(IList<Car> cars)
            : base(cargoTypeConst, cars)
        {
        }

        public override string GetCarsModels()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Car car in this.Cars.Where(c => c.Cargo.Type.Equals(this.CargoType) && c.Tyres.Any(t => t.Pressure < TyrePressureConst)))
            {
                sb.AppendLine(car.Model);
            }

            return sb.ToString();
        }
    }
}