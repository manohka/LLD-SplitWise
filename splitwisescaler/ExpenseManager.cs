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
            var paidBy = expense.PaidBy;

            foreach (var split in expense.Splits)
            {
                var owedBy = split.Key;
                var amount = split.Value;

                if (paidBy.Id == owedBy.Id) continue;

                // Update balance from owedBy to paidBy
                if (!_balanceSheet.ContainsKey(owedBy.Id))
                {
                    _balanceSheet[owedBy.Id] = new Dictionary<string, decimal>();
                }

                if (!_balanceSheet[owedBy.Id].ContainsKey(paidBy.Id))
                {
                    _balanceSheet[owedBy.Id][paidBy.Id] = 0;
                }

                _balanceSheet[owedBy.Id][paidBy.Id] += amount;

                // Also maintain reverse balance
                if (!_balanceSheet.ContainsKey(paidBy.Id))
                    _balanceSheet[paidBy.Id] = new Dictionary<string, decimal>();

                if (!_balanceSheet[paidBy.Id].ContainsKey(owedBy.Id))
                    _balanceSheet[paidBy.Id][owedBy.Id] = 0;

                _balanceSheet[paidBy.Id][owedBy.Id] -= amount;
            }
        }

        public void RemoveExpense(string expenseId)
        {
            var expense = _expenses.FirstOrDefault(e => e.Id == expenseId);
            if (expense != null)
            {
                // Reverse the balances (for undo functionality)
                ReverseBalanceSheet(expense);
                _expenses.Remove(expense);
            }
        }

        private void ReverseBalanceSheet(Expense expense)
        {
            var paidBy = expense.PaidBy;

            foreach (var split in expense.Splits)
            {
                var owedBy = split.Key;
                var amount = split.Value;

                if (paidBy.Id == owedBy.Id) continue;

                _balanceSheet[owedBy.Id][paidBy.Id] -= amount;
                _balanceSheet[paidBy.Id][owedBy.Id] += amount;

                // Clean up zero balances
                if (_balanceSheet[owedBy.Id][paidBy.Id] == 0)
                    _balanceSheet[owedBy.Id].Remove(paidBy.Id);

                if (_balanceSheet[paidBy.Id][owedBy.Id] == 0)
                    _balanceSheet[paidBy.Id].Remove(owedBy.Id);
            }
        }

        public Dictionary<User, decimal> GetAllBalances()
        {
            var netBalances = new Dictionary<User, decimal>();

            foreach (var user in _users)
            {
                decimal netBalance = 0;

                if (_balanceSheet.ContainsKey(user.Id))
                {
                    netBalance = _balanceSheet[user.Id].Values.Sum();
                }

                netBalances[user] = netBalance;
            }

            return netBalances;
        }

        public List<Transaction> SettleUp(string userId = null)
        {
            var netBalances = GetAllBalances();

            // If userId specified, only settle for that user

            if (!string.IsNullOrEmpty(userId))
            {
                var user = _users.FirstOrDefault(u => u.Id == userId);
                if (user == null) return new List<Transaction>();

                return SettleForUser(user, netBalances);
            }

            // otherwise settle for all
            return SettleAll(netBalances);

        }

        private List<Transaction> SettleAll(Dictionary<User, decimal> netBalances)
        {
            var transactions = new List<Transaction>();
            var debtors = new List<(User user, decimal amount)>();
            var creditors = new List<(User user, decimal amount)>();

            foreach (var kvp in netBalances)
            {
                if (kvp.Value < 0)
                {
                    debtors.Add((kvp.Key, -kvp.Value)); // Convert to positive amount they need to pay
                }
                else if (kvp.Value > 0)
                {
                    creditors.Add((kvp.Key, kvp.Value));
                }
            }

            debtors = debtors.OrderByDescending(d => d.amount).ToList();
            creditors = creditors.OrderByDescending(c => c.amount).ToList();

            int i = 0, j = 0;
            while (i < debtors.Count && j < creditors.Count)
            {
                var debtor = debtors[i];
                var creditor = creditors[j];

                decimal minAmount = Math.Min(debtor.amount, creditor.amount);

                if (minAmount > 0)
                {
                    transactions.Add(new Transaction(debtor.user, creditor.user, minAmount));

                    debtor.amount -= minAmount;
                    creditor.amount -= minAmount;

                    debtors[i] = debtor;
                    creditors[j] = creditor;
                }

                if (debtor.amount == 0) i++;
                if (creditor.amount == 0) j++;
            }

            return transactions;
        }

        private List<Transaction> SettleForUser(User user, Dictionary<User, decimal> netBalances)
        {
            var transactions = new List<Transaction>();
            var userBalance = netBalances[user];

            if (userBalance == 0) return transactions;

            if (userBalance > 0)
            {
                foreach (var kvp in netBalances)
                {
                    if (kvp.Key.Id != user.Id && kvp.Value < 0)
                    {
                        decimal amount = Math.Min(userBalance, -kvp.Value);
                        transactions.Add(new Transaction(kvp.Key, user, amount));
                        userBalance -= amount;

                        if (userBalance == 0) break;
                    }
                }
            }
            else  // User is debtor (they owe others)
            {
                userBalance = -userBalance;  // Make positive
                foreach (var kvp in netBalances)
                {
                    if (kvp.Key.Id != user.Id && kvp.Value > 0)
                    {
                        decimal amount = Math.Min(userBalance, kvp.Value);
                        transactions.Add(new Transaction(user, kvp.Key, amount));
                        userBalance -= amount;

                        if (userBalance == 0) break;
                    }
                }
            }

            return transactions;
        }

    }
}
