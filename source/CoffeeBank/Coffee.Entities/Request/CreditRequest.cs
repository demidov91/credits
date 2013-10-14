namespace Coffee.Entities
{
    public class CreditRequest
    {
        public long Id { get; set; }

        public decimal Amount { get; set; } //In belorussian rubles only

        public int Period { get; set; } //in months

        public PassportInfo PassportInfo { get; set; }

        public SalaryInfo SalaryInfo { get; set; }

        //...some other info

        public CreditRequest()
        {
            PassportInfo = new PassportInfo();
            SalaryInfo = new SalaryInfo();
        }
    }
}
