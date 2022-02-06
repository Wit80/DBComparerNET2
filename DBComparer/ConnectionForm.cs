using DBComparerLibrary.DBSQLExecutor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DBComparer
{
    public partial class ConnectionForm : Form
    {
        public ConnectionForm()
        {
            InitializeComponent();
            tbServer1.Text = Environment.MachineName;
            tbServer2.Text = Environment.MachineName;
            cbAutent1.SelectedIndex = 0; 
            cbAutent2.SelectedIndex = 0;

        }
        private const string DBListNoLoaded = "Список баз данных не загружен";
        public string ConnectionString1 { get { return connString1; }  }        
        public string ConnectionString2 { get { return connString2; } }
        private string connString1 { get; set; }
        private string connString2 { get; set; }

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


        private void cbDatabase1_DropDown(object sender, EventArgs e)
        {
            cbDatabase1.Items.Clear();
            cbDatabase1.Enabled = false;
            if (1 == cbAutent1.SelectedIndex) 
            {// вход через user/password
                if (0 == tbLoginName1.Text.Length)
                {
                    MessageBox.Show("Укажите имя пользователя");
                    tbLoginName1.Focus();
                    return;
                }
                if (0 == tbPassword1.Text.Length)
                {
                    MessageBox.Show("Укажите пароль пользователя");
                    tbPassword1.Focus();
                    return;
                }
            }

            Dictionary<string, string> arguments = new Dictionary<string, string>();
            arguments.Add("IS",Convert.ToBoolean(cbAutent1.SelectedIndex).ToString());
            arguments.Add("Serv",tbServer1.Text);
            arguments.Add("Login",tbLoginName1.Text);
            arguments.Add("Pass",tbPassword1.Text);
            pictureBox1.Visible = true;
            this.backgroundWorker1.RunWorkerAsync(arguments);

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = executeGetDBs(e);
        }
        
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cbDatabase1.Enabled = true;
            pictureBox1.Visible = false;
            if (e.Error != null)
            {
                string msg = String.Format("Ошибка при получении списка баз данных сервера: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }
            
            List<string> DBs = (List<string>)e.Result;
            if (DBs is null)
            {
                cbDatabase1.Items.Clear();
                cbDatabase1.Items.Add(DBListNoLoaded);
            }
            else
            {
                foreach (string s in DBs)
                {
                    cbDatabase1.Items.Add(s);
                }
            }
        }

        private void cbDatabase2_DropDown(object sender, EventArgs e)
        {
            cbDatabase2.Items.Clear();
            cbDatabase2.Enabled = false;
            if (1 == cbAutent2.SelectedIndex)
            {// вход через user/password
                if (0 == tbLoginName2.Text.Length)
                {
                    MessageBox.Show("Укажите имя пользователя");
                    tbLoginName2.Focus();
                    return;
                }
                if (0 == tbPassword2.Text.Length)
                {
                    MessageBox.Show("Укажите пароль пользователя");
                    tbPassword2.Focus();
                    return;
                }
            }

            Dictionary<string, string> arguments = new Dictionary<string, string>();
            arguments.Add("IS", Convert.ToBoolean(cbAutent2.SelectedIndex).ToString());
            arguments.Add("Serv", tbServer2.Text);
            arguments.Add("Login", tbLoginName2.Text);
            arguments.Add("Pass", tbPassword2.Text);
            pictureBox2.Visible = true;
            this.backgroundWorker2.RunWorkerAsync(arguments);
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = executeGetDBs(e);
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cbDatabase2.Enabled = true;
            pictureBox2.Visible = false;
            if (e.Error != null)
            {
                string msg = String.Format("Ошибка при получении списка баз данных сервера: {0}", e.Error.Message);
                MessageBox.Show(msg);
                return;
            }
            List<string> DBs = (List<string>)e.Result;
            if (DBs is null)
            {
                cbDatabase2.Items.Clear();
                cbDatabase2.Items.Add(DBListNoLoaded);
            }
            else
            {
                foreach (string s in DBs)
                {
                    cbDatabase2.Items.Add(s);
                }
            }
        }
        private List<string> executeGetDBs(DoWorkEventArgs e)
        {
            Dictionary<string, string> arguments = e.Argument as Dictionary<string, string>;
            ConncetionString cs = new ConncetionString(Convert.ToBoolean(arguments["IS"]), server: arguments["Serv"], userName: arguments["Login"], password: arguments["Pass"]);
            return SQLExecutor.GetDbsFromServer(cs.GetConnectionStringForDBList());
        }

        

        private void tbServer1_TextChanged(object sender, EventArgs e)
        {
            cbDatabase1.Items.Clear ();
        }

        private void tbServer2_TextChanged(object sender, EventArgs e)
        {
            cbDatabase2.Items.Clear();
        }

        private void tbLoginName1_TextChanged(object sender, EventArgs e)
        {
            cbDatabase1.Items.Clear();
        }

        private void tbPassword1_TextChanged(object sender, EventArgs e)
        {
            cbDatabase1.Items.Clear();
        }

        private void tbLoginName2_TextChanged(object sender, EventArgs e)
        {
            cbDatabase2.Items.Clear();
        }

        private void tbPassword2_TextChanged(object sender, EventArgs e)
        {
            cbDatabase2.Items.Clear();
        }

        private void butCompare_Click(object sender, EventArgs e)
        {
            if (0 == cbDatabase1.Text.Length || DBListNoLoaded == cbDatabase1.Text) 
            {
                MessageBox.Show("Укажите корректно данные источника 1 и выберете БД");
                return;
            }

            if (0 == cbDatabase2.Text.Length || DBListNoLoaded == cbDatabase2.Text) 
            {
                MessageBox.Show("Укажите корректно данные источника 2 и выберете БД");
                return;
            }

            connString1 = (new ConncetionString(Convert.ToBoolean(cbAutent1.SelectedIndex), server: tbServer1.Text, userName: tbLoginName1.Text, 
                password: tbPassword1.Text, initialCatalog:cbDatabase1.Text)).GetConnectionString();
            connString2 = (new ConncetionString(Convert.ToBoolean(cbAutent2.SelectedIndex), server: tbServer2.Text, userName: tbLoginName2.Text, 
                password: tbPassword2.Text, initialCatalog: cbDatabase2.Text)).GetConnectionString();
            DialogResult = DialogResult.OK;
        }
    }
}
