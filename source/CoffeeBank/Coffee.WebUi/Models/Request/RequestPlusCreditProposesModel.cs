using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Coffee.Entities;

namespace Coffee.WebUi.Models
{
    public class RequestPlusCreditProposesModel
    {
        public CreditRequest Request { get; set; }
        public List<CreditLine> Credits { get; set; }
        private List<SelectListItem> creditDropdownItems;

        public List<SelectListItem> GetCreditDropdownItems() {
            return this.creditDropdownItems;    
        }

        public RequestPlusCreditProposesModel() { }

        public RequestPlusCreditProposesModel(CreditRequest request, List<CreditLine> proposes){
            this.Request = request;
            this.Credits = proposes;
            this.creditDropdownItems = new List<SelectListItem>(proposes.Count);
            this.creditDropdownItems.Add(new SelectListItem { 
                Text = "- no credit product -",
                Value = "0",
                Selected = request.CreditLine == null || request.CreditLine.Id == 0
            });
            foreach (CreditLine credit in Credits) { 
                bool isSelected = credit.Equals(request.CreditLine);
                this.creditDropdownItems.Add(new SelectListItem
                {
                    Text = credit.Name,
                    Value = credit.Id.ToString(),
                    Selected = isSelected
                });
            }
        }


    }
}