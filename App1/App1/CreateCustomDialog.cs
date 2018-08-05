using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace App1
{

    class CreateContactDialog : DialogFragment
    {
        private Button mButtonCreateContact;
        private EditText txtName;
        private EditText txtUrl;
        private EditText txtImg;
        public Action<EditText> mActionPicSelected;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.AddCustom, container, false);
            mButtonCreateContact = view.FindViewById<Button>(Resource.Id.XbtnAddCas);
            txtName = view.FindViewById<EditText>(Resource.Id.RybrName);
            txtUrl = view.FindViewById<EditText>(Resource.Id.XUrlHost);
            txtImg = view.FindViewById<EditText>(Resource.Id.XimgScr);
            txtImg.FocusChange += TxtImg_FocusChange;
            mButtonCreateContact.Click += MButtonCreateContact_Click;
            return view;

        }

        private void TxtImg_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus == true)
            {
                txtImg.Enabled = false;
                mActionPicSelected.Invoke((EditText)sender);
            }
        }

        private void MButtonCreateContact_Click(object sender, EventArgs e)
        {
            //User has clicked the sign up button
            MySqlConnection con = new MySqlConnection("Server=172.17.100.2;Port=3306;database=testrivshdb;User Id=mysql;Password=mysql;charset=utf8;Connect Timeout=300");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    if (txtName.Text != "" && txtImg.Text != "")
                    {
                        con.Open();
                        // MySqlCommand cmd = new MySqlCommand("INSERT INTO tblTest(user,pass) VALUES(@user,@pass)",con);
                        // MySqlCommand cmd = new MySqlCommand("SELECT user FROM tblTest WHERE user = @user AND pass = @pass ", con);
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO CastomRubr(rubr_name,Host_URL,rubr_img) VALUES(@rubr_name,@Host_URL,@rubr_img)", con);
                        cmd.Parameters.AddWithValue("@rubr_name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Host_URL", txtUrl.Text);
                        cmd.Parameters.AddWithValue("@rubr_img", txtImg.Text);
                        cmd.ExecuteNonQuery();
                        this.Dismiss();
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
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);//Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}