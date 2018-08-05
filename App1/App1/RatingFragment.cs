using Android.OS;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace App1
{
    public class RatingFragment : Android.Support.V4.App.Fragment
    {
        public ArrayAdapter ScoreAdapter;
        public Action<ArrayAdapter> mActionGetArreyRep;
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
                    string ScoreSumm, username;
                    MySqlCommand cmd = new MySqlCommand("SELECT SUM(Rez) as 'ScoreSumm', username FROM ListPassedTest join User on User.Id_User = ListPassedTest.id_User GROUP BY User.username", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    listText = new List<string>();
                    while (reader.Read())
                    {
                        ScoreSumm = reader["ScoreSumm"].ToString();
                        username = (string)reader["username"];
                        listText.Add(username + " – " + ScoreSumm + " очков");
                    }
                    ScoreAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItem1, listText);
                    try
                    {
                        mActionGetArreyRep.Invoke(ScoreAdapter);
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