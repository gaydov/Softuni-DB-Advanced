using System;

namespace DateModifierPgm
{
    public static class DateModifier
    {
        public static int CaclDifferenceBetweenDates(string firstDate, string secondDate)
        {
            DateTime firstDateTime = DateTime.ParseExact(firstDate, "yyyy MM dd", null);
            DateTime secondDateTime = DateTime.ParseExact(secondDate, "yyyy MM dd", null);
            int difference = (int)(firstDateTime - secondDateTime).TotalDays;

            return Math.Abs(difference);
        }
    }
}