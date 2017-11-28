using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Employees.Data;
using Employees.Models;
using Employees.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Employees.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeesContext context;

        public EmployeeService(EmployeesContext context)
        {
            this.context = context;
        }

        public void AddEmployee(EmployeeDto empDto)
        {
            Employee employee = Mapper.Map<Employee>(empDto);

            this.context.Employees.Add(employee);
            this.context.SaveChanges();
        }

        public EmployeeDto GetEmployeeById(int employeeId)
        {
            Employee employee = this.GetEmployeeByIdFromDb(employeeId);

            EmployeeDto empDto = Mapper.Map<EmployeeDto>(employee);

            return empDto;
        }

        public void SetEmployeeBirthday(int employeeId, DateTime birthday)
        {
            Employee employee = this.GetEmployeeByIdFromDb(employeeId);

            employee.Birthday = birthday;
            this.context.SaveChanges();
        }

        public void SetEmployeeAddress(int employeeId, string address)
        {
            Employee employee = this.GetEmployeeByIdFromDb(employeeId);

            employee.Address = address;
            this.context.SaveChanges();
        }

        public string GetEmployeeInfo(int employeeId)
        {
            EmployeeDto empDto = this.GetEmployeeById(employeeId);

            return $"ID: {empDto.Id} - {empDto.FirstName} {empDto.LastName} - ${empDto.Salary:F2}";
        }

        public string GetEmployeePersonalInfo(int employeeId)
        {
            EmployeeDto empDto = this.GetEmployeeById(employeeId);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ID: {empDto.Id} - {empDto.FirstName} {empDto.LastName} - ${empDto.Salary:F2}");

            string birthday = "[no birthday entered]";

            if (empDto.Birthday != null)
            {
                birthday = empDto.Birthday.Value.ToString("dd-MM-yyyy");
            }

            sb.AppendLine($"Birthday: {birthday}");

            string address = "[no address entered]";

            if (empDto.Address != null)
            {
                address = empDto.Address;
            }

            sb.Append($"Adress: {address}");

            return sb.ToString();
        }

        public void SetEmployeeManager(int employeeId, int managerId)
        {
            Employee employee = this.GetEmployeeByIdFromDb(employeeId);
            Employee manager = this.GetEmployeeByIdFromDb(managerId);

            if (manager.Manager != null && manager.Manager.Id == employee.Id)
            {
                throw new ArgumentException($"Employee with ID {employee.Id} is already manager of {manager.Id}!");
            }

            employee.Manager = manager;
            this.context.SaveChanges();
        }

        public string GetManagerInfo(int employeeId)
        {
            Employee employee = this.GetEmployeeByIdFromDb(employeeId);
            ManagerDto managerDto = Mapper.Map<ManagerDto>(employee);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{managerDto.FirstName} {managerDto.LastName} | Employees: {managerDto.SubordinatesCount}");

            foreach (EmployeeDto subordinate in managerDto.Subordinates)
            {
                sb.AppendLine($"    - {subordinate.FirstName} {subordinate.LastName} - ${subordinate.Salary:F2}");
            }

            return sb.ToString().Trim();
        }

        public IList<EmployeeDto> GetEmployeesOlderThan(int age)
        {
            List<EmployeeDto> employees = this.context
            .Employees
            .Include(e => e.Manager)
            .Where(e => e.Birthday != null && Helpers.CalcCurrentAge(e.Birthday.Value) > age)
            .OrderByDescending(e => e.Salary)
            .ProjectTo<EmployeeDto>()
            .ToList();

            return employees;
        }

        private Employee GetEmployeeByIdFromDb(int employeeId)
        {
            Employee employee = this.context
                .Employees
                .Include(e => e.Subordinates)
                .SingleOrDefault(e => e.Id == employeeId);

            if (employee == null)
            {
                throw new ArgumentException($"Employee with ID {employeeId} not found.");
            }

            return employee;
        }
    }
}
