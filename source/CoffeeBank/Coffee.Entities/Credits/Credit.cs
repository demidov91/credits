using System;

namespace Coffee.Entities
{
    /// <summary>
    /// Personal credit. Some kind of "CreditLine" instance. 
    /// </summary>
    public class Credit: IUpdateable<Credit>
    {
        public long Id { get; set; }

        public DateTime IssueDate { get; set; }

        public CreditLine Line { get; set; }

        public decimal Amount { get; set; }

        public PassportInfo Passport { get; set; }

        public int Period { get; set; }

        public void Update(Credit other) {
            this.Amount = other.Amount;
            this.IssueDate = other.IssueDate;
            this.Line = other.Line;
            this.Passport = other.Passport;
            this.Period = other.Period;
            this.User = other.User;
        }

        /// <summary>
        /// Should be user, not string.
        /// </summary>
        public string User { get; set; }
    }
}
