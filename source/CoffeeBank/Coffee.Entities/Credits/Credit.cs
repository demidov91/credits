using System;
using System.ComponentModel.DataAnnotations;

namespace Coffee.Entities
{
    /// <summary>
    /// Personal credit. Some kind of "CreditLine" instance. 
    /// </summary>
    public class Credit: IUpdateable<Credit>
    {
        [Key]
        public long Id { get; set; }

        public virtual DateTime IssueDate { get; set; }

        public virtual CreditLine Line { get; set; }

        public decimal Amount { get; set; }

        public virtual PassportInfo Passport { get; set; }

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

        public override string ToString()
        {
            return string.Format("Credit \"{0}\", {1}BYR for {2} months issued on {3} to {4}, passport №{5}",
                Line.Name, Amount, Period, IssueDate.ToLocalDate(), Passport.FullName, Passport.PassportNumber);
        }
    }
}
