using System;
using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.CommandsModels;
using P01_BillsPaymentSystem.Data.Constants;
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
            this.context.Database.EnsureDeleted();
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
            this.context.SaveChanges();

            CreditCard firstCard = new CreditCard(1000, DateTime.ParseExact("10-01-2017", "dd-MM-yyyy", null));
            CreditCard secondCard = new CreditCard(2000, DateTime.ParseExact("15-02-2016", "dd-MM-yyyy", null));
            this.context.CreditCards.Add(firstCard);
            this.context.CreditCards.Add(secondCard);
            this.context.SaveChanges();

            BankAccount dskBankAccount = new BankAccount(1500, "DSK", "SWIFTDSK");
            BankAccount uncrBankAccount = new BankAccount(2500, "UNICREDIT", "UNCR");
            this.context.BankAccounts.Add(dskBankAccount);
            this.context.BankAccounts.Add(uncrBankAccount);
            this.context.SaveChanges();

            PaymentMethod firstUserFirstBankAccountMethod = new PaymentMethod(PaymentMethodType.BankAccount, firstUser, dskBankAccount, null);
            this.context.PaymentMethods.Add(firstUserFirstBankAccountMethod);
            this.context.SaveChanges();

            PaymentMethod firstUserSecondBankAccountMethod = new PaymentMethod(PaymentMethodType.BankAccount, firstUser, uncrBankAccount, null);
            this.context.PaymentMethods.Add(firstUserSecondBankAccountMethod);
            this.context.SaveChanges();

            PaymentMethod firstUserFirstCreditCardMethod = new PaymentMethod(PaymentMethodType.CreditCard, firstUser, null, firstCard);
            this.context.PaymentMethods.Add(firstUserFirstCreditCardMethod);
            this.context.SaveChanges();

            PaymentMethod firstUserSecondCreditCardMethod = new PaymentMethod(PaymentMethodType.CreditCard, firstUser, null, secondCard);
            this.context.PaymentMethods.Add(firstUserSecondCreditCardMethod);
            this.context.SaveChanges();
        }

        private void StartOptionsMenu()
        {
            while (true)
            {
                Console.WriteLine(InfoMessages.ListUserMethodsOption);
                Console.WriteLine(InfoMessages.PayBillsOption);
                Console.WriteLine(InfoMessages.ExitTheProgramOption);
                int optionId = Helpers.TryIntParseInputString(PromptMessages.ChooseOption);

                Command cmd = this.GenerateCommand(optionId);

                if (cmd == null)
                {
                    Console.WriteLine();
                    Console.WriteLine(ErrorMessages.InvalidOptionSelected);
                    Console.WriteLine();
                    continue;
                }

                cmd.Execute();
            }
        }

        private Command GenerateCommand(int optionId)
        {
            Command cmd = null;

            switch (optionId)
            {
                case 1:
                    cmd = new ListPaymentMethodsCommand(this.context);
                    break;

                case 2:
                    cmd = new PayBillsCommand(this.context);
                    break;

                case 3:
                    Environment.Exit(0);
                    break;

                default:
                    return null;
            }

            return cmd;
        }
    }
}