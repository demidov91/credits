using System;

namespace Coffee.Entities
{
    public class SalaryInfo
    {
        public string Company { get; set; }

        public string Position { get; set; }

        public TimeSpan WorkTime { get; set; }

        public decimal AverageSalary { get; set; }
    }
}
