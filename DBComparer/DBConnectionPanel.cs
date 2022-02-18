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
    public partial class DBConnectionPanel : UserControl
    {
        public event EventHandler ComboBoxDatabase_DropDown;
        public DBConnectionPanel()
        {
            InitializeComponent();
            cbDatabase.DropDown += this.HandleComboBoxDatabase_DropDown;
        }
        private void HandleComboBoxDatabase_DropDown(object sender, EventArgs e)
        {
            this.OnComboBoxDatabase_DropDown(EventArgs.Empty);
        }
        protected virtual void OnComboBoxDatabase_DropDown(EventArgs e)
        {
            EventHandler handler = this.ComboBoxDatabase_DropDown;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public ConnectionData Connection
        {
            get
            {
                return new ConnectionData() { DatabaseName = cbDatabase.Text.Trim(), ServerName = tbServer.Text.Trim(),
                    IsUserPasswordAutentification = Convert.ToBoolean(cbAutent.SelectedIndex), Login = tbLoginName.Text.Trim(),
                    Password = tbPassword.Text.Trim()
                };
            }
        }
        public void ShowProgress()
        {
            pictureBox.Visible = true;
        }
        public void HideProgress()
        {
            pictureBox.Visible = false;
        }
        private void cbAutent_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDatabase.Items.Clear();
            if (0 == cbAutent.SelectedIndex)
            {
                lblLogin.Visible = false;
                tbLoginName.Visible = false;
                lblPassword.Visible = false;
                tbPassword.Visible = false;
            }
            else
            {
                lblLogin.Visible = true;
                tbLoginName.Visible = true;
                lblPassword.Visible = true;
                tbPassword.Visible = true;
            }
        }
        
        private void DBConnectionPanel_Load(object sender, EventArgs e)
        {
            tbServer.Text = Environment.MachineName;
            cbAutent.SelectedIndex = 0;
            cbDatabase.Items.Clear();
            pictureBox.Visible = false;
        }

        private void cbDatabase_DropDown(object sender, EventArgs e)
        {
            if (0 == tbServer.Text.Length)
            {
                MessageBox.Show("Укажите сервер БД", "Недостаточно данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbDatabase.Items.Clear();
                tbServer.Focus();
                return;
            }
            if (1 == cbAutent.SelectedIndex)
            {// вход через user/password
                if (0 == tbLoginName.Text.Length)
                {
                    MessageBox.Show("Укажите имя пользователя","Недостаточно данных",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    cbDatabase.Items.Clear();
                    tbLoginName.Focus();
                    return;
                }
                if (0 == tbPassword.Text.Length)
                {
                    MessageBox.Show("Укажите пароль пользователя","Недостаточно данных",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    cbDatabase.Items.Clear();
                    tbPassword.Focus();
                    return;
                }
            }

            Dictionary<string, string> arguments = new Dictionary<string, string>();
            arguments.Add("IS", Convert.ToBoolean(cbAutent.SelectedIndex).ToString());
            arguments.Add("Serv", tbServer.Text);
            arguments.Add("Login", tbLoginName.Text);
            arguments.Add("Pass", tbPassword.Text);
            pictureBox.Visible = true;
            this.backgroundWorker1.RunWorkerAsync(arguments);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = executeGetDBs(e);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox.Visible = false;
            cbDatabase.Items.Clear();
            if (e.Error != null)
            {
                string msg = String.Format("Ошибка при получении списка баз данных сервера: {0}", e.Error.Message);
                MessageBox.Show(msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<string> DBs = (List<string>)e.Result;
            if (DBs is null)
            {
            }
            else
            {
                foreach (string s in DBs)
                {
                    cbDatabase.Items.Add(s);
                }
            }
        }
        private List<string> executeGetDBs(DoWorkEventArgs e)
        {
            Dictionary<string, string> arguments = e.Argument as Dictionary<string, string>;
            return GetDbsFromServer(ConncetionString.GetConnectionStringForDBList(Convert.ToBoolean(arguments["IS"]), server: arguments["Serv"], userName: arguments["Login"], password: arguments["Pass"]));
        }
        private static List<string> GetDbsFromServer(string connString)
        {

            SqlConnection _sqlConnection = new SqlConnection(connString);
            SqlCommand command;
            SqlDataReader reader;

            _sqlConnection.Open();

            command = new SqlCommand("select name from sys.databases", _sqlConnection);
            reader = command.ExecuteReader();

            List<string> schReturn = new List<string>();
            while (reader.Read())
            {
                schReturn.Add(reader[0].ToString());
            }
            _sqlConnection.Close();
            _sqlConnection.Dispose();
            return schReturn;
        }
    }
}
