using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coffee.Entities
{
    /// <summary>
    /// Kind of credit.
    /// </summary>
    public class CreditLine: IRequestBoundary
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public double Rate { get; set; }

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

            builder.AppendLine(string.Format("Credit line \"{0}\" with {1}% rate.", Name, Rate));

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
                Credit line "Easy Cash" with 39% rate.
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
