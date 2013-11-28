using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coffee.Entities;

namespace Coffee.WebUi.Scripts
{
    public class CreditHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration">Number of months.</param>
        /// <param name="amount"></param>
        /// <param name="percent">0.4 for 40% / year, for example.</param>
        /// <returns></returns>
        private static List<decimal> GetAnnuityPaymentsList(decimal amount, decimal percent, int duration) {
            percent = percent / 12;
            double koeficientFromPrettyDocThatLeschevSentUs = (double)percent / (1 - 1 / Math.Pow((double)(1 + percent), (double)duration));
            double perMonth = (double)amount * koeficientFromPrettyDocThatLeschevSentUs;
            List<decimal> result = new List<decimal>(duration);
            for (int i = 0; i < duration; i++) {
                result.Add((decimal)perMonth);
            }
            return result;
        }

        /// <summary>
        /// Get list of payments per month.
        /// </summary>
        /// <param name="amount">Money, bank gives the client.</param>
        /// <param name="percent">For example, *0.4* for 40% / year.</param>
        /// <param name="duration">Number of months.</param>
        /// <returns>List of payments to do.</returns>
        private static List<decimal> GetFacticalPaymentsList(decimal amount, decimal percent, int duration) {
            decimal monthPercent = percent / 12;
            decimal partOfCreditPerMonth = amount / duration;
            List<decimal> payments = new List<decimal>(duration);
            decimal remainigToPay = amount;
            for (int i = 0; i < duration; i++) {
                payments.Add(partOfCreditPerMonth + remainigToPay * monthPercent);
                remainigToPay -= partOfCreditPerMonth;
            }
            return payments;
        }

        /// <summary>
        /// Get list of payments per month.
        /// </summary>
        /// <param name="amount">Money, bank gives the client.</param>
        /// <param name="percent">For example, *0.4* for 40% / year.</param>
        /// <param name="duration">Number of months.</param>
        /// <returns>List of payments to do.</returns>
        private static List<decimal> GetOnlyPercentsPaymentsList(decimal amount, decimal percent, int duration) {
            decimal payPerMonth = amount * percent / 12;
            List<decimal> payments = new List<decimal>(duration);
            for (int i = 0; i < duration; i++) {
                payments.Add(payPerMonth);
            }
            payments[payments.Count - 1] =  payments.Last() + amount;
            return payments;
        }


        public static SortedDictionary<DateTime, decimal> GetPaymentsCalendar(Coffee.Entities.CreditRequest request){
            SortedDictionary<DateTime, decimal> paymentsTodo = new SortedDictionary<DateTime, decimal>();
            List<decimal> payments = null;
            switch (request.CreditLine.KindOfPayments) { 
                case CreditLine.PaymentKind.ANNUITY:
                    payments = GetAnnuityPaymentsList(request.Amount, request.CreditLine.Rate / 100, request.Period);
                    break;
                case CreditLine.PaymentKind.FACTICAL:
                    payments = GetFacticalPaymentsList(request.Amount, request.CreditLine.Rate / 100, request.Period);
                    break;
                case CreditLine.PaymentKind.PERCENTS_ONLY:
                    payments = GetOnlyPercentsPaymentsList(request.Amount, request.CreditLine.Rate / 100, request.Period);
                    break;
                default:
                    throw new NotImplementedException("Unknown kind of credit.");
            }
            DateTime dateToCountFor = new DateTime(request.IssueDate.Ticks);
            foreach (decimal payment in payments) {
                dateToCountFor = dateToCountFor.AddMonths(1);
                paymentsTodo.Add(dateToCountFor, payment);
            }
            return paymentsTodo;
        }

    }
}