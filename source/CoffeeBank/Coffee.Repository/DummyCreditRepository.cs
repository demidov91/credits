using System;
using System.Collections.Generic;
using Coffee.Entities;
using System.Linq;

namespace Coffee.Repository
{
    public class DummyCreditRepository : IRepository.ICreditRepository
    {
        private List<Credit> credits = new List<Credit>();
        private LinkedList<Payment> payments = new LinkedList<Payment>();
        private static DummyCreditRepository instance = null;
        private static Object _createLock = new Object();
        private Random random = new Random(); 


        private DummyCreditRepository() {}

        public static DummyCreditRepository GetInstance()
        {
            lock (_createLock) {
                if (instance == null) instance = new DummyCreditRepository();
            }
            return instance;
        }

        public void Add(Credit oneMore) {
            if (oneMore.Id == 0) {
                oneMore.Id = random.Next();
            }
            credits.Add(oneMore);    
        }

        public List<Payment> GetPaymentsForCredit(long creditId)
        {
            return payments.Where(x => x.Credit.Id == creditId).ToList();
        }

        public bool AcceptPayment(Payment p)
        {
            payments.AddLast(p);
            return true;
        }

        public List<Credit> GetAllIssuedCredits()
        {
            var resultCredits = credits;
            foreach (var credit in resultCredits) {
                credit.Line = DummyCreditLineRepository.getInstance().getById(credit.Line.Id);
            }
            return resultCredits;
        }

        public List<Credit> GetCreditsByOwner(string username)
        {
            return credits.Where(x => x.User == username).ToList();
        }

        public Credit GetCreditById(long id)
        {
            return credits.FirstOrDefault(x => x.Id == id);
        }
    }
}