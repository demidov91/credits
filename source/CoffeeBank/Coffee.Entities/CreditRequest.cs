namespace Coffee.Entities
{
    public class CreditRequest
    {
        public decimal Sum { get; set; } //In belorussian rubles only

        public PassportInfo PassportInfo { get; set; }

        public SalaryInfo SalaryInfo { get; set; }

        //...some other info
    }
}
