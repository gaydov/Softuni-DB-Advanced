using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Core;
using P01_BillsPaymentSystem.Data.Constants;
using P01_BillsPaymentSystem.Data.Enums;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Data.CommandsModels
{
    public class PayBillsCommand : Command
    {
        public PayBillsCommand(BillsPaymentSystemContext context)
            : base(context)
        {
        }

        public override void Execute()
        {
            int userId = Helpers.TryIntParseInputString(PromptMessages.UserIdInput);

            if (!Helpers.DoesUserWithThisIdExist(userId, this.Context))
            {
                Console.WriteLine();
                Console.WriteLine(string.Format(ErrorMessages.UserNotFound, userId));
                Console.WriteLine();
                return;
            }

            decimal billsAmount = Helpers.TryDecimalParseInputString(PromptMessages.MoneyAmountInput);
            this.PayBills(userId, billsAmount);
        }

        private void PayBills(int userId, decimal amount)
        {
            PaymentMethod[] accountsMethods = this.Context.Users
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.BankAccount)
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.CreditCard)
                .FirstOrDefault(u => u.UserId == userId)
                .PaymentMethods
                .Where(pm => pm.Type == PaymentMethodType.BankAccount)
                .OrderBy(pm => pm.BankAccountId)
                .ToArray();

            PaymentMethod[] cardsMethods = this.Context.Users
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.BankAccount)
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.CreditCard)
                .FirstOrDefault(u => u.UserId == userId)
                .PaymentMethods
                .Where(pm => pm.Type == PaymentMethodType.CreditCard)
                .OrderBy(pm => pm.CreditCardId)
                .ToArray();

            decimal totalMoney = accountsMethods.Sum(pm => pm.BankAccount.Balance) + cardsMethods.Sum(c => c.CreditCard.LimitLeft);

            if (totalMoney >= amount)
            {
                foreach (PaymentMethod account in accountsMethods)
                {
                    decimal moneyToBeTaken = Math.Min(amount, account.BankAccount.Balance);
                    account.BankAccount.Withdraw(moneyToBeTaken);
                    amount -= moneyToBeTaken;

                    if (amount == 0)
                    {
                        break;
                    }
                }

                foreach (PaymentMethod card in cardsMethods)
                {
                    if (amount == 0)
                    {
                        break;
                    }

                    decimal moneyToBeTaken = Math.Min(amount, card.CreditCard.LimitLeft);
                    card.CreditCard.Withdraw(moneyToBeTaken);
                    amount -= moneyToBeTaken;

                    if (amount == 0)
                    {
                        break;
                    }
                }

                decimal moneyLeft = accountsMethods.Sum(pm => pm.BankAccount.Balance);
                decimal cardsLimitLeft = cardsMethods.Sum(c => c.CreditCard.LimitLeft);
                decimal cardsOwedMoney = cardsMethods.Sum(c => c.CreditCard.MoneyOwed);
                Console.WriteLine();
                Console.WriteLine(string.Format(InfoMessages.BillsPaid, moneyLeft));
                Console.WriteLine(string.Format(InfoMessages.CardsLimitLeft, cardsLimitLeft));
                Console.WriteLine(string.Format(InfoMessages.CardsOwedMoney, cardsOwedMoney));
                Console.WriteLine();
                this.Context.SaveChanges();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(ErrorMessages.NotEnoughMoney);
                Console.WriteLine();
            }
        }
    }
}