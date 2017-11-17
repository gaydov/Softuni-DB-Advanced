using P01_BillsPaymentSystem.Data.Enums;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
        }

        public PaymentMethod(PaymentMethodType type, User user, BankAccount account, CreditCard creditCard)
        {
            this.Type = type;
            this.User = user;
            this.BankAccount = account;
            this.CreditCard = creditCard;
        }

        public int Id { get; set; }

        public PaymentMethodType Type { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int? BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }

        public int? CreditCardId { get; set; }

        public CreditCard CreditCard { get; set; }

        public override string ToString()
        {
            switch (this.Type)
            {
                case PaymentMethodType.BankAccount:
                    return this.BankAccount.ToString();

                case PaymentMethodType.CreditCard:
                    return this.CreditCard.ToString();
            }

            return string.Empty;
        }
    }
}