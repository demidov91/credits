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

        private DummyCreditRepository() {}

        public static DummyCreditRepository GetInstance()
        {
            lock (_createLock) {
                if (instance == null) instance = new DummyCreditRepository();
            }
            return instance;
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
            return credits;
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