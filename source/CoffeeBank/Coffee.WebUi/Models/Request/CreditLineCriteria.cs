namespace Coffee.WebUi.Models
{
    public class CreditLineCriteria
    {
        public decimal Amount { get; set; }
        public decimal MaxRate { get; set; }
        public int MinPeriod { get; set; }
        public int MaxPeriod { get; set; }

        public CreditLineCriteria() { }
        public CreditLineCriteria(decimal amount, decimal maxrate, int minperiod, int maxperiod)
        {
            Amount = amount;
            MaxRate = maxrate;
            MinPeriod = minperiod;
            MaxPeriod = maxperiod;
        }
    }
}
