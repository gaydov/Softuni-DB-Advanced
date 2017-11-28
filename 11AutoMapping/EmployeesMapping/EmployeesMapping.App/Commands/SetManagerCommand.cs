using Employees.Services.Interfaces;
using EmployeesMapping.App.Commands.Interfaces;

namespace EmployeesMapping.App.Commands
{
    public class SetManagerCommand : ICommand
    {
        private readonly IEmployeeService empService;

        public SetManagerCommand(IEmployeeService empService)
        {
            this.empService = empService;
        }

        public string Execute(params string[] arguments)
        {
            int employeeId = int.Parse(arguments[0]);
            int managerId = int.Parse(arguments[1]);

            this.empService.SetEmployeeManager(employeeId, managerId);

            return $"Employee with ID {employeeId}'s manager is now employee with ID {managerId}.";
        }
    }
}