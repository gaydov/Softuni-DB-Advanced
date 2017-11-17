using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Enums;
using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Core
{
    public class Engine
    {
        private readonly BillsPaymentSystemContext context;

        public Engine(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public void Run()
        {
            this.context.Database.Migrate();
            this.Seed();

            this.StartOptionsMenu();
        }

        private void Seed()
        {
            User firstUser = new User("Georgi", "Dimitrov", "gd@abv.bg", "pass1");
            User secondUser = new User("Dimo", "Georgiev", "dimo@abv.bg", "pass2");
            this.context.Users.Add(firstUser);
            this.context.Users.Add(secondUser);

            CreditCard firstCard = new CreditCard(1000, DateTime.ParseExact("10-01-2017", "dd-MM-yyyy", null));
            CreditCard secondCard = new CreditCard(2000, DateTime.ParseExact("15-02-2016", "dd-MM-yyyy", null));
            this.context.CreditCards.Add(firstCard);
            this.context.CreditCards.Add(secondCard);

            BankAccount dskBankAccount = new BankAccount(1500, "DSK", "SWIFTDSK");
            BankAccount uncrBankAccount = new BankAccount(2500, "UNICREDIT", "UNCR");
            this.context.BankAccounts.Add(dskBankAccount);
            this.context.BankAccounts.Add(uncrBankAccount);

            PaymentMethod firstUserFirstBankAccountMethod = new PaymentMethod(PaymentMethodType.BankAccount, firstUser, dskBankAccount, null);
            PaymentMethod firstUserSecondBankAccountMethod = new PaymentMethod(PaymentMethodType.BankAccount, firstUser, uncrBankAccount, null);

            PaymentMethod firstUserFirstCreditCardMethod = new PaymentMethod(PaymentMethodType.CreditCard, firstUser, null, firstCard);
            PaymentMethod firstUserSecondCreditCardMethod = new PaymentMethod(PaymentMethodType.CreditCard, firstUser, null, secondCard);

            this.context.PaymentMethods.Add(firstUserFirstBankAccountMethod);
            this.context.PaymentMethods.Add(firstUserSecondBankAccountMethod);

            this.context.PaymentMethods.Add(firstUserFirstCreditCardMethod);
            this.context.PaymentMethods.Add(firstUserSecondCreditCardMethod);

            this.context.SaveChanges();
        }

        private void StartOptionsMenu()
        {
            Console.WriteLine(InfoMessages.ListUserMethodsOption);
            Console.WriteLine(InfoMessages.PayBillsOption);
            Console.Write(PromptMessages.ChooseOption);
            int optionId = int.Parse(Console.ReadLine());

            switch (optionId)
            {
                case 1:
                    this.ListUserPaymentMethods();
                    break;

                case 2:
                    Console.Write(PromptMessages.UserIdInput);
                    int userId = int.Parse(Console.ReadLine());
                    Console.Write(PromptMessages.MoneyAmountInput);
                    decimal billsAmount = int.Parse(Console.ReadLine());
                    this.PayBills(userId, billsAmount);
                    break;

                default:
                    Console.WriteLine(ErrorMessages.InvalidOptionSelected);
                    break;
            }
        }
        
        private void ListUserPaymentMethods()
        {
            Console.Write(PromptMessages.UserIdInput);
            int userId = int.Parse(Console.ReadLine());

            User user = this.context.Users
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.BankAccount)
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.CreditCard)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                Console.WriteLine(string.Format(ErrorMessages.UserNotFound, userId));
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(user.ToString());
            }
        }

        private void PayBills(int userId, decimal amount)
        {
            User searchedUser = this.context.Users.Find(userId);

            if (searchedUser == null)
            {
                Console.WriteLine(string.Format(ErrorMessages.UserNotFound, userId));
                Console.WriteLine();
                return;
            }

            PaymentMethod[] accountsMethods = this.context.Users
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.BankAccount)
                .Include(u => u.PaymentMethods)
                .ThenInclude(pm => pm.CreditCard)
                .FirstOrDefault(u => u.UserId == userId)
                .PaymentMethods
                .Where(pm => pm.Type == PaymentMethodType.BankAccount)
                .OrderBy(pm => pm.BankAccountId)
                .ToArray();

            PaymentMethod[] cardsMethods = this.context.Users
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

                this.context.SaveChanges();
                decimal moneyLeft = accountsMethods.Sum(pm => pm.BankAccount.Balance) + cardsMethods.Sum(c => c.CreditCard.Limit);
                Console.WriteLine(string.Format(InfoMessages.BillsPaid, moneyLeft));
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(ErrorMessages.NotEnoughMoney);
                Console.WriteLine();
            }
        }
    }
}