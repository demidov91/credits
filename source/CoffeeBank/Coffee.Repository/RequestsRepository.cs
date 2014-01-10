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
            var results = Context.CreditRequests.AsEnumerable().ToList();
            foreach (var result in results)
            {
                Context.Entry(result).Reference(x => x.CreditLine);
                Context.Entry(result).Reference(x => x.PassportInfo);
                Context.Entry(result).Reference(x => x.SalaryInfo);
            }
            return results;
        }

        public List<CreditRequest> GetApprovedCreditRequests()
        {
            var results = Context.CreditRequests.Where(x => x.Decision != null && x.Decision.Verdict == true).ToList();
            foreach (var result in results)
            {
                Context.Entry(result).Reference(x => x.CreditLine);
                Context.Entry(result).Reference(x => x.PassportInfo);
                Context.Entry(result).Reference(x => x.SalaryInfo);
            }
            return results;
        }

        public List<CreditRequest> GetUndecidedCreditRequests()
        {
            var results = Context.CreditRequests.Where(x => x.Decision != null && x.Decision.Verdict == null).ToList();
            foreach (var result in results)
            {
                Context.Entry(result).Reference(x => x.CreditLine);
                Context.Entry(result).Reference(x => x.PassportInfo);
                Context.Entry(result).Reference(x => x.SalaryInfo);
            }
            return results;
        }

        public List<CreditRequest> GetRejectedCreditRequests()
        {
            var results = Context.CreditRequests.Where(x => x.Decision != null && x.Decision.Verdict == false).ToList();
            foreach (var result in results)
            {
                Context.Entry(result).Reference(x => x.CreditLine);
                Context.Entry(result).Reference(x => x.PassportInfo);
                Context.Entry(result).Reference(x => x.SalaryInfo);
            }
            return results;
        }

        public List<CreditRequest> GetRequestsByOwner(string username)
        {
            var results = Context.CreditRequests.Where(x => x.Username == username).ToList();
            foreach (var result in results)
            {
                Context.Entry(result).Reference(x => x.CreditLine);
                Context.Entry(result).Reference(x => x.PassportInfo);
                Context.Entry(result).Reference(x => x.SalaryInfo);
            }
            return results;
        }

        public CreditRequest GetRequestById(long id)
        {
            var result = Context.CreditRequests.Find(id);
            Context.Entry(result).Reference(x => x.CreditLine);
            Context.Entry(result).Reference(x => x.PassportInfo);
            Context.Entry(result).Reference(x => x.SalaryInfo);
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
