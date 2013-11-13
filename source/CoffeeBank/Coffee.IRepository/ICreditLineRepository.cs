using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Coffee.Entities; 


namespace Coffee.IRepository
{
    public interface ICreditLineRepository
    {
        List<CreditLine> getAll();

    }
}
