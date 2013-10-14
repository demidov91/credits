using System;
using NodaTime;

namespace Coffee.Entities
{
    public class AgeBoundary : IRequestBoundary
    {
        public int MinAge { get; set; }

        public int MaxAge { get; set; }

        public AgeBoundary()
        {
        }

        public AgeBoundary(int minAge, int maxAge)
        {
            MinAge = minAge;
            MaxAge = maxAge;
        }

        public bool IsAcceptable(CreditRequest request)
        {
            LocalDate birth = request.PassportInfo.BirthDate.ToLocalDate();
            LocalDate now = DateTime.Now.ToLocalDate();
            long age = Period.Between(birth, now).Years;

            return (age>=MinAge && age<=MaxAge);
        }

        public string Visualize()
        {
            return string.Format("{0} - {1} years old", MinAge, MaxAge);
        }
    }
}
