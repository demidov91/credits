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
        [Authorize(Roles = "CoffeeAdmin")]
        public ActionResult Date()
        {
            return View(new TimeSpan());
        }

        [HttpPost]
        [Authorize(Roles = "CoffeeAdmin")]
        public ActionResult Date(String days)
        {
            Coffee.Entities.DateTimeHelper.AddTimespan(new TimeSpan(Int32.Parse(days), 0, 0, 0));
            return Redirect("~");
        }

    }
}
