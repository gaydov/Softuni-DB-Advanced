using System;
using HospitalDbExtended.Core.IO;

namespace HospitalDbExtended.Utilities
{
    public static class Helpers
    {
        public static string IsNullOrEmptyValidator(string promptMessage)
        {
            ConsoleWriter.Write(promptMessage);
            string text = ConsoleReader.ReadLine();

            while (string.IsNullOrWhiteSpace(text))
            {
                ConsoleWriter.WriteLine("No text entered. Please enter a valid text.");
                ConsoleWriter.Write(Environment.NewLine);
                ConsoleWriter.Write(promptMessage);
                text = ConsoleReader.ReadLine();
            }

            return text;
        }

        public static string EnterHiddenPassword()
        {
            string password = string.Empty;

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            }

            return password;
        }

        public static bool ValidateBoolEntered(string promptMessage)
        {
            ConsoleWriter.Write(promptMessage);
            string enteredString = ConsoleReader.ReadLine();
            while (!enteredString.ToLower().Equals("y") && !enteredString.ToLower().Equals("n"))
            {
                ConsoleWriter.WriteLine("Invalid input. Please enter \"Y\" or \"N\".");
                ConsoleWriter.Write(Environment.NewLine);
                ConsoleWriter.Write(promptMessage);
                enteredString = ConsoleReader.ReadLine();
            }

            bool result = enteredString.ToLower().Equals("y");
            return result;
        }
    }
}