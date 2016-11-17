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
    [Activity(Label = "Calendar", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ISelectedDateChanged
    {
        int selectedMonthNumber = DateTime.Today.Month;
        int selectedYearNumber = DateTime.Today.Year;

        public static string From { get; set; }
        public static string To { get; set; }

        public void SelectedDateChanged(string SelectedDate)
        {

        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FrameLayout FLayout = FindViewById<FrameLayout>(Resource.Id.fragment_mainCalendar);

            FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();

            CalendarControlFragment calendarFragment = new CalendarControlFragment();

            fragmentTransaction.Replace(FLayout.Id, calendarFragment);

            fragmentTransaction.SetTransition(FragmentTransit.FragmentOpen);
            fragmentTransaction.Commit();

            Button btn_PickFrom = FindViewById<Button>(Resource.Id.btn_pickFrom);
            Button btn_PickTo = FindViewById<Button>(Resource.Id.btn_pickTo);

            TextView txt_From = FindViewById<TextView>(Resource.Id.lbl_FromValue);
            TextView txt_To = FindViewById<TextView>(Resource.Id.lbl_ToValue);

            if (!string.IsNullOrEmpty(Intent.GetStringExtra("From")))
            {
                From = Intent.GetStringExtra("From") ?? string.Empty;
            }

            if ( !string.IsNullOrEmpty(Intent.GetStringExtra("To")))
            {
                To = Intent.GetStringExtra("To") ?? string.Empty;
            }

            txt_From.Text = From;
            txt_To.Text = To;

            if (!string.IsNullOrEmpty(From) && !string.IsNullOrEmpty(To))
            {
                PopulateList(From,To);
            }


            btn_PickFrom.Click += (sender, e) => OpenCalendarForSelection(true);
            btn_PickTo.Click += (sender, e) => OpenCalendarForSelection(false);


        }

        private void PopulateList(string from, string to)
        {

            ListView listView = FindViewById<ListView>(Resource.Id.lv_SelectedDates);
            var DatesAdapterClear = new ArrayAdapter<string>(this, Resource.Id.lv_SelectedDates, new List<string>());
            listView.Adapter = DatesAdapterClear;
            List<string> SelectedDates = GetSelectedDatesAsString(CastDateTime(from), CastDateTime(to));
            var DatesAdapter = new ArrayAdapter<string>(this, Resource.Layout.ListRowLayout, SelectedDates);
            listView.Adapter = DatesAdapter;

        }

        private DateTime CastDateTime(string DateString)
        {
            string[] DateArray = DateString.Split('-');
            if(DateArray.Length == 3)
            {
                DateTime dateTime = new DateTime(int.Parse(DateArray[2]), int.Parse(DateArray[1]), int.Parse(DateArray[0]));
                return dateTime;
            }
            else
            {
                //fix this because with wrong parameters it will return Date.Min value
                return new DateTime();
            }
        }

        private void OpenCalendarForSelection(bool IsPickingFromValue)
        {
            var pickerActivity = new Intent(this, typeof(PickerActivity));
            pickerActivity.PutExtra("IsPickingFromValue", IsPickingFromValue);
            StartActivity(pickerActivity);
        }

        private List<DateTime> GetSelectedDates(DateTime From,DateTime To)
        {
            List<DateTime> SelectedDates = new List<DateTime>();
            for (DateTime DateIndex = From; DateIndex <= To; DateIndex = DateIndex.AddDays(1))
            {
                SelectedDates.Add(DateIndex);
            }
            return SelectedDates;
        }

        private List<string> GetSelectedDatesAsString(DateTime From, DateTime To)
        {
            List<string> SelectedDates = new List<string>();
            for (DateTime DateIndex = From; DateIndex <= To; DateIndex = DateIndex.AddDays(1))
            {
                SelectedDates.Add(DateIndex.ToShortDateString());
            }
            return SelectedDates;
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
        public int Month { get; set; }
        public int Year { get; set; }
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
        public List<DateTime> SelectedDates { get; set; }
        public Month(int Year, int _MonthNumber)
        {
            MonthNumber = _MonthNumber;

            int daysInMonth = DateTime.DaysInMonth(Year, MonthNumber);
            DateTime firstOfMonth = new DateTime(Year, MonthNumber, 1);
            int firstDayOfMonth = (int)firstOfMonth.DayOfWeek;
            int NumberOfWeeks = (int)Math.Ceiling((firstDayOfMonth + daysInMonth) / 7.0);

            MonthWeeks = new List<Week>(NumberOfWeeks);
            SelectedDates = new List<DateTime>();
            int dayCount = 0;
            for (int i = 1; i <= NumberOfWeeks; i++)
            {
                Week week;
                if (i == 1) // if first week
                {
                    week = new Week(7 - firstDayOfMonth, 1, firstDayOfMonth);
                    dayCount += 7 - firstDayOfMonth;
                }
                else if (i == NumberOfWeeks) //if last week
                {
                    week = new Week(daysInMonth - dayCount, i, 1);
                }
                else
                {
                    week = new Week(7, i, 1);
                    dayCount += 7;
                }

                week.MonthDaysCount = daysInMonth;
                week.Month = MonthNumber;
                week.Year = Year;
                MonthWeeks.Add(week);

            }
        }
    }
}

