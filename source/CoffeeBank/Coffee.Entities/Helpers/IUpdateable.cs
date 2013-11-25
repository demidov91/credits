using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coffee.Entities
{
    public interface IUpdateable<T>
    {
        void Update(T other);

    }
}
