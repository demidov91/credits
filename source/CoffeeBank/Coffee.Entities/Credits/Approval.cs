using System;

namespace Coffee.Entities
{
    public class Approval: IUpdateable<Approval>
    {
        public long Id { get; set; }

        public CreditRequest Request { get; set; }

        public BankWorker Approver { get; set; }

        public DateTime ApproveTime { get; set; }

        public void Update(Approval other) {
            this.Approver = other.Approver;
            this.ApproveTime = other.ApproveTime;
            this.Request = other.Request;
        }
    }
}
