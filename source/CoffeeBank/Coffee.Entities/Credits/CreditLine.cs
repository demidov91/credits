using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coffee.Entities
{
    public class CreditLine: IRequestBoundary
    {
        public List<IRequestBoundary> Conditions { get; set; }

        public bool IsAcceptable(CreditRequest request)
        {
            if (Conditions != null && Conditions.Any())
            {
                return Conditions.All(x => x.IsAcceptable(request));
            }

            return false;
        }

        public string Visualize()
        {
            StringBuilder builder = new StringBuilder();

            if (Conditions != null && Conditions.Any())
            {
                builder.AppendLine("Credit requirements:");


                foreach (var boundary in Conditions)
                {
                    builder.AppendLine(boundary.Visualize());
                }
            }

            return builder.ToString();
            /*
                Credit requirements:
                21 - 65 years old
                1000000 - 40000000 belorussian rubles
                6 - 24 months
             */
        }

        public CreditLine():this(new List<IRequestBoundary>())
        {
        }

        public CreditLine(IEnumerable<IRequestBoundary> conditions)
        {
            Conditions = conditions.ToList();
        }
    }
}
