using System;

namespace Coffee.Entities
{
    public class Payment
    {
        public Credit Credit { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentTime { get; set; }
    }
}
