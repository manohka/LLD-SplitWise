using splitwisescaler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace splitwisescaler.Strategy.Interface
{
    public class PercentageSplitStrategy : ISplitStrategy
    {
        public Dictionary<User, decimal> Split(decimal amount, List<User> participants, params object[] parameters)
        {
            var splits = new Dictionary<User, decimal>();
            var percentages = parameters[0] as List<decimal>;

            if (percentages == null || percentages.Sum() != 100)
            {
                throw new ArgumentException("Percentages must sum to 100");
            }

            for(int i = 0; i < participants.Count; i++)
            {
                splits.Add(participants[i], Math.Round(amount * percentages[i] / 100, 2));
                // or the above line can also be written as
                // splits[participants[i] = Math.Round(amount * percentages[i] / 100, 2)
            }

            return splits;
        }
    }
}
