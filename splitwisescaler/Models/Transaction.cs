namespace splitwisescaler.Models
{
    public class Transaction
    {
        public User From { get; set; }
        public User To { get; set; }
        public decimal Amount { get; set; }

        public Transaction(User from, User to, decimal amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }
    }
}
