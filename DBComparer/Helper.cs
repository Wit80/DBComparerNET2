using DBComparerLibrary.DBSchema;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DBComparer
{
    static class Helper
    {
        public static string AddZero(string text, bool isFirst)
        {
            if (isFirst)
                return text + "/0";
            else
                return "0/" + text;
        }
        public static void AlignLists(List<string> list1, List<string> list2)
        {
            if (list1.Count > list2.Count)
            {
                int iter = list1.Count - list2.Count;
                for (int i = 0; i < iter; i++)
                {
                    list2.Add(Environment.NewLine);
                }
            }
            else if (list1.Count < list2.Count)
            {
                int iter = list2.Count - list2.Count;
                for (int i = 0; i < list2.Count - list1.Count; i++)
                {
                    list1.Add(Environment.NewLine);
                }
            }
        }
        public static string PrepareText(DataBase db, SortedDictionary<int, string> path)
        {
            bool IsTable = true;
            string shemaName = "";
            string sTblViewName = "";
            string sTblItemName = "";
            SQLTableItemEnum sItemEnum = SQLTableItemEnum.column;

            StringBuilder sb = new StringBuilder();
            foreach (var key in path.Keys)
            {
                switch (key)
                {
                    case 0: //корень
                        {
                        }
                        break;
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
                                sb.Append("ОТСУТСТВУЕТ.");
                                return sb.ToString();
                            }

                        }
                        break;
                    case 2:// количество таблиц/вьюх
                        {
                            if (path[key].Contains("Table"))
                                IsTable = true;
                            else
                                IsTable = false;
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
                                    sb.Append($"{new string(' ', 5)}{db.schemas[shemaName].tables[sTblViewName].rowCount} записей," + Environment.NewLine +
                                        $"{new string(' ', 5)}Создана: {db.schemas[shemaName].tables[sTblViewName].dtCreate.ToString("dd-MM-yyyy HH:MM:ss")}");
                                    sb.Append(Environment.NewLine);
                                    sb.Append($"{new string(' ', 5)}Модифицирована:{db.schemas[shemaName].tables[sTblViewName].dtUpdate.ToString("dd-MM-yyyy HH:MM:ss")}");
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
                            if (path[key].Contains("Column"))
                                sItemEnum = SQLTableItemEnum.column;
                            else if (path[key].Contains("Index"))
                                sItemEnum = SQLTableItemEnum.index;
                            else if (path[key].Contains("Constra"))
                                sItemEnum = SQLTableItemEnum.constraint;
                            else
                                return sb.ToString();

                        }
                        break;
                    case 5:// имя столбца или имя индекса или имя ограничения
                        {
                            sTblItemName = GetWithIn(path[key]);
                            switch (sItemEnum)
                            {
                                case SQLTableItemEnum.column:
                                    {
                                        sb.Append($"Столбец:{sTblItemName}");

                                        if (db.schemas[shemaName].tables[sTblViewName].columns.ContainsKey(sTblItemName))
                                        {
                                            Column column = db.schemas[shemaName].tables[sTblViewName].columns[sTblItemName];
                                            string nullable = column.isNullable ? "NULL" : "NOT NULL";
                                            sb.Append(Environment.NewLine);
                                            sb.Append($"{new string(' ', 5)}Тип:[{column.TypeName}]" +
                                                $", Точность:({column.precision}.{column.scale}), max символов: {column.maxSymb}, {nullable}");
                                            sb.Append(Environment.NewLine);
                                            sb.Append(Environment.NewLine);
                                        }
                                        else
                                        {
                                            sb.Append("ОТСУТСТВУЕТ.");
                                            return sb.ToString();
                                        }
                                    }
                                    break;
                                case SQLTableItemEnum.index:
                                    {
                                        sb.Append($"Индекс:{sTblItemName}");

                                        if (db.schemas[shemaName].tables[sTblViewName].indexes.ContainsKey(sTblItemName))
                                        {
                                            Index index = db.schemas[shemaName].tables[sTblViewName].indexes[sTblItemName];
                                            sb.Append(Environment.NewLine);
                                            sb.Append($"{new string(' ', 5)}{index.columns.Count} столбцов" +
                                                $", Создан:{index.dtCreate.ToString("dd-MM-yyyy HH:MM:ss")}");
                                            sb.Append(Environment.NewLine);
                                            sb.Append($"{new string(' ', 5)}Модифицирован:{index.dtUpdate.ToString("dd-MM-yyyy HH:MM:ss")}");
                                            sb.Append(Environment.NewLine);
                                            sb.Append(Environment.NewLine);
                                        }
                                        else
                                        {
                                            sb.Append("ОТСУТСТВУЕТ.");
                                            return sb.ToString();
                                        }
                                    }
                                    break;
                                default:
                                    {
                                        sb.Append($"Ограничение:{sTblItemName}");

                                        if (db.schemas[shemaName].tables[sTblViewName].constraints.ContainsKey(sTblItemName))
                                        {
                                            Constraints constrain = db.schemas[shemaName].tables[sTblViewName].constraints[sTblItemName];
                                            sb.Append(Environment.NewLine);
                                            sb.Append($"{new string(' ', 5)}Type:{constrain.Type}" +
                                                $", Создан:{constrain.dtCreate.ToString("dd-MM-yyyy HH:MM:ss")}");
                                            sb.Append(Environment.NewLine);
                                            sb.Append($"{new string(' ', 5)}Модифицирован:{constrain.dtUpdate.ToString("dd-MM-yyyy HH:MM:ss")}");
                                            sb.Append(Environment.NewLine);
                                            sb.Append(Environment.NewLine);
                                        }
                                        else
                                        {
                                            sb.Append("ОТСУТСТВУЕТ.");
                                            return sb.ToString();
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case 6: //только у индекса имя столбца индекса
                        {
                            string sIndexColumn = GetWithIn(path[key]);
                            sb.Append($"Столбец:{sIndexColumn}");

                            if (db.schemas[shemaName].tables[sTblViewName].indexes[sTblItemName].columns.Contains(sIndexColumn))
                            {

                            }
                            else
                            {
                                sb.Append("ОТСУТСТВУЕТ.");
                                return sb.ToString();
                            }
                        }
                        break;
                    default:
                        { }
                        break;
                }
            }
            return sb.ToString();
        }
        public static string PrepareSQL(DataBase db, SortedDictionary<int, string> path)
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
                        }
                        break;
                    case 1://имя схемы 
                        {
                            shemaName = GetWithIn(path[key]);
                            if (!db.schemas.ContainsKey(shemaName))
                            {
                                return "";
                            }

                        }
                        break;
                    case 2:// количество таблиц/вьюх
                        {
                            if (path[key].Contains("Table"))
                                IsTable = true;
                            else
                                IsTable = false;
                        }
                        break;
                    case 3:// имя таблицы или вьюхи
                        {
                            sTblViewName = GetWithIn(path[key]);
                            if (IsTable)
                            {
                                if (!db.schemas[shemaName].tables.ContainsKey(sTblViewName))
                                    return "";

                                sb.Append($"CREATE TABLE [{shemaName}].[{sTblViewName}] (");
                                sb.Append(Environment.NewLine);
                                foreach (Column col in db.schemas[shemaName].tables[sTblViewName].columns.Values)
                                {
                                    string isNull = col.isNullable ? "NULL" : "NOT NULL";
                                    sb.Append($"{new string(' ', 5)}[{col.Name}]");
                                    sb.Append($"{new string(' ', 10)}[{col.TypeName}]{new string(' ', 5)} {isNull},");
                                    sb.Append(Environment.NewLine);
                                }
                                foreach (Constraints constr in db.schemas[shemaName].tables[sTblViewName].constraints.Values)
                                {
                                    sb.Append($"CONSTRAINT [{constr.Name}] {constr.Type},");
                                    sb.Append(Environment.NewLine);
                                }
                                sb.Append($")");
                                sb.Append(Environment.NewLine);
                                sb.Append($"GO");
                                sb.Append(Environment.NewLine);
                                if (db.schemas[shemaName].tables[sTblViewName].indexes.Count > 0)
                                {
                                    foreach (string indexKey in db.schemas[shemaName].tables[sTblViewName].indexes.Keys)
                                    {
                                        Index index = db.schemas[shemaName].tables[sTblViewName].indexes[indexKey];
                                        sb.Append($"CREATE INDEX [{index.indexName}] ON [{shemaName}].[{sTblViewName}] (");
                                        sb.Append(Environment.NewLine);
                                        foreach (string column in db.schemas[shemaName].tables[sTblViewName].indexes[indexKey].columns)
                                        {
                                            sb.Append($"{new string(' ', 5)}[{column}]");
                                            sb.Append(Environment.NewLine);
                                        }
                                        sb.Append($")");
                                        sb.Append($"GO");
                                        sb.Append(Environment.NewLine);

                                    }
                                    sb.Append($"GO");
                                    sb.Append(Environment.NewLine);
                                }

                            }
                            else
                            {
                                if (!db.schemas[shemaName].views.ContainsKey(sTblViewName))
                                    return "";
                                sb.Append($"CREATE VIEW [{sTblViewName}] ");
                                sb.Append(Environment.NewLine);
                                sb.Append("AS");
                                sb.Append(Environment.NewLine);
                                sb.Append("SELECT");

                            }
                            return sb.ToString();
                        }
                    default:
                        { }
                        break;
                }
            }
            return sb.ToString();
        }
        public static List<List<string>> PrepareSQLList(DataBase db, SortedDictionary<int, string> path)
        {
            bool IsTable = true;
            string shemaName = "";
            string sTblViewName = "";

            List<List<string>> list = new List<List<string>>();
            list.Add(new List<string>());
            list.Add(new List<string>());
            list.Add(new List<string>());
            foreach (var key in path.Keys)
            {
                switch (key)
                {
                    case 0: //корень
                        {
                        }
                        break;
                    case 1://имя схемы 
                        {
                            shemaName = GetWithIn(path[key]);
                            if (!db.schemas.ContainsKey(shemaName))
                            {
                                return list;
                            }

                        }
                        break;
                    case 2:// количество таблиц/вьюх
                        {
                            if (path[key].Contains("Table"))
                                IsTable = true;
                            else
                                IsTable = false;
                        }
                        break;
                    case 3:// имя таблицы или вьюхи
                        {
                            sTblViewName = GetWithIn(path[key]);
                            if (IsTable)
                            {
                                if (!db.schemas[shemaName].tables.ContainsKey(sTblViewName))
                                    return list;

                                list[0].Add($"CREATE TABLE [{shemaName}].[{sTblViewName}] (");
                                list[0].Add(Environment.NewLine);
                                foreach (Column col in db.schemas[shemaName].tables[sTblViewName].columns.Values)
                                {
                                    string isNull = col.isNullable ? "NULL" : "NOT NULL";
                                    list[0].Add($"{new string(' ', 5)}[{col.Name}]{new string(' ', 10)}[{col.TypeName}]{new string(' ', 5)} {isNull},");
                                    list[0].Add(Environment.NewLine);
                                }
                                foreach (Constraints constr in db.schemas[shemaName].tables[sTblViewName].constraints.Values)
                                {
                                    list[1].Add($"CONSTRAINT [{constr.Name}] {constr.Type},");
                                    list[1].Add(Environment.NewLine);
                                }
                                list[1].Add($")");
                                list[1].Add(Environment.NewLine);
                                list[1].Add($"GO");
                                list[1].Add(Environment.NewLine);
                                if (db.schemas[shemaName].tables[sTblViewName].indexes.Count > 0)
                                {
                                    foreach (string indexKey in db.schemas[shemaName].tables[sTblViewName].indexes.Keys)
                                    {
                                        Index index = db.schemas[shemaName].tables[sTblViewName].indexes[indexKey];
                                        list[2].Add($"CREATE INDEX [{index.indexName}] ON [{shemaName}].[{sTblViewName}] (");
                                        list[2].Add(Environment.NewLine);
                                        foreach (string column in db.schemas[shemaName].tables[sTblViewName].indexes[indexKey].columns)
                                        {
                                            list[2].Add($"{new string(' ', 5)}[{column}]");
                                            list[2].Add(Environment.NewLine);
                                        }
                                        list[2].Add($")");
                                        list[2].Add(Environment.NewLine);

                                    }
                                    list[2].Add($"GO");
                                    list[2].Add(Environment.NewLine);
                                }

                            }
                            else
                            {
                                if (!db.schemas[shemaName].views.ContainsKey(sTblViewName))
                                    return list;
                                list[0].Add($"CREATE VIEW [{sTblViewName}] ");
                                list[0].Add(Environment.NewLine);
                                list[0].Add("AS");
                                list[0].Add(Environment.NewLine);
                                list[0].Add("SELECT");

                            }
                            return list;
                        }
                    default:
                        { }
                        break;
                }
            }
            return list;
        }

        public static string GetWithIn(string str)
        {
            List<string> rez = new List<string>();

            Regex pattern =
                new Regex(@"\[(?<val>.*?)\]",
                    RegexOptions.Compiled |
                    RegexOptions.Singleline);

            foreach (Match m in pattern.Matches(str))
                if (m.Success)
                    rez.Add(m.Groups["val"].Value);

            if (rez.Count > 0)
                return rez[0];
            else
                return "";
        }
    }
}
