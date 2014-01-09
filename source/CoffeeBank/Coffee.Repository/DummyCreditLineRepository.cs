using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Coffee.IRepository;
using Coffee.Entities;

namespace Coffee.Repository
{
    class DummyCreditLineRepository: ICreditLineRepository
    {
        /**
         * Array is just used instead of database.
         * */
        private List<CreditLine> lines = new List<CreditLine>();
        

        private static Object _createLock = new Object();
        private Random random = new Random();
        private static DummyCreditLineRepository instance = null;

        private DummyCreditLineRepository() {
            CreditLine sample = new CreditLine();
            sample.MinAmountBoundary = 2000000;
            sample.MaxAmountBoundary = 6000000;
            sample.Name = "Sample credit product";
            sample.Rate = 25;
            sample.KindOfPayments = CreditLine.PaymentKind.FACTICAL;
            sample.Id = 1;
            this.lines.Add(sample);

            CreditLine sample2 = new CreditLine() { Name = "Credit product 2", Id = 2, Rate = 30, MinAmountBoundary = 1000000,
             MaxAmountBoundary = 20000000, MinWorkTimeBoundary = TimeSpan.FromDays(365), MinAverageSalaryBoundary = 1500000,
             KindOfPayments = CreditLine.PaymentKind.ANNUITY };
            this.lines.Add(sample2);

            CreditLine sample3 = new CreditLine() { Name = "Easy cash", Id = 3, Rate = 40, KindOfPayments = CreditLine.PaymentKind.PERCENTS_ONLY };
            this.lines.Add(sample3);
        }

        public CreditLine getById(long id) {
            if (id < 1)
            {
                return null;
            }
            return lines.Find(x => x.Id == id);
        }

        public List<CreditLine> getAll() {
            return new List<CreditLine>(lines);
        }

        public static DummyCreditLineRepository getInstance()
        {
            lock (_createLock)
            {
                if (instance == null)
                {
                    instance = new DummyCreditLineRepository();
                }
            }
            return instance;
        }

        private event EventHandler wasUpdated;

        public void AddUpdateListener(EventHandler handler) {
            wasUpdated += handler;
        }

        public void Add(CreditLine oneMore) {
            oneMore.Id = lines.Max(x => x.Id) + 1;
            this.lines.Add(oneMore);
            if (wasUpdated != null)
            {
                wasUpdated(null, null);
            }
        }

    }
}
