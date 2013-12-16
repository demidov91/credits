using System.Collections.Generic;
using Coffee.Entities;

namespace Coffee.IRepository
{
    public interface ICreditRepository
    {
        List<Credit> GetAllIssuedCredits();

        List<Credit> GetCreditsByOwner(string username);

        Credit GetCreditById(long id);

        List<Payment> GetPaymentsForCredit(long creditId);

        bool AcceptPayment(Payment p);
    }
}