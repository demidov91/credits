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

        List<Payment> GetPaymentsForCredit(long creditId);

        /// <summary>
        /// New entity of Credit will be created, new credit line will start working.  
        /// </summary>
        /// <param name="thiz">CreditRequest to make a credit.</param>
        /// <returns>Just created entity.</returns>
        Credit Accept(CreditRequest thiz);
    }
}
