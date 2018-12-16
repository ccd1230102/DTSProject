using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        [WebMethod]
        public bool PostRunningData(RunningData data)
        {
            return true;
        }

        [WebMethod]
        public List<AlarmInfo> GetAlarmInfoConfigration()
        {
            List<AlarmInfo> ret = new List<AlarmInfo>();

            AlarmInfo info1 = new AlarmInfo
            {
                ID = 1,
                Name = "test",
                Level = 1,
                Treatment = "test"
            };

            AlarmInfo info2 = new AlarmInfo
            {
                ID = 2,
                Name = "test2",
                Level = 2,
                Treatment = "test2"
            };

            ret.Add(info1);
            ret.Add(info2);

            return ret;
        }
    }
}
