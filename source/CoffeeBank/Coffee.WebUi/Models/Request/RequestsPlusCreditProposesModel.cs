using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Coffee.Entities;

namespace Coffee.WebUi.Models
{
    public class RequestsPlusCreditProposesModel
    {
        public List<CreditRequest> Requests { get; set; }
        public List<CreditLine> Credits { get; set; }
        public List<RequestPlusCreditProposesModel> GetRequestPlusBusinessProposesList() {
            List<RequestPlusCreditProposesModel> result = new List<RequestPlusCreditProposesModel>(Requests.Count);
            foreach (CreditRequest request in Requests) {
                result.Add(new RequestPlusCreditProposesModel(request, Credits));
            }
            return result;
        }
    }
}