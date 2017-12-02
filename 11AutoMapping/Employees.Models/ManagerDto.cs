using System.Collections.Generic;

namespace Employees.Models
{
    public class ManagerDto
    {
        public ManagerDto()
        {
            this.Subordinates = new HashSet<EmployeeDto>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int SubordinatesCount
        {
            get { return this.Subordinates.Count; }
        }

        public ICollection<EmployeeDto> Subordinates { get; set; }
    }
}