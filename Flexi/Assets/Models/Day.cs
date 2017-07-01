using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text.Format;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace Flexi.Assets.Models
{
    internal class Day
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Done { get; set; }

        public Day(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
        public Day(DateTime startTime, DateTime endTime, bool done)
        {
            StartTime = startTime;
            EndTime = endTime;
            Done = done;
        }
    }
}