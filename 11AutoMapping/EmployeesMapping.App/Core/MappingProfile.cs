using AutoMapper;
using Employees.Models;

namespace EmployeesMapping.App.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Employee, EmployeeDto>();
            this.CreateMap<EmployeeDto, Employee>();
            this.CreateMap<Employee, ManagerDto>();
        }
    }
}