using System;

namespace Coffee.Entities
{
    public class BankWorker
    {
        public long Id { get; set; }

        public Decision ApproveRequest(CreditRequest request)
        {
            return new Decision {
                Authority = this,
                Request = request,
                DecisionTime = DateTime.Now,
                Verdict = true
            };
        }

        public Decision RejectRequest(CreditRequest request)
        {
            return new Decision {
                Authority = this,
                Request = request,
                DecisionTime = DateTime.Now,
                Verdict = false
            };
        }
    }
}
