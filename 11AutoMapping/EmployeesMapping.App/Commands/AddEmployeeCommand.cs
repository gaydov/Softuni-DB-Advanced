using Employees.Models;
using Employees.Services.Interfaces;
using EmployeesMapping.App.Commands.Interfaces;

namespace EmployeesMapping.App.Commands
{
    public class AddEmployeeCommand : ICommand
    {
        private readonly IEmployeeService empService;

        public AddEmployeeCommand(IEmployeeService empService)
        {
            this.empService = empService;
        }

        public string Execute(params string[] arguments)
        {
            string firstName = arguments[0];
            string lastName = arguments[1];
            decimal salary = decimal.Parse(arguments[2]);

            EmployeeDto empDto = new EmployeeDto(firstName, lastName, salary);
            this.empService.AddEmployee(empDto);

            return $"Employee \"{firstName} {lastName}\" added successfully.";
        }
    }
}