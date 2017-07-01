using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Flexi.Assets.Controllers
{
    class TextHandler
    {
        public string FormatTime(TimeSpan time)
        {
            return "This week: " + FormatNumber((int)time.TotalHours) + ":" + FormatNumber(time.Minutes) + ":" + FormatNumber(time.Seconds);
        }

        private static string FormatNumber(int val)
        {
            if (val < 10)
            {
                return "0" + val;
            }
            else
            {
                return val.ToString();
            }
        }
    }
}