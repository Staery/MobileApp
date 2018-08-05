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
using MySql.Data.MySqlClient;

namespace App1
{


    public class UserFragment : Android.Support.V4.App.Fragment
    {
        public CustomListAdapter adapter;
        
        private ListView _listView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.UserFragmentLayout, container, false);

           _listView = view.FindViewById<ListView>(Resource.Id.XlistView);
            adapter = new CustomListAdapter((Activity)view.Context, GetCustomControl());
            try
            {
                Repository.mActionGetAdapter.Invoke(adapter);
            }
            catch
            {}
            _listView.Adapter = adapter;
            _listView.ItemClick += _listView_ItemClick1;
            return view;
        }

        private void _listView_ItemClick1(object sender, AdapterView.ItemClickEventArgs e)
        {
           
        }

        //data source
        private List<CustomControl> GetCustomControl()
        {

            
            try
            {
                Repository.con.Open();
                MySqlCommand cmd = new MySqlCommand("Select username, userimg from User", Repository.con);
                MySqlDataReader reader = cmd.ExecuteReader();

                var customControls = new List<CustomControl>();
                CustomControl c;

                while (reader.Read())
                {


                    // string s = Convert.ToBase64String(ObjectToByteArray(reader["userimg"]));
                    //    var ss = ;
                    string result = System.Text.Encoding.UTF8.GetString((byte[])reader["userimg"]);
                    byte[] data = Convert.FromBase64String(result);
                    c = new CustomControl { Name = reader["username"].ToString(), Image = data };
                    customControls.Add(c);


                    /*for (int i = 0; i < ss.GetLength(0); i++)
                    {

                    }*/
                }



                return customControls;
            }
            catch (MySqlException)
            {
                Toast.MakeText(Activity, "Ошибка, проверье своё интернет соединение", ToastLength.Long).Show();
                return null;
            }
            catch (TimeoutException)
            {
                Toast.MakeText(Activity, "Ошибка, проверье своё интернет соединение", ToastLength.Long).Show();
                return null;
            }
            catch (ArgumentNullException)
            {
                Toast.MakeText(Activity, "Ошибка, проверье своё интернет соединение", ToastLength.Long).Show();
                return null;
            }
            finally
            {
                Repository.con.Close();
            }
        }
        // listView itemClick
        private void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(Activity, adapter[e.Position].Name, ToastLength.Short).Show();
        }
    }
}