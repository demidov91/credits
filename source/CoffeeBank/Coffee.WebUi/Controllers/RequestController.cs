﻿using System.Web.Mvc;
using System.Linq;
using Coffee.Entities;
using Coffee.Repository;
using Coffee.IRepository;

using System.Collections.Generic;

using Coffee.WebUi.Models;
using System; 

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
            return View(new Coffee.WebUi.Models.Request.CreditRequest(RepoFactory.GetRequestsRepo().GetRequestById(requestToView.Id)));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Details(Coffee.WebUi.Models.Request.CreditRequest requestFromView)
        {
            Coffee.Entities.CreditRequest requestFromDB = RepoFactory.GetRequestsRepo().Update(requestFromView.GetAdaptee());
            RepoFactory.GetPassportInfoRepo().Update(requestFromView.PassportInfo);
            if (requestFromView.ActionEdit != null){
                return RedirectToAction("Details", "Request", new { Id = requestFromView.Id });
            } else if (requestFromView.ActionViewPayments != null) {
                return RedirectToAction("TeoreticalPayments", "CreditLine", requestFromDB);    
            } else if (requestFromView.ActionOpenCreditLine != null) {
                if (requestFromDB.CreditLine.IsAcceptable(requestFromDB))
                {
                    return View("CreditWasOpened", RepoFactory.GetRequestsRepo().Accept(requestFromDB));
                }
                else {
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
        
        /*
        //[Authorize]
        public ActionResult ApprovedList()
        {
            return View();
        }
        //*/

        public ActionResult UnapprovedList()
        {
            return View(RepoFactory.GetRequestsRepo().GetUndecidedCreditRequests());
        }

        [HttpGet]
        public ActionResult RequestDetail(long reqId)
        {
            return View(RepoFactory.GetRequestsRepo().GetRequestById(reqId));
        }

        [HttpPost]
        public ActionResult Approve(CreditRequest req)
        {
            Decision decision = new Decision
            {
                Authority = User.Identity.Name,
                Request = req,
                DecisionTime = DateTime.Now,
                Verdict = true
            };
            RepoFactory.GetRequestsRepo().RegisterDecision(decision);
            return Redirect("~");
        }

        [HttpPost]
        public ActionResult Reject(CreditRequest req)
        {
            Decision decision = new Decision
            {
                Authority = User.Identity.Name,
                Request = req,
                DecisionTime = DateTime.Now,
                Verdict = false
            };
            RepoFactory.GetRequestsRepo().RegisterDecision(decision);
            return Redirect("~");
        }
    }
}
