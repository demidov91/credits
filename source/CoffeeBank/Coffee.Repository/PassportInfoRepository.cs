using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coffee.Entities;
using Coffee.IRepository;

namespace Coffee.Repository
{
    class PassportInfoRepository : IDummyRepository<PassportInfo>
    {
        private readonly CoffeeDb Context;

        public PassportInfoRepository()
        {
            Context = new CoffeeDb();
        }

        public List<PassportInfo> GetAll()
        {
            return Context.PassportInfos.AsEnumerable().ToList();
        }

        public void Update(PassportInfo newData)
        {
            PassportInfo info = Context.PassportInfos.Find(newData.Id);
            if (info != null)
            {
                info.Update(newData);
                Context.SaveChanges();
            }
        }
    }
}
