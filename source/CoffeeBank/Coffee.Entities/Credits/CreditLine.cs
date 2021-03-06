﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System;

namespace Coffee.Entities
{
    /// <summary>
    /// Kind of credit.
    /// </summary>
    public class CreditLine
    {
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Credit line title
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Credit annual rate in %
        /// </summary>
        public decimal Rate { get; set; }

        [NotMapped]
        public PaymentKind KindOfPayments { get; set; }

        [Column("PaymentKind")]
        public int KindOfPaymentsId
        {
            get { return (int)this.KindOfPayments; }

            set { this.KindOfPayments = (PaymentKind)value; }
        }

        // constraints
        public int? MinAgeBoundary { get; set; }
        public int? MaxAgeBoundary { get; set; }
        public decimal? MinAmountBoundary { get; set; }
        public decimal? MaxAmountBoundary { get; set; }
        public int? MinMonthsBoundary { get; set; }
        public int? MaxMonthsBoundary { get; set; }
        public double? MinWorkYearsBoundary { get; set; }
        public decimal? MinAverageSalaryBoundary { get; set; }

        public bool IsActive { get; set; }

		/// <summary>
        /// Light weight form of IsAcceptable. It checks only fields of the new credit request form.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool CanAcceptFirstCreditRequest(CreditRequest request) {
            return (MinAmountBoundary == null || request.Amount >= MinAmountBoundary) &&
                (MaxAmountBoundary == null || request.Amount <= MaxAmountBoundary) &&
                (MinAverageSalaryBoundary == null || request.SalaryInfo.AverageSalary >= MinAverageSalaryBoundary);
        }

        public bool IsAcceptable(CreditRequest request)
        {
            return
                (MinAmountBoundary == null || request.Amount >= MinAmountBoundary) && 
                (MaxAmountBoundary == null || request.Amount <= MaxAmountBoundary) &&
                (MinAgeBoundary == null || (DateTimeHelper.GetCurrentTime() - request.PassportInfo.BirthDate).TotalDays >= MinAgeBoundary * 365) &&
                (MaxAgeBoundary == null || (DateTimeHelper.GetCurrentTime() - request.PassportInfo.BirthDate).TotalDays <= MaxAgeBoundary * 365) &&
                (MinMonthsBoundary == null || request.Period >= MinMonthsBoundary) && 
                (MaxMonthsBoundary == null || request.Period <= MaxMonthsBoundary) &&
                (MinAverageSalaryBoundary == null || request.SalaryInfo.AverageSalary >= MinAverageSalaryBoundary) &&
                (MinWorkYearsBoundary == null || request.SalaryInfo.WorkYears >= MinWorkYearsBoundary);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(string.Format("Credit line \"{0}\" with {1}% rate.", Name, Rate));

            if (MinAgeBoundary != null || MaxAgeBoundary != null || MinAmountBoundary != null || MaxAmountBoundary != null ||
                MinMonthsBoundary != null || MaxMonthsBoundary != null || MinWorkYearsBoundary != null || MinAverageSalaryBoundary != null)
            {
                builder.AppendLine("Credit requirements:");
                if (MinAgeBoundary != null && MaxAgeBoundary != null)
                    builder.AppendLine(string.Format("{0} - {1} years old", MinAgeBoundary, MaxAgeBoundary));
                else if (MinAgeBoundary != null)
                    builder.AppendLine(string.Format("No less than {0} years old.", MinAgeBoundary));
                else if (MaxAgeBoundary != null)
                    builder.AppendLine(string.Format("No more than {0} years old.", MaxAgeBoundary));

                if (MinAmountBoundary != null && MaxAmountBoundary != null)
                    builder.AppendLine(string.Format("From {0} to {1} belorussian rubles", MinAmountBoundary, MaxAmountBoundary));
                else if (MinAmountBoundary != null)
                    builder.AppendLine(string.Format("From {0} belorussian rubles", MinAmountBoundary));
                else if (MaxAmountBoundary != null)
                    builder.AppendLine(string.Format("Up to {0} belorussian rubles", MaxAmountBoundary));

                if (MinMonthsBoundary != null && MaxMonthsBoundary != null)
                    builder.AppendLine(string.Format("{0} - {1} months", MinMonthsBoundary, MaxMonthsBoundary));
                else if (MinMonthsBoundary != null)
                    builder.AppendLine(string.Format("Form {0} months", MinMonthsBoundary));
                else if (MaxMonthsBoundary != null)
                    builder.AppendLine(string.Format("Up to {0} months", MaxMonthsBoundary));

                if (MinWorkYearsBoundary != null)
                    builder.AppendLine(string.Format("No less than {0} years working in your current company", MinWorkYearsBoundary));

                if (MinAverageSalaryBoundary != null)
                    builder.AppendLine(string.Format("No less than {0}BYR average salary", (int)MinAverageSalaryBoundary));
            }
            else
            {
                builder.AppendLine("Only passport needed!");
            }

            return builder.ToString();
            /*
                Credit line "Easy Cash" with 39% rate.
                Credit requirements:
                21 - 65 years old
                1000000 - 40000000 belorussian rubles
                6 - 24 months
             */
        }

        public CreditLine()
        {
            IsActive = true;
        }
    }

    /// <summary>
    /// ANNUITY - same payment amount for each month.
    /// FACTICAL - pay equal share of credit amount each month + less and less percents. 
    /// PERCENTS_ONLY - pay only percents during all the credit period + all credit in the end.
    /// </summary>
    public enum PaymentKind { ANNUITY, FACTICAL, PERCENTS_ONLY }
}
