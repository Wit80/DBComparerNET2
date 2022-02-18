using DBComparerLibrary.DBSchema;
using DBComparerLibrary.DBSQLExecutor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;

namespace DBComparerLibrary
{
    public class DBsysWorker
    {
        private SortedDictionary<string, View> dictViews;
        private SortedDictionary<string, Schema> dictSchemas;
        private SortedDictionary<string, Table> dictTables;


        private string _connString;


        

        public DBsysWorker(string connString)
        {
            _connString = connString;
        }
        public DataBase GetDataFromDB() 
        {
            try
            {
                GetSysViews();
                GetSysSchemas();
                GetSysTables();
            }
            catch (Exception ex)
            {
                throw new ComparerException(ex.Message);
            }
            string ServerName = "";
            string Database = "";
            foreach (var item in _connString.Split(';')) 
            {
                string str = item.Trim();
                if (str.StartsWith("Server=")) 
                {
                    ServerName = str.Substring(7, str.Length - 7);
                    continue;
                }
                if (str.StartsWith("Initial Catalog="))
                {
                    Database = str.Substring(16, str.Length - 16);
                    continue;
                }
                if (str.StartsWith("database="))
                {
                    Database = str.Substring(9, str.Length - 9);
                }
            }

            return new DataBase(ServerName, Database, dictSchemas, dictTables, dictViews);
        }
        private void GetSysViews()
        {
            try
            {
                dictViews = GetViewsDictionary(execute(SQLs.GetSQLViews_WithScript()), execute(SQLs.GetSQLViews_WithColums()));
            }
            catch (Exception ex)
            {
                throw new ComparerException("Ошибка GetSysViews.Тип исключения: " + ex.GetType() + " : " + ex.Message);
            }
        }
        private void GetSysSchemas()
        {
            try
            {
                dictSchemas = GetSchemasDictionary(execute(SQLs.GetSQLSchemas()));
            }
            catch (Exception ex)
            {
                throw new ComparerException("Ошибка GetSysSchemas. Тип исключения: " + ex.GetType() + " : " + ex.Message);
            }
        }
        private void GetSysTables() 
        {
            try
            {
                dictTables = GetTablesDictionary(execute(SQLs.GetSQLTables_WithColumns(), SQLs.GetSQLIndexes(), SQLs.GetSQLForeignKeys()));

            }
            catch (Exception ex)
            {
                throw new ComparerException("Ошибка GetSysTables. Тип исключения: " + ex.GetType() + " : " + ex.Message);
            }
        }
        
        
        private SortedDictionary<string, View> GetViewsDictionary(DataSet dsScript, DataSet dsColums)
        {
            Dictionary<string, string> scripts = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow dr in dsScript.Tables[0].Rows)
            {
                if (!scripts.ContainsKey(dr[0].ToString()))
                    scripts.Add(dr["view_name"].ToString(), dr["definition"].ToString());
            }
            SortedDictionary<string, View> dictRet = new SortedDictionary<string, View>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow dr in dsColums.Tables[0].Rows)
            {
                string SchemaView = dr["view_name"].ToString();
                if (dictRet.ContainsKey(SchemaView))
                {
                    View tmpV = dictRet[SchemaView];
                    if (dr["referenced_entity_name"].ToString().Length > 0) 
                    {
                        tmpV.tables.Add(dr["referenced_entity_name"].ToString());
                    }
                    else if (!tmpV.columns.ContainsKey(dr["column_name"].ToString()))
                    {
                        tmpV.columns.Add(dr["column_name"].ToString(), new Column(dr["column_name"].ToString(), dr["data_type"].ToString(), Convert.ToInt32(dr["max_len"]),
                                       Convert.ToInt32(dr["precision"]), Convert.ToInt32(dr["scale"]), Convert.ToInt32(dr["maxSymbols"]), 
                                       Convert.ToBoolean(dr["isnullable"]),"","",dr["collation_name"].ToString()));
                    }
                }
                else
                {
                    if (dr["referenced_entity_name"].ToString().Length > 0)
                    {
                        dictRet.Add(SchemaView,
                            new View(SchemaView, scripts[SchemaView],
                               new Dictionary<string, Column>(StringComparer.InvariantCultureIgnoreCase)
                               , new List<string>() { dr["referenced_entity_name"].ToString()}));
                    }
                    else
                    {
                        dictRet.Add(SchemaView,
                            new View(SchemaView, scripts[SchemaView],
                               new Dictionary<string, Column>(StringComparer.InvariantCultureIgnoreCase)
                               {
                               { dr["column_name"].ToString(), new Column(dr["column_name"].ToString(), dr["data_type"].ToString(), Convert.ToInt32(dr["max_len"]),
                                       Convert.ToInt32(dr["precision"]), Convert.ToInt32(dr["scale"]), Convert.ToInt32(dr["maxSymbols"]), Convert.ToBoolean(dr["isnullable"]),
                                       "","",dr["collation_name"].ToString()) }
                               }, new List<string>()));
                    }
                }
            }
            return dictRet;
        }

        private SortedDictionary<string, Schema> GetSchemasDictionary(DataSet dsSchemas)
        {
            SortedDictionary<string, Schema> dictRet = new SortedDictionary<string, Schema>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow dr in dsSchemas.Tables[0].Rows)
            {
                if (!dictRet.ContainsKey(dr[0].ToString()))
                {
                    dictRet.Add(dr["schema_name"].ToString(), new Schema(sch_Name: dr["schema_name"].ToString(), sch_Owner: dr["schema_owner"].ToString()));
                }
            }
            return dictRet;
        }
        private SortedDictionary<string, Table> GetTablesDictionary(DataSet dsTablesItog)
        {
            /**************************************INDEXES*************************************************/
            Dictionary<string, Dictionary<string, Index>> dictIndexes = new Dictionary<string, Dictionary<string, Index>>(StringComparer.InvariantCultureIgnoreCase);
            Dictionary<string, PrimaryKey> dictPK = new Dictionary<string, PrimaryKey>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow dr in dsTablesItog.Tables[1].Rows)
            {
                string SchemaTable = dr["table_view"].ToString();
                if (1 == Convert.ToInt32(dr["PKtype"]))
                { //это PK
                    if (dictPK.ContainsKey(SchemaTable))
                    {
                        if (dictPK[SchemaTable].columns.ContainsKey(dr["column_name"].ToString()))
                            continue;
                        else 
                        {
                            dictPK[SchemaTable].columns.Add(dr["column_name"].ToString(), Convert.ToBoolean(dr["is_descending_key"]));
                        }
                    }
                    else 
                    {
                        dictPK.Add(SchemaTable, new PrimaryKey(dr["index_name"].ToString(), dr["clust"].ToString(),
                            new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase) { { dr["column_name"].ToString(), Convert.ToBoolean(dr["is_descending_key"]) } }));
                    }
                }
                else
                {
                    if (dictIndexes.ContainsKey(SchemaTable))
                    {
                        if (!dictIndexes[SchemaTable].ContainsKey(dr["index_name"].ToString()))
                        {
                            dictIndexes[SchemaTable].Add(dr["index_name"].ToString(),
                                new Index(dr["index_name"].ToString(), Convert.ToInt32(dr["index_type"]), Convert.ToBoolean(dr["isunique"]),  
                                new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase) { { dr["column_name"].ToString(), Convert.ToBoolean(dr["is_descending_key"]) } }, dr["clust"].ToString()));
                        }
                        else
                        {
                            Index index = dictIndexes[SchemaTable][dr["index_name"].ToString()];
                            index.columns.Add(dr["column_name"].ToString(), Convert.ToBoolean(dr["is_descending_key"]));
                        }
                    }
                    else
                    {

                        dictIndexes.Add(SchemaTable,
                            new Dictionary<string, Index>(StringComparer.InvariantCultureIgnoreCase)
                            {
                        { dr[1].ToString(),
                        new Index(dr["index_name"].ToString(), Convert.ToInt32(dr["index_type"]), Convert.ToBoolean(dr["isunique"]), new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase) { { dr["column_name"].ToString(), Convert.ToBoolean(dr["is_descending_key"]) } },dr["clust"].ToString())
                        }
                            });
                    }
                }
            }

            /**************************************FK*************************************************/
            Dictionary<string, Dictionary<string, ForeignKey>> dictFK = new Dictionary<string, Dictionary<string, ForeignKey>>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow dr in dsTablesItog.Tables[2].Rows)
            {
                string SchemaTable = dr["foreign_table"].ToString();
                if (dictFK.ContainsKey(SchemaTable))
                {
                    var fkDict = dictFK[SchemaTable];
                    if (fkDict.ContainsKey(dr["fk_constraint_name"].ToString()))
                    {//этот FK есть
                        if (fkDict[dr["fk_constraint_name"].ToString()].fkColumnName.Contains(dr["fk_column_name"].ToString()) || fkDict[dr["fk_constraint_name"].ToString()].pkColumnName.Contains(dr["pk_column_name"].ToString()))
                        {
                            continue;
                        }
                        fkDict[dr["fk_constraint_name"].ToString()].fkColumnName.Add(dr["fk_column_name"].ToString());
                        fkDict[dr["fk_constraint_name"].ToString()].pkColumnName.Add(dr["pk_column_name"].ToString());
                    }
                    else
                    {
                        fkDict.Add(dr["fk_constraint_name"].ToString(), new ForeignKey(dr["fk_constraint_name"].ToString(), dr["foreign_table"].ToString(), 
                            dr["primary_table"].ToString(), new List<string>() { dr["fk_column_name"].ToString() },
                            new List<string>() { dr["pk_column_name"].ToString() }, dr["delete_referential_action_desc"].ToString(), dr["update_referential_action_desc"].ToString()));
                    }
                }
                else
                {
                    dictFK.Add(SchemaTable, new Dictionary<string, ForeignKey>(StringComparer.InvariantCultureIgnoreCase)
                        {
                        { dr["fk_constraint_name"].ToString(), new ForeignKey(dr["fk_constraint_name"].ToString(), dr["foreign_table"].ToString(), 
                        dr["primary_table"].ToString(), new List<string>() { dr["fk_column_name"].ToString() },
                            new List<string>() { dr["pk_column_name"].ToString() }, dr["delete_referential_action_desc"].ToString(), dr["update_referential_action_desc"].ToString())
                                }

                        }
                    );
                }
            }
            /**************************************TABLES*************************************************/
            SortedDictionary<string, Table> dictRet = new SortedDictionary<string, Table>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow dr in dsTablesItog.Tables[0].Rows)
            {
                string SchemaTable = dr["table_name"].ToString();
                if (dictRet.ContainsKey(dr["table_name"].ToString()))
                {
                    Table tmpT = dictRet[SchemaTable];
                    if (!tmpT.columns.ContainsKey(dr["column_name"].ToString()))
                    {
                        tmpT.columns.Add(dr["column_name"].ToString(),
                            new Column(dr["column_name"].ToString(), dr["data_type"].ToString(), Convert.ToInt32(dr["max_length"]),
                                       Convert.ToInt32(dr["precision"]), Convert.ToInt32(dr["scale"]), Convert.ToInt32(dr["maxSymbols"]), 
                                       Convert.ToBoolean(dr["isnullable"]),dr["defaul"].ToString(), dr["DF_name"].ToString(),
                                       dr["collation_name"].ToString(),Convert.ToInt32(dr["seed_value"]), 
                                       Convert.ToInt32(dr["increment_value"]), dr["definit"].ToString()));
                    }
                }
                else 
                {
                    Dictionary<string, Index> ind;
                    if (dictIndexes.ContainsKey(SchemaTable))
                        ind = dictIndexes[SchemaTable];
                    else
                        ind = new Dictionary<string, Index>(StringComparer.InvariantCultureIgnoreCase);
                    Dictionary<string, ForeignKey> fk;
                    if (dictFK.ContainsKey(SchemaTable))
                        fk = dictFK[SchemaTable];
                    else
                        fk = new Dictionary<string, ForeignKey>(StringComparer.InvariantCultureIgnoreCase);

                    PrimaryKey pk = null;
                    if (dictPK.ContainsKey(SchemaTable))
                        pk = dictPK[SchemaTable];
                    else
                        pk = new PrimaryKey("","",new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase));
                    

                    dictRet.Add(SchemaTable,
                        new Table(SchemaTable, new Dictionary<string, Column>(StringComparer.InvariantCultureIgnoreCase)
                        {
                            { dr["column_name"].ToString(),
                            new Column(dr["column_name"].ToString(), dr["data_type"].ToString(), Convert.ToInt32(dr["max_length"]),
                                       Convert.ToInt32(dr["precision"]), Convert.ToInt32(dr["scale"]), Convert.ToInt32(dr["maxSymbols"]), 
                                       Convert.ToBoolean(dr["isnullable"]),dr["defaul"].ToString(),dr["DF_name"].ToString(),
                                       dr["collation_name"].ToString(),Convert.ToInt32(dr["seed_value"]), 
                                       Convert.ToInt32(dr["increment_value"]),dr["definit"].ToString())}
                            
                        }, ind, fk, pk));
                }
            }
            return dictRet;
        }

        

        private List<string> GetDefaultsFromDict(Dictionary<string, Dictionary<string, List<string>>> defaults, string tableName, string columnName)
        {
            if (defaults.ContainsKey(tableName))
            {
                if (defaults[tableName].ContainsKey(columnName)) 
                {
                    return defaults[tableName][columnName]; 
                }
            }
            return new List<string> { "","" };
        }
        private DataSet execute(string sSQL)
        {
            ISQLGetConnection _connection = new SQLDBConnection(_connString);
            ISQLExecutor _executor = new SQLExecutor();
            return _executor.ExecuteSQL(_connection.GetConnection(), sSQL);
        }
        private DataSet execute(string sSQLTbl1, string sSQLInd, string sSQLFK)
        {
            ISQLGetConnection _connection = new SQLDBConnection(_connString);
            ISQLExecutor _executor = new SQLExecutor();
            return _executor.ExecuteSQL(_connection.GetConnection(), sSQLTbl1, sSQLInd, sSQLFK);
        }


    }
}
