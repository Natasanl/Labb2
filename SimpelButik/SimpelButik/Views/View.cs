using SimpelButik.Data;
using SimpelButik.Models;
using SimpelButik.Models.Enums;
using System.Text.Json;

namespace SimpelButik.Views;

public static class View
{
    public static void Render(string view)
    {
        switch (view)
        {
            case "loginOrRegister":
                Console.WriteLine(loginOrRegisterView);
                string loginOrRegisterInput = TakeInput(new List<string>(){"E","L","R"});
                Render(loginOrRegisterInput);
                break;
            case "shop":
                Console.WriteLine(shopView);
                string shopInput = TakeInput(new List<string>() { "E", "S", "C", "P" });
                Render(shopInput);
                break;
            case "L":
                Console.WriteLine(usernameView);
                string loginUsername = TakeInput("Username");
                Console.WriteLine(passwordView);
                string loginPassword = TakeInput("Password");
                var loginCustomersJson = FileIO.ReadData("Customers.json");
                var loginCustomers = JsonSerializer.Deserialize<List<Customer>>(loginCustomersJson);

                var customer = loginCustomers.FirstOrDefault((customer) =>
                    customer.Username == loginUsername &&
                    customer.Password == loginPassword);
                if (customer == null)
                {
                    Console.WriteLine("Customer with these credentials doesn't exist.");
                    Render("loginOrRegister");
                }
                else
                {
                    Render("shop");
                }
                break;
            case "R":
                var registerCustomersJson = FileIO.ReadData("Customers.json");
                var registerCustomers = JsonSerializer.Deserialize<List<Customer>>(registerCustomersJson);

                var existingUsernames = registerCustomers.Select((customer) => customer.Username).ToList();
                Console.WriteLine(usernameView);
                string registerUsername = TakeInput("Username", existingUsernames);
                Console.WriteLine(passwordView);
                string registerPassword = TakeInput("Password");
                Console.WriteLine(marketView);
                string registerMarket = TakeInput(new List<string>() { "S", "U", "G" });
                Market registerMarketEnum = registerMarket == "S" ? Market.SE : registerMarket == "U" ? Market.UK : Market.DE;
                var newCustomer = new Customer(registerUsername, registerPassword, registerMarketEnum);
                registerCustomers.Add(newCustomer);
                FileIO.UpdateData("Customers.json", JsonSerializer.Serialize(registerCustomers));
                Render("shop");
                break;
            case "E":
                Console.WriteLine("Exit view");
                break;
            case "S":
                Console.WriteLine("Shopping product view ");
                break;
            case "C":
                Console.WriteLine(" Cart view");
                break;
            case "P":
                Console.WriteLine("Payment view");
                break;
            default:
                break;  
                
        }
    }

    public static string TakeInput(List<string> options)
    {
        string input;

        do
        {
            input = Console.ReadLine().ToUpper();
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

    private static readonly string loginOrRegisterView =
        "[L]ogin\n" +
        "[R]egister\n" +
        "[E]xit";

    private static readonly string shopView =
        "[S]hopping\n" +
        "[C]art\n" +
        "[P]ayment";

   
    private static readonly string usernameView = "Enter your username:";

    private static readonly string passwordView = "Enter your password:";

    private static readonly string marketView =
        "[S]weden\n" +
        "[U]nited Kingdom\n" +
        "[G]ermany";

}


