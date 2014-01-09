using System;
using System.Collections.Generic;
using System.Linq;
using Coffee.Entities;
using Coffee.IRepository;

namespace Coffee.Repository
{
    class CreditLineRepository : ICreditLineRepository
    {
        private readonly CoffeeDb Context;
        private event EventHandler wasUpdated;

        public CreditLineRepository()
        {
            Context = new CoffeeDb();
        }

        public List<CreditLine> getAll()
        {
            return Context.CreditLines.AsEnumerable().ToList();
        }

        public CreditLine getById(long id)
        {
            return Context.CreditLines.Find(id);
        }

        public void Add(CreditLine oneMore)
        {
            Context.CreditLines.Add(oneMore);
            Context.SaveChanges();

            if (wasUpdated != null)
            {
                wasUpdated(null, null);
            }
        }

        public void AddUpdateListener(EventHandler handler)
        {
            wasUpdated += handler;
        }
    }
}
