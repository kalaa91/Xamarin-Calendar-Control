using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Globalization;

namespace CalendarControl
{
    [Activity(Label = "CalendarControl", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int selectedMonthNumber = DateTime.Today.Month;
        int selectedYearNumber = DateTime.Today.Year;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FrameLayout FLayout = FindViewById<FrameLayout>(Resource.Id.fragment_calendarControl1);

            FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();

            //fragmentTransaction.AddToBackStack(null);
            CalendarControlFragment calendarFragment = new CalendarControlFragment();
           

            //fragmentTransaction.Add(FLayout.Id, calendarFragment);
            fragmentTransaction.Replace(FLayout.Id, calendarFragment);

            fragmentTransaction.SetTransition(FragmentTransit.FragmentOpen);
            fragmentTransaction.Commit();

        }

    }

    public class Constants
    {
        public const int StartOfWeek = (int)DayOfWeek.Monday;
        public const int FirstDayOff = (int)DayOfWeek.Sunday;
        public const int SecondDayOff = (int)DayOfWeek.Saturday;
    }
    public class Day
    {
        public int Number { get; set; }
        public bool IsSelected { get; set; }
        public bool IsWeekEnd
        {
            get
            {
                if (Number == Constants.FirstDayOff || Number == Constants.SecondDayOff)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                IsWeekEnd = value;
            }
        }
    }
    public class Week
    {
        public int WeekNumber { get; set; }
        public int WeekStartDayNumber { get; set; }
        public List<Day> WeekDays { get; set; }
        public int MonthDaysCount { get; set; }
        public Week(int DaysInWeeks, int _WeekNumber, int _WeekStartDayNumber)
        {
            WeekDays = new List<Day>(DaysInWeeks);
            WeekNumber = _WeekNumber;
            WeekStartDayNumber = _WeekStartDayNumber;
            for (int i = 0; i < DaysInWeeks; i++)
            {
                Day day = new Day();
                if (_WeekStartDayNumber == 1)
                {
                    day.Number = i;
                }
                else
                {
                    day.Number = i + WeekStartDayNumber;
                }
                WeekDays.Add(day);
            }
        }
    }
    public class Month
    {
        public int MonthNumber { get; set; }
        public List<Week> MonthWeeks { get; set; }
        public Month(int Year, int _MonthNumber)
        {
            MonthNumber = _MonthNumber;

            int daysInMonth = DateTime.DaysInMonth(Year, MonthNumber);
            DateTime firstOfMonth = new DateTime(Year, MonthNumber, 1);
            int firstDayOfMonth = (int)firstOfMonth.DayOfWeek;
            int NumberOfWeeks = (int)Math.Ceiling((firstDayOfMonth + daysInMonth) / 7.0);

            MonthWeeks = new List<Week>(NumberOfWeeks);
            int dayCount = 0;
            for (int i = 1; i <= NumberOfWeeks; i++)
            {
                Week week;
                if (i == 1) // if first week
                {
                    week = new Week(7 - firstDayOfMonth, 1, firstDayOfMonth);
                    week.MonthDaysCount = daysInMonth;
                    MonthWeeks.Add(week);
                    dayCount += 7 - firstDayOfMonth;
                }
                else if (i == NumberOfWeeks) //if last week
                {
                    week = new Week(daysInMonth - dayCount, i, 1);
                    week.MonthDaysCount = daysInMonth;
                    MonthWeeks.Add(week);
                }
                else
                {
                    week = new Week(7, i, 1);
                    dayCount += 7;
                    week.MonthDaysCount = daysInMonth;
                    MonthWeeks.Add(week);
                }
            }
        }
    }
}

