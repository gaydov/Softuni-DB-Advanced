using System;
using System.Globalization;
using HospitalDbExtended.Core.IO;

namespace HospitalDbExtended.Utilities
{
    public static class Helpers
    {
        public static string IsNullOrEmptyValidator(string promptMessage)
        {
            ConsoleWriter.Write(promptMessage);
            string inputString = ConsoleReader.ReadLine();

            while (string.IsNullOrWhiteSpace(inputString))
            {
                ConsoleWriter.WriteLine(ErrorMessages.EmptyInputString);
                ConsoleWriter.Write(Environment.NewLine);
                ConsoleWriter.Write(promptMessage);
                inputString = ConsoleReader.ReadLine();
            }

            return inputString;
        }

        public static string EnterPasswordHidden()
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
            string inputString = ConsoleReader.ReadLine();

            while (!inputString.ToLower().Equals("y") && !inputString.ToLower().Equals("n"))
            {
                ConsoleWriter.WriteLine(ErrorMessages.InvalidBoolInput);
                ConsoleWriter.Write(Environment.NewLine);
                ConsoleWriter.Write(promptMessage);
                inputString = ConsoleReader.ReadLine();
            }

            bool result = inputString.ToLower().Equals("y");
            return result;
        }

        public static int TryIntParseInputString(string promptMessage)
        {
            bool isEnteredValueInt = int.TryParse(IsNullOrEmptyValidator(promptMessage), out int intResult);

            while (!isEnteredValueInt)
            {
                ConsoleWriter.Write(Environment.NewLine);
                ConsoleWriter.WriteLine(ErrorMessages.InvalidIntegerInput);
                isEnteredValueInt = int.TryParse(IsNullOrEmptyValidator(promptMessage), out intResult);
            }

            return intResult;
        }

        public static void PrintCollection<T>(T[] items)
        {
            string collectionName = typeof(T).Name.ToLower() + "s";

            if (items.Length == 0)
            {
                ConsoleWriter.WriteLine(string.Format(InfoMessages.ExtractedDoctorCollectionEmpty, collectionName));
                ConsoleWriter.Write(Environment.NewLine);
            }
            else
            {
                ConsoleWriter.WriteLine(string.Format(InfoMessages.ExtractedEntityCollectionIndicator, collectionName));
                ConsoleWriter.Write(Environment.NewLine);

                foreach (T item in items)
                {
                    ConsoleWriter.WriteLine($"{item}");
                    ConsoleWriter.Write(Environment.NewLine);
                }
            }
        }

        public static DateTime TryParseDateInCertainFormat(string format)
        {
            string dateString = IsNullOrEmptyValidator(PromptingMessages.VisitationDate);
            bool isEnteredValueDatetime = DateTime.TryParseExact(dateString, format, null, DateTimeStyles.None, out DateTime date);

            while (!isEnteredValueDatetime)
            {
                ConsoleWriter.Write(Environment.NewLine);
                ConsoleWriter.WriteLine(string.Format(ErrorMessages.InvalidFormattedDateInput, format));
                dateString = IsNullOrEmptyValidator(PromptingMessages.VisitationDate);
                isEnteredValueDatetime = DateTime.TryParseExact(dateString, "dd/MM/yyyy HH:mm", null, DateTimeStyles.None, out date);
            }

            return date;
        }
    }
}