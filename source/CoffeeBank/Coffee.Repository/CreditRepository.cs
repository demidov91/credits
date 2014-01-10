using System.Collections.Generic;
using System.Linq;
using Coffee.Entities;
using Coffee.IRepository;

namespace Coffee.Repository
{
    class CreditRepository : ICreditRepository
    {
        private readonly CoffeeDb Context;

        public CreditRepository()
        {
            Context = new CoffeeDb();
        }

        public void Add(Credit oneMore)
        {
            Context.Credits.Add(oneMore);
            Context.SaveChanges();
        }

        public List<Credit> GetAllIssuedCredits()
        {
            return Context.Credits.AsEnumerable().ToList();
        }

        public List<Credit> GetCreditsByOwner(string username)
        {
            return Context.Credits.Where(x => x.User == username).ToList();
        }

        public Credit GetCreditById(long id)
        {
            return Context.Credits.Find(id);
        }

        public List<Payment> GetPaymentsForCredit(long creditId)
        {
            return Context.Payments.Where(x => x.Credit.Id == creditId).ToList();
        }

        public bool AcceptPayment(Payment p)
        {
            try
            {
                Context.Payments.Add(p);
                Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
