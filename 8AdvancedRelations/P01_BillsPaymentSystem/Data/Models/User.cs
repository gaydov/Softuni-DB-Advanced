using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data.Enums;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class User
    {
        public User()
        {
            this.PaymentMethods = new HashSet<PaymentMethod>();
        }

        public User(string firstName, string lastName, string email, string password)
            : this()
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Password = password;
        }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"User: {this.FirstName} {this.LastName}");
            sb.AppendLine("Bank Accounts:");

            BillsPaymentSystemContext context = new BillsPaymentSystemContext();
            using (context)
            {
                foreach (PaymentMethod paymentMethod in context.PaymentMethods.Include(p => p.BankAccount).Include(p => p.User).Where(p => p.UserId == this.UserId && p.Type == PaymentMethodType.BankAccount).ToArray())
                {
                    sb.AppendLine(paymentMethod.ToString());
                }

                sb.AppendLine("Credit Cards:");
                foreach (PaymentMethod paymentMethod in context.PaymentMethods.Include(p => p.CreditCard).Include(p => p.User).Where(p => p.UserId == this.UserId && p.Type == PaymentMethodType.CreditCard).ToArray())
                {
                    sb.AppendLine(paymentMethod.ToString());
                }
            }

            return sb.ToString();
        }
    }
}