using System;
using NodaTime;

namespace Coffee.Entities
{
    public static class DateTimeHelper
    {
        public static LocalDate ToLocalDate(this DateTime date)
        {
            LocalDate result = new LocalDate(date.Year, date.Month, date.Day);
            return result;
        }
    }
}
