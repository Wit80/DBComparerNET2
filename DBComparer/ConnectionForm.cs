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
        }
        private const string DBListNoLoaded = "Список баз данных не загружен";
        public string ConnectionString1 { get { return connString1; }  }        
        public string ConnectionString2 { get { return connString2; } }
        private string connString1 { get; set; }
        private string connString2 { get; set; }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = executeGetDBs(e);
        }
        
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }


        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = executeGetDBs(e);
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }
        private List<string> executeGetDBs(DoWorkEventArgs e)
        {
            Dictionary<string, string> arguments = e.Argument as Dictionary<string, string>;
            return SQLExecutor.GetDbsFromServer(ConncetionString.GetConnectionStringForDBList(Convert.ToBoolean(arguments["IS"]), server: arguments["Serv"], userName: arguments["Login"], password: arguments["Pass"]));
        }


        private void butCompare_Click(object sender, EventArgs e)
        {
            Progress progress = new Progress();
            progress.StartPosition = FormStartPosition.CenterScreen;

            progress.Show();
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
                //проверим корректность строк подключения
                connString1 = ConncetionString.GetConnectionString(dbConnectionPanel1.Connection.IsUserPasswordAutentification, dbConnectionPanel1.Connection.ServerName,
                    dbConnectionPanel1.Connection.Login, dbConnectionPanel1.Connection.Password, dbConnectionPanel1.Connection.DatabaseName);
                connString2 = ConncetionString.GetConnectionString(dbConnectionPanel2.Connection.IsUserPasswordAutentification, dbConnectionPanel2.Connection.ServerName,
                    dbConnectionPanel2.Connection.Login, dbConnectionPanel2.Connection.Password, dbConnectionPanel2.Connection.DatabaseName); ;
                if (!TestConnection(connString1))
                {
                    MessageBox.Show($"Нет связи с левой БД! Строка подключения {connString1}", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!TestConnection(connString2))
                {
                    MessageBox.Show($"Нет связи с правой БД! Строка подключения {connString2}", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            {
                progress.Close();
            }

            DialogResult = DialogResult.OK;
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

    }
}
