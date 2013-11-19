using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Coffee.Entities;

namespace Coffee.WebUi.Models
{
    public class CreditRequestFormModel
    {
        public CreditRequest CreditRequest { get; set; }
        public Boolean NoLines  { get; set; }
        public Boolean ForceSave { get; set; }
    }
}