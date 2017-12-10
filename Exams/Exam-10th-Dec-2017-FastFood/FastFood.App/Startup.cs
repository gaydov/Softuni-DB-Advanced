using System;
using System.Data.SqlClient;
using System.IO;
using AutoMapper;
using FastFood.Data;
using FastFood.DataProcessor;
using Microsoft.EntityFrameworkCore;

namespace FastFood.App
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            FastFoodDbContext context = new FastFoodDbContext();

            ResetDatabase(context, false);

            Console.WriteLine("Database Reset.");

            Mapper.Initialize(cfg => cfg.AddProfile<FastFoodProfile>());

            ImportEntities(context);

            ExportEntities(context);

            BonusTask(context);
        }

        private static void ImportEntities(FastFoodDbContext context, string baseDir = @"..\Datasets\")
        {
            const string ExportDir = "./ImportResults/";

            string employees = Deserializer.ImportEmployees(context, File.ReadAllText(baseDir + "employees.json"));
            PrintAndExportEntityToFile(employees, ExportDir + "Employees.txt");

            string items = Deserializer.ImportItems(context, File.ReadAllText(baseDir + "items.json"));
            PrintAndExportEntityToFile(items, ExportDir + "Items.txt");

            string orders = Deserializer.ImportOrders(context, File.ReadAllText(baseDir + "orders.xml"));
            PrintAndExportEntityToFile(orders, ExportDir + "Orders.txt");
        }

        private static void ExportEntities(FastFoodDbContext context)
        {
            const string ExportDir = "./ImportResults/";

            string jsonOutput = Serializer.ExportOrdersByEmployee(context, "Avery Rush", "ToGo");
            Console.WriteLine(jsonOutput);
            File.WriteAllText(ExportDir + "OrdersByEmployee.json", jsonOutput);

            string xmlOutput = Serializer.ExportCategoryStatistics(context, "Chicken,Drinks,Toys");
            Console.WriteLine(xmlOutput);
            File.WriteAllText(ExportDir + "CategoryStatistics.xml", xmlOutput);
        }

        private static void BonusTask(FastFoodDbContext context)
        {
            string bonusOutput = Bonus.UpdatePrice(context, "Cheeseburger", 6.50m);
            Console.WriteLine(bonusOutput);
        }

        private static void PrintAndExportEntityToFile(string entityOutput, string outputPath)
        {
            Console.WriteLine(entityOutput);
            File.WriteAllText(outputPath, entityOutput.TrimEnd());
        }

        private static void ResetDatabase(FastFoodDbContext context, bool shouldDeleteDatabase)
        {
            if (shouldDeleteDatabase)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            context.Database.EnsureCreated();

            string disableIntegrityChecksQuery = "EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(disableIntegrityChecksQuery);

            string deleteRowsQuery = "EXEC sp_MSforeachtable @command1='DELETE FROM ?'";
            context.Database.ExecuteSqlCommand(deleteRowsQuery);

            string enableIntegrityChecksQuery = "EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(enableIntegrityChecksQuery);

            string reseedQuery = "EXEC sp_MSforeachtable @command1='DBCC CHECKIDENT(''?'', RESEED, 0)'";
            try
            {
                context.Database.ExecuteSqlCommand(reseedQuery);
            }
            catch (SqlException)
            {
                // OrderItems table has no identity column, which isn't a problem
            }
        }
    }
}