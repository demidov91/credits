using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coffee.Entities;

namespace Coffee.WebUi.Controllers
{
    public class CreditLineController : Controller
    {
        public ActionResult List()
        {
            List<CreditLine> model = Coffee.Repository.RepoFactory.GetCreditLineRepo().getAll();
            return View(model);
        }

        public ActionResult Detail(long id)
        {
            CreditLine model = Coffee.Repository.RepoFactory.GetCreditLineRepo().getById(id);
            return View(model);
        }

        public ActionResult New()
        {
            return View();
        }
    }
}
