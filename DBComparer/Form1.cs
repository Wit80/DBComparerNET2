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

            
            Dictionary<string, Schema> dictDB = new Dictionary<string, Schema>();

            string connSring = "Server = WIN-B080H6IP1JD;integrated security=true; database = AdventureWorks2019";

            DBsysWorker wrk = new DBsysWorker(connSring);
            wrk.GetDataFromDB();
            if (0 == wrk.dsObjects.Tables[0].Rows.Count) 
            {//прочитано 0 объектов

            }
            /* ОБРАБОТКА КОЛИЧЕСТВА ЗАПИСЕЙ*/
            Dictionary<UInt64,int> dictRowsCount = new Dictionary<UInt64, int>();
            foreach (DataRow dr in wrk.dsRowsCount.Tables[0].Rows) 
            {
                dictRowsCount.Add(Convert.ToUInt64(dr[0]), Convert.ToInt32(dr[1]));
            }
            /*ОБРАБОТКА ИНДЕКСОВ*/
            Dictionary<UInt64, Dictionary<string,Index>> dictIndexes = new Dictionary<UInt64, Dictionary<string,Index>>();
            foreach (DataRow dr in wrk.dsIndexes.Tables[0].Rows)
            {
                if (dictIndexes.ContainsKey(Convert.ToUInt64(dr[0])))
                {//эта таблица уже обрабатывалась
                    if (dictIndexes[Convert.ToUInt64(dr[0])].ContainsKey(dr[3].ToString()))
                    {//этот индекс встречался, добавляес столбец
                        dictIndexes[Convert.ToUInt64(dr[0])][dr[3].ToString()].columns.Add(dr[4].ToString());
                    }
                    else 
                    {
                        dictIndexes[Convert.ToUInt64(dr[0])].Add(dr[3].ToString(), new Index(dr[3].ToString(), Convert.ToDateTime(dr[1]), Convert.ToDateTime(dr[2]), dr[4].ToString()));
                    }
                }
                else 
                {
                    dictIndexes.Add(Convert.ToUInt64(dr[0]), new Dictionary<string, Index>() { { dr[3].ToString(), new Index(dr[3].ToString(), Convert.ToDateTime(dr[1]), Convert.ToDateTime(dr[2]), dr[4].ToString()) } });
                }
            }
            /*ОБРАБОТКА СТОЛБЦОВ*/
            Dictionary<UInt64, Dictionary<string, Column>> dictColumns = new Dictionary<UInt64, Dictionary<string, Column>>();
            foreach (DataRow dr in wrk.dsColumns.Tables[0].Rows) 
            {
                if (dictColumns.ContainsKey(Convert.ToUInt64(dr[0])))
                {// эту таблицу уже встречали
                    if (!dictColumns[Convert.ToUInt64(dr[0])].ContainsKey(dr[1].ToString())) 
                    {
                        dictColumns[Convert.ToUInt64(dr[0])].Add(dr[1].ToString(),
                            new Column(dr[1].ToString(), Convert.ToInt32(dr[2]), Convert.ToInt32(dr[4]), Convert.ToInt32(dr[5]), Convert.ToInt32(dr[6])));
                    }
                }
                else 
                {
                    dictColumns.Add(Convert.ToUInt64(dr[0]),
                        new Dictionary<string, Column>() { { dr[1].ToString(),
                                new Column(dr[1].ToString(),Convert.ToInt32(dr[2]), Convert.ToInt32(dr[4]), Convert.ToInt32(dr[5]), Convert.ToInt32(dr[6])) } });
                }
            }





                DataView MyDataView = new DataView(wrk.dsRowsCount.Tables[0]);
            MyDataView.RowFilter = "[schemaName] = 'Person' AND [tableName]='Address'";
            int erert = MyDataView.Count;

            DataProcessor proc = new DataProcessor();
            DataBase db1 = new DataBase(wrk.dsObjects.Tables[0].Rows[0][(int)SQLObjectFieldsEnum.serverName].ToString(), wrk.dsObjects.Tables[0].Rows[0][(int)SQLObjectFieldsEnum.dbName].ToString());
            foreach (DataRow dr in wrk.dsObjects.Tables[0].Rows) 
            {
                proc.RowProcess(db1, dr);
                
            }
            







        }
    }
}
