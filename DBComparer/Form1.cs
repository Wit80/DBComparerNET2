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
using System.Text.RegularExpressions;

namespace DBComparer
{
    public partial class Form1 : Form
    {
        private DataBase db1 = null;
        private DataBase db2 = null;
        private static Color colorDB2 = Color.Green;  // структуры есть только в DB2
        private static Color colorDB1 = Color.Blue;    // структура есть только в DB1 
        private static Color colorDiferentIntern = Color.Gray; // структура есть в обоих БД, но их внутренняя структура отличается
        private static Color colorEqual = Color.Black; // одинаковые
        private string regExpr = @"CREATE|TABLE|CONSTRAINT|DEFAULT|PRIMARY KEY|CLUSTERED|NONCLUSTERED|INDEX| ON |GO|ALTER|SCHEMA|VIEW|SELECT|FROM|UNIQUE|AUTHORIZATION| AS ";
        private string regExprGray = @"NOT NULL| NULL";
        public Form1()
        {
            InitializeComponent();
            splitContainer2.SplitterDistance = splitContainer2.Size.Width / 2;
            tsNodeInfo.Text = "";
            tsLabelEquals.Visible = false;  
           
        }
        private string stringConnection1 = "";
        private string stringConnection2 = "";


        private void newFileMenu_Click(object sender, EventArgs e)
        {
            ConnectionForm connectionForm = new ConnectionForm();
            connectionForm.Parent = this.Parent;
            connectionForm.StartPosition = FormStartPosition.CenterParent;
            DialogResult dr = connectionForm.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            tsNodeInfo.Text = "";
            tsLabelEquals.Text = "";
            stringConnection1 = connectionForm.ConnectionString1;
            stringConnection2 = connectionForm.ConnectionString2;
            tsDBInfo.Visible = true;
            tsProcessImage1.Visible = true;
            tsProcessImage2.Visible = true;

            this.backgroundWorker1.RunWorkerAsync(stringConnection1);
            this.backgroundWorker2.RunWorkerAsync(stringConnection2);
        }

