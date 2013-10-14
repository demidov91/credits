using System.Collections.Generic;
using Coffee.Entities;

namespace Coffee.IRepository
{
    public interface IRequestRepository
    {
        List<CreditRequest> GetAllCreditRequests();

        List<CreditRequest> GetApprovedCreditRequests();

        List<CreditRequest> GetUnapprovedCreditRequests();

        CreditRequest GetRequestById(long id);

        bool Update(CreditRequest request);

        bool Approve(Approval approval);
    }
}
