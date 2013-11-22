using System;

namespace Coffee.Entities
{
    /// <summary>
    /// Personal credit. Some kind of "CreditLine" instance. 
    /// </summary>
    public class Credit
    {
        public long Id { get; set; }

        public DateTime IssueDate { get; set; }

        public CreditLine Line { get; set; }


        /// <summary>
        /// Should be user, not string.
        /// </summary>
        public string User { get; set; }
    }
}
