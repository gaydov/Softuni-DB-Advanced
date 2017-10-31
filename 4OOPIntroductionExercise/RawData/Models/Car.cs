using System.Collections.Generic;

namespace RawData.Models
{
    public class Car
    {
        private string model;
        private Engine engine;
        private Cargo cargo;
        private IList<Tyre> tyres;

        public Car(string model, Engine engine, Cargo cargo, IList<Tyre> tyres)
        {
            this.model = model;
            this.engine = engine;
            this.cargo = cargo;
            this.tyres = tyres;
        }

        public string Model
        {
            get { return this.model; }
            private set { this.model = value; }
        }

        public Engine Engine
        {
            get { return this.engine; }
            private set { this.engine = value; }
        }

        public Cargo Cargo
        {
            get { return this.cargo; }
            private set { this.cargo = value; }
        }

        public IList<Tyre> Tyres
        {
            get { return this.tyres; }
            private set { this.tyres = value; }
        }
    }
}