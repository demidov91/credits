using System;

namespace Coffee.Entities
{
    public class Approval
    {
        public long Id { get; set; }

        public CreditRequest Request { get; set; }

        public BankWorker Approver { get; set; }

        public DateTime ApproveTime { get; set; }
    }
}
