using Employees.Services.Interfaces;
using EmployeesMapping.App.Commands.Interfaces;

namespace EmployeesMapping.App.Commands
{
    public class EmployeeInfoCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public EmployeeInfoCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] arguments)
        {
            int employeeId = int.Parse(arguments[0]);

            string result = this.employeeService.GetEmployeeInfo(employeeId);
            return result;
        }
    }
}