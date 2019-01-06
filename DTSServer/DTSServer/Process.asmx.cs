using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;

namespace DTSServer
{
    /// <summary>
    /// PostData 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://127.0.0.1/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Process : System.Web.Services.WebService
    {
        public class RunningData
        {
            public int ID { get; set; }
            public string Status { get; set; }
        }

        public class AlarmInfo
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Level { get; set; }
            public string Treatment { get; set; }
        }

        public class ConsumableInfo
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Limit { get; set; }
        }

        [WebMethod]
        public bool PostRunningData(RunningData data)
        {
            string strIp = Context.Request.UserHostAddress.ToString();
            return true;
        }

        [WebMethod]
        public List<AlarmInfo> GetAlarmInfoConfigration()
        {
            List<AlarmInfo> ret = new List<AlarmInfo>();

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT * FROM Warning"
                };
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AlarmInfo info1 = new AlarmInfo
                    {
                        ID = (int)reader[0],
                        Name = (string)reader[1],
                        Level = (int)reader[2],
                        Treatment = (string)reader[3]
                    };

                    ret.Add(info1);
                }
                reader.Close();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {

            }

            return ret;
        }

        [WebMethod]
        public List<ConsumableInfo> GetConsumableInfoConfigration()
        {
            List<ConsumableInfo> ret = new List<ConsumableInfo>();

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT * FROM Consumable"
                };
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ConsumableInfo info1 = new ConsumableInfo
                    {
                        ID = (int)reader[0],
                        Name = (string)reader[1],
                        Limit = (int)reader[2]
                    };

                    ret.Add(info1);
                }
                reader.Close();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {

            }

            return ret;
        }
    }
}
