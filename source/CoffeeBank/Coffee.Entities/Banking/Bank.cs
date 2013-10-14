using System.Collections.Generic;

namespace Coffee.Entities
{
    public class Bank
    {
        private Account _account;

        public bool IssueCredit(CreditRequest approvedRequest)
        {
            return _account.WithdrawMoney(approvedRequest.Amount);
        }

        public bool TakePayment(Payment payment)
        {
            return _account.PutMoney(payment.Amount);
        }

        public List<CreditLine> CreditLines { get; set; }

        public List<BankWorker> Workers { get; set; }
    }
}
