using System;

namespace Coffee.Entities
{
    public class SalaryBoundary : IRequestBoundary
    {
        public long Id { get; set; }

        public TimeSpan MinWorkTime { get; set; }

        public decimal MinAverageSalary { get; set; }

        public SalaryBoundary() { }

        public SalaryBoundary(TimeSpan minWorkTime, decimal minAverageSalary)
        {
            MinWorkTime = minWorkTime;
            MinAverageSalary = minAverageSalary;
        }

        public bool IsAcceptable(CreditRequest request)
        {
            return (request.SalaryInfo.AverageSalary >= MinAverageSalary) &&
                   (request.SalaryInfo.WorkTime >= MinWorkTime);
        }

        public string Visualize()
        {
            return string.Format("No less than {0} months working in your current company, no less than {1}BYR average salary", 
                (int)MinWorkTime.TotalDays / 30, (int)MinAverageSalary);
        }
    }
}