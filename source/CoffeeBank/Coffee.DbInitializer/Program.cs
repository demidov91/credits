using System.Data.Entity;
using Coffee.Repository;

namespace Coffee.DbInitializer
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new CoffeeDb();
            //Database.SetInitializer<CoffeeDb>(null);
            db.Database.Initialize(true);
        }
    }
}
