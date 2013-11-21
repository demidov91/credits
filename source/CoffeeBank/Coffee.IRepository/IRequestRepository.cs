using System.Collections.Generic;
using Coffee.Entities;

namespace Coffee.IRepository
{
    public interface IRequestRepository
    {
        List<CreditRequest> GetAllCreditRequests();

        List<CreditRequest> GetApprovedCreditRequests();

        List<CreditRequest> GetUnapprovedCreditRequests();

        List<CreditRequest> GetRequestsByOwner(string username);

        CreditRequest GetRequestById(long id);

        CreditRequest Update(CreditRequest request);

        bool Approve(Approval approval);

        void AddCreditRequest(CreditRequest request);
    }
}
