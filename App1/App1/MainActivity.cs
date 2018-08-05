using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using System.Threading;

namespace App1
{
    [Activity(Label = "TestRIVSH", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Button BtnInsert, btnSignUp;
        public static readonly int PickImageId = 1000;
        private EditText etImg;
        private ProgressBar mProgressBar;
        //private EditText etImgScr;
        //private Button btnInsert;
        //private TextView txtSysLog;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Dialog_SinUp SingUpDialog = new Dialog_SinUp();
            Dialog_Login LoginDialog = new Dialog_Login();
            SingUpDialog.mActionBtnClikced = BtnClicked;
            SingUpDialog.mActionPicSelected = PicSelected;
            LoginDialog.mActionBtnLogin = LoginClick;
           

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            BtnInsert = FindViewById<Button>(Resource.Id.btnSignIn);
            btnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            BtnInsert.Click += (object sender, System.EventArgs args) =>
            {
                //Pull up dialog
                 FragmentTransaction transaction = FragmentManager.BeginTransaction();
                 SingUpDialog.Show(transaction, "dialog fragment");
               //StartActivity(typeof(DetailActivity));
            };
            btnSignUp.Click += (object sender, System.EventArgs args) =>
            {
                //Pull up dialog
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                LoginDialog.Show(transaction, "dialog fragment");
            };
            if (IsOnline() == false)
            {
                Toast.MakeText(this, "Отсутствует соедиение с интернетом", ToastLength.Long).Show();
            }

        }

        public bool IsOnline()
        {
            ConnectivityManager cm =
                (ConnectivityManager)GetSystemService(ConnectivityService);
            NetworkInfo netInfo = cm.ActiveNetworkInfo;
            return netInfo != null && netInfo.IsConnectedOrConnecting;
        }

        private void BtnClicked(Button buttonClikced)
        {
            mProgressBar.Visibility = ViewStates.Visible;
            Thread thread = new Thread(ActLikeARequest);
            thread.Start();

        }
        private void LoginClick (Button buttonLog)
        {
            mProgressBar.Visibility = ViewStates.Visible;
            Thread thread = new Thread(ActLikeARequest);
            thread.Start();
        }
       private void PicSelected(EditText selectedPic)
        {
            etImg = selectedPic;
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                Stream stream = ContentResolver.OpenInputStream(data.Data);

                Bitmap bitmap = BitmapFactory.DecodeStream(stream);
                MemoryStream memoryStream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Webp, 100, memoryStream);
                byte[] picData = memoryStream.ToArray();
                etImg.Text = Convert.ToBase64String(picData);
            }
        }

        private void ActLikeARequest()
        {
            Thread.Sleep(3000);

            RunOnUiThread(() => { mProgressBar.Visibility = ViewStates.Invisible; });
            int x = Resource.Animation.Slide_Right;
        }
    }
}

