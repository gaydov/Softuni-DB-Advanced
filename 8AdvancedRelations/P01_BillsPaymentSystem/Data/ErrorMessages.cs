using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace P01_BillsPaymentSystem.Data
{
    public static class ErrorMessages
    {
        public const string InvalidOptionSelected = "Invalid option.";
        public const string UserNotFound = "User with id {0} not found!";
        public const string NotEnoughMoney = "Insufficient funds!";
    }
}