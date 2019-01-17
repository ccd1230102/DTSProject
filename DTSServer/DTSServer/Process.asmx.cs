using System;
using System.Collections.Generic;
using System.Data;
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
            public int ID { get; set; }            // 设备ID
            public string Shift { get; set; }      // 产线班次
            public int Status { get; set; }        // 运行状态 0：停机，1：开机，2：运行
            public int Count { get; set; }         // 已生产数量
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
            bool ret = true;

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT COUNT(ID) FROM Device WHERE ID=@para1"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = data.ID;

                int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                if (count == 1)
                {
                    switch (data.Status)
                    {
                        case 0:
                            cmd = new SqlCommand
                            {
                                Connection = sqlConnection,
                                CommandText = "UPDATE Device SET Running=@para1, LastOperationTime=@para2 WHERE ID=@para3"
                            };
                            cmd.Parameters.Add("@para1", SqlDbType.Bit).Value = false;
                            cmd.Parameters.Add("@para2", SqlDbType.DateTime).Value = DateTime.Now;
                            cmd.Parameters.Add("@para3", SqlDbType.Int).Value = data.ID;
                            break;
                        case 1:
                            cmd = new SqlCommand
                            {
                                Connection = sqlConnection,
                                CommandText = "UPDATE Device SET Running=@para1, Shift=@para2, Count=0, LastOperationTime=@para3, LastWarningTime=@para3 WHERE ID=@para4"
                            };
                            cmd.Parameters.Add("@para1", SqlDbType.Bit).Value = true;
                            cmd.Parameters.Add("@para2", SqlDbType.NVarChar).Value = data.Shift;
                            cmd.Parameters.Add("@para3", SqlDbType.DateTime).Value = DateTime.Now;
                            cmd.Parameters.Add("@para4", SqlDbType.Int).Value = data.ID;
                            break;
                        case 2:
                            cmd = new SqlCommand
                            {
                                Connection = sqlConnection,
                                CommandText = "UPDATE Device SET Count=@para1 WHERE ID=@para2"
                            };
                            cmd.Parameters.Add("@para1", SqlDbType.Int).Value = data.Count;
                            cmd.Parameters.Add("@para2", SqlDbType.Int).Value = data.ID;
                            break;
                    }
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    if (data.Status != 1)
                    {
                        ret = false;
                    }
                    else
                    {
                        cmd = new SqlCommand
                        {
                            Connection = sqlConnection,
                            CommandText = "INSERT INTO Device(ID, Running, Shift, Count, LastOperationTime, LastWarningTime)VALUES(@para1, @para2, @para3, @para4, @para5, @para6)"
                        };
                        cmd.Parameters.Add("@para1", SqlDbType.Int).Value = data.ID;
                        cmd.Parameters.Add("@para2", SqlDbType.Bit).Value = true;
                        cmd.Parameters.Add("@para3", SqlDbType.NVarChar).Value = data.Shift;
                        cmd.Parameters.Add("@para4", SqlDbType.Int).Value = 0;

                        DateTime curDataTime = DateTime.Now;
                        cmd.Parameters.Add("@para5", SqlDbType.DateTime).Value = curDataTime;
                        cmd.Parameters.Add("@para6", SqlDbType.DateTime).Value = curDataTime;
                        cmd.ExecuteNonQuery();
                    }
                }

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;
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
