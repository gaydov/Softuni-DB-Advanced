using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace DeleteProjectById
{
    public class Launcher
    {
        public static void Main()
        {
            SoftUniContext db = new SoftUniContext();

            using (db)
            {
                Project projectToBeDeleted = db.Projects.Find(2);

                List<Employee> employeesWorkingOnTheDeletedProject = db.Employees
                    .Include(e => e.EmployeesProjects)
                    .ThenInclude(ep => ep.Project)
                    .ToList();

                foreach (Employee emp in employeesWorkingOnTheDeletedProject)
                {
                    foreach (EmployeeProject ep in emp.EmployeesProjects.ToList())
                    {
                        if (ep.Project.Equals(projectToBeDeleted))
                        {
                            emp.EmployeesProjects.Remove(ep);
                        }
                    }
                }

                db.Projects.Remove(projectToBeDeleted);
                db.SaveChanges();

                foreach (Project project in db.Projects.Take(10))
                {
                    Console.WriteLine(project.Name);
                }
            }
        }
    }
}