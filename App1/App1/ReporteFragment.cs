using Android.OS;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace App1
{
    public class ReporteFragment : Android.Support.V4.App.Fragment
    {
        public ArrayAdapter ScoreAdapter;
        public Action<ArrayAdapter> mActionGetArrey;
        private ListView _listView;
        private View view;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
             view = inflater.Inflate(Resource.Layout.UserFragmentLayout, container, false);
            _listView = view.FindViewById<ListView>(Resource.Id.XlistView);
            SetResult();
            return view;
        }

        private void SetResult()
        {
            List<string> listText;

            MySqlConnection con = new MySqlConnection("Server=172.17.100.2;Port=3306;database=testrivshdb;User Id=mysql;Password=mysql;charset=utf8;Connect Timeout=300");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand("SELECT NameTest, username, Date_passed FROM ListPassedTest join User " +
                        "on User.Id_User = ListPassedTest.id_User JOIN Test on Test.id_Test = ListPassedTest.id_ListTest", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    listText = new List<string>();
                    while (reader.Read())
                    {
                        listText.Add((string)reader["username"] + " прошёл тест «" + (string)reader["NameTest"] + "» " + reader["Date_passed"].ToString());
                    }
                    ScoreAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItem1, listText);
                    try
                    {
                        mActionGetArrey.Invoke(ScoreAdapter);
                    }
                    catch
                    { }
                    _listView.Adapter = ScoreAdapter;
                }

            }
            catch (MySqlException)
            {
                Toast.MakeText(Activity, "Ошибка, проверье своё интернет соединение", ToastLength.Long).Show();
            }
            catch (TimeoutException)
            {
                Toast.MakeText(Activity, "Ошибка, проверье своё интернет соединение", ToastLength.Long).Show();
            }
            catch (ArgumentNullException)
            {
                Toast.MakeText(Activity, "Ошибка, проверье своё интернет соединение", ToastLength.Long).Show();
            }
            finally
            {
                con.Close();
            }
        }
    }
}