using SimpelButik.Data;
using SimpelButik.Models;
using SimpelButik.Models.Enums;
using System.Globalization;
using System.Text.Json;


namespace SimpelButik.Views;

public static class View
{
    public static Customer? Render(string view)
    {
        switch (view)
        {
            case "L":
                Console.WriteLine(usernameView);
                string loginUsername = TakeInput("Username");

                var loginCustomersJson = FileIO.ReadData("Customers.json");
                var loginCustomers = JsonSerializer.Deserialize<List<Customer>>(loginCustomersJson);

                var customer = loginCustomers.FirstOrDefault((customer) =>
                    customer.Username == loginUsername);
                if (customer == null)
                {
                    Console.WriteLine("Not correct. You are not registered yet");
                    return null;
                }

                else
                {
                    Console.WriteLine(passwordView);
                    string loginPassword = TakeInput("Password");
                    if (loginPassword == customer.Password)
                    {
                        return customer;
                    }

                    Console.WriteLine("Incorrect password");
                }
                return null;

            case "R":
                var registerCustomersJson = FileIO.ReadData("Customers.json");
                var registerCustomers = JsonSerializer.Deserialize<List<Customer>>(registerCustomersJson);

                var existingUsernames = registerCustomers.Select((customer) => customer.Username).ToList();
                Console.WriteLine(usernameView);
                string registerUsername = TakeInput("Username", existingUsernames);
                Console.WriteLine(passwordView);
                string registerPassword = TakeInput("Password");
                Console.WriteLine(marketView);
                string registerMarket = TakeInput(new List<string>() { "S", "U", "R" });
                Market registerMarketEnum =
                    registerMarket == "S" ? Market.SE : registerMarket == "U" ? Market.UK : Market.RS;
                var newCustomer = new Customer(registerUsername, registerPassword, registerMarketEnum);
                registerCustomers.Add(newCustomer);
                FileIO.UpdateData("Customers.json", JsonSerializer.Serialize(registerCustomers));
                return newCustomer;

            case "E":
                Console.WriteLine("Good bye");
                return null;
            default:
                Console.WriteLine("Thank you for visiting our simple boutique!");
                return null;
        }
    }

    public static void Render(string view, Customer currentCustomer)
    {

        switch (view)
        {

            case "shop":
                Console.WriteLine(shopView);
                string shopInput = TakeInput(new List<string>() { "E", "S", "C", "P" });
                Render(shopInput, currentCustomer);
                break;

            case "E":
                Console.WriteLine("loging out");

                break;
            case "S":
                var shopProductsJson = FileIO.ReadData("Products.json");
                if (shopProductsJson == null)
                {
                    Console.WriteLine("Something went wrong..");
                }

                var shopProducts = JsonSerializer.Deserialize<List<Product>>(shopProductsJson);
                Console.WriteLine("Pick a product from the menu:\n");

                foreach (var product in shopProducts)
                {
                    Console.WriteLine(product.Print(currentCustomer.Market));
                }

                string userInput = Console.ReadLine().ToUpper();
                Product? selectedProduct = null;
                foreach (var product in shopProducts)
                {
                    if (product.Name.ToUpper() == userInput)
                    {
                        selectedProduct = product;
                    }
                }

                if (selectedProduct == null)
                {
                    Console.WriteLine($"We run out of {userInput}. Please select from the given list");
                }
                else
                {
                    currentCustomer.Cart.Add(selectedProduct);
                }
                Render("shop", currentCustomer);
                break;

            case "C":
                Console.WriteLine(" Cart view");
                var grouped = currentCustomer.Cart
                    .GroupBy((cartItem) => new { cartItem.Name, cartItem.Price })
                    .Select((group) => new
                    {
                        Name = group.Key.Name,
                        UnitPrice = group.Key.Price,
                        Count = group.Count(),
                        TotalPrice = group.Sum(groupItem => groupItem.Price)
                    })
                    .ToList();
                grouped.ForEach((product)
                    => Console.WriteLine(
                        $"{product.Name,-10} | {GetConvertedPrice(currentCustomer.Market, product.UnitPrice),10} | {product.Count,8} | {GetConvertedPrice(currentCustomer.Market, product.TotalPrice),11}"));
                Render("shop", currentCustomer);
                break;

            case "P":

                double totalPayment = currentCustomer.Cart.Sum(cartItem => cartItem.Price);
                string convertedPrice = GetConvertedPrice(currentCustomer.Market, totalPayment);
                
                Console.WriteLine($"Your total cost is: {convertedPrice}");
                Console.WriteLine("Press any key to confirm");
                currentCustomer.Cart.Clear();
                Console.ReadKey();
                break;
            default:
                Console.WriteLine("Thank you for visiting our simple boutique!");
                break;

        }
    }

    public static string TakeInput(List<string> options)
    {
        string input;

        do
        {
            input = Console.ReadLine()!.ToUpper();
            if (!options.Contains(input))
            {
                Console.WriteLine($"Invalid input, please enter {String.Join(", ", options)}");
            }
        } while (!options.Contains(input));

        return input;
    }

    public static string TakeInput(string field)
    {
        string input;

        do
        {
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine($"{field}is mandatory!");
            }
        } while (string.IsNullOrEmpty(input));

        return input;
    }

    public static string TakeInput(string field, List<string> existingData)
    {
        string input;

        do
        {
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine($"{field} is mandatory! Please try again!");
            }
            else if (existingData.Contains(input))
            {
                Console.WriteLine($"{field}\"{input}\"is already taken. Please try again!");
            }
        } while (string.IsNullOrEmpty(input) || existingData.Contains(input));

        return input;
    }


    private static readonly string shopView =
        "[S]hopping\n" +
        "[C]art\n" +
        "[P]ayment\n" +
        "[E]xit";


    private static readonly string usernameView = "Enter your username:";

    private static readonly string passwordView = "Enter your password:";

    private static readonly string marketView =
        "[S]weden\n" +
        "[U]nited Kingdom\n" +
        "[R]Serbia";

    private static string GetConvertedPrice(Market market, double amount)

    {
        var cultureInfo = market == Market.SE ? "sv-SE" :
            market == Market.UK ? "en-GB" : "rs-RS";
        double convertedPrice = 0;
        switch (market)

        {
            case Market.UK:
                convertedPrice = amount / 10;
                break;
            case Market.RS:
                convertedPrice = amount * 10;
                break;
            default:
                convertedPrice = amount;
                break;
        }

       return convertedPrice.ToString("C", new CultureInfo(cultureInfo));

    }
}


