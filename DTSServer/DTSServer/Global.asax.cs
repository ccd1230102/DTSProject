using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.SessionState;

namespace DTSServer
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);
            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "IF NOT EXISTS(SELECT * FROM AutomaticConfig) INSERT INTO AutomaticConfig(LeftDays, Enable) VALUES(7, 0)"
                };

                cmd.ExecuteNonQuery();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
            }

            System.Threading.Thread LoadServiceData = new System.Threading.Thread(new System.Threading.ThreadStart(LoadFromWebservice));
            LoadServiceData.Start();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(600);

            string RequestURL = "http://localhost";

            System.Net.HttpWebRequest __HttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(RequestURL);
            System.Net.HttpWebResponse __HttpWebResponse = (System.Net.HttpWebResponse)__HttpWebRequest.GetResponse();
            System.IO.Stream __rStream = __HttpWebResponse.GetResponseStream();
            __rStream.Close();
            __rStream.Dispose();
        }

        private void LoadFromWebservice()
        {
            System.Timers.Timer Wtimer = new System.Timers.Timer(3600 * 1000);
            Wtimer.Elapsed += new System.Timers.ElapsedEventHandler(Wtimer_Elapsed);
            Wtimer.Enabled = true;
            Wtimer.AutoReset = true;
        }

        private void Wtimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (e.SignalTime.Hour == 0)
            {
                HistoryDataCleaner cleaner = new HistoryDataCleaner();
                cleaner.CleanAutomatic();
            }
        }
    }
}