using System;

namespace Coffee.Entities
{
    public class Approval
    {
        public CreditRequest Request { get; set; }

        public BankWorker Approver { get; set; }

        public DateTime ApproveTime { get; set; }
    }
}
