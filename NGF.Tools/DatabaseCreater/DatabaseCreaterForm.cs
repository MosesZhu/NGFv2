using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NGF.Tools.DatabaseCreater
{
    public partial class DatabaseCreaterForm : Form
    {
        public DatabaseCreaterForm()
        {
            InitializeComponent();
        }

        private void DatabaseCreaterForm_Load(object sender, EventArgs e)
        {
            this.tbxConnectString.Text = ConfigurationManager.ConnectionStrings["MC"].ToString();
        }
    }
}
