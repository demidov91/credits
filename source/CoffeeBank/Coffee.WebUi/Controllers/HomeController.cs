using System.Web.Mvc;

using Coffee.Repository;
using System.Collections.Generic;
using Coffee.WebUi.Scripts;
using Coffee.Entities;

namespace Coffee.WebUi.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Home page for the simple bank worker (not cashier) is a list of credit requests.
        /// Home page for the authenticated outer user is UserHome.cshtml
        /// Home page for none-authenticated user is Index.cstml
        /// TODO: roles.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Request.IsAuthenticated) {
                if (MembershipHelper.IsExternalUser(User)) {
                    long? requestId = (long?)Session["last_credit_request_id"];
                    if (requestId != null){
                        CreditRequest lastCreditRequest = RepoFactory.GetRequestsRepo().GetRequestById((long)requestId);
                        lastCreditRequest.Username = User.Identity.Name;
                        RepoFactory.GetRequestsRepo().Update(lastCreditRequest);
                    }
                    return RedirectToAction("List", "CreditLine");
                } else if(User.IsInRole("Committee")) {
                    return RedirectToAction("UnapprovedList", "Request");
                }
                return RedirectToAction("List", "Request");
            }
            return RedirectToAction("List", "CreditLine");
        }

        /// <summary>
        /// Not in use.
        /// </summary>
        /// <returns></returns>
        public ActionResult UserHome() 
        {
            bool hasRequests = RepoFactory.GetRequestsRepo().GetRequestsByOwner(User.Identity.Name).Count > 0;
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["hasRequests"] = hasRequests;
            return View(data);
        }

    }
}
