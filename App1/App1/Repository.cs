using Android.Widget;
using MySql.Data.MySqlClient;
using System;

namespace App1
{
    public static class Repository 
    {
        public static int testResult;
        public static int Sum;
        public static int Count;
        public static Action<CustomListAdapter> mActionGetAdapter;
        public static Action<CustomListAdapter> mActionGetRubrAdapter;
        public static Action<ArrayAdapter> mActionGetArrey;
        public static string CurrUserName;
        public static byte[] UserImg;
        public static string URL;
        public static int UserId;
        public static int CurrTestId;
        public static byte listState = 0;
        public static string RubrName;
        public static MySqlConnection con = new MySqlConnection("Server=172.17.100.2;Port=3306;database=testrivshdb;User Id=mysql;Password=mysql;charset=utf8;Connect Timeout=300");
    }
}