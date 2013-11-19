using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coffee.IRepository
{
    public interface IDummyRepository<T>
    {
        List<T> GetAll();
    }
}
