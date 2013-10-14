using System;

namespace Coffee.Entities
{
    public class BankWorker
    {
        public long Id { get; set; }

        public Approval ApproveRequest(CreditRequest request)
        {
            Approval approval = new Approval();

            approval.Approver = this;
            approval.Request = request;
            approval.ApproveTime = DateTime.Now;

            return approval;
        }
    }
}
