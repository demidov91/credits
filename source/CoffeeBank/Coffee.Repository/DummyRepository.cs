using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Coffee.Entities;
using Coffee.IRepository;

namespace Coffee.Repository
{
    class DummyRepository<T>: IDummyRepository<T> where T: Coffee.Entities.IUpdateable<T>
    {
        protected static DummyRepository<T> instance = null;
        protected static Object _createLock = new Object();

        protected LinkedList<T> items = new LinkedList<T>();
        public static DummyRepository<T> getInstance()
        {
            lock (_createLock)
            {
                if (instance == null)
                {
                    instance = new DummyRepository<T>();
                }
            }
            return instance;
        }

        private DummyRepository(){}

        public List<T> GetAll() {
            return new List<T>(this.items);
        }

        public void Update(T newData){
            
        }

    }
}
