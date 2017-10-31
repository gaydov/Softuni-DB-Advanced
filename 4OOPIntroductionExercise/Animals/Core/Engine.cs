using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Animals.Models;
using Animals.Utilities;

namespace Animals.Core
{
    public class Engine
    {
        private readonly IList<Animal> animals;

        public Engine()
        {
            this.animals = new List<Animal>();
        }

        public void Run()
        {
            this.ProcessInput();
            this.PrintOutput();
        }

        private void ProcessInput()
        {
            string input = Console.ReadLine();

            while (!input.Equals("Beast!"))
            {
                string animalType = input;
                string[] animalArgs = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string name = animalArgs[0];
                int age = int.Parse(animalArgs[1]);
                string gender = animalArgs[2];

                try
                {
                    Type animalClassType = Assembly.GetExecutingAssembly()
                        .GetTypes()
                        .FirstOrDefault(t => t.Name.Equals(animalType.Trim()));

                    if (animalClassType == null)
                    {
                        throw new ArgumentException(ErrorMessages.InvalidInput);
                    }

                    Animal currentAnimal = (Animal)Activator.CreateInstance(animalClassType, new object[] { name, age, gender });
                    this.animals.Add(currentAnimal);
                }
                catch (Exception e)
                {
                    if (e.InnerException != null)
                    {
                        Console.WriteLine(e.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                input = Console.ReadLine();
            }
        }

        private void PrintOutput()
        {
            foreach (Animal animal in this.animals)
            {
                Console.WriteLine(animal);
            }
        }
    }
}