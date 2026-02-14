using splitwisescaler.Strategy.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace splitwisescaler.Models
{
    public class Expense
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public User PaidBy { get; set; }
        public DateTime Date { get; set; }
        public ISplitStrategy SplitStrategy { get; set; }
        public Dictionary<User, decimal> Splits { get; set; }

        public Expense(string description, decimal amount, User paidBy, ISplitStrategy splitStrategy)
        {
            Id = Guid.NewGuid().ToString();
            Description = description;
            Amount = amount;
            PaidBy = paidBy;
            Date = DateTime.Now;
            SplitStrategy = splitStrategy;
            Splits = new Dictionary<User, decimal>();
        }

        public void CalculateSplits(List<User> participants, params object[] parameters)
        {
            Splits = SplitStrategy.Split(Amount, participants, parameters);
        }
    }
}
