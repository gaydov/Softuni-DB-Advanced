using System;
using System.Globalization;
using Employees.Services.Interfaces;
using EmployeesMapping.App.Commands.Interfaces;

namespace EmployeesMapping.App.Commands
{
    public class SetBirthdayCommand : ICommand
    {
        private const string DateFormat = "dd-MM-yyyy";
        private readonly IEmployeeService employeeService;

        public SetBirthdayCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] arguments)
        {
            int employeeId = int.Parse(arguments[0]);

            bool isDateInValidFormat = DateTime.TryParseExact(arguments[1], DateFormat, null, DateTimeStyles.None, out DateTime date);

            if (!isDateInValidFormat)
            {
                throw new ArgumentException($"Invalid date format. Please enter a date in format {DateFormat}.");    
            }

            this.employeeService.SetEmployeeBirthday(employeeId, date);

            return $"The birthday of employee with ID {employeeId} was set to {date.ToString(DateFormat)}.";
        }
    }
}