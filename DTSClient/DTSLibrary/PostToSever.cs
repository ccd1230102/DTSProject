using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace DTSLibrary
{
    public class PostToSever
    {
        private string m_serverIP = "http://127.0.0.1";
        private string m_DeviceID = "0";

        // 通过本地配置文件，初始化服务器连接信息等
        public PostToSever(string localConfigFilePath)
        {
            XmlDocument xml = new XmlDocument();

            xml.Load(localConfigFilePath);

            XmlElement root = xml.DocumentElement;

            XmlNodeList serverNodes1 = root.GetElementsByTagName("Server");
            if(serverNodes1.Count > 0)
            {
                XmlElement serverEle1 = (XmlElement)serverNodes1[0];
                m_serverIP = serverEle1.InnerText;
            }

            XmlNodeList serverNodes2 = root.GetElementsByTagName("DeviceID");
            if (serverNodes2.Count > 0)
            {
                XmlElement serverEle2 = (XmlElement)serverNodes2[0];
                m_DeviceID = serverEle2.InnerText;
            }   
        }

        public bool PostRunningData(int type,string shift,int count)
        {
            bool ret = false;
            Hashtable Pars = null;
            if(type == 1)
            {
                Pars = new Hashtable
                {
                    // For start
                    { "ID", Convert.ToInt32(m_DeviceID) },
                    { "Shift", shift },
                    { "Status", 1 },
                    { "Count", count }
                };
            }

            if(type == 2)
            {
                Pars = new Hashtable
                {
                    /* For running*/
                    { "ID", Convert.ToInt32(m_DeviceID) },
                    { "Shift", shift },
                    { "Status", 2 },
                    { "Count", count }
                };
            }
            
            if(type == 3)
            {
                Pars = new Hashtable
                {
                    /* For stop*/
                    { "ID", Convert.ToInt32(m_DeviceID) },
                    { "Shift", shift },
                    { "Status", 0 },
                    { "Count", count }            
                };
            }

            try
            {
                XmlDocument xml = WebServiceHelper.QuerySoapWebService(m_serverIP + "/Process.asmx", "PostRunningData", "data", Pars);
                ret = xml.InnerText == "true";
            }
            catch (WebException ex)
            {

            } 

            return ret;
        }

        public void PostConsumableData(List<int> ids,List<int> times)
        {

            Hashtable Pars = new Hashtable
            {
                { "DeviceID", Convert.ToInt32(m_DeviceID)  },
                { "ConsumableIDs", ids },
                { "UsedTimes", times},
            };

            try
            {
                WebServiceHelper.QuerySoapWebService(m_serverIP + "/Process.asmx", "PostConsumableList", "list", Pars);
            }
            catch (WebException ex)
            {

            }
        }

        public bool PostConsumableReplaceData(string stReplacePeople, int ConsumableID, int UsedTimes,string stReplaceTime)
        {
            bool ret = false;
            Hashtable Pars = new Hashtable
            {
                { "DeviceID", Convert.ToInt32(m_DeviceID) },
                { "ConsumableID", ConsumableID},
                { "UsedTimes", UsedTimes},
                { "ReplaceTime", Convert.ToDateTime(stReplaceTime)},
                { "ReplacePeople", stReplacePeople}
            };
            try
            {
                XmlDocument xml = WebServiceHelper.QuerySoapWebService(m_serverIP + "/Process.asmx", "PostConsumableReplaceData", "data", Pars);
                ret = xml.InnerText == "true";
            }
            catch (WebException ex)
            {

            }
            return ret;
        }

        public int PostWarningData(int nWarningID)
        {
            Hashtable Pars = new Hashtable
            {
                { "DeviceID", Convert.ToInt32(m_DeviceID)},
                { "WarningID", nWarningID},
                { "OccurTime", DateTime.Now}
            };

            int WarningDataID = 0;
            try
            {
                XmlDocument xml = WebServiceHelper.QuerySoapWebService(m_serverIP + "/Process.asmx", "PostWarningData", "data", Pars);
                WarningDataID = Convert.ToInt32(xml.InnerText);
            }
            catch (WebException ex)
            {

            }
            return WarningDataID;
        }

        public void PostWarningFixedData(string WarningDataID,string Fixtime,string Result,int FixDuration)
        {
            Hashtable Pars = new Hashtable
            {
                { "DeviceID", Convert.ToInt32(m_DeviceID) },
                { "WarningDataID", Convert.ToInt32(WarningDataID) },
                { "FixTime", Convert.ToDateTime(Fixtime) },
                { "Treatment", "忽略" },
                { "Result", Result },
                { "FixDuration", FixDuration}
            };
            try
            {
                WebServiceHelper.QuerySoapWebService(m_serverIP + "/Process.asmx", "PostWarningFixedData", "data", Pars);
            }
            catch (WebException ex)
            {

            }
        }
        
    }
}
