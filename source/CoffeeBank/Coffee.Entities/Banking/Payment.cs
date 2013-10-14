using System;

namespace Coffee.Entities
{
    public class Payment
    {
        public long Id { get; set; }

        public Credit Credit { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentTime { get; set; }
    }
}
