using System.Collections.Generic;
using System;


namespace Coffee.Entities
{
    public class CreditRequest
    {
        public long Id { get; set; }

        public decimal Amount { get; set; } //In belorussian rubles only

        public int Period { get; set; } //in months

        public PassportInfo PassportInfo { get; set; }

        public SalaryInfo SalaryInfo { get; set; }

        public CreditLine CreditLine { get; set; }

        public List<Approval> Approvals { get; set; }

        public DateTime IssueDate { get; set; }
        

        public string AdditionalTextInfo { get; set; }

        /// <summary>
        /// *User* is better but I thought we had 2 different databases fot these entities. 
        /// </summary>
        public string Username { get; set; }
        

        //...some other info

        public CreditRequest()
        {
            PassportInfo = new PassportInfo();
            SalaryInfo = new SalaryInfo();
        }
    }
}
