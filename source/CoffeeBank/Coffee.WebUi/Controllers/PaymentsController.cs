using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coffee.Entities;

namespace Coffee.WebUi.Controllers
{
    public class PaymentsController : Controller
    {
        [HttpGet]
        public ActionResult AcceptPayment(CreditRequest req)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Accept(Coffee.Entities.Payment p)
        {
            (Coffee.Repository.RepoFactory.GetRequestsRepo() as Coffee.Repository.DummyRequestsRepository).AcceptPayment(p);
            return Redirect("~");
        }

        public ActionResult PaymentChart(long creditId)
        {
            try
            {
                IRepository.IRequestRepository repo = Coffee.Repository.RepoFactory.GetRequestsRepo();
                Coffee.Entities.CreditRequest req = repo.GetRequestById(creditId);
                decimal alreadyPaid = repo.GetPaymentsForCredit(req.Id).Sum(x => x.Amount), ap = alreadyPaid;
                var payments = Coffee.WebUi.Scripts.CreditHelper.GetPaymentsCalendar(req);
                decimal totalToPay = payments.Sum(x => x.Value);
                List<Tuple<DateTime, decimal>> model = new List<Tuple<DateTime, decimal>>();

                foreach (var x in payments)
                {
                    if (alreadyPaid >= x.Value) alreadyPaid -= x.Value;
                    else if (alreadyPaid > 0)
                    {
                        model.Add(new Tuple<DateTime, decimal>(x.Key, x.Value - alreadyPaid));
                        alreadyPaid = -1;
                    }
                    else model.Add(new Tuple<DateTime, decimal>(x.Key, x.Value));
                }

                return View(new Tuple<CreditRequest, decimal, decimal, List<Tuple<DateTime, decimal>>>(req, ap, totalToPay, model));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(404);
            }
        }
    }
}
