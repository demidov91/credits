using System;
using System.Data.Entity;
using Coffee.Repository;
using Coffee.Entities;

namespace Coffee.DbInitializer
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new CoffeeDb();
            db.Database.Initialize(true);
            db.PassportInfos.Add(new PassportInfo
            {
                BirthDate = DateTime.Now,
                ExpireDate = DateTime.Now,
                IssueDate = DateTime.Now,
                FirstName = "",
                Gender = Gender.Male,
                IdentificationNumber = "",
                PassportNumber = "12_crap"
            });

            db.PassportInfos.Add(new PassportInfo
            {
                BirthDate = DateTime.Now,
                ExpireDate = DateTime.Now,
                IssueDate = DateTime.Now,
                FirstName = "",
                Gender = Gender.Male,
                IdentificationNumber = "",
                PassportNumber = "12_crap"
            });

            db.PassportInfos.Add(new PassportInfo
            {
                BirthDate = DateTime.Now,
                ExpireDate = DateTime.Now,
                IssueDate = DateTime.Now,
                FirstName = "",
                Gender = Gender.Male,
                IdentificationNumber = "",
                PassportNumber = "12_crap"
            });

            db.PassportInfos.Add(new PassportInfo
            {
                BirthDate = DateTime.Now,
                ExpireDate = DateTime.Now,
                IssueDate = DateTime.Now,
                FirstName = "",
                Gender = Gender.Male,
                IdentificationNumber = "",
                PassportNumber = "12_crap"
            });

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                
                throw;
            }
            
            
        }
    }
}
