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

        public void Update(CreditLine line)
        {
            var result = Context.CreditLines.Find(line.Id);
            if (result == null)
            {
                Context.CreditLines.Add(line);
            }
            else
            {
                result.IsActive = line.IsActive;
                result.KindOfPayments = line.KindOfPayments;
                result.MaxAgeBoundary = line.MaxAgeBoundary;
                result.MaxAmountBoundary = line.MaxAmountBoundary;
                result.MaxMonthsBoundary = line.MaxMonthsBoundary;
                result.MinAgeBoundary = line.MinAgeBoundary;
                result.MinAmountBoundary = line.MinAmountBoundary;
                result.MinAverageSalaryBoundary = line.MinAverageSalaryBoundary;
                result.MinMonthsBoundary = line.MinMonthsBoundary;
                result.MinWorkYearsBoundary = line.MinWorkYearsBoundary;
                result.Name = line.Name;
                result.Rate = line.Rate;
            }
            Context.SaveChanges();
        }

        public void AddUpdateListener(EventHandler handler)
        {
            wasUpdated += handler;
        }
    }
}
