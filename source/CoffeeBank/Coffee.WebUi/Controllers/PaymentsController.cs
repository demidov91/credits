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
        [Authorize(Roles = "Cashier, CoffeeAdmin")]
        public ActionResult Accept(long reqId)
        {
            var model = new Payment();
            model.Credit = Repository.RepoFactory.GetCreditsRepo().GetCreditById(reqId);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Cashier, CoffeeAdmin")]
        public ActionResult Accept(Payment p)
        {
            p.PaymentTime = DateTimeHelper.GetCurrentTime();
            p.Credit = Repository.RepoFactory.GetCreditsRepo().GetCreditById(p.Credit.Id);
            Coffee.Repository.RepoFactory.GetCreditsRepo().AcceptPayment(p);
            return Redirect("~");
        }

        [HttpGet]
        [Authorize(Roles = "Cashier, Clerk, CoffeeAdmin")]
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
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Cashier, Clerk, CoffeeAdmin")]
        public ActionResult Balance(string username)
        {
            return View(Coffee.WebUi.Scripts.CreditHelper.GetAccountInfo(username));
        }
    }
}

