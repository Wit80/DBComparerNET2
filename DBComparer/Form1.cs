using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DBComparerLibrary;
using DBComparerLibrary.DBSQLExecutor;
using DBComparerLibrary.DBSchema;
using System.Threading;
using System.Diagnostics;

namespace DBComparer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ConnectionForm connectionForm = new ConnectionForm();
            DialogResult dr = connectionForm.ShowDialog();
            /*

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            

            string connSring1 = "Server = WIN-B080H6IP1JD;integrated security=true; database = AdventureWorks2019";
            string connSring2 = "Server = WIN-B080H6IP1JD;integrated security=true; database = AW";

            DataProcessor proc = new DataProcessor(connSring1);
            DataBase db1 = proc.RunProcess();

            DataProcessor proc2= new DataProcessor(connSring2);
            DataBase db2 = proc2.RunProcess();


            bool res = db1.Equals(db2);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            
           MessageBox.Show(elapsedTime);*/

        }
    }
}
