using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Coffee.Entities;
using Coffee.Repository;
using Coffee.WebUi.Scripts;

namespace Coffee.WebUi.Controllers
{
    public class CreditController : Controller
    {
  
        public ActionResult List(string passportNumber)
        {
            List<Credit> creditsToShow = RepoFactory.GetCreditsRepo().GetAllIssuedCredits();
            if (passportNumber != null)
            {
                creditsToShow = creditsToShow.FindAll(x => x.Passport.PassportNumber.StartsWith(passportNumber));
            }
            else if (MembershipHelper.IsExternalUser(User)) {
                creditsToShow = creditsToShow.FindAll(x => x.User == User.Identity.Name);
            }
            return View("~/Views/RealCredit/List.cshtml", creditsToShow);
        }

    }
}
