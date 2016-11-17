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
    [Activity(Label = "Calendar Picker")]
    public class PickerActivity : Activity, ISelectedDateChanged
    {
        public void SelectedDateChanged(string SelectedDate)
        {
            var MainActivity = new Intent(this, typeof(MainActivity));
            if (IsPickingFromValue)
            {
                MainActivity.PutExtra("From", SelectedDate);
            }
            else
            {
                MainActivity.PutExtra("To", SelectedDate);
            }

            StartActivity(MainActivity);
        }
        bool IsPickingFromValue = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Picker);

            FrameLayout FLayout2 = FindViewById<FrameLayout>(Resource.Id.fragment_calendarPicker);
            FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();

            CalendarControlFragment calendarFragment2 = new CalendarControlFragment();
            fragmentTransaction.Replace(FLayout2.Id, calendarFragment2);
            fragmentTransaction.SetTransition(FragmentTransit.FragmentOpen);
            fragmentTransaction.Commit();

            IsPickingFromValue = Intent.GetBooleanExtra("IsPickingFromValue", false);

        }
    }
}