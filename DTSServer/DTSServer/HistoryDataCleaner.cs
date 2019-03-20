using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DTSServer
{
    public class HistoryDataCleaner
    {
        public void CleanAutomatic()
        {
            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);
            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT * FROM AutomaticConfig"
                };

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    bool enableClean = (bool)reader[1];
                    int days = (int)reader[0];

                    if (enableClean)
                    {
                        CleanData(days);
                    }
                }

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
            }
        }

        public bool CleanData(int days)
        {
            DateTime fromDateTime = DateTime.Now.AddDays(-days);

            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);
            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "DELETE FROM DeviceShiftData WHERE StartTime < @para1"
                };
                cmd.Parameters.Add("@para1", SqlDbType.DateTime).Value = fromDateTime;

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "DELETE FROM WarningData WHERE OccurTime < @para1"
                };
                cmd.Parameters.Add("@para1", SqlDbType.DateTime).Value = fromDateTime;

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "DELETE FROM ConsumableReplaceData WHERE ReplacedTime < @para1"
                };
                cmd.Parameters.Add("@para1", SqlDbType.DateTime).Value = fromDateTime;

                cmd.ExecuteNonQuery();

                sqlConnection.Close();

                return true;
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
            }

            return false;
        }
    }
}