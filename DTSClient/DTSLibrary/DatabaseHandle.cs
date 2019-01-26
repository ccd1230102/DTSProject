using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using SQLiteQueryBrowser;
using System.Xml;

namespace DTSLibrary
{
    class DatabaseHandle
    {
        public static void CreateTable()
        {
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase; 
            string dbPath = EXEPath+"Demo.db3";
            //如果不存在改数据库文件，则创建该数据库文件
            if (!System.IO.File.Exists(dbPath))
            {
                SQLiteDBHelper.CreateDB(dbPath);               
            }
            InitConsumableRecord();
            //SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            //string sql = "CREATE TABLE Test3(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,Name char(3),TypeName varchar(50),addDate datetime,UpdateTime Date,Time time,Comments blob)";
        }
        public static void InitConsumableRecord()
        {
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            XmlDocument XMLalarmInfo = new XmlDocument();
            XMLalarmInfo.Load(EXEPath + "ConsumableInfoConfig.xml");
            XmlElement el = XMLalarmInfo.DocumentElement;
            XmlNodeList ConsumableInfoNodes = el.GetElementsByTagName("ConsumableInfo");

            foreach (XmlNode node in ConsumableInfoNodes)
            {
                XmlNodeList ConsumableNode = node.ChildNodes;
                string ConsumableID = ((XmlElement)ConsumableNode[0]).InnerText;
                string ConsumableName = ((XmlElement)ConsumableNode[1]).InnerText;
                string stLifetime = ((XmlElement)ConsumableNode[3]).InnerText;
                bool bIsInit = false;

                string sql1 = "select * from ConsumableLog where ConsumableID = " + ConsumableID;
                using (SQLiteDataReader reader = db.ExecuteReader(sql1, null))
                {
                    while (reader.Read())
                    {
                        bIsInit = true;
                    }
                }

                if(!bIsInit)
                {
                    string sql2 = "INSERT INTO ConsumableLog(datetime,ConsumableID,ConsumableName,Lifetime,WorkingTime,Changetime,ChangePeopleName)" +
                                     "values(@datetime,@ConsumableID,@ConsumableName,@Lifetime,@WorkingTime,@Changetime,@ChangePeopleName)";
                    SQLiteParameter[] parameters = new SQLiteParameter[]{
                            new SQLiteParameter("@datetime",DateTime.Now),
                            new SQLiteParameter("@ConsumableID",ConsumableID),
                            new SQLiteParameter("@ConsumableName",ConsumableName),
                            new SQLiteParameter("@Lifetime",stLifetime),
                            new SQLiteParameter("@WorkingTime","0"),
                            new SQLiteParameter("@Changetime",DateTime.Now),
                            new SQLiteParameter("@ChangePeopleName","Init")
                    };
                    db.ExecuteNonQuery(sql2, parameters);
                }

            }
        }
        public static void RecordStartToDB(bool bPost)
        {
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            //string sql1 = "SELECT LastBreakdownTime FROM RunningLog where id=(select MAX(id) from RunningLog)";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            string sql2 = "INSERT INTO RunningLog(datetime,Type,IsPost)" +
                                         "values(@datetime,@Type,@IsPost)";

            SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@datetime",DateTime.Now),
                    new SQLiteParameter("@Type","start"),
                    new SQLiteParameter("@IsPost",bPost)           
            };

            db.ExecuteNonQuery(sql2, parameters);
        }
        public static void RecordProduceToDB(int ProduceCount)
        {
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            string sql2 = "INSERT INTO RunningLog(datetime,Type,count)" +
                                         "values(@datetime,@Type,@count)";

            SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@datetime",DateTime.Now),
                    new SQLiteParameter("@Type","Produce"),
                    new SQLiteParameter("@count",ProduceCount)};

            db.ExecuteNonQuery(sql2, parameters);
        }
        public static void RecordStopToDB(bool bPost)
        {
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            string sql2 = "INSERT INTO RunningLog(datetime,Type,IsPost)" +
                                         "values(@datetime,@Type,@IsPost)";

            SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@datetime",DateTime.Now),
                    new SQLiteParameter("@Type","stop"),
                    new SQLiteParameter("@IsPost",bPost)   
            };
            db.ExecuteNonQuery(sql2, parameters);
        }
        public static void ShowData()
        {
            //查询从50条起的20条记录
            string sql = "select * from test3 order by id desc limit 50 offset 20";
            SQLiteDBHelper db = new SQLiteDBHelper("D:\\Demo.db3");
            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    Console.WriteLine("ID:{0},TypeName{1}", reader.GetInt64(0), reader.GetString(1));
                }
            }
        }

        public static string RecordAlarmToDB(AlarmInfo AlaInfo)
        {
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            string sql2 = "INSERT INTO AlarmLog(datetime,AlarmID,AlarmName,AlarmLevel,HandleAlarmMode,ServerWarningID,IsPost)" +
                                         "values(@datetime,@AlarmID,@AlarmName,@AlarmLevel,@HandleAlarmMode,@ServerWarningID,@IsPost)";

            SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@datetime",DateTime.Now),
                    new SQLiteParameter("@AlarmID",AlaInfo.AlarmID),
                    new SQLiteParameter("@AlarmName",AlaInfo.AlarmName),
                    new SQLiteParameter("@AlarmLevel",AlaInfo.AlarmLevel),
                    new SQLiteParameter("@HandleAlarmMode",AlaInfo.HandleAlarmMode),
                    new SQLiteParameter("@ServerWarningID",AlaInfo.ServerWarningID),
                    new SQLiteParameter("@IsPost",(AlaInfo.ServerWarningID==0)?0:1)
            };
            db.ExecuteNonQuery(sql2, parameters);

            string sql3 = "select max(id) from AlarmLog";

            string stID = "";
            using (SQLiteDataReader reader = db.ExecuteReader(sql3, null))
            {
                while (reader.Read())
                {
                    stID = reader["max(id)"].ToString();
                }
            }
            return stID;
        }
        
    }
}


