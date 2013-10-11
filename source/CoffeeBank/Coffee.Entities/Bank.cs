using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coffee.Entities
{
    public class Bank
    {
        private Account _account;

        public bool IssueCredit(CreditRequest approvedRequest)
        {
            return _account.WithdrawMoney(approvedRequest.Sum);
        }

        public bool TakePayment(Payment payment)
        {
            return _account.PutMoney(payment.Amount);
        }
    }
}
