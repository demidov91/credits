using System.Web.Mvc;
using Coffee.Entities;
using Coffee.Repository;
using Coffee.IRepository;

using System.Collections.Generic;

using Coffee.WebUi.Models; 

namespace Coffee.WebUi.Controllers
{
    public class RequestController : Controller
    {
        [HttpGet]
        public ActionResult New()
        {
            CreditRequestFormModel model = new CreditRequestFormModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult New(CreditRequestFormModel form)
        {
            CreditRequest model = form.CreditRequest;
            List<CreditLine> acceptableCreditLines = new List<CreditLine>();
            foreach (CreditLine line in RepoFactory.GetCreditLineRepo().getAll()) {
                if (line.IsAcceptable(model)) {
                    acceptableCreditLines.Add(line);
                }
            }
            model.Username = User.Identity.Name;
            if (acceptableCreditLines.Count > 0) {
                Session["creditRequest"] = model;
                return View("SelectCreditLine", acceptableCreditLines);
            }
            if (form.ForceSave)
            {
                RepoFactory.GetRequestsRepo().AddCreditRequest(model);
                return View("Success", model);
            }
            else {
                form.NoLines = true;
            }
            return View(form);
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
            if (preformulatedCreditRequest == null) {
                Session["creditLineRequest"] = line;
                return View("Request.New");
            }
            preformulatedCreditRequest.CreditLine = line;
            RepoFactory.GetRequestsRepo().AddCreditRequest(preformulatedCreditRequest);
            return View("Success", preformulatedCreditRequest);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AjaxUpdateRequest(RequestPlusCreditProposesModel rawData)
        {
            CreditRequest request = rawData.Request;
            
            bool isOwner = User.Identity.Name.Equals(request.Username);
            if (!isOwner) {
                Response.StatusCode = 403;
                return Json(null);
            }
            CreditRequest fromDb =  RepoFactory.GetRequestsRepo().Update(request);
            return PartialView("_RowForTheUserCreditRequest", new RequestPlusCreditProposesModel(fromDb, RepoFactory.GetCreditLineRepo().getAll()));
        }


        
        [Authorize]
        public ActionResult List(string passportNumber)
        {
            List<CreditRequest> requestsToShow = RepoFactory.GetRequestsRepo().GetAllCreditRequests();
            bool isSimpleUser = User.Identity.Name != "BankWorker";
            if(isSimpleUser){
                requestsToShow = requestsToShow.FindAll(x => x.Username == User.Identity.Name);
                return View("ListForUser", new RequestsPlusCreditProposesModel { 
                    Requests=requestsToShow,
                    Credits=RepoFactory.GetCreditLineRepo().getAll()
                });
            }
            if (passportNumber != null)
            {
                requestsToShow = requestsToShow.FindAll(x => x.PassportInfo.PassportNumber.StartsWith(passportNumber));
            }
            return View(requestsToShow);
        }


        [Authorize]
        [HttpGet]
        public ActionResult Details(CreditRequest requestToView)
        {
            return View(RepoFactory.GetRequestsRepo().GetRequestById(requestToView.Id));
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
