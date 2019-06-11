using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace DSPClientDeamon
{
    public partial class About : Form
    {
        public About()
        {
            
            InitializeComponent();
        }
        private void About_Load(object sender, EventArgs e)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            label4.Text = version;
            label3.Text = Application.CompanyName.ToString();
     
        }
    }
}
