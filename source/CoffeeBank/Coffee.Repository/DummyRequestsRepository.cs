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
            return null;
        }

        public bool Update(CreditRequest request) {
            return false;
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


    }
}
