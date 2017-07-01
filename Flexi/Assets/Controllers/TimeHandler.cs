using System;
using System.Collections.Generic;
using System.Globalization;
using Flexi.Assets.Models;

namespace Flexi.Assets.Controllers
{
    class TimeHandler
    {
        public TimeSpan GetWorkTime(Day day)
        {
            return day.EndTime - day.StartTime;
        }

        public TimeSpan GetCurrentWeekTime(List<Day> days)
        {
            var totalTime = new TimeSpan(0,0,0,0);

            for (var i = days.Count - 1; i >= 0; i--)
            {
                var dayOfWeek = DateTime.Now.DayOfWeek;
                var startOfWeek = DateTime.Now.AddDays(-(int)dayOfWeek);

                if (days[i].StartTime.Date < startOfWeek){ break; }

                var dayHours = GetWorkTime(days[i]);
                totalTime += dayHours;
            }

            return totalTime;
        }
    }
}