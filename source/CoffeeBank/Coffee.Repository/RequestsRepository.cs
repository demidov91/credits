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
            return Context.CreditRequests.Where(x => x.Decision!=null && x.Decision.Verdict == true).ToList();
        }

        public List<CreditRequest> GetUndecidedCreditRequests()
        {
            return Context.CreditRequests.Where(x => x.Decision != null && x.Decision.Verdict == null).ToList();
        }

        public List<CreditRequest> GetRejectedCreditRequests()
        {
            return Context.CreditRequests.Where(x => x.Decision != null && x.Decision.Verdict == false).ToList();
        }

        public List<CreditRequest> GetRequestsByOwner(string username)
        {
            return Context.CreditRequests.Where(x => x.Username == username).ToList();
        }

        public CreditRequest GetRequestById(long id)
        {
            return Context.CreditRequests.Find(id);
        }

        public CreditRequest Update(CreditRequest request)
        {
            CreditRequest old = Context.CreditRequests.Find(request.Id);

            if (old != null)
            {
                old.AdditionalTextInfo = request.AdditionalTextInfo;
                old.Amount = request.Amount;
                old.CreditLine = request.CreditLine;
                old.Decision = request.Decision;
                old.IssueDate = request.IssueDate;
                old.PassportInfo = request.PassportInfo;
                old.Period = request.Period;
                old.SalaryInfo = request.SalaryInfo;
                old.Username = request.Username;
                Context.SaveChanges();
            }
            return old;
        }
        
        public void AddCreditRequest(CreditRequest request)
        {
            Context.CreditRequests.Add(request);
            Context.SaveChanges();
        }

        public Credit Accept(CreditRequest thiz)
        {
            Credit justCreated = new Credit
            {
                Amount = thiz.Amount,
                IssueDate = thiz.IssueDate,
                Line = thiz.CreditLine,
                Passport = thiz.PassportInfo,
                Period = thiz.Period,
                User = thiz.Username
            };
            RepoFactory.GetCreditsRepo().Add(justCreated);
            Context.CreditRequests.Remove(thiz);
            return justCreated;
        }
    }
}
