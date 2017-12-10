using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            EmployeeDto[] employeesFromJson = ImportFromJson<EmployeeDto>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Employee> resultEmployees = new List<Employee>();

            foreach (EmployeeDto employeeDto in employeesFromJson)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Position position = context.Positions.SingleOrDefault(p => p.Name == employeeDto.Position);

                if (position == null)
                {
                    position = new Position
                    {
                        Name = employeeDto.Position
                    };

                    context.Positions.Add(position);
                    context.SaveChanges();
                }

                Employee currentEmployee = new Employee
                {
                    Position = context.Positions.SingleOrDefault(p => p.Name == employeeDto.Position),
                    Name = employeeDto.Name,
                    Age = employeeDto.Age
                };

                resultEmployees.Add(currentEmployee);
                sb.AppendLine(string.Format(SuccessMessage, currentEmployee.Name));
            }

            context.Employees.AddRange(resultEmployees);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            ItemDto[] itemsFromJson = ImportFromJson<ItemDto>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Item> resultItems = new List<Item>();

            foreach (ItemDto itemDto in itemsFromJson)
            {
                if (!IsValid(itemDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool itemAlreadyExists = resultItems.Any(i => i.Name == itemDto.Name);

                if (itemAlreadyExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Category category = context.Categories.SingleOrDefault(c => c.Name == itemDto.Category);

                if (category == null)
                {
                    category = new Category
                    {
                        Name = itemDto.Category
                    };

                    context.Categories.Add(category);
                    context.SaveChanges();
                }

                Item currentItem = new Item
                {
                    Category = category,
                    Name = itemDto.Name,
                    Price = itemDto.Price
                };

                resultItems.Add(currentItem);
                sb.AppendLine(string.Format(SuccessMessage, itemDto.Name));
            }

            context.Items.AddRange(resultItems);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            OrderDto[] ordersFromXml = (OrderDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();
            List<Order> resultOrders = new List<Order>();

            foreach (OrderDto orderDto in ordersFromXml)
            {
                if (!IsValid(orderDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Employee employee = context.Employees.SingleOrDefault(e => e.Name == orderDto.Employee);

                if (employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!orderDto.Items.All(i => context.Items.Any(dbItem => dbItem.Name == i.Name)))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                DateTime dateTime = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                OrderType type = Enum.Parse<OrderType>(orderDto.Type);

                Order currentOrder = new Order
                {
                    Customer = orderDto.Customer,
                    DateTime = dateTime,
                    Employee = employee,
                    Type = type
                };

                foreach (OrderItemDto item in orderDto.Items)
                {
                    OrderItem orderItem = new OrderItem
                    {
                        Item = context.Items.Single(i => i.Name == item.Name),
                        Order = currentOrder,
                        Quantity = item.Quantity
                    };

                    currentOrder.OrderItems.Add(orderItem);
                }

                resultOrders.Add(currentOrder);
                sb.AppendLine($"Order for {currentOrder.Customer} on {currentOrder.DateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");
            }

            context.Orders.AddRange(resultOrders);
            context.SaveChanges();

            string result = sb.ToString().Trim();
            return result;
        }

        private static T[] ImportFromJson<T>(string jsonString)
        {
            T[] deserializedObjects = JsonConvert.DeserializeObject<T[]>(jsonString);

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