﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace SQLiteQueryBrowser
{
    /// <summary> 
    /// 说明：这是一个针对System.Data.SQLite的数据库常规操作封装的通用类。 
    /// </summary> 
    public class SQLiteDBHelper
    {
        private string connectionString = string.Empty;
        /// <summary> 
        /// 构造函数 
        /// </summary> 
        /// <param name="dbPath">SQLite数据库文件路径</param> 
        public SQLiteDBHelper(string dbPath)
        {
            this.connectionString = "Data Source=" + dbPath;
        }
        /// <summary> 
        /// 创建SQLite数据库文件 
        /// </summary> 
        /// <param name="dbPath">要创建的SQLite数据库文件路径</param> 
        public static void CreateDB(string dbPath)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "CREATE TABLE RunningLog" +
                                        "(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                        "datetime datetime," +                                    
                                        "Type VARCHAR(50)," +//类型
                                        "count integer DEFAULT 0," +//数量，当类型为Produce时使用，其他类型为0；
                                        "RelateID integer DEFAULT 0," +//相关数据库中的ID，不一定用
                                        "IsPost integer DEFAULT 0)";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE AlarmLog" +
                                        "(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                        "datetime datetime," +
                                        "AlarmID integer," +
                                        "AlarmName VARCHAR(50)," +
                                        "AlarmLevel integer," +
                                        "HandleAlarmMode VARCHAR(250)," +
                                        "ServerWarningID integer," +
                                        "HandleTime datetime," +
                                        "HandlePeopleName VARCHAR(50)," +
                                        "HandleResult VARCHAR(250)," +
                                        "Remarks VARCHAR(50)," +
                                        "IsPost integer DEFAULT 0)";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE ConsumableLog" +
                                        "(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                        "datetime datetime," +
                                        "ConsumableID integer," +
                                        "ConsumableName VARCHAR(50)," +
                                        "ConsumableInfo VARCHAR(250)," +
                                        "Lifetime integer," +
                                        "WorkingTime integer," +
                                        "Changetime datetime," +
                                        "ChangePeopleName VARCHAR(50)," +                                        
                                        "Remarks VARCHAR(50)," +
                                        "IsPost integer DEFAULT 0)";
                    command.ExecuteNonQuery();
                }
            }
        }
        /// <summary> 
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。 
        /// </summary> 
        /// <param name="sql">要执行的增删改的SQL语句</param> 
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = sql;
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        affectedRows = command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
            return affectedRows;
        }
        /// <summary> 
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public SQLiteDataReader ExecuteReader(string sql, SQLiteParameter[] parameters)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }
        /// <summary> 
        /// 执行一个查询语句，返回查询结果的第一行第一列 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public Object ExecuteScalar(string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }
        /// <summary> 
        /// 查询数据库中的所有数据类型信息 
        /// </summary> 
        /// <returns></returns> 
        public DataTable GetSchema()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                DataTable data = connection.GetSchema("TABLES");
                connection.Close();
                //foreach (DataColumn column in data.Columns) 
                //{ 
                //  Console.WriteLine(column.ColumnName); 
                //} 
                return data;
            }
        }
    }
}