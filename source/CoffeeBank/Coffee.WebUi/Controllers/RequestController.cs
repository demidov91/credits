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
            List<CreditLine> acceptableCreditLines = new List<CreditLine>();
            foreach (CreditLine line in RepoFactory.GetCreditLineRepo().getAll()) {
                if (line.IsAcceptable(model)) {
                    acceptableCreditLines.Add(line);
                }
            }
            if (acceptableCreditLines.Count > 0) {
                Session["creditRequest"] = model;
                return View("SelectCreditLine", acceptableCreditLines);
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
            CreditRequest preformulatedCreditRequest = (CreditRequest)Session["creditRequest"];
            CreditLine lineFromDB = RepoFactory.GetCreditLineRepo().getById(line.Id);
            if (preformulatedCreditRequest == null) {
                Session["creditLineRequest"] = lineFromDB;
                return View("Request.New");
            }
            preformulatedCreditRequest.CreditLine = lineFromDB;
            RepoFactory.GetRequestsRepo().AddCreditRequest(preformulatedCreditRequest);
            return View("Success", preformulatedCreditRequest);
        }


        
        [Authorize]
        public ActionResult List(string passportNumber)
        {
            List<CreditRequest> requestsToShow = RepoFactory.GetRequestsRepo().GetAllCreditRequests();
            if (passportNumber != null){
                requestsToShow = requestsToShow.FindAll(x => x.PassportInfo.PassportNumber.StartsWith(passportNumber));
            }
            return View(requestsToShow);
        }

        [Authorize]
        public ActionResult Details(CreditRequest requestToView)
        {
            return View();
        }


        /*
        //
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
        public ActionResult Approve()
        {
            return View();
        }*/
    }
}
