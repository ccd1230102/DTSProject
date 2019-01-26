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
            mDTSManager.Start("xxx");
        }

        private void Button_running_data_Click(object sender, EventArgs e)
        {
            mDTSManager.Start("666");
        }

        private void Button_upload_Click(object sender, EventArgs e)
        {
            mDTSManager.ShowRunningDataDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hashtable Pars = new Hashtable
            {
                { "TYPE", "Produce"},
                { "Count", 1 }
            };
            mDTSManager.UpdateRunningData(Pars);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Hashtable Pars = new Hashtable
            {
                { "TYPE", "Alarm"},
                { "AlarmID", 1 },
                { "AlarmName", 1 },
            };
            mDTSManager.ReportAlarmInfo(Pars);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mDTSManager.Stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mDTSManager.Stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mDTSManager.ShowAlarmInfoDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            mDTSManager.ShowConsumableInfoDialog();
        }
    }
}
