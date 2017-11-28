using System;

namespace Employees.Services
{
    public static class Helpers
    {
        public static int CalcCurrentAge(DateTime birthday)
        {
            DateTime currentDate = DateTime.Now;

            int age = currentDate.Year - birthday.Year;

            if (birthday > currentDate.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}