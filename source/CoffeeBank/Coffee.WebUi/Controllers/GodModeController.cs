using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coffee.WebUi.Controllers
{
    public class GodModeController : Controller
    {
        [HttpGet]
        public ActionResult Date()
        {
            return View(new TimeSpan());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Date(String hours)
        {
            Coffee.Entities.DateTimeHelper.AddTimespan(new TimeSpan(Int32.Parse(hours), 0, 0));
            return View(new TimeSpan());
        }

    }
}
