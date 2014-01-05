using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using Coffee.Entities;
using Coffee.Repository;
using System.ComponentModel.DataAnnotations;

namespace Coffee.WebUi.Models.Request
{
    /// <summary>
    /// Don't be angry with me. Purposes of this class are just the same as for "RequestPlusCreditProposes",
    /// but I think it is a little better. Or not.
    /// Adapter for Coffee.Entities.CreditRequest
    /// </summary>
    public class CreditRequest
    {
        public enum Action { SAVE, VIEW_PAYMENTS, MAKE_CREDIT}


        private static Object _singletonLock = new Object();
        private static List<CreditLine> _creditKinds = null;
        public static void OnCreditKindsUpdate(object sender, EventArgs eventArgs) {
            _creditKinds = RepoFactory.GetCreditLineRepo().getAll();        
        }

        [DisplayFormat(ApplyFormatInEditMode=true, DataFormatString="{0:dd.MM.yyyy}")]
        public DateTime IssueDate { get { return adaptee.IssueDate; } set { adaptee.IssueDate = value; } }

        private Coffee.Entities.CreditRequest adaptee;

        public CreditRequest() {
            this.adaptee = new Coffee.Entities.CreditRequest();
        }

        public CreditRequest(Coffee.Entities.CreditRequest request) {
            lock (_singletonLock)
            {
                if (_creditKinds == null)
                {
                    OnCreditKindsUpdate(null, null);
                    RepoFactory.GetCreditLineRepo().AddUpdateListener(new EventHandler(OnCreditKindsUpdate));
                }
            }
            this.adaptee = request;
        }

        public long Id { get { return adaptee.Id; } set { adaptee.Id = value; } }

        public decimal Amount { get { return adaptee.Amount; } set { adaptee.Amount = value; } }

        public int Period { get { return adaptee.Period; } set { adaptee.Period = value; } }

        public PassportInfo PassportInfo { get { return adaptee.PassportInfo; } set { adaptee.PassportInfo = value; } }

        public SalaryInfo SalaryInfo { get { return adaptee.SalaryInfo; } set { adaptee.SalaryInfo = value; } }

        public CreditLine CreditLine { get { return adaptee.CreditLine; } set { adaptee.CreditLine = value; } }

        /// <summary>
        /// Actions for different submit buttons.
        /// </summary>
        public Object ActionEdit { get; set; }
        public Object ActionViewPayments { get; set; }
        public Object ActionOpenCreditLine { get; set; }

        public string AdditionalTextInfo { get { return adaptee.AdditionalTextInfo; } set { adaptee.AdditionalTextInfo = value; } }

        public string Username { get { return adaptee.Username; } set { adaptee.Username = value; } }

        public Coffee.Entities.CreditRequest GetAdaptee() {
            return this.adaptee;
        }

        public List<SelectListItem> GetCreditDropdownItems() {
            var creditDropdownItems = new List<SelectListItem>(_creditKinds.Count);
            creditDropdownItems.Add(new SelectListItem
            {
                Text = "- no credit product -",
                Value = "0",
                Selected = CreditLine == null || CreditLine.Id == 0
            });
            foreach (CreditLine credit in _creditKinds)
            {
                bool isSelected = credit.Equals(CreditLine);
                creditDropdownItems.Add(new SelectListItem
                {
                    Text = credit.Name,
                    Value = credit.Id.ToString(),
                    Selected = isSelected
                });
            }
            return creditDropdownItems;
        }
        
    }
}