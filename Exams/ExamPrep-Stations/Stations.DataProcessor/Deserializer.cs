using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stations.Data;
using Stations.DataProcessor.Dto.Import;
using Stations.Models;
using Stations.Models.Enums;

namespace Stations.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportStations(StationsDbContext context, string jsonString)
        {
            StationDto[] stationsFromJson = ImportFromJson<StationDto>(jsonString, false);
            StringBuilder sb = new StringBuilder();
            List<Station> resultStations = new List<Station>();

            foreach (StationDto stationDto in stationsFromJson)
            {
                if (!IsValid(stationDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (resultStations.Any(s => s.Name == stationDto.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (stationDto.Town == null)
                {
                    stationDto.Town = stationDto.Name;
                }

                Station currentStation = new Station
                {
                    Name = stationDto.Name,
                    Town = stationDto.Town
                };

                resultStations.Add(currentStation);
                sb.AppendLine(string.Format(SuccessMessage, currentStation.Name));
            }

            context.Stations.AddRange(resultStations);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        public static string ImportClasses(StationsDbContext context, string jsonString)
        {
            SeatingClass[] classesFromJson = ImportFromJson<SeatingClass>(jsonString, false);
            StringBuilder sb = new StringBuilder();
            List<SeatingClass> resultClasses = new List<SeatingClass>();

            foreach (SeatingClass seatingClass in classesFromJson)
            {
                if (!IsValid(seatingClass))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (resultClasses.Any(c => c.Name == seatingClass.Name || c.Abbreviation == seatingClass.Abbreviation))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                resultClasses.Add(seatingClass);
                sb.AppendLine(string.Format(SuccessMessage, seatingClass.Name));
            }

            context.SeatingClasses.AddRange(resultClasses);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        public static string ImportTrains(StationsDbContext context, string jsonString)
        {
            TrainDto[] trainsFromJson = ImportFromJson<TrainDto>(jsonString, true);
            StringBuilder sb = new StringBuilder();
            List<Train> resultTrains = new List<Train>();

            foreach (TrainDto trainDto in trainsFromJson)
            {
                if (!IsValid(trainDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool trainAlreadyExists = resultTrains.Any(t => t.TrainNumber == trainDto.TrainNumber);
                if (trainAlreadyExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool trainSeatsAreValid = trainDto.Seats.All(IsValid);
                if (!trainSeatsAreValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool seatClassesAreValid = trainDto.Seats.All(s =>
                    context.SeatingClasses.Any(sc => sc.Name == s.Name && sc.Abbreviation == s.Abbreviation));
                if (!seatClassesAreValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                TrainType type = Enum.Parse<TrainType>(trainDto.Type);

                TrainSeat[] trainSeats = trainDto.Seats.Select(s => new TrainSeat
                {
                    SeatingClass =
                            context.SeatingClasses.SingleOrDefault(sc =>
                                sc.Name == s.Name && sc.Abbreviation == s.Abbreviation),
                    Quantity = s.Quantity.Value
                })
                .ToArray();

                Train currentTrain = new Train
                {
                    TrainNumber = trainDto.TrainNumber,
                    Type = type,
                    TrainSeats = trainSeats
                };

                resultTrains.Add(currentTrain);
                sb.AppendLine(string.Format(SuccessMessage, currentTrain.TrainNumber));
            }

            context.Trains.AddRange(resultTrains);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        public static string ImportTrips(StationsDbContext context, string jsonString)
        {
            TripDto[] tripsFromJson = ImportFromJson<TripDto>(jsonString, true);
            StringBuilder sb = new StringBuilder();
            List<Trip> resultTrips = new List<Trip>();

            foreach (TripDto tripDto in tripsFromJson)
            {
                if (!IsValid(tripDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Train train = context.Trains.SingleOrDefault(t => t.TrainNumber == tripDto.Train);
                Station originStation = context.Stations.SingleOrDefault(s => s.Name == tripDto.OriginStation);
                Station destinationStation = context.Stations.SingleOrDefault(s => s.Name == tripDto.DestinationStation);

                if (train == null || originStation == null || destinationStation == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                DateTime departureTime = DateTime.ParseExact(tripDto.DepartureTime, "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture);
                DateTime arrivalTime = DateTime.ParseExact(tripDto.ArrivalTime, "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture);

                if (departureTime > arrivalTime)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                TripStatus status = Enum.Parse<TripStatus>(tripDto.Status);

                TimeSpan? timeDifference = null;
                if (tripDto.TimeDifference != null)
                {
                    timeDifference =
                        TimeSpan.ParseExact(tripDto.TimeDifference, @"hh\:mm", CultureInfo.InvariantCulture);
                }

                Trip currentTrip = new Trip
                {
                    ArrivalTime = arrivalTime,
                    DepartureTime = departureTime,
                    Train = train,
                    Status = status,
                    OriginStation = originStation,
                    DestinationStation = destinationStation,
                    TimeDifference = timeDifference
                };

                resultTrips.Add(currentTrip);
                sb.AppendLine($"Trip from {currentTrip.OriginStation.Name} to {currentTrip.DestinationStation.Name} imported.");
            }

            context.Trips.AddRange(resultTrips);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        public static string ImportCards(StationsDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CardDto[]), new XmlRootAttribute("Cards"));
            CardDto[] cardsFromXml = (CardDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();
            List<CustomerCard> resultCards = new List<CustomerCard>();

            foreach (CardDto cardDto in cardsFromXml)
            {
                if (!IsValid(cardDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                CardType type = Enum.Parse<CardType>(cardDto.CardType);

                CustomerCard currentCard = new CustomerCard
                {
                    Name = cardDto.Name,
                    Age = cardDto.Age,
                    Type = type
                };

                resultCards.Add(currentCard);
                sb.AppendLine(string.Format(SuccessMessage, currentCard.Name));
            }

            context.Cards.AddRange(resultCards);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        public static string ImportTickets(StationsDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TicketDto[]), new XmlRootAttribute("Tickets"));
            TicketDto[] ticketsFromXml = (TicketDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();
            List<Ticket> resultTickets = new List<Ticket>();

            foreach (TicketDto ticketDto in ticketsFromXml)
            {
                if (!IsValid(ticketDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                DateTime departureTime =
                    DateTime.ParseExact(ticketDto.Trip.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                Trip trip = context.Trips
                    .Include(t => t.OriginStation)
                    .Include(t => t.DestinationStation)
                    .Include(t => t.Train)
                    .ThenInclude(tr => tr.TrainSeats)
                    .SingleOrDefault(t => t.OriginStation.Name == ticketDto.Trip.OriginStation &&
                                          t.DestinationStation.Name == ticketDto.Trip.DestinationStation &&
                                          t.DepartureTime == departureTime);

                if (trip == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                CustomerCard card = null;
                if (ticketDto.Card != null)
                {
                    card = context.Cards.SingleOrDefault(c => c.Name == ticketDto.Card.Name);

                    if (card == null)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }
                }

                string seatingClassAbbreviation = ticketDto.Seat.Substring(0, 2);
                int seatingClassNumber = int.Parse(ticketDto.Seat.Substring(2));

                bool seatClassExistsInTrain =
                    trip.Train.TrainSeats.Any(ts => ts.SeatingClass.Abbreviation == seatingClassAbbreviation);

                if (seatClassExistsInTrain)
                {
                    bool isSeatingNumberValid =
                         trip.Train
                        .TrainSeats
                        .Single(ts => ts.SeatingClass.Abbreviation == seatingClassAbbreviation)
                        .Quantity >= seatingClassNumber;

                    if (!isSeatingNumberValid)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }
                }
                else
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Ticket currentTicket = new Ticket
                {
                    Trip = trip,
                    CustomerCard = card,
                    Price = ticketDto.Price,
                    SeatingPlace = ticketDto.Seat
                };

                resultTickets.Add(currentTicket);
                sb.AppendLine($"Ticket from {trip.OriginStation.Name} to {trip.DestinationStation.Name} departing at {trip.DepartureTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} imported.");
            }

            context.Tickets.AddRange(resultTickets);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        private static T[] ImportFromJson<T>(string jsonString, bool ignoreNullValues)
        {
            T[] deserializedObjects;

            if (ignoreNullValues)
            {
                deserializedObjects = JsonConvert.DeserializeObject<T[]>(jsonString, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            else
            {
                deserializedObjects = JsonConvert.DeserializeObject<T[]>(jsonString);
            }

            return deserializedObjects;
        }

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }
    }
}