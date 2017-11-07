using System;
using System.Collections.Generic;
using System.Linq;
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
                const int ProjectToBeDeletedId = 2;

                Project projectToBeDeleted = db.Projects.Find(ProjectToBeDeletedId);
                List<EmployeeProject> employeeProjectsToBeDeleted = db.EmployeesProjects.Where(ep => ep.Project.ProjectId == ProjectToBeDeletedId).ToList();

                db.EmployeesProjects.RemoveRange(employeeProjectsToBeDeleted);
                db.SaveChanges();

                db.Remove(projectToBeDeleted);
                db.SaveChanges();

                foreach (string projectName in db.Projects.Select(p => p.Name).Take(10))
                {
                    Console.WriteLine(projectName);
                }
            }
        }
    }
}