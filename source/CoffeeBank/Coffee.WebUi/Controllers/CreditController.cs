using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Coffee.Entities;
using Coffee.Repository;


namespace Coffee.WebUi.Controllers
{
    public class CreditController : Controller
    {
  
        public ActionResult List(string passportNumber)
        {
            List<Credit> creditsToShow = RepoFactory.GetCreditsRepo().GetAllIssuedCredits();
            bool isSimpleUser = User.Identity.Name != "BankWorker";                             ///kill this part of code
            if (passportNumber != null)
            {
                creditsToShow = creditsToShow.FindAll(x => x.Passport.PassportNumber.StartsWith(passportNumber));
            }
            else if (isSimpleUser) {
                creditsToShow = creditsToShow.FindAll(x => x.User == User.Identity.Name);
            }
            return View("~/Views/RealCredit/List.cshtml", creditsToShow);
        }

    }
}
