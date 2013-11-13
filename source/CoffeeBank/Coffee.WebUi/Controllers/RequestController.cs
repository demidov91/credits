using System.Web.Mvc;
using Coffee.Entities;
using Coffee.Repository;
using Coffee.IRepository;

using System.Collections.Generic;

namespace Coffee.WebUi.Controllers
{
    public class RequestController : Controller
    {
        [HttpGet]
        public ActionResult New()
        {
            CreditRequest model = new CreditRequest();
            return View(model);
        }

        [HttpPost]
        public ActionResult New(CreditRequest model)
        {
            LinkedList<CreditLine> acceptableCreditLines = new LinkedList<CreditLine>();
            foreach (CreditLine line in RepoFactory.GetCreditLineRepo().getAll()) {
                if (line.IsAcceptable(model)) {
                    acceptableCreditLines.AddLast(line);
                }
            }
            IRequestRepository repository = RepoFactory.GetRequestsRepo();
            repository.AddCreditRequest(model);
            if (acceptableCreditLines.Count > 0) {
                Session["creditRequest"] = model;
                return View("Request.SelectCreditLine", acceptableCreditLines);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult SelectCreditLine()
        {   
            return View(RepoFactory.GetCreditLineRepo().getAll());
        }

        [HttpPost]
        public ActionResult SelectCreditLine(CreditLine line)
        {
            return null ;
        }


        /*
        //[Authorize]
        public ActionResult List()
        {
            return View();
        }

        //[Authorize]
        public ActionResult ApprovedList()
        {
            return View();
        }

        //[Authorize]
        public ActionResult UnapprovedList()
        {
            return View();
        }

        //[Authorize]
        public ActionResult Details()
        {
            return View();
        }

        //[Authorize]
        public ActionResult Approve()
        {
            return View();
        }*/
    }
}
