
namespace Coffee.Entities.Conditions
{
    public class PeriodBoundary: IRequestBoundary
    {
        public int MinMonths { get; set; }

        public int MaxMonths { get; set; }

        public PeriodBoundary()
        {
        }

        public PeriodBoundary(int minMonths, int maxMonths)
        {
            MinMonths = minMonths;
            MaxMonths = maxMonths;
        }

        public bool IsAcceptable(CreditRequest request)
        {
            return (request.Period >= MinMonths && request.Period <= MaxMonths);
        }

        public string Visualize()
        {
            return string.Format("{0} - {1} months", MinMonths, MaxMonths);
        }
    }
}
