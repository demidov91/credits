using System.Collections.Generic;
using System.Web.Mvc;
using Coffee.Entities;
using Coffee.Repository;
using Coffee.WebUi.Models.Credit;
using System.Linq;

namespace Coffee.WebUi.Controllers
{
    public class CreditLineController : Controller
    {
        public ActionResult List()
        {
            List<CreditLine> model = new List<CreditLine>(RepoFactory.GetCreditLineRepo().getAll().Where(x => x.IsActive));
            
            if (!User.Identity.IsAuthenticated) {
                return View("ListNoneAuth", model);
            }
            return View(model);
        }

        public ActionResult Detail(long id)
        {
            CreditLine model = RepoFactory.GetCreditLineRepo().getById(id);

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult New(CreditLine line)
        {
            RepoFactory.GetCreditLineRepo().Add(line);
            return Redirect("~");
        }

        [HttpGet]
        [Authorize(Roles = "Clerk, CoffeeAdmin")]
        public ActionResult TeoreticalPayments(Coffee.Entities.CreditRequest creditRequest)
        {
            return View(new Payments(RepoFactory.GetRequestsRepo().GetRequestById(creditRequest.Id)));
        }

        [HttpGet]
        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult ListForCommittee()
        {
            List<CreditLine> model = RepoFactory.GetCreditLineRepo().getAll();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult ActivateDeactivate(CreditLine line)
        {
            CreditLine fromDB =  RepoFactory.GetCreditLineRepo().getById(line.Id);
            fromDB.IsActive = !line.IsActive;
            RepoFactory.GetCreditLineRepo().Update(fromDB);
            return RedirectToAction("ListForCommittee", "CreditLine");
        }

    }
}
