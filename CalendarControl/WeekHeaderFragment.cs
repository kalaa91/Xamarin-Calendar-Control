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

namespace CalendarControl
{
    public class WeekHeaderFragment : Fragment
    {
        public TextView sat
        {
            get;
            set;
        }

        //public LinearLayout FLayout { get; set; }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HeaderList, container, false);
            sat = view.FindViewById<TextView>(Resource.Id.sat);
            //FLayout = view.FindViewById<LinearLayout>(Resource.Id.fragment_container);
            return view;
        }

        public void GetSat(int x)
        {
            //var act = (MainActivity)this.Activity;
            //var asdasd = act.x;
            //WeekDaysFragment wdf = new WeekDaysFragment();


            //for (int i = 0; i < x; i++)
            //{
            //    WeekDaysFragment weekHeaderFragment = new WeekDaysFragment();

            //    FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
            //    //FrameLayout fl = new FrameLayout(this.Context);
            //    //fl.Id = i;

            //    fragmentTransaction.Add(FLayout.Id, weekHeaderFragment);
            //    fragmentTransaction.Commit();

            //    //FLayout.AddView(fl);
            //}

        }

        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    // Use this to return your custom view for this Fragment
        //    // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

        //    return base.OnCreateView(inflater, container, savedInstanceState);
        //}
    }
}