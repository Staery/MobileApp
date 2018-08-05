using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using SearchView = Android.Support.V7.Widget.SearchView;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;


namespace App1
{


    [Activity(Label = "DetailActivity", Theme = "@style/MyTheme")]
    public class DetailActivity : ActionBarActivity
    {
        TabLayout tabLayout;
        private EditText etImg;
        private SupportToolbar mToolbar;
        public static readonly int PickImageId = 1000;
        private MyActionBarDrowrToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ScrollView mLeftDrawer;
        private ArrayAdapter mLeftAdapter;
        private List<string> mLeftDataSet;
        private CustomListAdapter UserAdapter;
        private CustomListAdapter RubricAdapter;
        private ArrayAdapter RubricArrey;
        private ArrayAdapter RaitingArrey;
        private ArrayAdapter ReportArrey;
        private SearchView _searchView;
        private TextView TextUserName, TextScore, TestPassed;
        private ImageView UserImg;
        private Button WebClic;
        private WebView webView;





        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.DetailPage);

            mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            mLeftDrawer = FindViewById<ScrollView>(Resource.Id.left_drawer);
            TextUserName = FindViewById<TextView>(Resource.Id.txtUserName);
            TextScore = FindViewById<TextView>(Resource.Id.txtScore);
            TestPassed = FindViewById<TextView>(Resource.Id.txtTestPassed);
            UserImg = FindViewById<ImageView>(Resource.Id.XimgUser);
            WebClic = FindViewById<Button>(Resource.Id.XbtnOpenWeb);
            WebClic.Visibility = ViewStates.Invisible;
            MemoryStream ms = new MemoryStream(Repository.UserImg);
            Bitmap image = BitmapFactory.DecodeStream(ms);
            UserImg.SetImageBitmap(image);
            TextUserName.Text = Repository.CurrUserName;
            TextScore.Text = "Набрано очков: " + Repository.Sum.ToString();
            TestPassed.Text = "Пройдено тестов: " + Repository.Count.ToString();
            FnInitTabLayout();

            mLeftDrawer.Tag = 0;

            SetSupportActionBar(mToolbar);

            mLeftAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mLeftDataSet);
           // mLeftDrawer.Adapter = mLeftAdapter;

            mDrawerToggle = new MyActionBarDrowrToggle(
                this, //Host Activity
                mDrawerLayout, //DrawerLayout
                Resource.String.openDrawer, //Open Message
                Resource.String.closeDrawer // Closed MEssage
                );
            mDrawerLayout.AddDrawerListener﻿(mDrawerToggle);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            mDrawerToggle.SyncState();

            if (savedInstanceState != null)
            {
                if (savedInstanceState.GetString("DrawerState") == "Opened")
                {
                    SupportActionBar.SetTitle(Resource.String.openDrawer);
                }

                else
                {
                    SupportActionBar.SetTitle(Resource.String.closeDrawer);
                }
            }

            else
            {
                //This is the first the time the activity is ran
                SupportActionBar.SetTitle(Resource.String.closeDrawer);
            }
            if (Repository.UserId == 61)
            {
                WebClic.Click += WebClic_Click;
                WebClic.Visibility = ViewStates.Visible;
            }
        }

        private void WebClic_Click(object sender, EventArgs e)
        {
            //RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.WebLayout);
            webView = FindViewById<WebView>(Resource.Id.webview);
            webView.LoadUrl(@"https://onlinetestpad.com/ru-ru/Main/TestMaking.aspx");
            webView.Settings.JavaScriptEnabled = true;

        }

        private void GetUserAdapter(CustomListAdapter adapt)
        {
            UserAdapter = adapt;
        }

        private void GetRubricAdapter(CustomListAdapter adapt)
        {
            RubricAdapter = adapt;
        }
        private void GetArrey(ArrayAdapter adapt)
        {
            RubricArrey = adapt;
        }

        private void GetRatingAdapter(ArrayAdapter adapt)
        {
            RaitingArrey = adapt;
        }

        private void GetReporteAdapter(ArrayAdapter adapt)
        {
            ReportArrey = adapt;
        }

        private void FnInitTabLayout()
        {
            tabLayout.SetTabTextColors(Android.Graphics.Color.Aqua, Android.Graphics.Color.AntiqueWhite);
            //Fragment array
            UserFragment userFragment = new UserFragment();
            RubricFragment rubricFragment = new RubricFragment();
            RatingFragment ratingFragment = new RatingFragment();
            ReporteFragment reporteFragment = new ReporteFragment();
            Repository.mActionGetRubrAdapter = GetRubricAdapter;
            Repository.mActionGetArrey = GetArrey;
            Repository.mActionGetAdapter = GetUserAdapter;
            ratingFragment.mActionGetArreyRep = GetRatingAdapter;
            reporteFragment.mActionGetArrey = GetReporteAdapter;
            if (Repository.UserId == 61)
            {
                var fragments = new Android.Support.V4.App.Fragment[]
                {
    userFragment,
    rubricFragment,
    ratingFragment,
    reporteFragment,
                };
                //Tab title array
                var titles = CharSequence.ArrayFromStringArray(new[] {
    "Пользователи",
    "Рубрики",
    "Рейтинг",
    "Отчёт",
   });
                var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
                //viewpager holding fragment array and tab title text
                viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments, titles);
                // Give the TabLayout the ViewPager 

                tabLayout.SetupWithViewPager(viewPager);
            }
           else if (Repository.UserId != 61)
            {
                var fragments = new Android.Support.V4.App.Fragment[]
               {
    rubricFragment,
    ratingFragment,
               };
                //Tab title array
                var titles = CharSequence.ArrayFromStringArray(new[] {
    "Рубрики",
    "Рейтинг",
   });
                var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
                //viewpager holding fragment array and tab title text
                viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments, titles);
                // Give the TabLayout the ViewPager 

                tabLayout.SetupWithViewPager(viewPager);
            }
        }

   

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {

                case Android.Resource.Id.Home:
                    //The hamburger icon was clicked which means the drawer toggle will handle the event
                    //all we need to do is ensure the right drawer is closed so the don't overlap
                    mDrawerLayout.CloseDrawer(mLeftDrawer);
                    mDrawerToggle.OnOptionsItemSelected(item);
                    return true;
                case Resource.Id.add:
                    CreateContactDialog dialog = new CreateContactDialog();
                    Android.App.FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    dialog.mActionPicSelected = PicSelected;
                    dialog.Show(transaction, "create contact");
                    return true;
                case Resource.Id.refresh:
                    Parser.SetSqlRubrData();
                     Recreate();
                    return true;
                case Resource.Id.trash:
                    Repository.listState = 1;
                    Recreate();
                    return true;
                case Resource.Id.close:
                    Repository.listState = 0;
                    Recreate();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar, menu);
            if (Repository.UserId != 61)
            {
                menu.FindItem(Resource.Id.close).SetVisible(false);
                menu.FindItem(Resource.Id.trash).SetVisible(false);
                menu.FindItem(Resource.Id.add).SetVisible(false);
                menu.FindItem(Resource.Id.refresh).SetVisible(false);
            }
            if (Repository.listState == 0 || Repository.listState == 2)
            {
                menu.FindItem(Resource.Id.close).SetVisible(false);
            }
            else if (Repository.listState == 1)
            {
                menu.FindItem(Resource.Id.trash).SetVisible(false);
            }
       
            var item = menu.FindItem(Resource.Id.search);
            var searchView = MenuItemCompat.GetActionView(item);
            _searchView = searchView.JavaCast<SearchView>();
            _searchView.QueryHint = "Поиск...";
            try
            {

                _searchView.QueryTextChange += (s, e) =>
                {
                    if (UserAdapter != null)
                        UserAdapter.Filter.InvokeFilter(e.NewText);
                };
                _searchView.QueryTextChange += (s, e) =>
                {
                    if (RubricAdapter != null)
                        RubricAdapter.Filter.InvokeFilter(e.NewText);
                };
                _searchView.QueryTextChange += (s, e) => {
                    if (RubricArrey != null)
                    RubricArrey.Filter.InvokeFilter(e.NewText);
                }; 
            }
            catch { }

            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            if (mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left))
            {
                outState.PutString("DrawerState", "Opened");
            }

            else
            {
                outState.PutString("DrawerState", "Closed");
            }

            base.OnSaveInstanceState(outState);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            mDrawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            mDrawerToggle.OnConfigurationChanged(newConfig);
            Recreate();
        }

        public override void OnBackPressed()
        {
            MySqlConnection con = new MySqlConnection("Server=172.17.100.2;Port=3306;database=testrivshdb;User Id=mysql;Password=mysql;charset=utf8;Connect Timeout=300");
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    string queryString;
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
                }
            }
            catch (MySqlException)
            {
                Toast.MakeText(this, "Ошибка, проверье своё интернет соединение", ToastLength.Long).Show();
            }
            catch (TimeoutException)
            {
                Toast.MakeText(this, "Ошибка, проверье своё интернет соединение", ToastLength.Long).Show();
            }
            catch (ArgumentNullException)
            {
                Toast.MakeText(this, "Ошибка, проверье своё интернет соединение", ToastLength.Long).Show();
            }
            finally
            {
                con.Close();
            }
            TextScore.Text = "Набрано очков: " + Repository.Sum.ToString();
            TestPassed.Text = "Пройдено тестов: " + Repository.Count.ToString();
            Recreate();
            Repository.listState = 0;

        }
    }
    
}