using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace splitwisescaler.CommandPattern
{
    public class ShowBalancesCommand : ICommand
    {
        private ExpenseManager _expenseManager;

        public ShowBalancesCommand(ExpenseManager expenseManager)
        {
            _expenseManager = expenseManager;
        }

        public void Execute()
        {
            var balances = _expenseManager.GetAllBalances();
            Console.WriteLine("\n=== Current Balances ===");
            foreach (var balance in balances)
            {
                Console.WriteLine($"{balance.Key.Name}: {balance.Value:C}");
            }
        }

        public bool CanExecute()
        {
            return true;
        }

        public void Undo()
        {
            // Nothing to undo for showing balances
        }
    }
}
