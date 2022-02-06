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
        private static Color colorEqual = Color.Green;  // структуры есть в обоих БД и их внутренняя структура идентична
        private static Color colorEpson = Color.Red;    // структура отсутствует в одной из БД 
        private static Color colorDiferentIntern = Color.Coral; // структура есть в обоих БД, но их внутренняя структура отличается
        public Form1()
        {
            InitializeComponent();
            tsDBInfo.Visible = false;
            splitContainer3.SplitterDistance = splitContainer3.Size.Width / 2;
            HideNotes();



        }
        private string stringConnection1 = "";
        private string stringConnection2 = "";

        private void newFileMenu_Click(object sender, EventArgs e)
        {
            ConnectionForm connectionForm = new ConnectionForm();
            DialogResult dr = connectionForm.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            tsLabelEquals.Text = "";
            stringConnection1 = connectionForm.ConnectionString1;
            stringConnection2 = connectionForm.ConnectionString2;
            tsDBInfo.Visible = true;
            tsDB1Info.Text = stringConnection1;
            tsDB2Info.Text = "/n" + stringConnection2;
            tsProcessImage1.Visible = true;
            tsProcessImage2.Visible = true;
            db1 = null;
            db2 = null;
            HideNotes();

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
                string msg = String.Format("Ошибка при получении информации о БД1: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }

            db1 = (DataBase)e.Result;
            if (db1 is null)
            {// информации нет
                MessageBox.Show("Ошибка получения Информации о структуре БД1");
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
                string msg = String.Format("Ошибка при получении информации о БД2: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }

            db2 = (DataBase)e.Result;
            if (db2 is null)
            {// информации нет
                MessageBox.Show("Ошибка получения Информации о структуре БД2");
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
            ShowNotes();
            
            tsDB1Info.Text = $"Server:{db1.dbServer}\nDataBase: {db1.dbName}";
            tsDB2Info.Text = $"Server:{db2.dbServer}\nDataBase: {db2.dbName}";
            if (db1.Equals(db2))
            {
                tsLabelEquals.Text = "Структура баз одинакова               ";
            }
            else
            {
                tsLabelEquals.Text = "Структура баз отличается              ";
            }
            var Noda00SchemasRoot = treeView1.Nodes;
            Noda00SchemasRoot.Clear();
            treeView1.ImageIndex = 0;
            AddNoda(Noda00SchemasRoot, $"Schemas({ db1.schemas.Count}/{db2.schemas.Count})", 1, colorDiferentIntern);
            var Noda01SchemaNames = Noda00SchemasRoot[0].Nodes;
            int Noda01Num = 0;
            if (CollectionComparer.DictEquals(db1.schemas, db2.schemas))
            {//СХЕМЫ ОДИНАКОВЫЕ
                Noda00SchemasRoot[0].ForeColor = colorEqual;
                foreach (var schemaKey in db1.schemas.Keys)
                {
                    SetEqualSchema(db1.schemas[schemaKey], db2.schemas[schemaKey], Noda01SchemaNames, Noda01Num);
                    Noda01Num++;
                }
                return;
            }
            // ЕСТЬ ОТЛИЧИЯ
            // получим список ключей отличающихся схем
            var diffKeys = CollectionComparer.GetDifference(new List<string>(db1.schemas.Keys), new List<string>(db2.schemas.Keys));
            
            // ОТЛИЧАЮЩИЕСЯ СХЕМЫ
            foreach (var schema in diffKeys)
            {
                if (db1.schemas.ContainsKey(schema))
                {// схема есть в левой БД, а в правой нет
                    SetUniqSchemas(db1.schemas[schema], Noda01SchemaNames, Noda01Num, true);
                }
                else
                {// схема есть в правой БД, а в левой нет
                    SetUniqSchemas(db2.schemas[schema], Noda01SchemaNames, Noda01Num, false);
                }
                Noda01Num++;
            }
            // теперь похожие ключи (названия схем )
            foreach (var schemaKey in db1.schemas.Keys)
            {
                if (diffKeys.Contains(schemaKey))
                    continue;// эту схему уже вывели в Tree
                if (db1.schemas[schemaKey].Equals(db2.schemas[schemaKey]))
                {//схемы одинаковые и по содержанию
                    SetEqualSchema(db1.schemas[schemaKey], db2.schemas[schemaKey], Noda01SchemaNames, Noda01Num);
                }
                else 
                {//схемы отличаются по содержанию
                    
                    Schema schema1 = db1.schemas[schemaKey];
                    Schema schema2 = db2.schemas[schemaKey];
                    AddNoda(Noda01SchemaNames
                                , $"[{schema1.Name}]({schema1.tables.Count} tables {schema1.views.Count} views / {schema2.tables.Count} tables {schema2.views.Count} views)"
                                , 1, colorDiferentIntern);
                    if (CollectionComparer.DictEquals(schema1.tables, schema2.tables))
                    {//ТАБЛИЦЫ одинаковые
                        if (schema1.tables.Count > 0)
                        {
                            var Noda02TablesRoot = Noda01SchemaNames[Noda01Num].Nodes;
                            AddNoda(Noda02TablesRoot
                                , $"Tables {schema1.tables.Count}"
                                , 2, colorEqual);
                            int iNodaTablesNameLevel = 0;
                            foreach (var tableKey in schema1.tables.Keys)
                            {
                                var Noda03TableNames = Noda02TablesRoot[Noda02TablesRoot.Count - 1].Nodes;
                                var table = schema1.tables[tableKey];
                                AddNoda(Noda03TableNames
                                    , $"[{table.Name}] ({table.columns.Count} columns {table.rowCount} rows /{schema2.tables[tableKey].rowCount} rows)"
                                    , 2, colorEqual);


                                int iNodaTableCollectLevel = 0;
                                // СТОЛБЦЫ
                                AddColumnsNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorEqual, ref iNodaTableCollectLevel);
                                // ИНДЕКСЫ
                                AddIndexesNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorEqual, ref iNodaTableCollectLevel);
                                // ОГРАНИЧЕНИЯ
                                AddConstraintsNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorEqual, ref iNodaTableCollectLevel);
                                iNodaTablesNameLevel++;
                            }
                        }
                    }
                    else
                    {//ТАБЛИЦЫ отличаются
                        var Noda02TablesRoot = Noda01SchemaNames[Noda01Num].Nodes;
                        AddNoda(Noda02TablesRoot
                            , $"Tables {schema1.tables.Count}/{schema2.tables.Count}"
                            , 2, colorDiferentIntern);
                        int iNodaTablesNameLevel = 0;
                        var diffKeysTables = CollectionComparer.GetDifference(new List<string>(schema1.tables.Keys), new List<string>(schema2.tables.Keys));
                        if (0 != diffKeysTables.Count)
                        {// некоторые называются по разному
                            foreach (var tableKey in diffKeysTables)
                            {
                                var Noda03TableNames = Noda02TablesRoot[Noda02TablesRoot.Count - 1].Nodes;
                                int iNodaTableCollectLevel = 0;
                                if (schema1.tables.ContainsKey(tableKey))
                                {
                                    var table = schema1.tables[tableKey];
                                    AddNoda(Noda03TableNames
                                            , $"[{table.Name}] ({table.columns.Count} columns {table.rowCount} rows /0)", 2, colorEpson);

                                    
                                    // СТОЛБЦЫ
                                    AddColumnsNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorEpson, ref iNodaTableCollectLevel);
                                    // ИНДЕКСЫ
                                    AddIndexesNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorEpson, ref iNodaTableCollectLevel);
                                    // ОГРАНИЧЕНИЯ
                                    AddConstraintsNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorEpson, ref iNodaTableCollectLevel);
                                }
                                else 
                                {
                                    var table = schema2.tables[tableKey];
                                    AddNoda(Noda03TableNames
                                            , $"[{table.Name}] (0/{table.columns.Count} columns {table.rowCount} rows)", 2, colorEpson);

                                    // СТОЛБЦЫ
                                    AddColumnsNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorEpson, ref iNodaTableCollectLevel);
                                    // ИНДЕКСЫ
                                    AddIndexesNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorEpson, ref iNodaTableCollectLevel);
                                    // ОГРАНИЧЕНИЯ
                                    AddConstraintsNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorEpson, ref iNodaTableCollectLevel);
                                }
                                iNodaTablesNameLevel++;
                            }

                        }
                        foreach (var tableKey in schema1.tables.Keys)
                        {
                            if (diffKeysTables.Contains(tableKey))
                                continue;// уже вывели в Tree
                            var Noda03TableNames = Noda02TablesRoot[Noda02TablesRoot.Count - 1].Nodes;
                            var table1 = schema1.tables[tableKey];
                            var table2 = schema2.tables[tableKey];
                            int iNodaTableCollectLevel = 0;
                            if (table1.Equals(table2))
                            {//таблицы одинаковые
                                AddNoda(Noda03TableNames
                                , $"[{table1.Name}] ({table1.columns.Count} columns {table1.rowCount} rows /{table2.columns.Count} columns {table2.rowCount} rows)"
                                , 2, colorEqual);

                                
                                // СТОЛБЦЫ
                                AddColumnsNodes(table1, Noda03TableNames, iNodaTablesNameLevel, colorEqual, ref iNodaTableCollectLevel);
                                // ИНДЕКСЫ
                                AddIndexesNodes(table1, Noda03TableNames, iNodaTablesNameLevel, colorEqual, ref iNodaTableCollectLevel);
                                // ОГРАНИЧЕНИЯ
                                AddConstraintsNodes(table1, Noda03TableNames, iNodaTablesNameLevel, colorEqual, ref iNodaTableCollectLevel);
                            }
                            else
                            {
                                AddNoda(Noda03TableNames
                                    , $"[{table1.Name}] ({table1.columns.Count} columns {table1.rowCount} rows /{table2.columns.Count} columns {table2.rowCount} rows)"
                                    , 2, colorDiferentIntern);

                                // СТОЛБЦЫ
                                if (CollectionComparer.DictEquals(table1.columns, table2.columns))
                                {//одинаковые
                                    AddColumnsNodes(table1, Noda03TableNames, iNodaTablesNameLevel, colorEqual, ref iNodaTableCollectLevel);
                                }
                                else
                                {//отличаются
                                    var Noda04ColumnRoot = Noda03TableNames[iNodaTablesNameLevel].Nodes;
                                    AddNoda(Noda04ColumnRoot, $"Columns {schema1.tables[tableKey].columns.Count}/{schema2.tables[tableKey].columns.Count}", 4, colorDiferentIntern);
                                    var diffKeysColumns = CollectionComparer.GetDifference(new List<string>(schema1.tables[tableKey].columns.Keys), new List<string>(schema2.tables[tableKey].columns.Keys));
                                    //сначала пройдем по отличающимся
                                    int iColumnNum = 0;
                                    foreach (var columnKey in diffKeysColumns)
                                    {
                                        var Noda05ColumnNames = Noda04ColumnRoot[iNodaTableCollectLevel].Nodes;
                                        if (schema1.tables[tableKey].columns.ContainsKey(columnKey))
                                        {// есть в левой БД, а в правой нет
                                            AddNoda(Noda05ColumnNames, $"[{schema1.tables[tableKey].columns[columnKey].Name}] ({schema1.tables[tableKey].columns[columnKey].TypeName}/0)", 4, colorDiferentIntern);
                                        }
                                        else
                                        {// есть в правой БД, а в левой нет
                                            AddNoda(Noda05ColumnNames, $"[{schema2.tables[tableKey].columns[columnKey].Name}] (0/{schema2.tables[tableKey].columns[columnKey].TypeName}", 4, colorDiferentIntern);
                                        }
                                        iColumnNum++;
                                    }
                                    foreach (var columnKey in schema1.tables[tableKey].columns.Keys)
                                    {
                                        if (diffKeysColumns.Contains(columnKey))
                                            continue;// уже вывели в Tree
                                        var Noda05ColumnNames = Noda04ColumnRoot[iNodaTableCollectLevel].Nodes;
                                        AddNoda(Noda05ColumnNames, $"[{schema1.tables[tableKey].columns[columnKey].Name}] ({schema1.tables[tableKey].columns[columnKey].TypeName}" +
                                            $"/ {schema2.tables[tableKey].columns[columnKey].TypeName})", 4, colorDiferentIntern);
                                    }
                                    iNodaTableCollectLevel++;
                                }
                                // ИНДЕКСЫ
                                if (CollectionComparer.DictEquals(table1.indexes, table2.indexes))
                                {//одинаковые
                                    AddIndexesNodes(table1, Noda03TableNames, iNodaTablesNameLevel, colorEqual, ref iNodaTableCollectLevel);
                                }
                                else
                                {//отличаются
                                    var Noda04IndexRoot = Noda03TableNames[iNodaTablesNameLevel].Nodes;
                                    AddNoda(Noda04IndexRoot, $"Indexes {schema1.tables[tableKey].indexes.Count}/{schema2.tables[tableKey].indexes.Count}", 4, colorDiferentIntern);
                                    var diffKeysIndexes = CollectionComparer.GetDifference(new List<string>(schema1.tables[tableKey].indexes.Keys), new List<string>(schema2.tables[tableKey].indexes.Keys));
                                    //сначала пройдем по отличающимся
                                    int iIndexNum = 0;
                                    foreach (var indexKey in diffKeysIndexes)
                                    {
                                        var Noda05IndexNames = Noda04IndexRoot[iNodaTableCollectLevel].Nodes;
                                        if (schema1.tables[tableKey].indexes.ContainsKey(indexKey))
                                        {// ограничение есть в левой БД, а в правой нет

                                            AddNoda(Noda05IndexNames, $"[{schema1.tables[tableKey].indexes[indexKey].indexName}] ({schema1.tables[tableKey].indexes[indexKey].columns.Count} columns/0)", 4, colorDiferentIntern);
                                            var Noda06IndexColumns = Noda05IndexNames[iIndexNum].Nodes;
                                            foreach (var column in schema1.tables[tableKey].indexes[indexKey].columns)
                                            {
                                                AddNoda(Noda06IndexColumns, $"[{column}]", 4, colorEpson);
                                            }
                                        }
                                        else
                                        {// ограничение есть в правой БД, а в левой нет
                                            AddNoda(Noda05IndexNames, $"[{schema2.tables[tableKey].indexes[indexKey].indexName}] (0/{schema2.tables[tableKey].indexes[indexKey].columns.Count} columns)", 4, colorDiferentIntern);
                                            var Noda06IndexColumns = Noda05IndexNames[iIndexNum].Nodes;
                                            foreach (var column in schema2.tables[tableKey].indexes[indexKey].columns)
                                            {
                                                AddNoda(Noda06IndexColumns, $"[{column}]", 4, colorEpson);
                                            }
                                        }
                                        iIndexNum++;

                                    }
                                    foreach (var indexKey in schema1.tables[tableKey].indexes.Keys)
                                    {
                                        if (diffKeysIndexes.Contains(indexKey))
                                            continue;// уже вывели в Tree

                                        var Noda05IndexNames = Noda04IndexRoot[iNodaTableCollectLevel].Nodes;
                                        AddNoda(Noda05IndexNames, $"[{schema1.tables[tableKey].indexes[indexKey].indexName}] ({schema1.tables[tableKey].indexes[indexKey].columns.Count} columns" +
                                            $"/ {schema2.tables[tableKey].indexes[indexKey].columns.Count} columns)", 4, colorDiferentIntern);
                                        var Noda06IndexColumns = Noda05IndexNames[iIndexNum].Nodes;
                                        var diffKeysIndexesColumns = CollectionComparer.GetDifference(new List<string>(schema1.tables[tableKey].indexes[indexKey].columns), new List<string>(schema2.tables[tableKey].indexes[indexKey].columns));
                                        foreach (var indexColumn in diffKeysIndexesColumns)
                                        {
                                            AddNoda(Noda06IndexColumns, $"[{indexColumn}]", 4, colorEpson);
                                        }
                                        foreach (var indexColumn in schema1.tables[tableKey].indexes[indexKey].columns)
                                        {
                                            if (diffKeysIndexesColumns.Contains(indexColumn))
                                                continue;
                                            AddNoda(Noda06IndexColumns, $"[{indexColumn}]", 4, colorDiferentIntern);
                                        }

                                        iIndexNum++;
                                    }
                                    iNodaTableCollectLevel++;
                                }
                                // ОГРАНИЧЕНИЯ
                                if (CollectionComparer.EnumEquals(table1.constraints, table2.constraints))
                                {//одинаковые
                                    AddConstraintsNodes(table1, Noda03TableNames, iNodaTablesNameLevel, colorEqual, ref iNodaTableCollectLevel);
                                }
                                else
                                {//отличаются

                                    var Noda04ConstRoot = Noda03TableNames[iNodaTablesNameLevel].Nodes;
                                    AddNoda(Noda04ConstRoot, $"Constraints {schema1.tables[tableKey].constraints.Count}/{schema2.tables[tableKey].constraints.Count}", 6, colorDiferentIntern);
                                    var diffKeysConstrName = CollectionComparer.GetDifference(schema1.tables[tableKey].constraints, schema2.tables[tableKey].constraints);

                                    //сначала пройдем по отличающимся
                                    foreach (var constrKey in diffKeysConstrName)
                                    {
                                        var Noda05ConstrNames = Noda04ConstRoot[iNodaTableCollectLevel].Nodes;
                                        if (schema1.tables[tableKey].constraints.Contains(constrKey))
                                        {// ограничение есть в левой БД, а в правой нет

                                            AddNoda(Noda05ConstrNames, $"[{constrKey.Name}] ({constrKey.Type}/0)", 6, colorDiferentIntern);
                                        }
                                        else
                                        {// ограничение есть в правой БД, а в левой нет
                                            AddNoda(Noda05ConstrNames, $"[{constrKey.Name}] (0/{constrKey.Type})", 6, colorDiferentIntern);
                                        }

                                    }
                                    foreach (var constrKey in schema1.tables[tableKey].constraints)
                                    {
                                        if (diffKeysConstrName.Contains(constrKey))
                                            continue;// уже вывели в Tree

                                        var Noda05ConstrNames = Noda04ConstRoot[iNodaTableCollectLevel].Nodes;
                                        AddNoda(Noda05ConstrNames, $"[{constrKey.Name}] ({constrKey.Type})", 6, colorEqual);
                                    }
                                    iNodaTableCollectLevel++;
                                }
                            }
                            iNodaTablesNameLevel++;
                        }
                    }
                    if (CollectionComparer.DictEquals(schema1.views, schema2.views))
                    {//ВЬЮХИ одинаковые
                        if (schema1.views.Count > 0)
                        {
                            var Noda02 = Noda01SchemaNames[Noda01Num].Nodes;
                            AddNoda(Noda02
                                , $"Views {schema1.views.Count}"
                                , 5, colorEqual);

                            foreach (var viewKey in schema1.views.Keys)
                            {
                                var Noda03 = Noda02[Noda02.Count -1].Nodes;
                                var view = schema1.views[viewKey];
                                AddNoda(Noda03, $"[{view.Name}]", 5, colorEqual);
                            }
                        }
                    }
                    else 
                    {//ВЬЮХИ отличаются
                        var Noda02 = Noda01SchemaNames[Noda01Num].Nodes;
                        AddNoda(Noda02
                            , $"Views {schema1.views.Count}/{schema2.views.Count}"
                            , 5, colorDiferentIntern);
                        var diffKeysViews = CollectionComparer.GetDifference(new List<string>(schema1.views.Keys), new List<string>(schema2.views.Keys));
                        var Noda03 = Noda02[Noda02.Count - 1].Nodes;
                        //сначала пройдем по отличающимся
                        foreach (var viewKey in diffKeysViews)
                        {
                            if (schema1.views.ContainsKey(viewKey))
                            {// вьюха есть в левой БД, а в правой нет
                                AddNoda(Noda03, $"[{schema1.views[viewKey].Name}]/0", 5, colorDiferentIntern);
                            }
                            else
                            {// вьюха есть в правой БД, а в левой нет
                                AddNoda(Noda03, $"0/[{schema2.views[viewKey].Name}]", 5, colorDiferentIntern);
                            }
                        }
                        //теперь остальные
                        foreach (var viewKey in schema1.views.Keys)
                        {
                            AddNoda(Noda03, $"[{schema1.views[viewKey].Name}]", 5, colorEqual);
                        }
                        
                    }
                }

                Noda01Num++;
            }


        }
        private void SetUniqSchemas(Schema schema, TreeNodeCollection Noda01SchemaNames, int Noda01Num, bool IsLeftDB)
        {
            SetSchemas(schema, null, Noda01SchemaNames, Noda01Num, colorEpson, false, IsLeftDB);
        }
        private void SetEqualSchema(Schema schema1, Schema schema2, TreeNodeCollection Noda01SchemaNames, int Noda01Num)
        {
            SetSchemas(schema1, schema2, Noda01SchemaNames, Noda01Num, colorEqual, true);
        }
        private void SetSchemas(Schema schema1, Schema schema2, TreeNodeCollection Noda01SchemaNames, int Noda01Num, Color colorNoda, bool isEqual, bool IsLeftDB = false)
        {
            string sNodeText;
            if (isEqual)
            {
                sNodeText = $"[{schema1.Name}]({schema1.tables.Count} tables {schema1.views.Count} views)";
            }
            else
            {
                sNodeText = AddZero($"[{schema1.Name}]({schema1.tables.Count} tables {schema1.views.Count} views)", IsLeftDB);
            }
            AddNoda(Noda01SchemaNames, sNodeText, 1, colorNoda);
            // ТАБЛИЦЫ
            if (schema1.tables.Count > 0)
            {
                var Noda02TablesRoot = Noda01SchemaNames[Noda01Num].Nodes;
                if (isEqual)
                {
                    sNodeText = $"Tables {schema1.tables.Count}";
                }
                else
                {
                    sNodeText = AddZero($"Tables {schema1.tables.Count}", IsLeftDB);
                }
                AddNoda(Noda02TablesRoot, sNodeText, 2, colorNoda);
                int iNodaTablesNameLevel = 0;
                foreach (var tableKey in schema1.tables.Keys)
                {
                    var Noda03TableNames = Noda02TablesRoot[Noda02TablesRoot.Count-1].Nodes;
                    var table = schema1.tables[tableKey];
                    if (isEqual)
                    {
                        sNodeText = $"[{table.Name}] ({table.columns.Count} columns {table.rowCount} rows /{schema2.tables[tableKey].rowCount} rows)";
                    }
                    else
                    {
                        sNodeText = AddZero($"[{table.Name}] ({table.columns.Count} columns {table.rowCount} rows)", IsLeftDB);
                    }
                    AddNoda(Noda03TableNames, sNodeText, 2, colorNoda);


                    int iNodaTableCollectLevel = 0;
                    // СТОЛБЦЫ
                    AddColumnsNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorNoda, ref iNodaTableCollectLevel);
                    // ИНДЕКСЫ
                    AddIndexesNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorNoda, ref iNodaTableCollectLevel);

                    // ОГРАНИЧЕНИЯ
                    AddConstraintsNodes(table, Noda03TableNames, iNodaTablesNameLevel, colorNoda, ref iNodaTableCollectLevel);

                    iNodaTablesNameLevel++;
                }
            }
            // ВЬЮХИ
            if (schema1.views.Count > 0)
            {
                var Noda02ViewsRoot = Noda01SchemaNames[Noda01Num].Nodes;
                if (isEqual)
                {
                    sNodeText = $"Views {schema1.views.Count}";
                }
                else
                {
                    sNodeText = AddZero($"Views {schema1.views.Count}", IsLeftDB);
                }
                AddNoda(Noda02ViewsRoot, sNodeText, 5, colorNoda);
                var Noda03 = Noda02ViewsRoot[Noda02ViewsRoot.Count-1].Nodes;
                foreach (var viewKey in schema1.views.Keys)
                {
                    AddNoda(Noda03, $"[{schema1.views[viewKey].Name}]", 5, colorNoda);
                }
            }
        }
        public string AddZero(string text, bool isFirst)
        {
            if (isFirst)
                return text + "/0";
            else
                return "0/" + text;

        }

        private void AddColumnsNodes(Table table, TreeNodeCollection Noda03TableNames, int iNodaTablesNameLevel, Color colorNoda, ref int iNodaTableCollectLevel)
        {
            if (table.columns.Count > 0)
            {

                var Noda04ColumnsRoot = Noda03TableNames[iNodaTablesNameLevel].Nodes;
                AddNoda(Noda04ColumnsRoot, $"Columns {table.columns.Count}", 4, colorNoda);
                var Noda05ColumnNames = Noda04ColumnsRoot[iNodaTableCollectLevel].Nodes;
                foreach (var column in table.columns.Values)
                {
                    AddNoda(Noda05ColumnNames, $"[{column.Name}] {column.TypeName}", 4, colorNoda);
                }
                iNodaTableCollectLevel++;

            }
        }
        private void AddIndexesNodes(Table table, TreeNodeCollection Noda03TableNames, int iNodaTablesNameLevel, Color colorNoda, ref int iNodaTableCollectLevel)
        {
            if (table.indexes.Count > 0)
            {
                var Noda04IndexRoot = Noda03TableNames[iNodaTablesNameLevel].Nodes;
                AddNoda(Noda04IndexRoot, $"Indexes {table.indexes.Count}", 4, colorNoda);
                int iIndexNum = 0;
                foreach (var index in table.indexes.Values)
                {
                    var Noda05IndexNames = Noda04IndexRoot[iNodaTableCollectLevel].Nodes;
                    AddNoda(Noda05IndexNames, $"[{index.indexName}] ({index.columns.Count} columns)", 4, colorNoda);
                    var Noda06IndexColumns = Noda05IndexNames[iIndexNum].Nodes;
                    foreach (var column in index.columns)
                    {
                        AddNoda(Noda06IndexColumns, $"[{column}]", 4, colorNoda);
                    }
                    iIndexNum++;
                }
                iNodaTableCollectLevel++;
            }
        }
        private void AddConstraintsNodes(Table table, TreeNodeCollection Noda03TableNames, int iNodaTablesNameLevel, Color colorNoda, ref int iNodaTableCollectLevel)
        {
            if (table.constraints.Count > 0)
            {
                var Noda04ConstRoot = Noda03TableNames[iNodaTablesNameLevel].Nodes;
                AddNoda(Noda04ConstRoot, $"Constraints {table.constraints.Count}", 6, colorNoda);
                var Noda05ConstrNames = Noda04ConstRoot[iNodaTableCollectLevel].Nodes;
                foreach (var constraint in table.constraints)
                {
                    AddNoda(Noda05ConstrNames, $"[{constraint.Name}] ({constraint.Type})", 6, colorNoda);
                }
                iNodaTableCollectLevel++;
            }
        }
        private void AddNoda(TreeNodeCollection noda, string Text, int imageIndex, Color color)
        {
            int nodaCount = noda.Count;
            noda.Add(Text);
            noda[nodaCount].ImageIndex = imageIndex;
            noda[nodaCount].ForeColor = color;

        }
        private void ShowNotes() 
        {
            labColorDifferent.Visible = true;
            labColorDifferent.Text = "отличие во внутренней стр-ре";
            labColorDifferent.ForeColor = colorDiferentIntern;
            labColorEqual.Visible = true;
            labColorEqual.Text = "структуры одинаковы";
            labColorEqual.ForeColor = colorEqual;
            labColorEpson.Visible = true;
            labColorEpson.Text = "отсутствует в одной из БД";
            labColorEpson.ForeColor = colorEpson;
        }
        private void HideNotes() 
        {
            labColorDifferent.Visible = false;
            labColorEqual.Visible = false;
            labColorEpson.Visible = false;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            labelInfo1.Text = PrepareText(db1, GetFullPath(e));
            labelInfo2.Text = PrepareText(db2, GetFullPath(e));




        }
        private Dictionary<int, string> GetFullPath(TreeViewEventArgs e) 
        {
            Dictionary<int, string> route = new Dictionary<int, string>();
            int iLevel = e.Node.Level;
            TreeNode tn = e.Node;
            do
            {
                route.Add(iLevel, tn.Text);
                if (iLevel > 0)
                {
                    iLevel--;
                    tn = tn.Parent;
                }
            } while (iLevel > 0);
            return route;
        }
        private string PrepareText(DataBase db, Dictionary<int, string> path) 
        {
            bool IsTable = true;
            string shemaName = "";
            string sTblViewName = "";
            StringBuilder sb = new StringBuilder();
            foreach (var key in path.Keys) 
            {
                switch (key) 
                {
                    case 0: //корень
                        {
                        }break;
                    case 1://имя схемы 
                        {
                            shemaName = GetWithIn(path[key]);
                            sb.Append($"Схема:{shemaName} ");
                            if (db.schemas.ContainsKey(shemaName))
                            {
                                sb.Append(Environment.NewLine);
                                sb.Append($"{new string(' ', 5)}{db.schemas[shemaName].tables.Count} таблиц, {db.schemas[shemaName].views.Count} представлений");
                                sb.Append(Environment.NewLine);
                                sb.Append(Environment.NewLine);
                            }
                            else 
                            {
                                sb.Append("ОТСУТСТВУЕТ/");
                                return sb.ToString();
                            }

                        }
                        break;
                    case 2:// количество таблиц/вьюх
                        {
                            if(path[key].Contains("Table"))
                                IsTable = true;
                        }
                        break;
                    case 3:// имя таблицы или вьюхи
                        {
                            sTblViewName = GetWithIn(path[key]);
                            if (IsTable)
                            {
                                sb.Append($"Таблица:{sTblViewName} ");
                                if (db.schemas[shemaName].tables.ContainsKey(sTblViewName))
                                {
                                    sb.Append(Environment.NewLine);
                                    sb.Append($"{new string(' ', 5)}{db.schemas[shemaName].tables[sTblViewName].columns.Count} столбцов" +
                                        $", {db.schemas[shemaName].tables[sTblViewName].indexes.Count} индексов" +
                                        $", {db.schemas[shemaName].tables[sTblViewName].constraints.Count} ограничений");
                                    sb.Append(Environment.NewLine);
                                    sb.Append(Environment.NewLine);
                                }
                                else
                                {
                                    sb.Append("ОТСУТСТВУЕТ.");
                                    return sb.ToString();
                                }
                            }
                            else 
                            {
                                sb.Append($"Представление:{sTblViewName} ");
                                if (db.schemas[shemaName].views.ContainsKey(sTblViewName))
                                {
                                    sb.Append(Environment.NewLine);
                                }
                                else
                                {
                                    sb.Append("ОТСУТСТВУЕТ.");
                                    
                                }
                                return sb.ToString();
                            }


                        }
                        break;
                    case 4:// только у таблцы, информация о кол-ве столбцов, индексов и ограничений
                        {
                        }
                        break;
                    case 5:// имя столбца или имя индекса или имя ограничения
                        {
                        }
                        break;
                    case 6: //только у индекса имя столбца индекса
                        {
                        }
                        break;
                    default: 
                        { }break;
                }
            }
            return "";
        }

        
        private string GetWithIn(string str)
        {
            List<string> rez = new List<string>();

            Regex pattern =
                new Regex(@"\[(?<val>.*?)\]",
                    RegexOptions.Compiled |
                    RegexOptions.Singleline);

            foreach (Match m in pattern.Matches(str))
                if (m.Success)
                    //меж скобок ( )  
                    rez.Add(m.Groups["val"].Value);

            if (rez.Count > 0)
                return rez[0];
            else
                return "";
        }


    }
}
