using System.Linq;
using Employees.Services.Interfaces;
using EmployeesMapping.App.Commands.Interfaces;

namespace EmployeesMapping.App.Commands
{
    public class SetAddressCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public SetAddressCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] arguments)
        {
            int employeeId = int.Parse(arguments[0]);
            string[] addressElements = arguments.Skip(1).ToArray();
            string fullAddress = string.Join(" ", addressElements);

            this.employeeService.SetEmployeeAddress(employeeId, fullAddress);

            return $"The address of employee with ID {employeeId} was set to \"{fullAddress}\".";
        }
    }
}