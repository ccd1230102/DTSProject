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
        // 客户端上传数据接口
        public class RunningData
        {
            public int ID { get; set; }            // 设备ID
            public string Shift { get; set; }      // 产线班次
            public int Status { get; set; }        // 运行状态 0：停机，1：开机，2：运行
            public int Count { get; set; }         // 已生产数量
        }

        public class WarningData
        {
            public int DeviceID { get; set; }
            public int WarningID { get; set; }
            public DateTime OccurTime { get; set; }
        }

        public class WarningFixedData
        {
            public int DeviceID { get; set; }
            public int WarningDataID { get; set; }
            public DateTime FixTime { get; set; }
            public string Treatment { get; set; }
            public string Result { get; set; }
            public int FixDuration { get; set; }
        }

        public class ConsumableList
        {
            public int DeviceID { get; set; }
            public List<int> ConsumableIDs { get; set; }
            public List<int> Residuals { get; set; }    // 剩余次数或者时间
        }

        public class ConsumableReplaceData
        {
            public int DeviceID { get; set; }
            public int ConsumableID { get; set; }
            public DateTime ReplaceTime { get; set; }
            public string ReplacePeople { get; set; }
        }
        // End

        // 客户端下载数据接口
        public class AlarmInfo
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Level { get; set; }
            public bool Popup { get; set; }
            public string Treatment { get; set; }
        }

        public class ConsumableInfo
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Information { get; set; }
            public int Type { get; set; }           // 0：按次 1：按时间（小时）
            public int Limit { get; set; }
        }
        // End

        [WebMethod]
        public bool PostRunningData(RunningData data)
        {
            bool ret = true;
            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);
            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "IF NOT EXISTS(SELECT * FROM DeviceData WHERE ID=@para1) INSERT INTO DeviceData(ID, ZeroFaultTime) VALUES(@para1, 0)"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = data.ID;

                cmd.ExecuteNonQuery();

                switch (data.Status)
                {
                    case 0:
                        cmd = new SqlCommand
                        {
                            Connection = sqlConnection,
                            CommandText = "UPDATE DeviceShiftData SET Count = @para1, StopTime = @para2 WHERE ID = (SELECT TOP 1 ID FROM DeviceShiftData WHERE DeviceID=@para3 ORDER BY StartTime DESC)"
                        };
                        cmd.Parameters.Add("@para1", SqlDbType.Int).Value = data.Count;
                        cmd.Parameters.Add("@para2", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@para3", SqlDbType.Int).Value = data.ID;

                        cmd.ExecuteNonQuery();
                        break;
                    case 1:
                        cmd = new SqlCommand
                        {
                            Connection = sqlConnection,
                            CommandText = "INSERT INTO DeviceShiftData(DeviceID, Shift, Count, StartTime) VALUES(@para1, @para2, @para3, @para4)"
                        };
                        cmd.Parameters.Add("@para1", SqlDbType.Int).Value = data.ID;
                        cmd.Parameters.Add("@para2", SqlDbType.NVarChar).Value = data.Shift;
                        cmd.Parameters.Add("@para3", SqlDbType.Int).Value = data.Count;
                        cmd.Parameters.Add("@para4", SqlDbType.DateTime).Value = DateTime.Now;

                        cmd.ExecuteNonQuery();
                        break;
                    case 2:
                        cmd = new SqlCommand
                        {
                            Connection = sqlConnection,
                            CommandText = "UPDATE DeviceShiftData SET Count = @para1 WHERE ID = (SELECT TOP 1 ID FROM DeviceShiftData WHERE DeviceID=@para2 ORDER BY StartTime DESC)"
                        };
                        cmd.Parameters.Add("@para1", SqlDbType.Int).Value = data.Count;
                        cmd.Parameters.Add("@para2", SqlDbType.Int).Value = data.ID;
                        
                        cmd.ExecuteNonQuery();
                        break;
                }

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                ret = false;
            }

            return ret;
        }

        [WebMethod]
        public int PostWarningData(WarningData data)
        {
            int ret = -1;
            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);

            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "update DeviceData set ZeroFaultTime = 0 where ID = @para1"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = data.DeviceID;

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "INSERT INTO WarningData(DeviceID, WarningID, OccurTime)VALUES(@para1, @para2, @para3) SELECT @@IDENTITY"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = data.DeviceID;
                cmd.Parameters.Add("@para2", SqlDbType.Int).Value = data.WarningID;
                cmd.Parameters.Add("@para3", SqlDbType.DateTime).Value = data.OccurTime;

                ret = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                sqlConnection.Close();
            }
            catch(Exception ex)
            {
                sqlConnection.Close();
            }

            return ret;
        }

        [WebMethod]
        public bool PostWarningFixedData(WarningFixedData data)
        {
            bool ret = true;
            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);

            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "UPDATE WarningData SET FixTime=@para1, Treatment=@para2, Result=@para3, FixDuration=@para4 WHERE ID=@para5 AND DeviceID=@para6"
                };
                cmd.Parameters.Add("@para1", SqlDbType.DateTime).Value = data.FixTime;
                cmd.Parameters.Add("@para2", SqlDbType.NVarChar).Value = data.Treatment;
                cmd.Parameters.Add("@para3", SqlDbType.NVarChar).Value = data.Result;
                cmd.Parameters.Add("@para4", SqlDbType.Int).Value = data.FixDuration;
                cmd.Parameters.Add("@para5", SqlDbType.Int).Value = data.WarningDataID;
                cmd.Parameters.Add("@para6", SqlDbType.Int).Value = data.DeviceID;

                cmd.ExecuteNonQuery();

            sqlConnection.Close();
            }
            catch(Exception ex)
            {
                sqlConnection.Close();
                ret = false;
            }

            return ret;
        }

        [WebMethod]
        public bool PostConsumableList(ConsumableList list)
        {
            bool ret = true;
            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);

            try
            {
                if (list.ConsumableIDs.Count != list.Residuals.Count) return false;

                sqlConnection.Open();

                for (int n = 0; n < list.ConsumableIDs.Count; ++n)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = sqlConnection,
                        CommandText = "IF EXISTS(SELECT * FROM ConsumableData WHERE DeviceID = @para1 AND ConsumableID = @para2) " +
                                      "BEGIN " +
                                      "UPDATE ConsumableData SET Residual = @para3 WHERE DeviceID = @para1 AND ConsumableID = @para2 " +
                                      "END " +
                                      "ELSE " +
                                      "BEGIN " +
                                      "INSERT INTO ConsumableData(DeviceID, ConsumableID, Residual) VALUES(@para1, @para2, @para3) " +
                                      "END"
                    };
                    
                    cmd.Parameters.Add("@para1", SqlDbType.Int).Value = list.DeviceID;
                    cmd.Parameters.Add("@para2", SqlDbType.Int).Value = list.ConsumableIDs[n];
                    cmd.Parameters.Add("@para3", SqlDbType.Int).Value = list.Residuals[n];

                    cmd.ExecuteNonQuery();
                }

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                ret = false;
            }

            return ret;
        }

        [WebMethod]
        public bool PostConsumableReplaceData(ConsumableReplaceData data)
        {
            bool ret = true;
            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);

            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "INSERT INTO ConsumableReplaceData(DeviceID, ConsumableID, ReplacedTime, ReplacedPeople)VALUES(@para1, @para2, @para3, @para4)"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = data.DeviceID;
                cmd.Parameters.Add("@para2", SqlDbType.Int).Value = data.ConsumableID;
                cmd.Parameters.Add("@para3", SqlDbType.DateTime).Value = data.ReplaceTime;
                cmd.Parameters.Add("@para4", SqlDbType.NVarChar).Value = data.ReplacePeople;

                cmd.ExecuteNonQuery();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
            }

            return ret;
        }

        [WebMethod]
        public List<AlarmInfo> GetAlarmInfoConfigration()
        {
            List<AlarmInfo> ret = new List<AlarmInfo>();
            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);

            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT * FROM WarningConfig"
                };
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AlarmInfo info1 = new AlarmInfo
                    {
                        ID = (int)reader[0],
                        Name = (string)reader[1],
                        Level = (int)reader[2],
                        Popup = (bool)reader[3],
                        Treatment = (string)reader[4]
                    };

                    ret.Add(info1);
                }
                reader.Close();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
            }

            return ret;
        }

        [WebMethod]
        public List<ConsumableInfo> GetConsumableInfoConfigration()
        {
            List<ConsumableInfo> ret = new List<ConsumableInfo>();
            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);

            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT * FROM ConsumableConfig"
                };
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ConsumableInfo info1 = new ConsumableInfo
                    {
                        ID = (int)reader[0],
                        Name = (string)reader[1],
                        Information = (string)reader[2],
                        Type = (int)reader[3],
                        Limit = (int)reader[4]
                    };

                    ret.Add(info1);
                }
                reader.Close();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
            }

            return ret;
        }
    }
}
