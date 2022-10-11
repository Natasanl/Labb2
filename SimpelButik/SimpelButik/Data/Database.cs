
using System.Text.Json;
using SimpelButik.Models;
using SimpelButik.Models.Enums;

namespace SimpelButik.Data;

    public static class Database
    {
        public static void InitializeData()
        {
            var customers = new List<Customer>()
            {
                new("Knatte", "123", Market.SE),
                new("Fnatte", "321", Market.UK),
                new("Tjatte", "231", Market.DE),
            };

            FileIO.AddData("Customers.json", JsonSerializer.Serialize(customers));

            var products = new List<Product>()
            {
                new("Sausage", 25),
                new("Drink", 30),
                new("Apple", 15)
            };

            FileIO.AddData("Products.json", JsonSerializer.Serialize(products));
        }

    }

