using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DTSLibrary;

namespace DTSClient
{
    public partial class Form1 : Form
    {
        private DTSManager mDTSManager;

        public Form1()
        {
            InitializeComponent();

            mDTSManager = new DTSManager("config.xml");
            mDTSManager.Start();
        }

        private void Button_running_data_Click(object sender, EventArgs e)
        {
            mDTSManager.ShowRunningDataDialog();
        }

        private void Button_upload_Click(object sender, EventArgs e)
        {
            Hashtable Pars = new Hashtable
            {
                // For start
                { "ID", 1 },
                { "Shift", "测试" },
                { "Status", 1 },
                { "Count", 0 }
                //

                /* For running
                { "ID", 1 },
                { "Shift", "测试" },
                { "Status", 2 },
                { "Count", 100 }
                */

                /* For stop
                { "ID", 1 },
                { "Shift", "测试" },
                { "Status", 0 },
                { "Count", 0 }
                */
            };
            mDTSManager.UpdateRunningData(Pars);
        }
    }
}
