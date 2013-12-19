using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Coffee.IRepository;
using Coffee.Entities;

namespace Coffee.Repository
{
    public class DummyRequestsRepository: IRepository.IRequestRepository 
    {
        private static DummyRequestsRepository instance = null;
        private static Object _createLock = new Object();
        
        private LinkedList<CreditRequest> requests = new LinkedList<CreditRequest>();
        private Random random = new Random();

        public List<CreditRequest> GetAllCreditRequests()
        {
            return new List<CreditRequest>(requests);
        }

        public List<CreditRequest> GetApprovedCreditRequests()
        {
            return requests.Where(x => x.Decision != null && x.Decision.Verdict == true).ToList();
        }

        public List<CreditRequest> GetUndecidedCreditRequests()
        {
            return requests.Where(x => x.Decision == null).ToList();
        }

        public List<CreditRequest> GetRejectedCreditRequests()
        {
            return requests.Where(x => x.Decision != null && x.Decision.Verdict == false).ToList();
        }

        public CreditRequest GetRequestById(long id)
        {
            return requests.First(x => x.Id == id);
        }

        public CreditRequest Update(CreditRequest request) {
            CreditRequest fromDb = requests.Single(x => x.Id == request.Id);
            if (request.AdditionalTextInfo != null) {
                fromDb.AdditionalTextInfo = request.AdditionalTextInfo;
            }
            if (request.Amount != null)
            {
                fromDb.Amount = request.Amount;
            }
            if (request.CreditLine != null && request.CreditLine != null)
            {
                fromDb.CreditLine = RepoFactory.GetCreditLineRepo().getById(request.CreditLine.Id);
            }
            if (request.PassportInfo != null)
            {
                fromDb.PassportInfo = request.PassportInfo;
            }
            if (request.Period != null)
            {
                fromDb.Period = request.Period;
            }
            if (request.SalaryInfo != null)
            {
                fromDb.SalaryInfo = request.SalaryInfo;
            }
            if (request.Username != null)
            {
                fromDb.Username = request.Username;
            }
            if (request.IssueDate != null) {
                fromDb.IssueDate = request.IssueDate;
            }
            return fromDb;
        }

        public bool RegisterDecision(Decision decision) {
            return false;
        }

        private DummyRequestsRepository() { }

        public static DummyRequestsRepository getInstance() {
            lock (_createLock) {
                if (instance == null) {
                    instance = new DummyRequestsRepository();
                }
            }
            return instance;
        }

        public void AddCreditRequest(CreditRequest request) {
            request.Id = random.Next();
            requests.AddLast(request);
        }

        public List<CreditRequest> GetRequestsByOwner(string username) {
            return new List<CreditRequest>(requests.Where(x => x.Username == username));
        }

        /// <summary>
        /// New entity of Credit will be created, new credit line will start working.  
        /// Use it carefully as any conditions of credit line will be accepted without additional check.
        /// </summary>
        public Credit Accept(CreditRequest thiz) {
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
            requests.Remove(thiz);
            return justCreated;
        }
    }
}
