using System;
using System.Text;
using Mankind.Utilities;

namespace Mankind.Models
{
    public class Worker : Human
    {
        private decimal weekSalary;
        private double workHoursPerDay;

        public Worker(string firstName, string lastName, decimal weekSalary, double workHoursPerDay)
            : base(firstName, lastName)
        {
            this.WeekSalary = weekSalary;
            this.WorkHoursPerDay = workHoursPerDay;
        }

        public decimal WeekSalary
        {
            get
            {
                return this.weekSalary;
            }

            private set
            {
                if (value < Constants.WorkerMinSalary)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.WorkerInvalidSalary, nameof(this.weekSalary)));
                }

                this.weekSalary = value;
            }
        }

        public double WorkHoursPerDay
        {
            get
            {
                return this.workHoursPerDay;
            }

            private set
            {
                if (value < Constants.WorkerMinWorkingHours || value > Constants.WorkerMaxWorkingHours)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.WorkerInvalidWorkHours, nameof(this.workHoursPerDay)));
                }

                this.workHoursPerDay = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendLine($"Week Salary: {this.WeekSalary:F2}");
            sb.AppendLine($"Hours per day: {this.workHoursPerDay:F2}");
            sb.AppendLine($"Salary per hour: {this.CalcSalaryPerHour():F2}");

            return sb.ToString();
        }

        private decimal CalcSalaryPerHour()
        {
            decimal result = this.WeekSalary / (decimal)(Constants.WeekWorkingDaysCount * this.WorkHoursPerDay);
            return result;
        }
    }
}