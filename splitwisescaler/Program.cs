/*// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
*/

using splitwisescaler;
using splitwisescaler.CommandPattern;
using splitwisescaler.Models;
using splitwisescaler.Strategy.Interface;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Splitwise Application ===\n");

        // Initialize
        var expenseManager = new ExpenseManager();
        var commandInvoker = new CommandInvoker();

        // Create users
        var alice = new User("Alice", "alice@email.com");
        var bob = new User("Bob", "bob@email.com");
        var charlie = new User("Charlie", "charlie@email.com");

        expenseManager.AddUser(alice);
        expenseManager.AddUser(bob);
        expenseManager.AddUser(charlie);

        Console.WriteLine("Users created: Alice, Bob, Charlie\n");

        // Example 1: Equal Split
        Console.WriteLine("1. Adding Dinner Expense (Equal Split):");
        var dinnerExpense = new Expense("Dinner", 300, alice, new EqualSplitStrategy());
        var addDinnerCmd = new AddExpenseCommand(expenseManager, dinnerExpense, new List<User> { alice, bob, charlie });

        commandInvoker.ExecuteCommand(addDinnerCmd);

        commandInvoker.ExecuteCommand(new ShowBalancesCommand(expenseManager));


        // Example 2: Percentage Split

        Console.WriteLine("\n2. Adding Grocery Expense (Percentage Split):");

        var groceryExpense = new Expense("Groceries", 200, bob, new PercentageSplitStrategy());

        var addGroceryCmd = new AddExpenseCommand(expenseManager, groceryExpense,
                                                 new List<User> { alice, bob, charlie },
                                                 new List<decimal> { 50, 30, 20 });

        commandInvoker.ExecuteCommand(addGroceryCmd);

        commandInvoker.ExecuteCommand(new ShowBalancesCommand(expenseManager));

        // Example 3: Exact Split
        Console.WriteLine("\n3. Adding Movie Expense (Exact Split):");
        var movieExpense = new Expense("Movie", 150, charlie, new ExactSplitStrategy());
        var addMovieCmd = new AddExpenseCommand(expenseManager, movieExpense,
                                               new List<User> { alice, bob, charlie },
                                               new List<decimal> { 50, 60, 40 });

        commandInvoker.ExecuteCommand(addMovieCmd);

        commandInvoker.ExecuteCommand(new ShowBalancesCommand(expenseManager));

        // Settle Up
        Console.WriteLine("\n4. Settling all debts:");
        commandInvoker.ExecuteCommand(new SettleUpCommand(expenseManager));

        // Undo last expense
        Console.WriteLine("\n5. Undoing last expense:");
        commandInvoker.UndoLastCommand();

        commandInvoker.ExecuteCommand(new ShowBalancesCommand(expenseManager));

        // Settle for specific user
        Console.WriteLine("\n6. Settling only Alice's debts:");
        commandInvoker.ExecuteCommand(new SettleUpCommand(expenseManager, alice.Id));

        Console.WriteLine("\n=== Application Ended ===");
    }
}