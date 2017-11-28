using System.Collections.Generic;
using System.Text;
using Employees.Models;
using Employees.Services.Interfaces;
using EmployeesMapping.App.Commands.Interfaces;

namespace EmployeesMapping.App.Commands
{
    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly IEmployeeService empService;

        public ListEmployeesOlderThanCommand(IEmployeeService empService)
        {
            this.empService = empService;
        }

        public string Execute(params string[] arguments)
        {
            int age = int.Parse(arguments[0]);

            IList<EmployeeDto> employees = this.empService.GetEmployeesOlderThan(age);

            if (employees.Count == 0)
            {
                return "No such employees found.";
            }

            StringBuilder sb = new StringBuilder();

            foreach (EmployeeDto employeeDto in employees)
            {
                string managerName = "[no manager]";

                if (employeeDto.Manager != null)
                {
                    managerName = employeeDto.Manager.LastName;
                }

                sb.AppendLine(
                    $"{employeeDto.FirstName} {employeeDto.LastName} - ${employeeDto.Salary:F2} - Manager: {managerName}");
            }

            return sb.ToString().Trim();
        }
    }
}