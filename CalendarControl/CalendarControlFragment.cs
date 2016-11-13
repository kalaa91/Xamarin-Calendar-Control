using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Globalization;

namespace CalendarControl
{
    public class CalendarControlFragment : Fragment
    {
        public Month FragmentMonth { get; set; }
        int selectedMonthNumber = DateTime.Today.Month;
        int selectedYearNumber = DateTime.Today.Year;
        View view = null;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            view = inflater.Inflate(Resource.Layout.CalendarControlUI, container, false);
            CalendarInit();
            Button Previousbtn = view.FindViewById<Button>(Resource.Id.btn_Prev);
            Button Nextbtn = view.FindViewById<Button>(Resource.Id.btn_Next);
            Previousbtn.Click += Previousbtn_Click;
            Nextbtn.Click += Nextbtn_Click;
            return view;
        }

        private void Nextbtn_Click(object sender, EventArgs e)
        {
            if (selectedMonthNumber + 1 > 12)
            {
                selectedMonthNumber = 1;
                selectedYearNumber++;
            }
            else
            {
                selectedMonthNumber++;
            }

            CalendarInit();
        }

        private void Previousbtn_Click(object sender, EventArgs e)
        {
            if (selectedMonthNumber - 1 < 1)
            {
                selectedMonthNumber = 12;
                selectedYearNumber--;
            }
            else
            {
                selectedMonthNumber--;
            }

            CalendarInit();
        }

        void CalendarInit()
        {

            TextView CurrentMonthText = view.FindViewById<TextView>(Resource.Id.txt_Current);
            string monthName = new DateTime(selectedYearNumber, selectedMonthNumber, 1).ToString("MMMM", CultureInfo.InvariantCulture);
            CurrentMonthText.Text = monthName + "   " + selectedYearNumber;

            FragmentMonth = new Month(selectedYearNumber, selectedMonthNumber);

            FrameLayout frameLayout = view.FindViewById<FrameLayout>(Resource.Id.daysNames_fragment);

            WeekHeaderFragment weekHeaderFragment = new WeekHeaderFragment();
            FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
            fragmentTransaction.Add(frameLayout.Id, weekHeaderFragment);



            LinearLayout linearLayout = view.FindViewById<LinearLayout>(Resource.Id.fragment_weekDays);
            linearLayout.RemoveAllViews();

            for (int i = 0; i < FragmentMonth.MonthWeeks.Count; i++)
            {
                WeekDaysFragment weekDaysFragment = new WeekDaysFragment();
                fragmentTransaction.Add(linearLayout.Id, weekDaysFragment);
                weekDaysFragment.FragmentWeek = FragmentMonth.MonthWeeks[i];
            }

            fragmentTransaction.Commit();
        }
    }
}