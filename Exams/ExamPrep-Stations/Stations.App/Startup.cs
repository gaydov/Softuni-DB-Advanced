using System;
using System.Data.SqlClient;
using System.IO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stations.Data;

namespace Stations.App
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            StationsDbContext context = new StationsDbContext();
            ResetDatabase(context, true);

            Console.WriteLine("Database Reset.");

            Mapper.Initialize(cfg => cfg.AddProfile<StationsProfile>());

            ImportEntities(context);
            ExportEntities(context);
        }

        private static void ImportEntities(StationsDbContext context, string baseDir = "../Datasets/")
        {
            const string ExportDir = "../ImportResults/";

            string stations = DataProcessor.Deserializer.ImportStations(context, File.ReadAllText(baseDir + "stations.json"));
            PrintAndExportEntityToFile(stations, ExportDir + "Stations.txt");

            string classes = DataProcessor.Deserializer.ImportClasses(context, File.ReadAllText(baseDir + "classes.json"));
            PrintAndExportEntityToFile(classes, ExportDir + "Classes.txt");

            string trains = DataProcessor.Deserializer.ImportTrains(context, File.ReadAllText(baseDir + "trains.json"));
            PrintAndExportEntityToFile(trains, ExportDir + "Trains.txt");

            string trips = DataProcessor.Deserializer.ImportTrips(context, File.ReadAllText(baseDir + "trips.json"));
            PrintAndExportEntityToFile(trips, ExportDir + "Trips.txt");

            string cards = DataProcessor.Deserializer.ImportCards(context, File.ReadAllText(baseDir + "cards.xml"));
            PrintAndExportEntityToFile(cards, ExportDir + "Cards.txt");

            string tickets = DataProcessor.Deserializer.ImportTickets(context, File.ReadAllText(baseDir + "tickets.xml"));
            PrintAndExportEntityToFile(tickets, ExportDir + "Tickets.txt");
        }

        private static void ExportEntities(StationsDbContext context)
        {
            const string ExportDir = "../ExportResults/";

            string jsonOutput = DataProcessor.Serializer.ExportDelayedTrains(context, "01/01/2017");
            Console.WriteLine(jsonOutput);
            File.WriteAllText(ExportDir + "DelayedTrains.json", jsonOutput);

            string xmlOutput = DataProcessor.Serializer.ExportCardsTicket(context, "Elder");
            Console.WriteLine(xmlOutput);
            File.WriteAllText(ExportDir + "CardsTicket.xml", xmlOutput);
        }

        private static void PrintAndExportEntityToFile(string entityOutput, string outputPath)
        {
            Console.WriteLine(entityOutput);
            File.WriteAllText(outputPath, entityOutput.TrimEnd());
        }

        private static void ResetDatabase(StationsDbContext context, bool shouldDeleteDatabase)
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