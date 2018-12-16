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
                { "ID", 123 },
                { "Status", "Running" }
            };
            mDTSManager.UpdateRunningData(Pars);
        }
    }
}
