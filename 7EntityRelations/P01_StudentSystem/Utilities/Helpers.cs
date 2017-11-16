using System;
using System.Globalization;

namespace P01_StudentSystem.Utilities
{
    public static class Helpers
    {
        public static string IsNullOrEmptyValidator(string promptMessage)
        {
            Console.Write(promptMessage);
            string inputString = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(inputString))
            {
                Console.WriteLine("No text entered. Please enter a valid text.");
                Console.Write(Environment.NewLine);
                Console.Write(promptMessage);
                inputString = Console.ReadLine();
            }

            return inputString;
        }

        public static DateTime TryParseDateInCertainFormat(string format, string promptMessage)
        {
            string dateString = IsNullOrEmptyValidator(promptMessage);
            bool isEnteredValueDatetime = DateTime.TryParseExact(dateString, format, null, DateTimeStyles.None, out DateTime date);

            while (!isEnteredValueDatetime)
            {
                Console.WriteLine("Invalid date entered. Please enter date in format {0}.", format);
                Console.Write(Environment.NewLine);
                dateString = IsNullOrEmptyValidator(promptMessage);
                isEnteredValueDatetime = DateTime.TryParseExact(dateString, format, null, DateTimeStyles.None, out date);
            }

            return date;
        }

        public static bool ValidateBoolEntered(string promptMessage)
        {
            Console.Write(promptMessage);
            string inputString = Console.ReadLine();

            while (!inputString.ToLower().Equals("y") && !inputString.ToLower().Equals("n"))
            {
                Console.WriteLine("Invalid input. Please enter \"Y\" or \"N\".");
                Console.Write(Environment.NewLine);
                Console.Write(promptMessage);
                inputString = Console.ReadLine();
            }

            bool result = inputString.ToLower().Equals("y");
            return result;
        }
    }
}