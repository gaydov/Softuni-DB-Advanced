using System;
using Mankind.Models;

namespace Mankind
{
    public class Launcher
    {
        public static void Main()
        {
            string[] studentInfo = Console.ReadLine().Split();
            string studentFirstName = studentInfo[0];
            string studentLastName = studentInfo[1];
            string facNumber = studentInfo[2];

            string[] workerInfo = Console.ReadLine().Split();
            string workerFirstName = workerInfo[0];
            string workerLastName = workerInfo[1];
            decimal weeklySalary = decimal.Parse(workerInfo[2]);
            double weeklyWorkHours = double.Parse(workerInfo[3]);

            try
            {
                Student student = new Student(studentFirstName, studentLastName, facNumber);
                Worker worker = new Worker(workerFirstName, workerLastName, weeklySalary, weeklyWorkHours);

                Console.WriteLine(student);
                Console.WriteLine(worker);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }
        }
    }
}