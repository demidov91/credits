using System.Collections.Generic;
using System.Web.Mvc;
using Coffee.Entities;
using Coffee.Repository;

using Coffee.Repository;
using Coffee.WebUi.Models.Credit;

namespace Coffee.WebUi.Controllers
{
    public class CreditLineController : Controller
    {
        public ActionResult List()
        {
            List<CreditLine> model = RepoFactory.GetCreditLineRepo().getAll();
            return View(model);
        }

        public ActionResult Detail(long id)
        {
            CreditLine model = RepoFactory.GetCreditLineRepo().getById(id);
            return View(model);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TeoreticalPayments(Coffee.Entities.CreditRequest creditRequest){
            return View(new Payments(RepoFactory.GetRequestsRepo().GetRequestById(creditRequest.Id)));
        }
    }
}
