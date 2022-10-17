using SimpelButik.Data;
using SimpelButik.Models;
using SimpelButik.Views;

Database.InitializeData();
LoginOrRegister();

static void LoginOrRegister()
{
    Console.WriteLine("[L]ogin\n" +
                      "[R]egister\n" +
                      "[E]xit");
    string input = View.TakeInput(new List<string>() { "E", "L", "R" });
    Customer? currentCustomer = View.Render(input);
    if (currentCustomer == null)
    {
        LoginOrRegister();
    }
    else View.Render("shop", currentCustomer);
}
