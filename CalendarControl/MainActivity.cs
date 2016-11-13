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

            //Month SelectedMonth = new Month(selectedYearNumber, selectedMonthNumber);
            //// Get our button from the layout resource,
            //// and attach an event to it
            //Button Previousbtn = FindViewById<Button>(Resource.Id.btn_Prev);
            //Button Nextbtn = FindViewById<Button>(Resource.Id.btn_Next);


            ////txt_Current
            //Previousbtn.Click += Previousbtn_Click;
            //Nextbtn.Click += Nextbtn_Click;
            //UpdateUI();



            FrameLayout FLayout = FindViewById<FrameLayout>(Resource.Id.fragment_calendarControl1);

            FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();

            //fragmentTransaction.AddToBackStack(null);
            CalendarControlFragment calendarFragment = new CalendarControlFragment();
           

            //fragmentTransaction.Add(FLayout.Id, calendarFragment);
            fragmentTransaction.Replace(FLayout.Id, calendarFragment);

            fragmentTransaction.SetTransition(FragmentTransit.FragmentOpen);
            fragmentTransaction.Commit();

        }

        //private void Nextbtn_Click(object sender, EventArgs e)
        //{
        //    if (selectedMonthNumber + 1 > 12)
        //    {
        //        selectedMonthNumber = 1;
        //        selectedYearNumber++;
        //    }
        //    else
        //    {
        //        selectedMonthNumber++;
        //    }

        //    UpdateUI();
        //}

        //private void Previousbtn_Click(object sender, EventArgs e)
        //{
        //    if (selectedMonthNumber - 1 < 1)
        //    {
        //        selectedMonthNumber = 12;
        //        selectedYearNumber--;
        //    }
        //    else
        //    {
        //        selectedMonthNumber--;
        //    }

        //    UpdateUI();
        //}
        //void UpdateUI()
        //{
        //    TextView CurrentMonthText = FindViewById<TextView>(Resource.Id.txt_Current);
        //    string monthName = new DateTime(selectedYearNumber, selectedMonthNumber, 1).ToString("MMMM", CultureInfo.InvariantCulture);
        //    CurrentMonthText.Text = monthName + "   "+ selectedYearNumber;

        //    Month SelectedMonth = new Month(selectedYearNumber, selectedMonthNumber);
        //    //Fragment calendarFragment = FindViewById<Fragment>(Resource.Id.fragment_container);

        //    //LinearLayout FLayout = FindViewById<LinearLayout>(Resource.Id.fragment_container);
        //    //FLayout.RemoveAllViews();

        //    FrameLayout FLayout = FindViewById<FrameLayout>(Resource.Id.fragment_container);

        //    FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
            
        //    //fragmentTransaction.AddToBackStack(null);
        //    CalendarControlFragment calendarFragment = new CalendarControlFragment();

        //    //fragmentTransaction.Add(FLayout.Id, calendarFragment);
        //    fragmentTransaction.Replace(FLayout.Id, calendarFragment);
        //    fragmentTransaction.SetTransition(FragmentTransit.FragmentOpen);
        //    calendarFragment.FragmentMonth = SelectedMonth;
        //    fragmentTransaction.Commit();
        //}
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

            //for (int i = WeekStartDayNumber; i <= 7; i++)
            //{
            //    //if (DaysInWeeks < 7 && i == DaysInWeeks)
            //    //{
            //    //    break;
            //    //}

            //    Day day = new Day();
            //    day.Number = i ;
            //    //else
            //    //{
            //    //    day.Number = i-1;
            //    //}
            //    WeekDays.Add(day);
            //}
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

