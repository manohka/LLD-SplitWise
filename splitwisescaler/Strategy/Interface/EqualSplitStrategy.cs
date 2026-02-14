using splitwisescaler.Models;

namespace splitwisescaler.Strategy.Interface
{
    public class EqualSplitStrategy : ISplitStrategy
    {
        public Dictionary<User, decimal> Split(decimal amount, List<User> participants, params object[] parameters)
        {
            var splits = new Dictionary<User, decimal>();

            decimal share = Math.Round(amount / participants.Count, 2);

            decimal total = 0;

            for (int i = 0; i < participants.Count; i++)
            {
                // base case
                if (i == participants.Count - 1)
                {
                    splits[participants[i]] = Math.Round(amount - total, 2);
                }
                else
                {
                    splits.Add(participants[i], share);
                    total += share;
                }
            }
            return splits;
        }
    }
}
