﻿using System.Data.Entity;
using Coffee.Repository;

namespace Coffee.DbInitializer
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
