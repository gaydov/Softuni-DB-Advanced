using System;
using System.Globalization;
using System.Text;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class CreditCard
    {
        public CreditCard()
        {
        }

        public CreditCard(decimal limit, DateTime expirationDate)
        {
            this.Limit = limit;
            this.ExpirationDate = expirationDate;
        }

        public int CreditCardId { get; set; }

        public decimal Limit { get; private set; }

        public decimal MoneyOwed { get; private set; }

        public decimal LimitLeft
        {
            get { return this.Limit - this.MoneyOwed; }
        }

        public DateTime ExpirationDate { get; set; }

        public int PaymentMethodId { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public void Withdraw(decimal amount)
        {
            this.MoneyOwed += amount;
        }

        public void Deposit(decimal amount)
        {
            this.MoneyOwed -= amount;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"-- ID: {this.CreditCardId}");
            sb.AppendLine($"--- Limit: {this.Limit:F2}");
            sb.AppendLine($"--- Money Owed: {this.MoneyOwed:F2}");
            sb.AppendLine($"--- Limit Left: {this.LimitLeft:F2}");
            sb.Append($"--- Expiration Date: {this.ExpirationDate.ToString("yyyy/MM", CultureInfo.InvariantCulture)}");

            return sb.ToString();
        }
    }
}