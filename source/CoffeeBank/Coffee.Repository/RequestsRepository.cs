using System;
using System.Collections.Generic;
using System.Linq;
using Coffee.Entities;
using Coffee.IRepository;

namespace Coffee.Repository
{
    class RequestsRepository : IRequestRepository
    {
        private readonly CoffeeDb Context;

        public RequestsRepository()
        {
            Context = new CoffeeDb();
        }

        public List<CreditRequest> GetAllCreditRequests()
        {
            return Context.CreditRequests.AsEnumerable().ToList();
        }

        public List<CreditRequest> GetApprovedCreditRequests()
        {
            return Context.CreditRequests.Where(x => x.Decision.Verdict == true).ToList();
        }

        public List<CreditRequest> GetUndecidedCreditRequests()
        {
            throw new NotImplementedException();
        }

        public List<CreditRequest> GetRejectedCreditRequests()
        {
            throw new NotImplementedException();
        }

        public List<CreditRequest> GetRequestsByOwner(string username)
        {
            throw new NotImplementedException();
        }

        public CreditRequest GetRequestById(long id)
        {
            throw new NotImplementedException();
        }

        public CreditRequest Update(CreditRequest request)
        {
            throw new NotImplementedException();
        }
        
        public void AddCreditRequest(CreditRequest request)
        {
            throw new NotImplementedException();
        }

        public Credit Accept(CreditRequest thiz)
        {
            throw new NotImplementedException();
        }
    }
}
