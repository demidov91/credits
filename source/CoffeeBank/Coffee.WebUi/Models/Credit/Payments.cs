using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Coffee.Entities;
using Coffee.WebUi.Scripts;

namespace Coffee.WebUi.Models.Credit
{
    public class Payments
    {
        private SortedDictionary<DateTime, decimal> _calendar = null;
       

        public IDictionary<DateTime, decimal> Calendar {get {
            if (_calendar == null){
                _calendar = CreditHelper.GetPaymentsCalendar(this.CreditRequest);
            }
            return _calendar;
        }}


        public CreditRequest CreditRequest { get; private set;  }

        public Payments(CreditRequest request) {
            this.CreditRequest = request;
        }
    }
}