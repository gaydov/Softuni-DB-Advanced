using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace FastFood.DataProcessor
{
    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            EmployeeWithOrdersDto employeeWithOrdersDto = new EmployeeWithOrdersDto
            {
                Name = employeeName,
                Orders = context.Employees
                    .Single(e => e.Name == employeeName)
                    .Orders
                    .Where(o => o.Type == Enum.Parse<OrderType>(orderType))
                    .Select(o => new OrderDto
                    {
                        Customer = o.Customer,

                        Items = o.OrderItems.Select(oi => new ItemDto
                            {
                                Name = oi.Item.Name,
                                Price = oi.Item.Price,
                                Quantity = oi.Quantity
                            })
                            .ToList(),

                        TotalPrice = o.TotalPrice
                    })
                .OrderByDescending(or => or.TotalPrice)
                .ThenByDescending(or => or.Items.Count)
                .ToList()
            };

            string serializedOrders = JsonConvert.SerializeObject(employeeWithOrdersDto, Formatting.Indented);
            return serializedOrders;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            List<CategoryDto> resultCategoryDtos = new List<CategoryDto>();
            string[] inputCategoriesNames = categoriesString.Split(",");

            foreach (string inputCategoryName in inputCategoriesNames)
            {
                List<CategoryItemDto> itemDtos = new List<CategoryItemDto>();

                Category category = context.Categories.SingleOrDefault(c => c.Name == inputCategoryName);
                
                Item[] categoryItems = category.Items
                    .Where(i => i.Category.Name == inputCategoryName && i.OrderItems.Any())
                    .ToArray();

                foreach (Item item in categoryItems)
                {
                    int quantity = item.OrderItems
                        .Where(oi => oi.ItemId == item.Id)
                        .Sum(oi => oi.Quantity);

                    decimal totalMoney = quantity * item.Price;

                    CategoryItemDto itemDto = new CategoryItemDto
                    {
                        Name = item.Name,
                        TimesSold = quantity,
                        TotalMade = totalMoney
                    };

                    itemDtos.Add(itemDto);
                }

                decimal maxTotalMoneyMade = itemDtos.Max(x => x.TotalMade);
                CategoryItemDto mostPopularItem = itemDtos.Single(i => i.TotalMade == maxTotalMoneyMade);

                CategoryDto currentCategoryDto = new CategoryDto
                {
                    Name = category.Name,
                    MostPopularItem = mostPopularItem
                };

                resultCategoryDtos.Add(currentCategoryDto);
            }

            resultCategoryDtos = resultCategoryDtos.OrderByDescending(x => x.MostPopularItem.TotalMade)
                                               .ThenByDescending(x => x.MostPopularItem.TimesSold)
                                               .ToList();

            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(List<CategoryDto>), new XmlRootAttribute("Categories"));
            serializer.Serialize(new StringWriter(sb), resultCategoryDtos, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            string serializedCategories = sb.ToString();
            return serializedCategories;
        }
    }
}