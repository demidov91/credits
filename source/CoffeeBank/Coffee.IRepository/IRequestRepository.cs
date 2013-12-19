using System.Collections.Generic;
using Coffee.Entities;

namespace Coffee.IRepository
{
    public interface IRequestRepository
    {
        List<CreditRequest> GetAllCreditRequests();

        List<CreditRequest> GetApprovedCreditRequests();

        List<CreditRequest> GetUndecidedCreditRequests();

        List<CreditRequest> GetRejectedCreditRequests();

        List<CreditRequest> GetRequestsByOwner(string username);

        CreditRequest GetRequestById(long id);

        CreditRequest Update(CreditRequest request);

        bool RegisterDecision(Decision decision);

        void AddCreditRequest(CreditRequest request);

        /// <summary>
        /// New entity of Credit will be created, new credit line will start working.  
        /// </summary>
        /// <param name="thiz">CreditRequest to make a credit.</param>
        /// <returns>Just created entity.</returns>
        Credit Accept(CreditRequest thiz);
    }
}
