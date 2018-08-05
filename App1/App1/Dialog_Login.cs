using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace App1
{
    class Dialog_Login : DialogFragment
    {
        private Button mbtnLoginIn;
        private EditText etLogin, etPass;
        public Action<Button> mActionBtnLogin;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.LoginPage, container, false);
            mbtnLoginIn = view.FindViewById<Button>(Resource.Id.XbtnLoginIn);
            etLogin = view.FindViewById<EditText>(Resource.Id.XetLogin);
            etPass = view.FindViewById<EditText>(Resource.Id.XetPass);
            mbtnLoginIn.Click += MbtnLoginIn_Click;
            return view;
        }

        private void MbtnLoginIn_Click(object sender, EventArgs e)
        {
            //User has clicked the sign up button
            MySqlConnection con = new MySqlConnection("Server=172.17.100.2;Port=3306;database=testrivshdb;User Id=mysql;Password=mysql;charset=utf8;Connect Timeout=300");
            try
             {
                 if (con.State == ConnectionState.Closed)
                 {
                    if (etLogin.Text != "" && etPass.Text != "")
                    {
                        con.Open();
                        DataSet tickets = new DataSet();
                        string userName = AesCryp.Encrypt(etLogin.Text);
                        string password = AesCryp.Encrypt(etPass.Text);
                        string queryString = "SELECT * from  User WHERE userlogin='" + userName + "' and userpass='" + password + "'";
                        MySqlDataAdapter cmd = new MySqlDataAdapter(queryString, con);
                        cmd.Fill(tickets, "userlogin");
                        if (tickets.Tables["userlogin"].Rows.Count > 0)
                        {
                            Activity.StartActivity(typeof(DetailActivity));
                            MySqlCommand cmdsel = new MySqlCommand(queryString, con);
                            MySqlDataReader reader = cmdsel.ExecuteReader();
                            while (reader.Read())
                            {
                                Repository.UserId = (int)(reader["Id_User"]);
                                string result = System.Text.Encoding.UTF8.GetString((byte[])reader["userimg"]);
                                Repository.UserImg = Convert.FromBase64String(result);
                                Repository.CurrUserName = (string)(reader["username"]);
                            }
                            reader.Close();
                            queryString = "Select SUM(Rez) from ListPassedTest where Id_User = " + Repository.UserId;
                            MySqlCommand showresult = new MySqlCommand(queryString, con);
                            try
                            {
                                Repository.Sum = int.Parse(showresult.ExecuteScalar().ToString());
                                queryString = "Select Count(Rez) from ListPassedTest where Id_User = " + Repository.UserId;
                                MySqlCommand showcount = new MySqlCommand(queryString, con);
                                Repository.Count = int.Parse(showcount.ExecuteScalar().ToString());
                            }
                            catch { }
                            mActionBtnLogin.Invoke((Button)sender);
                        }
                        else
                        {
                            Toast.MakeText(this.Activity, "Неверный логин или пароль", ToastLength.Long).Show();
                        }
                       
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, "Все поля должны быть заполнены", ToastLength.Long).Show();
                    }
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
           
            this.Dismiss();
           
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}