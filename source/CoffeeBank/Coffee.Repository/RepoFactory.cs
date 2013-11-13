using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coffee.IRepository;

namespace Coffee.Repository
{
    public class RepoFactory
    {
        public static IRequestRepository GetRequestsRepo() {
            return DummyRequestsRepository.getInstance();
        }

        public static ICreditLineRepository GetCreditLineRepo() {
            return DummyCreditLineRepository.getInstance();
        }
    }
}
