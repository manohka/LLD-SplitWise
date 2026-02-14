using splitwisescaler.Models;

namespace splitwisescaler
{
    public class ExpenseManager
    {
        private List<Expense> _expenses;
        private List<User> _users;
        private Dictionary<string, Dictionary<string, decimal>> _balanceSheet;

        public ExpenseManager()
        {
            _expenses = new List<Expense>();
            _users = new List<User>();
            _balanceSheet = new Dictionary<string, Dictionary<string, decimal>>();
        }

        public void AddUser(User user)
        {
            _users.Add(user);
            _balanceSheet[user.Id] = new Dictionary<string, decimal>();
        }

        public void AddExpense(Expense expense)
        {
            _expenses.Add(expense);
            UpdateBalanceSheet(expense);
        }

        private void UpdateBalanceSheet(Expense expense)
        {
            // CONTINUE FROM HERE
            // CONTINUE FROM HERE
            // CONTINUE FROM HERE
        }

    }
}
