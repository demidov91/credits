using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coffee.Entities;
using Coffee.Repository;

namespace Coffee.WebUi.Controllers
{
    public class PaymentsController : Controller
    {
        [HttpGet]
        public ActionResult Accept(Credit req)
        {
            var model = new Payment();
            model.Credit = RepoFactory.GetCreditsRepo().GetCreditById(req.Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Accept(Payment p)
        {
            p.PaymentTime = DateTime.Now;
            Coffee.Repository.RepoFactory.GetCreditsRepo().AcceptPayment(p);
            return Redirect("~");
        }

        [HttpGet]
        public ActionResult PaymentChart(long creditId)
        {
            try
            {
                IRepository.ICreditRepository repo = Coffee.Repository.RepoFactory.GetCreditsRepo();
                Coffee.Entities.Credit req = repo.GetCreditById(creditId);
                decimal alreadyPaid = repo.GetPaymentsForCredit(req.Id).Sum(x => x.Amount);
                decimal currentDebt = Coffee.WebUi.Scripts.CreditHelper.GetCurrentDebt(req);
                var payments = Coffee.WebUi.Scripts.CreditHelper.GetPaymentsCalendar(req)
                    .Select(x => new Tuple<DateTime, decimal>(x.Key, x.Value)).ToList();
                return View(new Coffee.WebUi.Models.TotalCreditInfo(req, alreadyPaid, currentDebt, payments));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(404);
            }
        }

        [HttpGet]
        public ActionResult Balance(string username)
        {
            return View(Coffee.WebUi.Scripts.CreditHelper.GetAccountInfo(username));
        }
    }
}
