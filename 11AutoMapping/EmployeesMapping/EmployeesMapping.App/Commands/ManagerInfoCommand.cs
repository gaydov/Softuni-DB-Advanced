using Employees.Services.Interfaces;
using EmployeesMapping.App.Commands.Interfaces;

namespace EmployeesMapping.App.Commands
{
    public class ManagerInfoCommand : ICommand
    {
        private readonly IEmployeeService empService;

        public ManagerInfoCommand(IEmployeeService empService)
        {
            this.empService = empService;
        }

        public string Execute(params string[] arguments)
        {
            int employeeId = int.Parse(arguments[0]);

            string result = this.empService.GetManagerInfo(employeeId);
            return result;
        }
    }
}