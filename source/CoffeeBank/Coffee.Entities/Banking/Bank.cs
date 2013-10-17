using System.Collections.Generic;
using System.Linq;

namespace Coffee.Entities
{
    public class Bank
    {
        public long Id { get; set; }

        private Account _account;

        public List<CreditLine> CreditLines { get; set; }

        public List<BankWorker> Workers { get; set; }

        public bool IssueCredit(CreditRequest approvedRequest)
        {
            return _account.WithdrawMoney(approvedRequest.Amount);
        }

        public bool TakePayment(Payment payment)
        {
            return _account.PutMoney(payment.Amount);
        }

        public IEnumerable<CreditLine> GetAvailableCreditLines(CreditRequest request)
        {
            return CreditLines.Where(x => x.IsAcceptable(request));
        }
    }
}
