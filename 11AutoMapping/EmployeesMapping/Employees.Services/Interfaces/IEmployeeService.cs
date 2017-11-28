using System;
using System.Collections.Generic;
using Employees.Models;

namespace Employees.Services.Interfaces
{
    public interface IEmployeeService
    {
        EmployeeDto GetEmployeeById(int employeeId);

        void AddEmployee(EmployeeDto empDto);

        void SetEmployeeBirthday(int employeeId, DateTime birthday);

        void SetEmployeeAddress(int employeeId, string address);

        string GetEmployeeInfo(int employeeId);

        string GetEmployeePersonalInfo(int employeeId);

        void SetEmployeeManager(int employeeId, int managerId);

        string GetManagerInfo(int employeeId);

        IList<EmployeeDto> GetEmployeesOlderThan(int age);
    }
}