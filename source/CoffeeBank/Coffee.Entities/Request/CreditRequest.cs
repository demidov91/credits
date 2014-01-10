using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;


namespace Coffee.Entities
{
    public class CreditRequest
    {
        [Key]
        public long Id { get; set; }

        public decimal Amount { get; set; } //In belorussian rubles only

        public int Period { get; set; } //in months

        public virtual PassportInfo PassportInfo { get; set; }

        public virtual SalaryInfo SalaryInfo { get; set; }

        public virtual CreditLine CreditLine { get; set; }

        public virtual Decision Decision { get; set; }

        public DateTime IssueDate { get; set; }
        

        public string AdditionalTextInfo { get; set; }

        /// <summary>
        /// *User* is better but I thought we had 2 different databases fot these entities. 
        /// </summary>
        public string Username { get; set; }
        

        //...some other info

        public CreditRequest(bool activate)
        {
            PassportInfo = new PassportInfo();
            SalaryInfo = new SalaryInfo();
            IssueDate = new DateTime(2000, 1, 1);
        }

        public CreditRequest()
        {
        }

    }
}
