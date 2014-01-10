using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coffee.Repository;
using Coffee.Entities;

namespace Coffee.WebUi.Controllers
{
    public class StatisticsController : Controller
    {
        //
        // GET: /Statistics/

        public ActionResult Overview()
        {
            return View("StatisticsOverview");
        }

        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult DistributionOfCreditProducts() {
            return Json(
                RepoFactory.GetCreditsRepo().GetAllIssuedCredits().GroupBy(
                    x => x.Line.Name,
                    (x,y) => new { Name = x, Num = y.Count() }
                ).ToArray()
            );
        }

        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult NumberOfProblemCredits() {
            return Json(
                RepoFactory.GetCreditsRepo().GetAllIssuedCredits()
                    .Where(x => Coffee.WebUi.Scripts.CreditHelper.IsUnderfunded(x))
                    .GroupBy(
                        x => x.Line.Name,
                        (x,y) => new { Name = x, Num = y.Count() }
                    ).ToArray()
            );
        }

        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult DistributionProductsAges() {
            return Json(
                RepoFactory.GetCreditsRepo().GetAllIssuedCredits().GroupBy(
                    x => x.Line.Name,
                    (x, y) =>
                    {
                        var list = y.GroupBy(
                            z =>
                            {
                                int years = (DateTimeHelper.GetCurrentTime() - z.Passport.BirthDate).Days / 365;
                                if (years < 22) return 0;
                                else if (years >= 22 && years < 30) return 1;
                                else if (years >= 30 && years < 40) return 2;
                                else if (years >= 40 && years < 55) return 3;
                                else return 4;
                            },
                            (a, b) => new { Group=a, Count=b.Count() }
                        ).ToArray();
                        return new
                        {
                            LessThan22 = (list.FirstOrDefault(c => c.Group == 0) ?? new { Group=-1, Count=0 }).Count,
                            From22To30 = (list.FirstOrDefault(c => c.Group == 1) ?? new { Group=-1, Count=0 }).Count,
                            From30To40 = (list.FirstOrDefault(c => c.Group == 2) ?? new { Group=-1, Count=0 }).Count,
                            From40To55 = (list.FirstOrDefault(c => c.Group == 3) ?? new { Group=-1, Count=0 }).Count,
                            MoreThan55 = (list.FirstOrDefault(c => c.Group == 4) ?? new { Group=-1, Count=0 }).Count
                        };
                    }
                ).ToArray()
            );
        }

        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult NumberOfUsersAges() {
            var list = RepoFactory.GetPassportInfoRepo().GetAll().GroupBy(
                x => {
                    int years = (DateTimeHelper.GetCurrentTime() - x.BirthDate).Days / 365;
                    if (years < 22) return 0;
                    else if (years >= 22 && years < 30) return 1;
                    else if (years >= 30 && years < 40) return 2;
                    else if (years >= 40 && years < 55) return 3;
                    else return 4;
                },
                (x, y) => new { Group=x, Count=y.Count() }
            ).ToArray();
            return Json(
                new
                {
                    LessThan22 = (list.FirstOrDefault(x => x.Group == 0) ?? new { Group=-1, Count=0 }).Count,
                    From22To30 = (list.FirstOrDefault(x => x.Group == 1) ?? new { Group=-1, Count=0 }).Count,
                    From30To40 = (list.FirstOrDefault(x => x.Group == 2) ?? new { Group=-1, Count=0 }).Count,
                    From40To55 = (list.FirstOrDefault(x => x.Group == 3) ?? new { Group=-1, Count=0 }).Count,
                    MoreThan55 = (list.FirstOrDefault(x => x.Group == 4) ?? new { Group=-1, Count=0 }).Count
                }
            );
        }

        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult DistributionProductsAmounts() {
            return Json(
                RepoFactory.GetCreditsRepo().GetAllIssuedCredits().GroupBy(
                    x => x.Line.Name,
                    (x, y) =>
                    {
                        var list = y.GroupBy(
                            z =>
                            {
                                decimal amount = z.Amount;
                                if (amount < 1500000) return 0;
                                else if (amount >= 1500000 && amount < 5000000) return 1;
                                else if (amount >= 5000000 && amount < 20000000) return 2;
                                else if (amount >= 20000000 && amount < 50000000) return 3;
                                else return 4;
                            },
                            (a, b) => new { Group=a, Count=b.Count() }
                        ).ToArray();
                        return new
                        {
                            LessThan1_5M = (list.FirstOrDefault(c => c.Group == 0) ?? new { Group=-1, Count=0 }).Count,
                            From1_5MTo5M = (list.FirstOrDefault(c => c.Group == 1) ?? new { Group=-1, Count=0 }).Count,
                            From5MTo20M  = (list.FirstOrDefault(c => c.Group == 2) ?? new { Group=-1, Count=0 }).Count,
                            From20MTo50M = (list.FirstOrDefault(c => c.Group == 3) ?? new { Group=-1, Count=0 }).Count,
                            MoreThan50M  = (list.FirstOrDefault(c => c.Group == 4) ?? new { Group=-1, Count=0 }).Count
                        };
                    }
                ).ToArray()
            );
        }

        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult DistributionOfOverdueCredits() {
            return Json(
                RepoFactory.GetCreditsRepo().GetAllIssuedCredits()
                    .Where(x => x.IssueDate.AddMonths(x.Period).Date > DateTimeHelper.GetCurrentTime().Date && Coffee.WebUi.Scripts.CreditHelper.IsUnderfunded(x))
                    .GroupBy(
                        x => x.Line.Name,
                        (x,y) => new { Name = x, Num = y.Count() }
                    ).ToArray()
            );
        }

        [Authorize(Roles = "Committee, CoffeeAdmin")]
        public ActionResult ProblemCreditsIssueTime() {
            var list = RepoFactory.GetCreditsRepo().GetAllIssuedCredits()
                    .Where(x => Coffee.WebUi.Scripts.CreditHelper.IsUnderfunded(x))
                    .GroupBy(
                        x =>
                        {
                            int days = (DateTimeHelper.GetCurrentTime() - x.IssueDate).Days;
                            if (days < 183) return 0;
                            else if (days >= 183 && days < 365) return 1;
                            else if (days >= 365 && days < 730) return 2;
                            else return 3;
                        },
                        (x, y) => new { Group = x, Count = y.Count() }
                    ).ToArray();
            return Json(
                new
                {
                    LessThanHalfYear = (list.FirstOrDefault(x => x.Group == 0) ?? new { Group=-1, Count=0 }).Count,
                    FromHalfTo1Year  = (list.FirstOrDefault(x => x.Group == 1) ?? new { Group=-1, Count=0 }).Count,
                    From1To2Years    = (list.FirstOrDefault(x => x.Group == 2) ?? new { Group=-1, Count=0 }).Count,
                    MoreThan2Years   = (list.FirstOrDefault(x => x.Group == 3) ?? new { Group=-1, Count=0 }).Count
                }
            );
        }
    }
}
