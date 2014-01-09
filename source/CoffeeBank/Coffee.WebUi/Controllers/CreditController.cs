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
        [Authorize(Roles = "Clerk, Cashier, Committee, CoffeeAdmin")]
        public ActionResult List(string passportNumber)
        {
            List<Credit> creditsToShow = RepoFactory.GetCreditsRepo().GetAllIssuedCredits();
            if (passportNumber != null)
            {
                creditsToShow = creditsToShow.FindAll(x => x.Passport.PassportNumber.StartsWith(passportNumber));
            }
            return View("~/Views/RealCredit/List.cshtml", creditsToShow);
        }

    }
}
