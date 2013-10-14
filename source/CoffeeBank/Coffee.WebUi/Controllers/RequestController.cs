using System.Web.Mvc;
using Coffee.Entities;

namespace Coffee.WebUi.Controllers
{
    public class RequestController : Controller
    {
        public ActionResult New()
        {
            CreditRequest model = new CreditRequest();

            return View(model);
        }

        [HttpPost]
        public ActionResult New(CreditRequest model)
        {
            //some DB-saving magic

            return View(model);
        }

        /*
        //[Authorize]
        public ActionResult List()
        {
            return View();
        }

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
        public ActionResult Details()
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
