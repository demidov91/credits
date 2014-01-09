using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coffee.Repository;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new CoffeeDb();
            db.Database.Initialize(true);
        }
    }
}