        private void openFileMenu_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = GetDataBaseInfo(e);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tsProcessImage1.Visible = false;
            if (e.Error != null)
            {
                MessageBox.Show($"Ошибка при получении информации о БД1: {e.Error.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            db1 = (DataBase)e.Result;
            if (db1 is null)
            {// информации нет
                MessageBox.Show("Ошибка получения информации о структуре БД1");
                return;
            }
            if (db1 != null && db2 != null)
            {
                ShowDbsInfo();
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = GetDataBaseInfo(e);
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tsProcessImage2.Visible = false;
            if (e.Error != null)
            {
                MessageBox.Show($"Ошибка при получении информации о БД2: {e.Error.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            db2 = (DataBase)e.Result;
            if (db2 is null)
            {// информации нет
                MessageBox.Show("Ошибка получения информации о структуре БД2");
                return;
            }
            if (db1 != null && db2 != null)
            {
                ShowDbsInfo();

            }
        }
        private DataBase GetDataBaseInfo(DoWorkEventArgs e)
        {
            DataProcessor proc = new DataProcessor(e.Argument as string);
            return proc.RunProcess();
        }
        private void ShowDbsInfo() 
        {
            tsLabelEquals.Visible = true;
            tsDB1Info.Text = $"Server:{db1.dbServer}\nDataBase: {db1.dbName}";
            tsDB2Info.Text = $"Server:{db2.dbServer}\nDataBase: {db2.dbName}";
            var NodaRoot = treeView1.Nodes;
            NodaRoot.Clear();
            AddNoda(NodaRoot, $"Schemas({ db1.schemas.Count}/{db2.schemas.Count})", 1, colorEqual);
            AddNoda(NodaRoot, $"Views({ db1.views.Count}/{db2.views.Count})", 3, colorEqual);
            AddNoda(NodaRoot, $"Tables({ db1.tables.Count}/{db2.tables.Count})", 2, colorEqual);

            if (db1.Equals(db2))
            {
                tsLabelEquals.Text = "Структура баз одинакова               ";
                foreach (var schema in db1.schemas.Values) 
                {
                    AddNoda(NodaRoot[(int)DbUnitsEnum.schema].Nodes, $"{schema.SchemaName}", 1, colorEqual);
                }
                foreach (var table in db1.tables.Values)
                {
                    AddNoda(NodaRoot[(int)DbUnitsEnum.table].Nodes, $"{table.TableName}", 2, colorEqual);
                }
                foreach (var view in db1.views.Values)
                {
                    AddNoda(NodaRoot[(int)DbUnitsEnum.view].Nodes, $"{view.ViewName}", 3, colorEqual);
                }
                return;
            }
            else
            {
                tsLabelEquals.Text = "Структура баз отличается              ";
            }
            var difRes = DataProcessor.DBCompare(db1, db2);
            // отобразим дерево
            // Schemas
            foreach (var schema in difRes[DbUnitsEnum.schema][CompareItogEnum.missing]) 
            {
                if (db1.schemas.ContainsKey(schema))
                    AddNoda(NodaRoot[(int)DbUnitsEnum.schema].Nodes, $"{schema}", 1, colorDB1);
                else
                    AddNoda(NodaRoot[(int)DbUnitsEnum.schema].Nodes, $"{schema}", 1, colorDB2);
            }
            foreach (var schema in difRes[DbUnitsEnum.schema][CompareItogEnum.extendetDifference])
            {
                AddNoda(NodaRoot[(int)DbUnitsEnum.schema].Nodes, $"{schema}", 1, colorDiferentIntern);
            }
            foreach (var schemaKey in db1.schemas.Keys)
            {
                if (difRes[DbUnitsEnum.schema][CompareItogEnum.missing].Contains(schemaKey))
                    continue;
                if (difRes[DbUnitsEnum.schema][CompareItogEnum.extendetDifference].Contains(schemaKey))
                    continue;
                AddNoda(NodaRoot[(int)DbUnitsEnum.schema].Nodes, $"{db1.schemas[schemaKey].SchemaName}", 1, colorEqual);
            }
            // Tables
            foreach (var table in difRes[DbUnitsEnum.table][CompareItogEnum.missing])
            {
                if (db1.tables.ContainsKey(table))
                    AddNoda(NodaRoot[(int)DbUnitsEnum.table].Nodes, $"{table}", 2, colorDB1);
                else
                    AddNoda(NodaRoot[(int)DbUnitsEnum.table].Nodes, $"{table}", 2, colorDB2);
            }
            foreach (var table in difRes[DbUnitsEnum.table][CompareItogEnum.extendetDifference])
            {
                AddNoda(NodaRoot[(int)DbUnitsEnum.table].Nodes, $"{table}", 2, colorDiferentIntern);
            }
            foreach (var tableKey in db1.tables.Keys)
            {
                if (difRes[DbUnitsEnum.table][CompareItogEnum.missing].Contains(tableKey))
                    continue;
                if (difRes[DbUnitsEnum.table][CompareItogEnum.extendetDifference].Contains(tableKey))
                    continue;
                AddNoda(NodaRoot[(int)DbUnitsEnum.table].Nodes, $"{db1.tables[tableKey].TableName}", 2, colorEqual);
            }
            // Views
            foreach (var view in difRes[DbUnitsEnum.view][CompareItogEnum.missing])
            {
                if (db1.views.ContainsKey(view))
                    AddNoda(NodaRoot[(int)DbUnitsEnum.view].Nodes, $"{view}", 3, colorDB1);
                else
                    AddNoda(NodaRoot[(int)DbUnitsEnum.view].Nodes, $"{view}", 3, colorDB2);
            }
            foreach (var view in difRes[DbUnitsEnum.view][CompareItogEnum.extendetDifference])
            {
                AddNoda(NodaRoot[(int)DbUnitsEnum.view].Nodes, $"{view}", 3, colorDiferentIntern);
            }
            foreach (var viewKey in db1.views.Keys)
            {
                if (difRes[DbUnitsEnum.view][CompareItogEnum.missing].Contains(viewKey))
                    continue;
                if (difRes[DbUnitsEnum.view][CompareItogEnum.extendetDifference].Contains(viewKey))
                    continue;
                AddNoda(NodaRoot[(int)DbUnitsEnum.view].Nodes, $"{db1.views[viewKey].ViewName}", 3, colorEqual);
            }

        }
        
        private void AddNoda(TreeNodeCollection noda, string Text, int imageIndex, Color color)
        {
            int nodaCount = noda.Count;
            noda.Add(Text);
            noda[nodaCount].ImageIndex = imageIndex;
            noda[nodaCount].SelectedImageIndex = imageIndex;
            noda[nodaCount].ForeColor = color;

        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            if (0 == e.Node.Level)
                return;
            
            try
            {
                var list = DataProcessor.PrepareSQLList(db1, db2, Helper.GetFullPath(e));
                tsNodeInfo.Text = list.statusText;
                richTextBox1.Text = string.Join("", list.script1.ToArray());
                richTextBox2.Text = string.Join("", list.script2.ToArray());

                
                // выделим ключевые слова
                MarkKeyWords(richTextBox1, regExpr, Color.Blue);
                MarkKeyWords(richTextBox2, regExpr, Color.Blue);
                MarkKeyWords(richTextBox1, regExprGray, Color.Gray);
                MarkKeyWords(richTextBox2, regExprGray, Color.Gray);
                // выделим отличающиеся строки
                MarkStrings(richTextBox1,list.difs);
                MarkStrings(richTextBox2, list.difs);




            }
            catch (ComparerException ex)
            {
                MessageBox.Show("Ошибка в программе:" + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void MarkStrings(RichTextBox rtb, List<int> nums) 
        {
            foreach (var line in nums)
            {
                int start = rtb.GetFirstCharIndexFromLine(line);
                int lengthSelection = rtb.Lines[line].Length;
                rtb.Select(start, lengthSelection);
                rtb.SelectionColor = Color.Red;
            }
        }
        private void MarkKeyWords(RichTextBox rtb,string keyWords, Color color) 
        {
            foreach (Match m in Regex.Matches(rtb.Text, keyWords))
            {
                rtb.SelectionStart = m.Index;
                rtb.SelectionLength = m.Length;
                rtb.SelectionColor = color;
            }
        }










    }
}
