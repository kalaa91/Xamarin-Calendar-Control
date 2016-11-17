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
        LinearLayout daysLinearLayoutHolder = null;

        public CalendarControlFragment()
        { }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                // Use this to return your custom view for this Fragment
                // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

                View view = inflater.Inflate(Resource.Layout.CalendarControlUI, container, false);
                CalendarInit(view, container);
                Button Previousbtn = view.FindViewById<Button>(Resource.Id.btn_Prev);
                Button Nextbtn = view.FindViewById<Button>(Resource.Id.btn_Next);
                Previousbtn.Click += (sender, args) =>
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
                    CalendarInit(view, container);

                };

                Nextbtn.Click += (sender, args) =>
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
                    CalendarInit(view, container);
                };

                return view;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        void CalendarInit(View view, ViewGroup viewGroup)
        {
            TextView CurrentMonthText = view.FindViewById<TextView>(Resource.Id.txt_Current);
            LinearLayout ParentView = (LinearLayout)view;
            if (daysLinearLayoutHolder == null)
            {
                daysLinearLayoutHolder = new LinearLayout(viewGroup.Context);
                daysLinearLayoutHolder.Orientation = Orientation.Vertical;
                Random r = new Random();
                daysLinearLayoutHolder.Id = r.Next();
                ParentView.AddView(daysLinearLayoutHolder);
            }
            else
            {
                daysLinearLayoutHolder.RemoveAllViews();
            }
            string monthName = new DateTime(selectedYearNumber, selectedMonthNumber, 1).ToString("MMMM", CultureInfo.InvariantCulture);
            CurrentMonthText.Text = monthName + "   " + selectedYearNumber;
            FragmentMonth = new Month(selectedYearNumber, selectedMonthNumber);
            FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
            FrameLayout frameLayout = view.FindViewById<FrameLayout>(Resource.Id.daysNames_fragment);
            WeekHeaderFragment weekHeaderFragment = new WeekHeaderFragment();
            fragmentTransaction.Add(frameLayout.Id, weekHeaderFragment);
            for (int i = 0; i < FragmentMonth.MonthWeeks.Count; i++)
            {
                WeekDaysFragment weekDaysFragment = new WeekDaysFragment();
                fragmentTransaction.Add(daysLinearLayoutHolder.Id, weekDaysFragment);
                weekDaysFragment.FragmentWeek = FragmentMonth.MonthWeeks[i];
            }
            fragmentTransaction.Commit();
        }
    }
}