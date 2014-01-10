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
            var result = Context.Credits.Find(id);
            Context.Entry(result).Reference(x => x.Line);
            Context.Entry(result).Reference(x => x.Passport);
            return Context.Credits.Find(id);
        }

        public List<Payment> GetPaymentsForCredit(long creditId)
        {
            var results = Context.Payments.Where(x => x.Credit.Id == creditId).ToList();
            foreach (var result in results)
            {
                Context.Entry(result).Reference(x => x.Credit);
            }
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
