using System;
using System.ComponentModel.DataAnnotations;

namespace Coffee.Entities
{
    public class SalaryInfo
    {
        [Key]
        public long Id { get; set; }

        public string Company { get; set; }

        public string Position { get; set; }

        public TimeSpan WorkTime { get; set; }

        public decimal AverageSalary { get; set; }
    }
}
