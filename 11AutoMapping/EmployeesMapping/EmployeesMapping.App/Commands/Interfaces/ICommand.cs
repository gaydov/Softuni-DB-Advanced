namespace EmployeesMapping.App.Commands.Interfaces
{
    public interface ICommand
    {
        string Execute(params string[] arguments);
    }
}