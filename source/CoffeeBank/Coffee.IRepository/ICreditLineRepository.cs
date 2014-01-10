using System;
using System.Collections.Generic;
using Coffee.Entities; 


namespace Coffee.IRepository
{
    public interface ICreditLineRepository
    {
        List<CreditLine> getAll();

        CreditLine getById(long id);

        void Add(CreditLine oneMore);
        
        void AddUpdateListener(EventHandler handler);

    }
}
