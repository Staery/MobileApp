using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using System;
using SupportActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;

namespace App1
{
    public class MyActionBarDrowrToggle : SupportActionBarDrawerToggle
    {
        private ActionBarActivity mHostActivity;
        private int mOpenedResource;
        private int mClosedResource;
        public MyActionBarDrowrToggle(ActionBarActivity host, DrawerLayout drowerLayout, int opendedResource, int closedResource)
            :base(host,drowerLayout, opendedResource, closedResource)
        {
            mHostActivity = host;
            mOpenedResource = opendedResource;
            mClosedResource = closedResource;

        }
        public override void OnDrawerOpened(View drawerView)
        {
            int drawerType = (int)drawerView.Tag;

            if (drawerType == 0)
            {
                base.OnDrawerOpened(drawerView);
                mHostActivity.SupportActionBar.SetTitle(mOpenedResource);
            }
        }

        public override void OnDrawerClosed(View drawerView)
        {
            int drawerType = (int)drawerView.Tag;

            if (drawerType == 0)
            {
                base.OnDrawerClosed(drawerView);
                mHostActivity.SupportActionBar.SetTitle(mClosedResource);
            }
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            int drawerType = (int)drawerView.Tag;

            if (drawerType == 0)
            {
                base.OnDrawerSlide(drawerView, slideOffset);
            }
        }
    }
}