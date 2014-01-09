using System.Linq;
using System.Web.Mvc;
using Coffee.Entities;
using Coffee.Repository;
using System.Collections.Generic;
using Coffee.WebUi.Models;
using Coffee.WebUi.Scripts;

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
            List<CreditLine> acceptableCreditLines = RepoFactory.GetCreditLineRepo().getAll()
                .Where(line => line.IsAcceptable(model)).ToList();

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


            form.NoLines = true;
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
                return View("New");
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
        
        [Authorize(Roles = "Clerk, Cashier, CoffeeAdmin, Committee")]
        public ActionResult List(string passportNumber)
        {
            List<CreditRequest> requestsToShow = RepoFactory.GetRequestsRepo().GetAllCreditRequests();
            if(MembershipHelper.IsExternalUser(User)){
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

        [Authorize(Roles = "Clerk, Cashier, CoffeeAdmin, Committee")]
        [HttpGet]
        public ActionResult Details(CreditRequest requestToView)
        {
            var request = RepoFactory.GetRequestsRepo().GetRequestById(requestToView.Id);
            request.IssueDate = DateTimeHelper.GetCurrentTime();
            return View(new Coffee.WebUi.Models.Request.CreditRequest(request));
        }

        [Authorize(Roles = "Clerk, Cashier, CoffeeAdmin, Committee")]
        [HttpPost]
        public ActionResult Details(Coffee.WebUi.Models.Request.CreditRequest requestFromView)
        {
            Coffee.Entities.CreditRequest requestFromDB = RepoFactory.GetRequestsRepo().Update(requestFromView.GetAdaptee());
            RepoFactory.GetPassportInfoRepo().Update(requestFromView.PassportInfo);

            if (requestFromView.ActionEdit != null){
                return RedirectToAction("Details", "Request", new { Id = requestFromView.Id });
            } else if (requestFromView.ActionViewPayments != null) {
                return RedirectToAction("TeoreticalPayments", "CreditLine", requestFromDB);    
            } else if (requestFromView.ActionTentative != null) {
                return Tentative(requestFromDB.Id);
            } else if (requestFromView.ActionOpenCreditLine != null) {
                if (requestFromDB.CreditLine.IsAcceptable(requestFromDB))
                {
                    return View("CreditWasOpened", RepoFactory.GetRequestsRepo().Accept(requestFromDB));
                }
                else
                {
                    return null;
                }
            } 
            return null;
        }

        [HttpGet]
        public ActionResult ChooseCreditLine()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChooseCreditLine(CreditLineCriteria criteria)
        {
            var lines = RepoFactory.GetCreditLineRepo().getAll();
            var complies = lines.FirstOrDefault(x =>
                criteria.Amount >= x.MinAmountBoundary &&
                criteria.Amount <= x.MaxAmountBoundary &&
                criteria.MaxRate <= x.Rate &&
                criteria.MaxPeriod >= x.MinMonthsBoundary &&
                criteria.MinPeriod <= x.MaxMonthsBoundary
                );
            if (complies != null) return View("OptimalCreditLine", complies);

            complies = lines.FirstOrDefault(x =>
                //criteria.Amount >= x.MinAmountBoundary &&
                criteria.Amount <= x.MaxAmountBoundary &&
                criteria.MaxRate <= x.Rate &&
                criteria.MaxPeriod >= x.MinMonthsBoundary &&
                criteria.MinPeriod <= x.MaxMonthsBoundary
                );
            if (complies != null) return View("OptimalCreditLine", complies);

            complies = lines.FirstOrDefault(x =>
                criteria.Amount >= x.MinAmountBoundary &&
                criteria.Amount <= x.MaxAmountBoundary &&
                criteria.MaxRate <= x.Rate &&
                //criteria.MaxPeriod >= x.MinMonthsBoundary &&
                criteria.MinPeriod <= x.MaxMonthsBoundary
                );
            if (complies != null) return View("OptimalCreditLine", complies);

            complies = lines.FirstOrDefault(x =>
                criteria.Amount >= x.MinAmountBoundary &&
                criteria.Amount <= x.MaxAmountBoundary &&
                criteria.MaxRate <= x.Rate + 10 &&
                criteria.MaxPeriod >= x.MinMonthsBoundary &&
                criteria.MinPeriod <= x.MaxMonthsBoundary
                );
            if (complies != null) return View("OptimalCreditLine", complies);

            return View("OptimalCreditLine", null);
        }
        
        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult UnapprovedList()
        {
            return View(RepoFactory.GetRequestsRepo().GetUndecidedCreditRequests());
        }

        [Authorize(Roles = "Cashier, CoffeeAdmin, Committee")]
        public ActionResult ApprovedList()
        {
            return View(RepoFactory.GetRequestsRepo().GetApprovedCreditRequests());
        }

        [HttpGet]
        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult RequestDetail(long reqId)
        {
            return View(RepoFactory.GetRequestsRepo().GetRequestById(reqId));
        }

        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult Approve(long reqId)
        {
            var request = Repository.RepoFactory.GetRequestsRepo().GetRequestById(reqId);

            Decision decision = new Decision
            {
                Authority = User.Identity.Name,
                DecisionTime = DateTimeHelper.GetCurrentTime(),
                Verdict = true
            };

            request.Decision = decision;
            
            RepoFactory.GetRequestsRepo().CommitChanges();
            return Redirect("~");
        }

        [Authorize(Roles = "Clerk, Committee, CoffeeAdmin")]
        public ActionResult Tentative(long reqId)
        {
            var request = Repository.RepoFactory.GetRequestsRepo().GetRequestById(reqId);

            Decision decision = new Decision
            {
                Authority = User.Identity.Name,
                DecisionTime = DateTimeHelper.GetCurrentTime(),
                Verdict = null
            };

            request.Decision = decision;

            RepoFactory.GetRequestsRepo().CommitChanges();
            return Redirect("~");
        }

        [Authorize(Roles = "CoffeeAdmin, Committee")]
        public ActionResult Reject(long reqId)
        {
            var request = Repository.RepoFactory.GetRequestsRepo().GetRequestById(reqId);

            Decision decision = new Decision
            {
                Authority = User.Identity.Name,
                DecisionTime = DateTimeHelper.GetCurrentTime(),
                Verdict = false
            };

            request.Decision = decision;

            RepoFactory.GetRequestsRepo().CommitChanges();
            return Redirect("~");
        }

        [Authorize(Roles = "Cashier, Clerk, CoffeeAdmin")]
        public ActionResult OpenCredit(long reqId)
        {
            var repo = RepoFactory.GetRequestsRepo();
            return View("CreditWasOpened", repo.Accept(repo.GetRequestById(reqId)));
        }
    }
}
