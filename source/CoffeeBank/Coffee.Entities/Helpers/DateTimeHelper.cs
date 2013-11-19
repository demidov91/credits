using System;
using NodaTime;

namespace Coffee.Entities
{
    public static class DateTimeHelper
    {
        private static TimeSpan _delta = new TimeSpan();

        public static LocalDate ToLocalDate(this DateTime date)
        {
            LocalDate result = new LocalDate(date.Year, date.Month, date.Day);
            return result;
        }

        /// <summary>
        /// I don't know what is the difference between LocalDateTime and DateTime, but first hasn't .Now property.
        /// </summary>
        /// <returns>
        /// Demo time. It can be changed with AddTimespan method.
        /// </returns>
        public static DateTime GetCurrentTime() {
            return DateTime.Now + _delta;
        }

        /// <summary>
        /// Add some timespan to the "system" time.
        /// </summary>
        public static void AddTimespan(TimeSpan delta) {
            _delta += delta;
        }

    }
}
