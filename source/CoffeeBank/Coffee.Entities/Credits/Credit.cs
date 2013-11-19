using System;

namespace Coffee.Entities
{
    public class Credit
    {
        public long Id { get; set; }

        public DateTime IssueDate { get; set; }

        public CreditLine Line { get; set; }
    }
}
