using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coffee.Entities;
using Coffee.WebUi.Models;

namespace Coffee.WebUi.Scripts
{
    public class CreditHelper
    {
        /// <summary>
        /// Rounds all payments so that they are multiplies of X rubles
        /// </summary>
        /// <param name="list">List of decimals to normalize</param>
        /// <param name="leastNominal">Least valued banknote used</param>
        /// <returns>List of integer payments</returns>
        private static List<decimal> Normalize(List<decimal> list, int leastNominal = 50)
        {
            decimal roundError = 0;
            List<decimal> result = new List<decimal>();
            foreach (decimal x in list)
            {
                decimal p = (Math.Ceiling(x / leastNominal)) * leastNominal;
                roundError += (p - x);
                result.Add(p);
            }

            result[result.Count - 1] -= Math.Floor(roundError / leastNominal) * leastNominal;
            return result;
        }

        /// <summary>
        /// Rounds a number so that it is a multiple of X rubles
        /// </summary>
        /// <param name="amount">Decimal to normalize up.</param>
        /// <param name="leastNominal">Least valued banknote used</param>
        /// <returns>Decimal </returns>
        private static decimal Normalize(decimal amount, int leastNominal = 50)
        {
            return Math.Ceiling(amount / leastNominal) * leastNominal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration">Number of months.</param>
        /// <param name="amount"></param>
        /// <param name="percent">0.4 for 40% / year, for example.</param>
        /// <returns></returns>
        private static List<decimal> GetAnnuityPaymentsList(decimal amount, decimal percent, int duration)
        {
            //        m
            //      Ap (p - 1)                 percent
            // x = ------------, where p = 1 + -------
            //         m                         12
            //        p  - 1

            List<decimal> result = new List<decimal>(duration);
            if (Math.Abs(percent) < 1e-9M)
            {
                for (int i = 0; i < duration; ++i) result.Add(amount / duration);
            }
            else
            {
                double p = 1 + (double)(percent / 12), m = duration, A = (double)amount;
                double x = (A * Math.Pow(p, m) * (p - 1)) / (Math.Pow(p, m) - 1);
                for (int i = 0; i < duration; ++i) result.Add((decimal)x);
            }
            return Normalize(result);
        }

        /// <summary>
        /// Get list of payments per month.
        /// </summary>
        /// <param name="amount">Money, bank gives the client.</param>
        /// <param name="percent">For example, *0.4* for 40% / year.</param>
        /// <param name="duration">Number of months.</param>
        /// <returns>List of payments to do.</returns>
        private static List<decimal> GetFacticalPaymentsList(decimal amount, decimal percent, int duration)
        {
            decimal monthPercent = percent / 12;
            decimal partOfCreditPerMonth = amount / duration;
            List<decimal> payments = new List<decimal>(duration);
            decimal remainigToPay = amount;
            for (int i = 0; i < duration; i++)
            {
                payments.Add(partOfCreditPerMonth + remainigToPay * monthPercent);
                remainigToPay -= partOfCreditPerMonth;
            }
            return Normalize(payments);
        }

        /// <summary>
        /// Get list of payments per month.
        /// </summary>
        /// <param name="amount">Money, bank gives the client.</param>
        /// <param name="percent">For example, *0.4* for 40% / year.</param>
        /// <param name="duration">Number of months.</param>
        /// <returns>List of payments to do.</returns>
        private static List<decimal> GetOnlyPercentsPaymentsList(decimal amount, decimal percent, int duration)
        {
            decimal payPerMonth = amount * percent / 12;
            List<decimal> payments = new List<decimal>(duration);
            for (int i = 0; i < duration; i++)
            {
                payments.Add(payPerMonth);
            }
            payments[payments.Count - 1] = payments.Last() + amount;
            return Normalize(payments);
        }

        public static SortedDictionary<DateTime, decimal> GetPaymentsCalendar(Coffee.Entities.CreditRequest request)
        {
            SortedDictionary<DateTime, decimal> paymentsTodo = new SortedDictionary<DateTime, decimal>();
            List<decimal> payments = null;
            switch (request.CreditLine.KindOfPayments)
            {
                case PaymentKind.ANNUITY:
                    payments = GetAnnuityPaymentsList(request.Amount, request.CreditLine.Rate / 100, request.Period);
                    break;
                case PaymentKind.FACTICAL:
                    payments = GetFacticalPaymentsList(request.Amount, request.CreditLine.Rate / 100, request.Period);
                    break;
                case PaymentKind.PERCENTS_ONLY:
                    payments = GetOnlyPercentsPaymentsList(request.Amount, request.CreditLine.Rate / 100, request.Period);
                    break;
                default:
                    throw new NotImplementedException("Unknown kind of credit.");
            }
            DateTime dateToCountFor = new DateTime(request.IssueDate.Ticks);
            foreach (decimal payment in payments)
            {
                dateToCountFor = dateToCountFor.AddMonths(1);
                paymentsTodo.Add(dateToCountFor, payment);
            }
            return paymentsTodo;
        }

        public static SortedDictionary<DateTime, decimal> GetPaymentsCalendar(Coffee.Entities.Credit credit)
        {
            SortedDictionary<DateTime, decimal> payments = new SortedDictionary<DateTime, decimal>();
            List<decimal> paymentAmounts = null;

            int duration;
            DateTime endDate = credit.IssueDate.AddMonths(credit.Period).Date, startOfMonth, start;
            if ((DateTimeHelper.GetCurrentTime().Date.Month == credit.IssueDate.Date.Month) ||
                (DateTimeHelper.GetCurrentTime().Date.Month == credit.IssueDate.Date.Month + 1 &&
                 DateTimeHelper.GetCurrentTime().Date.Day <= credit.IssueDate.Date.Day))
            {
                duration = credit.Period;
                startOfMonth = credit.IssueDate.Date;
                start = startOfMonth.AddMonths(1);
            }
            else
            {
                DateTime currentDate = DateTimeHelper.GetCurrentTime().Date;
                duration = (endDate.Year - currentDate.Year) * 12 +
                    (endDate.Month - currentDate.Month) + (currentDate.Day <= endDate.Day ? 1 : 0);
                startOfMonth = credit.IssueDate.Date.AddDays(1);
                while (startOfMonth.AddMonths(1) <= DateTimeHelper.GetCurrentTime().Date) startOfMonth = startOfMonth.AddMonths(1);
                start = startOfMonth.AddMonths(1).AddDays(-1);
            }

            List<Payment> p = Repository.RepoFactory.GetCreditsRepo().GetPaymentsForCredit(credit.Id);
            decimal amount = GetCurrentDebt(credit, p.Where(x => x.PaymentTime < startOfMonth).ToList()) / (1 + credit.Line.Rate / 1200);

            switch (credit.Line.KindOfPayments)
            {
                case PaymentKind.ANNUITY:
                    paymentAmounts = GetAnnuityPaymentsList(amount, credit.Line.Rate / 100, duration);
                    break;
                case PaymentKind.FACTICAL:
                    paymentAmounts = GetFacticalPaymentsList(amount, credit.Line.Rate / 100, duration);
                    break;
                case PaymentKind.PERCENTS_ONLY:
                    paymentAmounts = GetOnlyPercentsPaymentsList(amount, credit.Line.Rate / 100, duration);
                    break;
                default: throw new NotImplementedException();
            }
            decimal paidThisMonth = p.Where(x => x.PaymentTime >= startOfMonth).Sum(x => x.Amount);
            for (int i = 0; i < paymentAmounts.Count; ++i)
            {
                if (paidThisMonth == 0) break;
                decimal min = Math.Min(paymentAmounts[i], paidThisMonth);
                paymentAmounts[i] -= min;
                paidThisMonth -= min;
            }

            foreach (decimal payment in paymentAmounts)
            {
                payments.Add(start, payment);
                start = start.AddMonths(1);
            }
            return payments;
        }

        public static decimal GetCurrentDebt(Credit credit, int leastNominal = 50)
        {
            return GetCurrentDebt(credit, Repository.RepoFactory.GetCreditsRepo().GetPaymentsForCredit(credit.Id), leastNominal);
        }

        private static decimal GetCurrentDebt(Credit credit, List<Payment> p, int leastNominal = 50)
        {
            decimal ans = credit.Amount;
            DateTime start = credit.IssueDate.Date, next = start.AddMonths(1);
            int ptr = 0;
            do
            {
                ans *= (1 + credit.Line.Rate / 1200);
                while (ptr < p.Count && p[ptr].PaymentTime.Date <= next) ans -= p[ptr++].Amount;
                start = next;
                next = next.AddMonths(1);
            }
            while (start < DateTimeHelper.GetCurrentTime().Date);
            return (Math.Ceiling(ans / leastNominal)) * leastNominal;
        }

        public static TotalAccountInfo GetAccountInfo(string username)
        {
            IRepository.ICreditRepository repo = Repository.RepoFactory.GetCreditsRepo();
            List<Tuple<Credit, decimal, decimal>> info =
                (from c in repo.GetCreditsByOwner(username)
                 let p = repo.GetPaymentsForCredit(c.Id)
                 select new Tuple<Credit, decimal, decimal>(c, p.Sum(x => x.Amount), GetCurrentDebt(c, p)))
                .ToList();
            return new TotalAccountInfo(info, info.Sum(x => x.Item3), username);
        }
    }
}