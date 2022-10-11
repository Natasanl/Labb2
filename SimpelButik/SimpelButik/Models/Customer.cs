using SimpelButik.Models.Enums;

namespace SimpelButik.Models;

public class Customer
{
    public string Username { get; private set; }

    public string Password { get; set; }

    private List<Product> _cart;
    public List<Product>Cart {get {return _cart;}}

    public Market Market { get; set; }
    public double TotalSpent { get; set; }
    public Category Category { get; set; }

    public Customer(string username, string password, Market market)
    {
        Username = username;
        Password = password;
        Market = market;
        _cart = new List<Product>();
        TotalSpent = 0;
        Category = Category.Basic;
    }

    public override string ToString()
    {
        return $"Username:{Username},Password:{Password},Market:{Market},TotalSpent:{TotalSpent},{Category}\n";
    }
}