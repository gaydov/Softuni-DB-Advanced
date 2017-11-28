using System;
using EmployeesMapping.App.Commands.Interfaces;

namespace EmployeesMapping.App.Commands
{
    public class ExitCommand : ICommand
    {
        public string Execute(params string[] arguments)
        {
            Environment.Exit(0);
            return string.Empty;
        }
    }
}