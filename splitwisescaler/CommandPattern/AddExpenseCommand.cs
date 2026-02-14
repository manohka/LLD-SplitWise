using splitwisescaler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace splitwisescaler.CommandPattern
{
    public class AddExpenseCommand : ICommand
    {
        private ExpenseManager _expenseManager;
        private Expense _expense;
        private List<User> _participants;
        private object[] _splitParams;

        public AddExpenseCommand(ExpenseManager expenseManager, Expense expense,
                            List<User> participants, params object[] splitParams)
        {
            _expenseManager = expenseManager;
            _expense = expense;
            _participants = participants;
            _splitParams = splitParams;
        }
        public void Execute()
        {
            _expense.CalculateSplits(_participants, _splitParams);
            _expenseManager.AddExpense(_expense);
            Console.WriteLine($"Expense '{_expense.Description}' added successfully!");
        }

        public bool CanExecute()
        {
            return _expense != null &&
               _participants != null &&
               _participants.Count > 0 &&
               _expense.Amount > 0;
        }

        public void Undo()
        {
            _expenseManager.RemoveExpense(_expense.Id);
            Console.WriteLine($"Expense '{_expense.Description}' removed!");
        }
    }
}
