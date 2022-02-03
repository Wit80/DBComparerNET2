using DBComparerLibrary.DBSQLExecutor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DBComparer
{
    public partial class ConnectionForm : Form
    {

        private string ConnectionString1;
        private string ConnectionString2;
        public ConnectionForm()
        {
            InitializeComponent();
            cbAutent1.SelectedIndex = 0; 
            cbAutent2.SelectedIndex = 0;

            //ISQLGetConnection _connection = new SQLDBConnection("Server = WIN - B080H6IP1JD; integrated security = true;");

            List<string> ss = SQLExecutor.GetDbsFromServer("Server = WIN-B080H6IP1JD; integrated security = true;");
            
            

        }

        private void cbAutent1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (0 == cbAutent1.SelectedIndex)
            {
                lLoginName1.Visible = false;
                tbLoginName1.Visible = false;
                lPassword1.Visible = false;
                tbPassword1.Visible = false;
            }
            else 
            {
                lLoginName1.Visible = true;
                tbLoginName1.Visible = true;
                lPassword1.Visible = true;
                tbPassword1.Visible = true;
            }
        }

        private void cbAutent2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (0 == cbAutent1.SelectedIndex)
            {
                lLoginName2.Visible = false;
                tbLoginName2.Visible = false;
                lPassword2.Visible = false;
                tbPassword2.Visible = false;
            }
            else
            {
                lLoginName2.Visible = true;
                tbLoginName2.Visible = true;
                lPassword2.Visible = true;
                tbPassword2.Visible = true;
            }
        }
    }
}
