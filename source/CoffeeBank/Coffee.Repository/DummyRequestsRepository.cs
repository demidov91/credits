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
            return null;
        }

        public List<CreditRequest> GetUnapprovedCreditRequests()
        {
            return null;
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
            return fromDb;
        }

        public bool Approve(Approval approval) {
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

        public List<Approval> ApprovalsForRrequest(CreditRequest request) {
            List<Approval> all = RepoFactory.GetApprovalRepo().GetAll();
            return all.FindAll(x => x.Request.Equals(request));
        }

        public List<CreditRequest> GetRequestsByOwner(string username) {
            return new List<CreditRequest>(requests.Where(x => x.Username == username));
        }


    }
}
