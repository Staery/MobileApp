using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Webkit;
using System.Text.RegularExpressions;

namespace App1
{
    [Activity(Label = "WebActivity")]
    public class WebActivity : Activity
    {
        private WebClient mWebClient;
        private WebView webView;
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            mWebClient = new WebClient();
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.WebLayout);
            webView = FindViewById<WebView>(Resource.Id.webview);
            webView.LoadUrl(Repository.URL);
            webView.Settings.JavaScriptEnabled = true;
            webView.SetWebViewClient(mWebClient);
        }
    }

    public class  WebClient : WebViewClient
    {
        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {

            bool result = Regex.IsMatch(url, "result");
            if (result == true)
            {
                Parser.GetCurrTestResult(url);
                Parser.SetSqlPassedTestData();
            }
            base.OnPageStarted(view, url, favicon);
        }
    }
}