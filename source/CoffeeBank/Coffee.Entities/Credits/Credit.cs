using System;
using System.Collections.Generic;

namespace Coffee.Entities
{
    public class Credit
    {
        public long Id { get; set; }

        public DateTime IssueDate { get; set; }

        public CreditLine Line { get; set; }

        public CreditRequest Request { get; set; }

        public List<Approval> Approvals { get; set; }
    }
}
