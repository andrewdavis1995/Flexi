using System;
using System.Collections.Generic;
using System.IO;
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
        private TextView _weekLabel;
        private ImageView _weekIcon;
        private TextView _sessionText;
        private Timer _timer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            ActionBar.Hide();

            var appPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var fullPath = Path.Combine(appPath, "Flexi");
            Directory.CreateDirectory(fullPath);

            _dayList = new DataLoader().ReadAllFiles();

            AssignEventHandlers();

            //AddSampleData();

            _timeText = FindViewById<TextView>(Resource.Id.txtTimeThisWeek);
            _sessionText = FindViewById<TextView>(Resource.Id.txtSession);
            _weekLabel = FindViewById<TextView>(Resource.Id.txtWeekMsg);
            _weekIcon = FindViewById<ImageView>(Resource.Id.imgWeekIcon);

            _timer = new Timer {Interval = 1000};
            _timer.Elapsed += TimerUpdate;
            _timer.Start();

            var weekTime = _timeHandler.GetCurrentWeekTime(_dayList);
            SetWeekValues(weekTime);
            _timeText.Text = _timeText.Text = "This week: " + new TextHandler().FormatTime(weekTime);
        }

        private void TimerUpdate(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var day = _dayList[_dayList.Count - 1];
            if (day.Done) return;

            day.EndTime = DateTime.Now;

            var weekTime = _timeHandler.GetCurrentWeekTime(_dayList);
            RunOnUiThread(() => _timeText.Text = "This week: " + new TextHandler().FormatTime(weekTime));

            var sessionTime = day.EndTime - day.StartTime;
            RunOnUiThread(() => _sessionText.Text = "This session: " + new TextHandler().FormatTime(sessionTime));

            SetWeekValues(weekTime);
        }

        private void SetWeekValues(TimeSpan weekTime)
        {
            var minTarget = new TimeSpan(30, 0, 0);
            var avTarget = new TimeSpan(37, 0, 0);

            // Testing Values
            //var minTarget = new TimeSpan(0, 0, 10);
            //var avTarget = new TimeSpan(0, 0, 20);

            if (weekTime <= minTarget)
            {
                RunOnUiThread(() =>_weekIcon.SetImageResource(Resource.Drawable.cross));
                RunOnUiThread(() => _weekLabel.Text = "You have not reached your weekly hours target yet");        
            }
            else if (weekTime <= avTarget)
            {
                RunOnUiThread(() => _weekIcon.SetImageResource(Resource.Drawable.exclaim));
                RunOnUiThread(() => _weekLabel.Text = "You have reached the minimum allowance for this week");
            }
            else
            {
                RunOnUiThread(() => _weekIcon.SetImageResource(Resource.Drawable.tick));
                RunOnUiThread(() => _weekLabel.Text = "You have reached your weekly target!");
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
            FindViewById<ImageView>(Resource.Id.cmdIn).Click += SignIn;
            FindViewById<ImageView>(Resource.Id.cmdOut).Click += SignOut;
        }

        private void SignIn(object sender, EventArgs eventArgs)
        {
            if (_dayList[_dayList.Count - 1].Done)
            {
                _dayList.Add(new Day(DateTime.Now, DateTime.Now));
            }
            new DataWriter().WriteAllToFile(_dayList);
        }

        private void SignOut(object sender, EventArgs eventArgs)
        {
            if (_dayList.Count <= 0) return;
            _dayList[_dayList.Count - 1].Done = true;
            _dayList[_dayList.Count - 1].EndTime = DateTime.Now;
            new DataWriter().WriteAllToFile(_dayList);
        }
        
    }
}

