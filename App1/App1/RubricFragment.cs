using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace App1
{
    public class RubricFragment : Android.Support.V4.App.Fragment
    {
        public CustomListAdapter adapter;
        public ArrayAdapter mLeftAdapter;
        private ListView _listView;
        private WebView webView;
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
            webView = view.FindViewById<WebView>(Resource.Id.webview);
            _listView = view.FindViewById<ListView>(Resource.Id.XlistView);
            adapter = new CustomListAdapter((Activity)view.Context, GetCustomControl());
            try
            {
                Repository.mActionGetRubrAdapter.Invoke(adapter);
            }
            catch
            { }
            _listView.Adapter = adapter;
            _listView.ItemClick += _listView_ItemClick1;
            return view;
        }

        private void _listView_ItemClick1(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (Repository.listState == 1)
            {
                try
                {
                    string sql = "DELETE rt FROM CastomRubr LEFT JOIN Test rt ON rt.id_rubr = CastomRubr.id_rubr WHERE CastomRubr.rubr_name = @rubr_name";
                    Repository.con.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, Repository.con);
                    cmd.Parameters.AddWithValue("@rubr_name", adapter[e.Position].Name);
                    cmd.ExecuteNonQuery();
                    sql = "DELETE FROM CastomRubr WHERE CastomRubr.rubr_name = @rubr_name";
                    MySqlCommand cm = new MySqlCommand(sql, Repository.con);
                    cm.Parameters.AddWithValue("@rubr_name", adapter[e.Position].Name);
                    cm.ExecuteNonQuery();

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
                    Repository.con.Close();
                }
            }
            else if (Repository.listState == 0)
            {
                List<string> listText;
                
                MySqlConnection con = new MySqlConnection("Server=172.17.100.2;Port=3306;database=testrivshdb;User Id=mysql;Password=mysql;charset=utf8;Connect Timeout=300");
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT NameTest, rubr_name FROM CastomRubr join Test on Test.id_rubr = CastomRubr.id_rubr", con);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        listText = new List<string>();
                        while (reader.Read())
                        {
                            if (adapter[e.Position].Name == (string)reader["rubr_name"])
                            {

                                listText.Add((string)reader["NameTest"]);
                            }
                        }
                        mLeftAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItem1, listText);
                        try
                        {
                            Repository.mActionGetArrey.Invoke(mLeftAdapter);
                        }
                        catch
                        { }
                        _listView.Adapter = mLeftAdapter;
                        Repository.listState = 2;
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
            else if (Repository.listState == 2)
            {

                MySqlConnection con = new MySqlConnection("Server=172.17.100.2;Port=3306;database=testrivshdb;User Id=mysql;Password=mysql;charset=utf8;Connect Timeout=300");
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT URL_test, NameTest, id_Test FROM Test", con);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if ((string)mLeftAdapter.GetItem(e.Position) == (string)reader["NameTest"])
                            {
                                Repository.URL = (string)reader["URL_test"];
                                Repository.CurrTestId = (int)reader["id_Test"]; 
                                Activity.StartActivity(typeof(WebActivity));
                            }
                        }
                        Repository.listState = 2;
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

        //data source
        private List<CustomControl> GetCustomControl()
        {

            MySqlConnection con = new MySqlConnection("Server=172.17.100.2;Port=3306;database=testrivshdb;User Id=mysql;Password=mysql;charset=utf8;Connect Timeout=300");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Select rubr_name, rubr_img from CastomRubr", con);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    var customControls = new List<CustomControl>();
                    CustomControl c;

                    while (reader.Read())
                    {


                        // string s = Convert.ToBase64String(ObjectToByteArray(reader["userimg"]));
                        //    var ss = ;
                        string result = System.Text.Encoding.UTF8.GetString((byte[])reader["rubr_img"]);
                        byte[] data = Convert.FromBase64String(result);
                        c = new CustomControl { Name = reader["rubr_name"].ToString(), Image = data };
                        customControls.Add(c);


                        /*for (int i = 0; i < ss.GetLength(0); i++)
                        {

                        }*/
                    }
                    return customControls;
                }
                return null;
               
            }
            catch (MySqlException ex)
            {
                string deb;
                deb = ex.ToString();
                return null;
            }
            finally
            {
                con.Close();
            }
        }
        // listView itemClick
        private void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(Activity, adapter[e.Position].Name, ToastLength.Short).Show();
        }
    }



}
