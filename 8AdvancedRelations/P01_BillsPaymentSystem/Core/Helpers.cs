using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Constants;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Core
{
    public static class Helpers
    {
        public static string IsNullOrEmptyValidator(string promptMessage)
        {
            Console.Write(promptMessage);
            string inputString = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(inputString))
            {
                Console.WriteLine(ErrorMessages.EmptyInputString);
                Console.Write(Environment.NewLine);
                Console.Write(promptMessage);
                inputString = Console.ReadLine();
            }

            return inputString;
        }

        public static int TryIntParseInputString(string promptMessage)
        {
            bool isEnteredValueInt = int.TryParse(IsNullOrEmptyValidator(promptMessage), out int intResult);

            while (!isEnteredValueInt)
            {
                Console.WriteLine(ErrorMessages.InvalidIntegerInput);
                Console.Write(Environment.NewLine);
                isEnteredValueInt = int.TryParse(IsNullOrEmptyValidator(promptMessage), out intResult);
            }

            return intResult;
        }

        public static decimal TryDecimalParseInputString(string promptMessage)
        {
            bool isEnteredValueDecimal = decimal.TryParse(IsNullOrEmptyValidator(promptMessage), out decimal decimalResult);

            while (!isEnteredValueDecimal)
            {
                Console.WriteLine(ErrorMessages.InvalidDecimalInput);
                Console.Write(Environment.NewLine);
                isEnteredValueDecimal = decimal.TryParse(IsNullOrEmptyValidator(promptMessage), out decimalResult);
            }

            return decimalResult;
        }

        public static bool DoesUserWithThisIdExist(int userId, BillsPaymentSystemContext context)
        {
            User user = context.Users
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.BankAccount)
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.CreditCard)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}