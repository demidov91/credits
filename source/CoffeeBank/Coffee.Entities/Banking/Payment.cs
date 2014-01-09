using System;
using System.ComponentModel.DataAnnotations;

namespace Coffee.Entities
{
    public class Payment
    {
        [Key]
        public long Id { get; set; }

        public virtual Credit Credit { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentTime { get; set; }
    }
}
