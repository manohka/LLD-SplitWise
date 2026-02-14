using splitwisescaler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace splitwisescaler.Strategy.Interface
{
    public class ExactSplitStrategy : ISplitStrategy
    {
        public Dictionary<User, decimal> Split(decimal amount, List<User> participants, params object[] parameters)
        {
            var splits = new Dictionary<User, decimal>();
            var exactAmounts = parameters[0] as List<decimal>;

            if (exactAmounts == null || exactAmounts.Sum() != amount)
                throw new ArgumentException("Exact amounts must sum to total amount");

            decimal total = 0;

            for (int i = 0; i < participants.Count; i++)
            {
                splits.Add(participants[i], Math.Round(exactAmounts[i], 2));
                total += exactAmounts[i];
            }

            return splits;
        }
    }
}
