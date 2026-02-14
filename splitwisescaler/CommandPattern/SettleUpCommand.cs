using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace splitwisescaler.CommandPattern
{
    public class SettleUpCommand : ICommand
    {
        private ExpenseManager _expenseManager;
        private string _userId;

        public SettleUpCommand(ExpenseManager expenseManager, string userId = null)
        {
            _expenseManager = expenseManager;
            _userId = userId;
        }

        public void Execute()
        {
            var transactions = _expenseManager.SettleUp(_userId);

            Console.WriteLine("\n=== Settlement Transactions ===");
            if (transactions.Count == 0)
            {
                Console.WriteLine("No settlements needed!");
            }
            else
            {
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"{transaction.From.Name} → {transaction.To.Name}: {transaction.Amount:C}");
                }
            }
        }

        public bool CanExecute()
        {
            return true;
        }

        public void Undo()
        {
            // Settlement is typically not undoable
            Console.WriteLine("Settlement cannot be undone!");
        }
    }
}
