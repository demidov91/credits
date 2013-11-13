using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Coffee.IRepository;
using Coffee.Entities;

namespace Coffee.Repository
{
    class DummyRequestsRepository: IRepository.IRequestRepository 
    {
        private static Object _createLock = new Object();

        List<CreditRequest> GetAllCreditRequests() {
            return new List<CreditRequest>(requests);
        }

        List<CreditRequest> GetApprovedCreditRequests();

        List<CreditRequest> GetUnapprovedCreditRequests();

        CreditRequest GetRequestById(long id);

        bool Update(CreditRequest request);

        bool Approve(Approval approval);

        private LinkedList<CreditRequest> requests = new LinkedList<CreditRequest>();
        private static DummyRequestsRepository instance = null;

        private DummyRequestsRepository() { }

        public static DummyRequestsRepository getInstance() {
            lock (_createLock) {
                if (instance == null) {
                    instance = new DummyRequestsRepository();
                }
            }
            return instance;
        }


    }
}
