using System;
using System.Collections.Generic;
using System.Timers;
using Android.App;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using Flexi.Assets.Controllers;
using Flexi.Assets.Models;

namespace Flexi
{
    [Activity(Label = "Flexi", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        private List<Day> _dayList = new List<Day>();
        private TimeHandler _timeHandler = new TimeHandler();
        private TextView _timeText;
        private Timer _timer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            AssignEventHandlers();

            AddSampleData();

            _timeText = FindViewById<TextView>(Resource.Id.txtTimeThisWeek);
            _timer = new Timer {Interval = 1000};
            _timer.Elapsed += TimerUpdate;
            _timer.Start();

            var weekTime = _timeHandler.GetCurrentWeekTime(_dayList);

            _timeText.Text = _timeText.Text = new TextHandler().FormatTime(weekTime);
        }

        private void TimerUpdate(object sender, ElapsedEventArgs elapsedEventArgs)
        {

            var day = _dayList[_dayList.Count - 1];
            if (!day.Done)
            {
                day.EndTime = DateTime.Now;

                var weekTime = _timeHandler.GetCurrentWeekTime(_dayList);
                RunOnUiThread(() => _timeText.Text = new TextHandler().FormatTime(weekTime));
            }
        }

        private void AddSampleData()
        {
            _dayList.Add(new Day(new DateTime(2017, 6, 26, 09, 00, 00), new DateTime(2017, 6, 26, 17, 00, 00), true));
            _dayList.Add(new Day(new DateTime(2017, 6, 27, 08, 30, 00), new DateTime(2017, 6, 27, 17, 05, 00), true));
            _dayList.Add(new Day(new DateTime(2017, 6, 28, 09, 00, 00), new DateTime(2017, 6, 28, 17, 00, 00), true));
            _dayList.Add(new Day(new DateTime(2017, 6, 29, 08, 40, 00), new DateTime(2017, 6, 29, 17, 15, 00), true));
            _dayList.Add(new Day(new DateTime(2017, 6, 30, 09, 00, 00), new DateTime(2017, 6, 30, 16, 15, 00), true));
        }

        private void AssignEventHandlers()
        {
            FindViewById<Button>(Resource.Id.cmdIn).Click += SignIn;
            FindViewById<Button>(Resource.Id.cmdOut).Click += SignOut;
        }

        private void SignIn(object sender, EventArgs eventArgs)
        {
            _dayList.Add(new Day(DateTime.Now, DateTime.Now));
        }

        private void SignOut(object sender, EventArgs eventArgs)
        {
            if (_dayList.Count <= 0) return;
            _dayList[_dayList.Count - 1].Done = true;
            _dayList[_dayList.Count - 1].EndTime = DateTime.Now;
        }
        
    }
}

