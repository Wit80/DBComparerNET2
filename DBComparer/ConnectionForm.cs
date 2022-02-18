using DBComparerLibrary.DBSQLExecutor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBComparer
{
    public partial class ConnectionForm : Form
    {
        public ConnectionForm()
        {
            InitializeComponent();
            splitContainer1.SplitterDistance = splitContainer1.Size.Width / 2;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
        }
        private bool db1 = false;
        private bool db2 = false;
        private const string DBListNoLoaded = "Список баз данных не загружен";
        public string ConnectionString1 { get { return connString1; }  }        
        public string ConnectionString2 { get { return connString2; } }
        private string connString1 { get; set; }
        private string connString2 { get; set; }

      
        private void butCompare_Click(object sender, EventArgs e)
        {
            db1 = false;
            db2 = false;
            try
            {
                if (!CheckConnectionData(dbConnectionPanel1.Connection))
                {

                    return;
                }
                if (!CheckConnectionData(dbConnectionPanel2.Connection))
                {
                    return;
                }
                connString1 = ConncetionString.GetConnectionString(dbConnectionPanel1.Connection.IsUserPasswordAutentification, dbConnectionPanel1.Connection.ServerName,
                    dbConnectionPanel1.Connection.Login, dbConnectionPanel1.Connection.Password, dbConnectionPanel1.Connection.DatabaseName);
                connString2 = ConncetionString.GetConnectionString(dbConnectionPanel2.Connection.IsUserPasswordAutentification, dbConnectionPanel2.Connection.ServerName,
                    dbConnectionPanel2.Connection.Login, dbConnectionPanel2.Connection.Password, dbConnectionPanel2.Connection.DatabaseName); 
                this.backgroundWorker3.RunWorkerAsync(connString1);
                this.backgroundWorker4.RunWorkerAsync(connString2);
                pictureBox1.Visible = true;
                pictureBox2.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private bool TestConnection(string connString) 
        {
            
            SqlConnection sqlConn = new SqlConnection(connString);
            SqlCommand command = sqlConn.CreateCommand();
            SqlDataReader Reader;
            command.CommandText = "SELECT 1";
            try
            {
                sqlConn.Open();
                Reader = command.ExecuteReader();
            }
            catch
            {
                
                return false;
            }
            finally 
            { 
                sqlConn.Close();
                sqlConn.Dispose();
            }
            return true;
        }
        private bool CheckConnectionData(ConnectionData cd) 
        {
            if (0 == cd.ServerName.Length)
            {
                MessageBox.Show($"Не указан сервер в настройках подключения!", "Укажите сервер", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cd.IsUserPasswordAutentification)
            {
                if(0 == cd.Login.Length)
                {
                    MessageBox.Show($"Не указано имя пользователя!", "Укажите имя пользователя", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (0 == cd.Password.Length)
                {
                    MessageBox.Show($"Не указан пароль пользователя!", "Укажите пароль", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            if (0 == cd.DatabaseName.Length)
            {
                MessageBox.Show($"Не указанв БД в настройках подключения!", "Укажите БД", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = TestConnection(e.Argument as string);

        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox1.Visible = false;
            if (e.Error != null)
            {
                MessageBox.Show($"Нет связи с левой БД! Строка подключения {connString1}", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(bool)e.Result)
            {// информации нет
                MessageBox.Show($"Нет связи с левой БД! Строка подключения {connString1}", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            db1 = true;
            if (db2)
                DialogResult = DialogResult.OK;
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = TestConnection(e.Argument as string);
        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox2.Visible = false;
            if (e.Error != null)
            {
                MessageBox.Show($"Нет связи с правой БД! Строка подключения {connString2}", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(bool)e.Result)
            {// информации нет
                MessageBox.Show($"Нет связи с правой БД! Строка подключения {connString2}", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            db2 = true;
            if(db1)
                DialogResult = DialogResult.OK;
        }
    }
}
