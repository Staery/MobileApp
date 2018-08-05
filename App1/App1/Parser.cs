using CsQuery;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Net;

namespace App1
{
    public static class Parser
    {
        public static string RubricName;
        public static string URL_tes;
        public static string NameTest;
        public static string Host_URL;

        public static void SetSqlRubrData()
        {
            try
            {

                if (Repository.con.State == ConnectionState.Closed)
                {
                    Repository.con.Open();
                    MySqlCommand cmdsel = new MySqlCommand("Select Host_URL, id_rubr from CastomRubr", Repository.con);
                    MySqlDataReader reader = cmdsel.ExecuteReader();
                    while (reader.Read())
                    {
                        LoadParsedInfo((string)(reader["Host_URL"]), (int)(reader["id_rubr"]));
                    }
                }

            }
            catch (MySqlException)
            {
               
            }
            catch (TimeoutException)
            {
                
            }
            catch (ArgumentNullException)
            {
              
            }

            finally
            {

                Repository.con.Close();
            }
        }

        public static void SetSqlPassedTestData()
        {
            MySqlConnection con = new MySqlConnection("Server=172.17.100.2;Port=3306;database=testrivshdb;User Id=mysql;Password=mysql;charset=utf8;Connect Timeout=300");
            try
            {

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    DataSet tickets = new DataSet();
                    string queryString = "SELECT * from  Test WHERE URL_test='" + URL_tes + "'";
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(queryString, con);
                    dataAdapter.Fill(tickets, "URL_test");
                    if (tickets.Tables["URL_test"].Rows.Count == 0)
                    {
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO ListPassedTest(Date_passed,Rez,id_ListTest,id_User) VALUES(@Date_passed,@Rez,@id_ListTest,@id_User)", con);
                        string date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " +
                            DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                        cmd.Parameters.AddWithValue("@Date_passed",date); 
                        cmd.Parameters.AddWithValue("@Rez", Repository.testResult);
                        cmd.Parameters.AddWithValue("@id_ListTest",Repository.CurrTestId);
                        cmd.Parameters.AddWithValue("@id_User", Repository.UserId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
             catch (MySqlException)
                {
                    
                }
                catch (TimeoutException)
                {
                   
                }
                catch (ArgumentNullException)
                {
                   
                }

            finally
            {

                con.Close();
            }
        }

        public static void LoadParsedInfo(string url, int RubricId)
        {
            string SiteStartPage = @"https://onlinetestpad.com/";
            Host_URL = url;

            string html;
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    html = reader.ReadToEnd();
                }
            }
            response.Close();

            MySqlConnection con = new MySqlConnection("Server=172.17.100.2;Port=3306;database=testrivshdb;User Id=mysql;Password=mysql;charset=utf8;Connect Timeout=300");
            CQ cq = CQ.Create(html);
            foreach (IDomObject DivElement in cq.Find("div"))
            {
                if (DivElement.ClassName == "col-lg-3 col-md-4 col-sm-6 col-xs-12")
                {
                    foreach (IDomObject ChildElement in DivElement.FirstElementChild.ChildElements)
                    {
                        if (ChildElement.ClassName == "name")
                        {
                            NameTest = ChildElement.FirstElementChild
                                .InnerText.Replace(System.Environment.NewLine, "");
                            URL_tes = SiteStartPage +
                                ChildElement.FirstElementChild.GetAttribute("href").ToString();
                            try
                            {
                                
                                if (con.State == ConnectionState.Closed)
                                {
                                    con.Open();
                                    DataSet tickets = new DataSet();
                                    string queryString = "SELECT * from  Test WHERE URL_test='" + URL_tes + "'";
                                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(queryString, con);
                                    dataAdapter.Fill(tickets, "URL_test");
                                    if (tickets.Tables["URL_test"].Rows.Count == 0)
                                    {
                                        MySqlCommand cmd = new MySqlCommand("INSERT INTO Test(URL_test,NameTest,id_rubr) VALUES(@URL_test,@NameTest,@id_rubr)", con);
                                        cmd.Parameters.AddWithValue("@URL_test", URL_tes);
                                        cmd.Parameters.AddWithValue("@NameTest", NameTest);
                                        cmd.Parameters.AddWithValue("@id_rubr", RubricId);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (MySqlException)
                            {

                            }
                            catch (TimeoutException)
                            {

                            }
                            catch (ArgumentNullException)
                            {

                            }

                            finally
                            {

                               con.Close();
                            }

                        }
                    }
                }

            }
        }
        public static void GetCurrTestResult(string url)
        {
            string ResultUrl = url;


            string html;
            WebRequest request = WebRequest.Create(ResultUrl);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    html = reader.ReadToEnd();
                }
            }
            response.Close();


            CQ cq = CQ.Create(html);
            /* FIRST VARIABLE
            foreach (IDomObject tdElement in cq.Find("div"))
            {
                if (tdElement.ClassName == "dSolidGaugePercent")
                {
                    var ss = tdElement.GetAttribute("sgp-percent");
                }
            }*/
            
            int count = 0;
            int rest;
            foreach (IDomObject tdElement in cq.Find("td"))
            {
                if (tdElement.ClassName == "ta-c")
                {
                    count++;
                    if (count == 3)
                    {
                        rest = int.Parse(tdElement.InnerText.Replace(System.Environment.NewLine, "")) % 10;
                       
                        if (rest >= 5)
                        {
                            Repository.testResult  = (int.Parse(tdElement.InnerText.Replace(System.Environment.NewLine, ""))/10 + 1);
                        }
                        else
                        {
                            Repository.testResult = int.Parse(tdElement.InnerText.Replace(System.Environment.NewLine, ""))/10 ;
                        }
                        break;
                    }
                }
            }
        }


    }

}
