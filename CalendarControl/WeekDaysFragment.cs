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

namespace CalendarControl
{
    public class WeekDaysFragment : Fragment
    {
        public Week FragmentWeek { get; set; }
        static int dayCount = 1;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.WeekDays, container, false);

            for (int i = 0; i < FragmentWeek.WeekDays.Count; i++)
            {
                int DayIndex = FragmentWeek.WeekDays[i].Number;
                bool IsWeekEnd = FragmentWeek.WeekDays[i].IsWeekEnd;
                TextView CurrentDay = null;
                switch (DayIndex)
                {
                    case 0:
                        CurrentDay = view.FindViewById<TextView>(Resource.Id.txt_SunDay);
                        CurrentDay.Text = (dayCount++).ToString();
                        break;
                    case 1:
                        CurrentDay = view.FindViewById<TextView>(Resource.Id.txt_MonDay);
                        CurrentDay.Text = (dayCount++).ToString();
                        break;
                    case 2:
                        CurrentDay = view.FindViewById<TextView>(Resource.Id.txt_TueDay);
                        CurrentDay.Text = (dayCount++).ToString();
                        break;
                    case 3:
                        CurrentDay = view.FindViewById<TextView>(Resource.Id.txt_WedDay);
                        CurrentDay.Text = (dayCount++).ToString();
                        break;
                    case 4:
                        CurrentDay = view.FindViewById<TextView>(Resource.Id.txt_ThurDay);
                        CurrentDay.Text = (dayCount++).ToString();
                        break;
                    case 5:
                        CurrentDay = view.FindViewById<TextView>(Resource.Id.txt_FriDay);
                        CurrentDay.Text = (dayCount++).ToString();
                        break;
                    case 6:
                        CurrentDay = view.FindViewById<TextView>(Resource.Id.txt_SatDay);
                        CurrentDay.Text = (dayCount++).ToString();
                        break;
                    default:
                        break;
                }

                if (IsWeekEnd)
                {
                    CurrentDay.SetBackgroundColor(new Android.Graphics.Color(255, 191, 0));
                    CurrentDay.SetTextColor(new Android.Graphics.Color(255, 255, 255));
                }
            }

            if (dayCount >= FragmentWeek.MonthDaysCount)
            {
                dayCount = 1;
            }
            return view;
        }
    }
}