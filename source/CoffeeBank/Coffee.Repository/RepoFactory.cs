using Coffee.IRepository;

using Coffee.Entities;

namespace Coffee.Repository
{
    public class RepoFactory
    {
        public static IRequestRepository GetRequestsRepo() {
            return new RequestsRepository();
        }

        public static ICreditLineRepository GetCreditLineRepo() {
            return new CreditLineRepository();
        }

        public static IDummyRepository<PassportInfo> GetPassportInfoRepo() {
            return new PassportInfoRepository();
        }

        public static ICreditRepository GetCreditsRepo() {
            return new CreditRepository();
        }
    }
}
