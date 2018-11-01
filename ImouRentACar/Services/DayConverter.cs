using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImouRentACar.Services
{
    public class DayConverter
    {
        //Convert Months to Days
        public int MonthsToDays(int months)
        {
            var days = months * 31;
            return days;
        }

        //Convert Weeks to Days
        public int WeeksToDays(int weeks)
        {
            var days = weeks * 7;
            return days;
        }

        //Conert Years to Days
        public int YearsToDays(int years)
        {
            var days = years * 365;
            return days;
        }
    }
}
