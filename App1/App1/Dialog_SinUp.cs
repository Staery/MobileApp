using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace App1
{
    class Dialog_SinUp:DialogFragment
    {
        private Button BtnSignUp;
        public static readonly int PickImageId = 1000;
        private EditText etUsername, etPassword, etName, etImg;
        public Action<EditText> mActionPicSelected;
        public Action<Button> mActionBtnClikced;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.RegPage, container, false);
            BtnSignUp = view.FindViewById<Button>(Resource.Id.XbtnInsert);
            etUsername = view.FindViewById<EditText>(Resource.Id.XetUsername);
            etPassword = view.FindViewById<EditText>(Resource.Id.XetPassword);
            etName = view.FindViewById<EditText>(Resource.Id.XetName);
            etImg = view.FindViewById<EditText>(Resource.Id.XetImg);
            etImg.FocusChange += EtImg_FocusChange;
            BtnSignUp.Click += BtnSignUp_Click;
            return view;
        }

        private void EtImg_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus == true)
            {
                etImg.Enabled = false;
                mActionPicSelected.Invoke((EditText)sender);
            }
        }


        

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            //User has clicked the sign up button  //Используйте 10.0.2.2 для AVD по умолчанию и 10.0.3.2 для genymotion.
            MySqlConnection con = new MySqlConnection("Server=172.17.100.2;Port=3306;database=testrivshdb;User Id=mysql;Password=mysql;charset=utf8;Connect Timeout=300");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    if (etUsername.Text != "" && etPassword.Text != "" && etName.Text != "" && etImg.Text != "")
                    {
                        if (etPassword.Text.Length > 5 && etPassword.Text.Length < 31)
                        {
                           con.Open(); con.Open();
                            string enclog = AesCryp.Encrypt(etName.Text);
                            string encpass = AesCryp.Encrypt(etPassword.Text);
                            DataSet tickets = new DataSet();
                            string queryString = "SELECT * from  User WHERE userlogin='" + enclog + "'";
                            MySqlDataAdapter comn = new MySqlDataAdapter(queryString, con);
                            comn.Fill(tickets, "userlogin");
                            if (tickets.Tables["userlogin"].Rows.Count == 0)
                            {
                                MySqlCommand cmd = new MySqlCommand("INSERT INTO User(userlogin,userpass,username,userimg) VALUES(@userlogin,@userpass,@username,@userimg)", con);
                                cmd.Parameters.AddWithValue("@userlogin", enclog);
                                cmd.Parameters.AddWithValue("@userpass", encpass);
                                cmd.Parameters.AddWithValue("@username", etUsername.Text);
                                cmd.Parameters.AddWithValue("@userimg", etImg.Text);
                                cmd.ExecuteNonQuery();
                                this.Dismiss();
                                mActionBtnClikced.Invoke((Button)sender);
                            }
                            else
                            {
                                Toast.MakeText(this.Activity, "Такой логин уже существует", ToastLength.Long).Show();
                            }
                        }
                        else
                        {
                            Toast.MakeText(this.Activity, "Размер пароля от 6 до 30 символов", ToastLength.Long).Show();
                        }
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, "Все поля должны быть заполнены", ToastLength.Long).Show();
                    }

                }
            }
            catch (MySqlException ex)
            {
                string deb = ex.ToString();
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

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }

    }
}