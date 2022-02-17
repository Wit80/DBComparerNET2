using DBComparerLibrary.DBSchema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace DBComparerLibrary
{
    public class DataProcessor
    {

        private DBsysWorker wrk;

        public DataProcessor(string connString)
        {
            wrk = new DBsysWorker(connString);
        }
        public DataBase RunProcess() 
        {
            try
            {
                return wrk.GetDataFromDB();
            } 
            catch (Exception ex) 
            {
                throw new ComparerException(ex.Message);
            }
        }
        public static Dictionary<DbUnitsEnum, Dictionary<CompareItogEnum, List<string>>> DBCompare(DataBase db1, DataBase db2) 
        {
            
            return new Dictionary<DbUnitsEnum, Dictionary<CompareItogEnum, List<string>>>() 
            {
                { DbUnitsEnum.schema, Comparer.dbItemCompare<string,Schema>(db1.schemas, db2.schemas)},
                { DbUnitsEnum.table,  Comparer.dbItemCompare<string, Table>(db1.tables, db2.tables)},
                { DbUnitsEnum.view,  Comparer.dbItemCompare<string, View>(db1.views, db2.views)}
            };
        }
        public static SQLScriptsList PrepareSQLList(DataBase db1, DataBase db2,SortedDictionary<int, string> path)
        {
            if (path[0].StartsWith("Schema"))
            {
                return PrepareSQLListSchems(db1, db2, path[1]);
            }
            else if (path[0].StartsWith("Table"))
            {
                return PrepareSQLListTables(db1, db2, path[1]);
            }
            else 
            {
                return PrepareSQLListViews(db1, db2, path[1]);
            }
           
        }
        private static SQLScriptsList PrepareSQLListSchems(DataBase db1, DataBase db2, string SchemaName) 
        {
            try
            {
                if (db1.schemas.ContainsKey(SchemaName) && db2.schemas.ContainsKey(SchemaName))
                {// Обе схемы есть
                    if (db1.schemas[SchemaName].Equals(db2.schemas[SchemaName]))
                    {// схемы одинаковые
                        return new SQLScriptsList(SchemaScript(db1.schemas[SchemaName]), SchemaScript(db2.schemas[SchemaName]), new List<int>(),"Схемы одинаковы");
                    }
                    else
                    {
                        var script1 = SchemaScript(db1.schemas[SchemaName]);
                        var script2 = SchemaScript(db2.schemas[SchemaName]);
                        return new SQLScriptsList(script1, script2, new List<int>() { 0 },"Схемы отличаются внутренней структурой");
                    }
                }
                else 
                {// схема только в одной БД
                    if(db1.schemas.ContainsKey(SchemaName))
                        return new SQLScriptsList(SchemaScript(db1.schemas[SchemaName]), new List<string>(), new List<int>(), "Схема есть только в DB1");
                    else 
                        return new SQLScriptsList(new List<string>(), SchemaScript(db2.schemas[SchemaName]), new List<int>(), "Схема есть только в DB2");

                }
            }
            catch (Exception ex) 
            {
                throw new ComparerException($"При сравнении схем произошла ошибка {ex.Message}");
            }
        }
        private static SQLScriptsList PrepareSQLListTables(DataBase db1, DataBase db2, string TableName)
        {
            try
            {
                if (db1.tables.ContainsKey(TableName) && db2.tables.ContainsKey(TableName))
                {// Обе есть
                    if (db1.tables[TableName].Equals(db2.tables[TableName]))
                    {// одинаковые
                        return new SQLScriptsList(TableScript(db1.tables[TableName]), TableScript(db2.tables[TableName]), new List<int>(), "Таблицы одинаковые");
                    }
                    else
                    {//различаются
                        return TableScript(db1.tables[TableName], db2.tables[TableName]);
                    }
                }
                else
                {// только в одной БД
                    if (db1.tables.ContainsKey(TableName))
                        return new SQLScriptsList(TableScript(db1.tables[TableName]), new List<string>(), new List<int>(), "Таблица есть только в DB1");
                    else
                        return new SQLScriptsList(new List<string>(), TableScript(db2.tables[TableName]), new List<int>(), "Таблица есть только в DB2");

                }
            }
            catch (Exception ex)
            {
                throw new ComparerException($"При сравнении таблиц произошла ошибка {ex.Message}");
            }
        }
        private static SQLScriptsList PrepareSQLListViews(DataBase db1, DataBase db2, string TableName) 
        {
             try
            {
                if (db1.views.ContainsKey(TableName) && db2.views.ContainsKey(TableName))
                {// Обе есть
                    if (db1.views[TableName].Equals(db2.views[TableName]))
                    {// одинаковые
                        return new SQLScriptsList(ViewScript(db1.views[TableName]), ViewScript(db2.views[TableName]), new List<int>(), "Представления одинаковые");
                    }
                    else
                    {//различаются

                        return ViewScript(db1.views[TableName], db2.views[TableName]);
                    }
                }
                else
                {// только в одной БД
                    if (db1.views.ContainsKey(TableName))
                        return new SQLScriptsList(ViewScript(db1.views[TableName]), new List<string>(), new List<int>(), "Представление есть только в DB1");
                    else
                        return new SQLScriptsList(new List<string>(), ViewScript(db2.views[TableName]), new List<int>(), "Представление есть только в DB2");

                }
            }
            catch (Exception ex)
            {
                throw new ComparerException($"При сравнении таблиц произошла ошибка {ex.Message}");
            }
        }
        private static List<string> SchemaScript(Schema schema) 
        {
            List<string> result = new List<string>();
            result.Add($"CREATE SCHEMA [{schema.SchemaName}] AUTHORIZATION [{schema.Owner}]{Environment.NewLine}");
            result.Add($"GO");

            return result;
        }
        private static List<string> TableScript(Table table)
        {
            List<string> result = new List<string>();
            string[] tn = table.TableName.Split('.');
            result.Add($"CREATE TABLE [{tn[0]}].[{tn[1]}]({Environment.NewLine}");
            int iter = 0;
            foreach (var column in table.columns.Values) 
            {
                result.Add(CreateColumnStr(column));
                iter++;
                if (iter < table.columns.Count)
                {
                    result.Add($",{Environment.NewLine}");
                }

            }
            // PrimaryKey
            if (table.PrimaryKey.PkName.Length > 0)
            {
                result.Add(PkForScript(table.PrimaryKey));
            }
            else
            { 
                result.Add($"{Environment.NewLine}"); 
            }
            result.Add($") {Environment.NewLine}GO{Environment.NewLine}");
            // indexes
            Indexes(table, result);
            //ForeignKey
            ForeignKeys(table, result);
            return result;
        }
        private static string PkForScript(PrimaryKey pk)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var col in pk.columns)
            {
                sb.Append($"[{col.Key}] {(col.Value ? "DESC" : "ASC")},");

            }
            //удалим последнюю запятую
            sb.Remove(sb.Length - 1, 1);
            return ($",{Environment.NewLine}{new string(' ', 5)}CONSTRAINT [{pk.PkName}] PRIMARY KEY {pk.Clustered} ({sb.ToString()}){Environment.NewLine}");

        }
        private static SQLScriptsList TableScript(Table table1, Table table2) 
        {
            List<string> result1 = new List<string>();
            List<string> result2 = new List<string>();
            List<int> difs = new List<int>();
            bool ColumnDif = false;
            bool IndexDif = false;
            bool PkDif = false;
            bool FKDif = false;
            
            string[] tn = table1.TableName.Split('.');
            result1.Add($"CREATE TABLE [{tn[0]}].[{tn[1]}]({Environment.NewLine}");
            result2.Add($"CREATE TABLE [{tn[0]}].[{tn[1]}]({Environment.NewLine}");
            if (Comparer.DictEquals(table1.columns,table2.columns))
            {//стлобцы одинаковые по названиям и внутренней структуре
                int iter = 0;
                foreach (var column in table1.columns.Values)
                {
                    result1.Add(CreateColumnStr(column));
                    result2.Add(CreateColumnStr(column));
                    
                    iter++;
                    if (iter < table1.columns.Count)
                    {
                        result1.Add($",{Environment.NewLine}");
                        result2.Add($",{Environment.NewLine}");
                    }
                }
            }
            else
            {
                ColumnDif = true;
                //установим различающиеся названия столбцов
                var defColumnsName = Comparer.GetDifference(new List<string>(table1.columns.Keys), new List<string>(table2.columns.Keys));
                int iter = 0;
                if (defColumnsName.Count > 0)
                {//есть отличающиеся по названию столбцы
                    // сначала выведем отличающиеся по названию
                    
                    foreach (var column in defColumnsName)
                    {
                        if (table1.columns.ContainsKey(column))
                        {
                            result1.Add(CreateColumnStr(table1.columns[column]));
                            iter++;
                            if (iter < table1.columns.Count)
                            {
                                result1.Add($",");
                                result1.Add($"{Environment.NewLine}");
                                result2.Add($"{Environment.NewLine}");
                                
                            }
                            
                        }
                        else 
                        {
                            result2.Add(CreateColumnStr(table2.columns[column]));
                            iter++;
                            if (iter < table2.columns.Count)
                            {
                                result2.Add($",");
                                result1.Add($"{Environment.NewLine}");
                                result2.Add($"{Environment.NewLine}");
                            }
                            
                        }
                        difs.Add(GetCurrentLine(result1));

                    }
                }
                //теперь выведем остальные столбцы
                foreach (var columnKey in table1.columns.Keys) 
                {
                    if (defColumnsName.Contains(columnKey))
                        continue;
                    
                    result1.Add(CreateColumnStr(table1.columns[columnKey]));
                    result2.Add(CreateColumnStr(table2.columns[columnKey]));
                    if (!table1.columns[columnKey].Equals(table2.columns[columnKey]))
                    {//
                        difs.Add(GetCurrentLine(result1));
                    }

                   
                    result1.Add($",");
                    result1.Add($"{Environment.NewLine}");
                    result2.Add($",");
                    result2.Add($"{Environment.NewLine}");
                }
                result1.RemoveAt(result1.Count - 1);
                result1.RemoveAt(result1.Count - 1);
                result2.RemoveAt(result2.Count - 1);
                result2.RemoveAt(result2.Count - 1);
            }

            // PrimaryKey
            bool pkFlag1 = false;
            bool pkFlag2 = false;
            if (table1.PrimaryKey.PkName.Length > 0)
            {
                pkFlag1 = true;
            }
            if (table2.PrimaryKey.PkName.Length > 0)
            {
                pkFlag2 = true;
            }
            //Сравним PK
            if (pkFlag1 && pkFlag2)
            {

                result1.Add(PkForScript(table1.PrimaryKey));
                result2.Add(PkForScript(table2.PrimaryKey));
                if (!table1.PrimaryKey.Equals(table2.PrimaryKey))
                {
                    difs.Add(GetCurrentLine(result1));
                    PkDif = true;
                }
            }
            else if (pkFlag1 || pkFlag2)
            {
                PkDif = true;
                if (pkFlag1)
                {
                    result1.Add(PkForScript(table1.PrimaryKey));
                    result2.Add($"{Environment.NewLine}");
                }
                else
                {
                    result2.Add(PkForScript(table2.PrimaryKey));
                    result2.Add($"{Environment.NewLine}");
                }
                difs.Add(GetCurrentLine(result1));
            }
            else 
            {
                result1.Add($"{Environment.NewLine}");
                result2.Add($"{Environment.NewLine}");
            }
            result1.Add($") {Environment.NewLine}GO{Environment.NewLine}");
            result2.Add($") {Environment.NewLine}GO{Environment.NewLine}");

            /// индексы
            /// 
            if (Comparer.DictEquals(table1.indexes,table2.indexes))
            {//индексы одинаковые
                Indexes(table1, result1);
                Indexes(table2, result2);
            }
            else 
            {
                IndexDif = true;
                //установим различающиеся названия индексы
                var defIndexName = Comparer.GetDifference(new List<string>(table1.indexes.Keys), new List<string>(table2.indexes.Keys));
                if (defIndexName.Count > 0)
                {//есть отличающиеся по названию индексы

                    foreach (var index in defIndexName)
                    {
                        if (table1.indexes.ContainsKey(index))
                        {
                            string ss = createIndexStr(table1.indexes[index], tn);
                            if (ss.Length > 0)
                                result1.Add(ss);
                            else
                                continue;
                            result2.Add($"{Environment.NewLine}");
                            result2.Add($"{Environment.NewLine}");
                            difs.Add(GetCurrentLine(result1));
                        }
                        else
                        {
                            string ss = createIndexStr(table2.indexes[index], tn);
                            if (ss.Length > 0)
                                result2.Add(ss);
                            else
                                continue;
                            result1.Add($"{Environment.NewLine}");
                            result1.Add($"{Environment.NewLine}");
                            difs.Add(GetCurrentLine(result2));
                        }

                    }
                }
                //теперь выведем остальные индексы
                foreach (var indexKey in table1.indexes.Keys)
                {
                    if (defIndexName.Contains(indexKey))
                        continue;
                    string ss = createIndexStr(table1.indexes[indexKey], tn);
                    
                    
                    if (ss.Length > 0)
                        result1.Add(ss);
                    else
                        result1.Add(Environment.NewLine);
                    ss = createIndexStr(table2.indexes[indexKey], tn);
                    if (ss.Length > 0)
                        result2.Add(ss);
                    else
                        result2.Add(Environment.NewLine);
                    if (!table1.indexes[indexKey].Equals(table2.indexes[indexKey]))
                    {//
                        difs.Add(GetCurrentLine(result1));
                    }

                }

            }

            //FK
            if (Comparer.DictEquals(table1.foreignKeys,table2.foreignKeys))
            {//индексы одинаковые
                ForeignKeys(table1, result1);
                ForeignKeys(table2, result2);
            }
            else
            {
                FKDif = true;
                //установим различающиеся названия fk
                var defFKName = Comparer.GetDifference(new List<string>(table1.foreignKeys.Keys), new List<string>(table2.foreignKeys.Keys));
                if (defFKName.Count > 0)
                {//есть отличающиеся по названию индексы

                    foreach (var fk in defFKName)
                    {
                        if (table1.foreignKeys.ContainsKey(fk))
                        {
                            result1.Add(FKForScript(table1.foreignKeys[fk], tn));
                        }
                        else
                        {
                            result2.Add(FKForScript(table2.foreignKeys[fk], tn));
                        }
                        difs.Add(GetCurrentLine(result1));

                    }
                }
                //теперь выведем остальные индексы
                foreach (var fk in table1.foreignKeys.Keys)
                {
                    if (defFKName.Contains(fk))
                        continue;
                   
                    
                    result1.Add(FKForScript(table1.foreignKeys[fk], tn));
                    result2.Add(FKForScript(table2.foreignKeys[fk], tn));
                    if (!table1.foreignKeys[fk].Equals(table2.foreignKeys[fk]))
                    {//
                        difs.Add(GetCurrentLine(result1));
                    }
                }
            }

            StringBuilder statusText = new StringBuilder();
            statusText.Append("Таблицы различаются ");
            if (ColumnDif)
                statusText.Append("столбцами");
            if (PkDif) 
            {
                if (ColumnDif)
                    statusText.Append(", ");
                 statusText.Append(" первичными ключами");
            }
            if (IndexDif) 
            {
                if(ColumnDif || PkDif)
                    statusText.Append(", ");
                statusText.Append(" индексами");
            }
            if (FKDif)
            {
                if (ColumnDif || PkDif || IndexDif)
                    statusText.Append(", ");
                statusText.Append(" связями");
            }

            return new SQLScriptsList(result1,result2,difs,statusText.ToString());
        }
        private static int GetCurrentLine(List<string> list) 
        {
            int minusLine = 0;
            if (list[list.Count - 1].EndsWith("GO" + Environment.NewLine))
                minusLine = 2;
            else if (list[list.Count - 1].EndsWith(Environment.NewLine))
                minusLine = 1;
            else
                minusLine = 0;
            return new Regex(Environment.NewLine).Matches(string.Join("", list.ToArray())).Count - minusLine;
        }
        private static string CreateColumnStr(Column column) 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{new string(' ', 5)}[{column.ColumnName}] ");
            if (column.Definition.Length > 0)
            {
                sb.Append($"AS {column.Definition}");
            }
            else 
            {
                sb.Append($"{ColumnType(column)}");

            }
            return sb.ToString();
        }
        private static string createIndexStr(Index index, string[] tn) 
        {
            string uniq = index.IsUniq ? "UNIQUE" : "";
            switch ((IndexTypeEnum)index.IndexType)
            {
                case IndexTypeEnum.heap:
                    {
                    }
                    break;
                case IndexTypeEnum.clustered_rowsore:
                case IndexTypeEnum.nonclustered_rowsore:
                    {//TODO ------
                        return ($"CREATE {uniq} {index.Clustered} INDEX [{index.IndexName}] ON [{tn[0]}].[{tn[1]}] ({IndexColumnsForScript(index)}){Environment.NewLine}GO{Environment.NewLine}");
                    }
                case IndexTypeEnum.xml_index:
                    {
                        return ($"CREATE PRIMARY XML INDEX [{index.IndexName}] ON [{tn[0]}].[{tn[1]}] ({IndexColumnsForScript(index)})  {Environment.NewLine}GO{Environment.NewLine}");
                    }
                case IndexTypeEnum.spatial_index:
                    {
                        //TODO ------
                    }
                    break;
                case IndexTypeEnum.clustered_columnstore:
                    {
                        //TODO ------
                    }
                    break;
                case IndexTypeEnum.nonclustered_columnstore:
                    {
                        //TODO ------
                    }
                    break;
                default:
                    {
                        //TODO ------
                    }
                    break;

            }
            return "";
        }
        private static List<string> ViewScript(View view)
        {//[{ String.Join("], [", pk_col.ToArray())}]
            List<string> result = new List<string>();
            result.Add($"{view.Script}");
            return result;
        }

        private static SQLScriptsList ViewScript(View view1, View view2) 
        {
            //TODO--
            //проверим отличающиеся столбцы
            var difColumnsName = Comparer.GetDifference(new List<string>(view1.columns.Keys), new List<string>(view2.columns.Keys));
            var difTablesName = Comparer.GetDifference(view1.tables, view2.tables);
            var stText = "";
            if (difColumnsName.Count > 0)
            {// есть отличающиеся по названию столбцы
                stText = "Представления отличаются названинями столбцов";
            }
            if (difTablesName.Count > 0)
            {
                if (difColumnsName.Count > 0)
                {// есть отличающиеся по названию столбцы
                    stText += " и названиями используемых таблиц.";
                }
                else
                {
                    stText = "Представления отличаются названиями используемых таблиц.";
                }
            }
            else 
            {
                stText += ".";
            }

            
            return new SQLScriptsList(ViewScript(view1), ViewScript(view2),new List<int>(), stText);

        }
        private static string ColumnType(Column column) 
        {
            StringBuilder sb = new StringBuilder();
            switch (column.TypeName) 
            {
                case "binary":
                case "datetimeoffset":
                case "time":
                case "varbinary":
                    {
                        string len = -1 == column.MaxLength ? "max" : column.MaxLength.ToString();
                        sb.Append($"[{column.TypeName}]({len})");
                    }
                    break;
                case "datetime2": 
                    {
                        sb.Append($"[{column.TypeName}]({column.Scale})");
                    }
                    break;
                case "char":
                case "nchar":
                case "nvarchar":
                case "varchar": 
                    {
                        string len = -1 == column.MaxSymb ? "max" : column.MaxSymb.ToString();
                        sb.Append($"[{column.TypeName}]({len})");
                    }break;
                case "decimal": 
                    {
                        sb.Append($"[{column.TypeName}]({column.Precision},{column.Scale})");
                    }
                    break;
                default: 
                    {
                        string[] ut = column.TypeName.Split('.');
                        if(ut.Length > 1)
                            sb.Append($"[{ut[0]}].[{ut[1]}]");
                        else
                            sb.Append($"[{column.TypeName}]");
                    }
                    break;
            }
            if (column.CollationName.Length > 0)
            {
                sb.Append($"{new string(' ', 3)}COLLATE {column.CollationName}");
            }
            if (column.IncrementValue > 0) 
            {
                sb.Append($" IDENTITY({column.SeedValue},{column.IncrementValue})");
            }
            if (column.IsNullable)
            {
                sb.Append($"{new string(' ', 1)}NULL");
            }
            else 
            {
                sb.Append($"{new string(' ', 1)}NOT NULL");
            }
            if (column.TypeName.Equals("uniqueidentifier") && column.ColumnName.Equals("rowguid")) 
            {
                sb.Append($"{new string(' ', 1)}ROWGUIDCOL");
            }
            if (column.DefaultVal.Length > 0 && column.ConstraintName.Length > 0) 
            {
                sb.Append($"{new string(' ', 1)}CONSTRAINT [{column.ConstraintName}] {column.DefaultVal}");
            }
                
            return sb.ToString();
        }


        private static void Indexes(Table table, List<string> result) 
        {
            string[] tn = table.TableName.Split('.');
            foreach (var index in table.indexes.Values)
            {
                string ss = createIndexStr(index, tn);
                if(ss.Length > 0)
                    result.Add(ss);
            }
        }
        
        private static void ForeignKeys(Table table, List<string> result)
        {
            string[] tn = table.TableName.Split('.');
            foreach (var fk in table.foreignKeys.Values)
            {
                result.Add(FKForScript(fk,tn));
            }
        }
        private static string IndexColumnsForScript(Index index) 
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var col in index.columns) 
            {
                string desc = col.Value ? "DESC" : "ASC";
                sb.Append($" [{col.Key}] {desc} ");
                i++;
                if (i < index.columns.Count)
                    sb.Append($",");
            }
            return sb.ToString();
        }
        private static string FKForScript(ForeignKey fk, string[] tn) 
        {
            StringBuilder sb = new StringBuilder();
            if ("NO_ACTION" != fk.deleteRef)
            {
                sb.Append($"ON DELETE {fk.deleteRef} ");
            }
            if ("NO_ACTION" != fk.updateRef)
            {
                sb.Append($"ON UPDATE {fk.updateRef} ");
            }
            return $"ALTER TABLE [{tn[0]}].[{tn[1]}] ADD CONSTRAINT [{fk.FKName}] FOREIGN KEY ([{ String.Join("], [", fk.fkColumnName.ToArray())}]) REFERENCES [{String.Join("].[", fk.prTableName.Split('.'))}] ([{ String.Join("], [", fk.pkColumnName.ToArray())}]) {sb.ToString()}{Environment.NewLine}GO{Environment.NewLine}";
        }

    }
}
