using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Core;
using P01_BillsPaymentSystem.Data.Constants;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Data.CommandsModels
{
    public class ListPaymentMethodsCommand : Command
    {
        public ListPaymentMethodsCommand(BillsPaymentSystemContext context)
            : base(context)
        {
        }

        public override void Execute()
        {
            int userId = Helpers.TryIntParseInputString(PromptMessages.UserIdInput);
            Console.WriteLine();

            if (Helpers.DoesUserWithThisIdExist(userId, this.Context))
            {
                User user = this.Context.Users
                    .Include(u => u.PaymentMethods)
                    .ThenInclude(pm => pm.BankAccount)
                    .Include(u => u.PaymentMethods)
                    .ThenInclude(pm => pm.CreditCard)
                    .FirstOrDefault(u => u.UserId == userId);

                Console.WriteLine(user.ToString());
            }
            else
            {
                Console.WriteLine(string.Format(ErrorMessages.UserNotFound, userId));
                Console.WriteLine();
            }
        }
    }
}