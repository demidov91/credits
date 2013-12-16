using System;
using Coffee.Entities;
using System.Collections.Generic;

namespace Coffee.WebUi.Models
{
    public class TotalCreditInfo
    {
        public TotalCreditInfo() { }
        public Coffee.Entities.Credit Item1 { get; set; }
        public decimal Item2 { get; set; }
        public decimal Item3 { get; set; }
        public List<Tuple<DateTime, decimal>> Item4 { get; set; }

        public TotalCreditInfo(Coffee.Entities.Credit item1, decimal item2, decimal item3, List<Tuple<DateTime, decimal>> item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }
    }

    public class TotalAccountInfo
    {
        public TotalAccountInfo() { }
        public decimal TotalDebt { get; set; }
        public List<Tuple<Coffee.Entities.Credit, decimal, decimal>> Info { get; set; }
        public string Username { get; set; }

        public TotalAccountInfo(List<Tuple<Coffee.Entities.Credit, decimal, decimal>> info, decimal totalDebt, string username)
        {
            Info = info;
            TotalDebt = totalDebt;
            Username = username;
        }
    }
}
