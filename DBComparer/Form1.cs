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

namespace DBComparer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            

            string connSring = "Server = WIN-B080H6IP1JD;integrated security=true; database = AdventureWorks2019";

            DataProcessor proc = new DataProcessor(connSring);
            DataBase db1 = proc.RunProcess();

            DataProcessor proc2= new DataProcessor(connSring);
            DataBase db2 = proc2.RunProcess();

            string hc1 = db1.GetHashCode().ToString();
            string hc2 = db2.GetHashCode().ToString();

            bool res = db1.Equals(db2);

           









        }
    }
}
