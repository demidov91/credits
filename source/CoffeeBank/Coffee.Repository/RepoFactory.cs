using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coffee.IRepository;

using Coffee.Entities;

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

        public static IDummyRepository<PassportInfo> GetPassportInfoRepo() {
            return DummyRepository<PassportInfo>.getInstance();
        }

        public static ICreditRepository GetCreditsRepo() {
            return DummyCreditRepository.GetInstance();
        }
    }
}
