namespace Coffee.Entities
{
    public class AmountBoundary:IRequestBoundary
    {
        public long Id { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }

        public AmountBoundary()
        {
        }

        public AmountBoundary(decimal minAmount, decimal maxAmount)
        {
            MinAmount = minAmount;
            MaxAmount = maxAmount;
        }

        public bool IsAcceptable(CreditRequest request)
        {
            return (request.Amount >= MinAmount && request.Amount <= MaxAmount);
        }

        public string Visualize()
        {
            return string.Format("from {0} to {1} belorussian rubles", MinAmount, MaxAmount);
        }
    }
}
