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
                    scripts.Add(dr[0].ToString(), dr[1].ToString());
            }
            SortedDictionary<string, View> dictRet = new SortedDictionary<string, View>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow dr in dsColums.Tables[0].Rows)
            {
                string SchemaView = dr[0].ToString();
                if (dictRet.ContainsKey(SchemaView))
                {
                    View tmpV = dictRet[SchemaView];
                    if (dr[9].ToString().Length > 0) 
                    {
                        tmpV.tables.Add(dr[9].ToString());
                    }
                    else if (!tmpV.columns.ContainsKey(dr[1].ToString()))
                    {
                        tmpV.columns.Add(dr[1].ToString(), new Column(dr[1].ToString(), dr[2].ToString(), Convert.ToInt32(dr[3]),
                                       Convert.ToInt32(dr[4]), Convert.ToInt32(dr[5]), Convert.ToInt32(dr[6]), 
                                       Convert.ToBoolean(dr[7]),"","",dr[8].ToString()));
                    }
                }
                else
                {
                    if (dr[9].ToString().Length > 0)
                    {
                        dictRet.Add(SchemaView,
                            new View(SchemaView, scripts[SchemaView],
                               new Dictionary<string, Column>(StringComparer.InvariantCultureIgnoreCase)
                               , new List<string>() { dr[9].ToString()}));
                    }
                    else
                    {
                        dictRet.Add(SchemaView,
                            new View(SchemaView, scripts[SchemaView],
                               new Dictionary<string, Column>(StringComparer.InvariantCultureIgnoreCase)
                               {
                               { dr[1].ToString(), new Column(dr[1].ToString(), dr[2].ToString(), Convert.ToInt32(dr[3]),
                                       Convert.ToInt32(dr[4]), Convert.ToInt32(dr[5]), Convert.ToInt32(dr[6]), Convert.ToBoolean(dr[7]),
                                       "","",dr[8].ToString()) }
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
                    dictRet.Add(dr[0].ToString(), new Schema(sch_Name: dr[0].ToString(), sch_Owner: dr[1].ToString()));
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
                string SchemaTable = dr[0].ToString();
                if (1 == Convert.ToInt32(dr[5]))
                { //это PK
                    if (dictPK.ContainsKey(SchemaTable))
                    {
                        if (dictPK[SchemaTable].columns.ContainsKey(dr[2].ToString()))
                            continue;
                        else 
                        {
                            dictPK[SchemaTable].columns.Add(dr[2].ToString(), Convert.ToBoolean(dr[6]));
                        }
                    }
                    else 
                    {
                        dictPK.Add(SchemaTable, new PrimaryKey(dr[1].ToString(), dr[7].ToString(),
                            new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase) { { dr[2].ToString(), Convert.ToBoolean(dr[6]) } }));
                    }
                }
                else
                {
                    if (dictIndexes.ContainsKey(SchemaTable))
                    {
                        if (!dictIndexes[SchemaTable].ContainsKey(dr[1].ToString()))
                        {
                            dictIndexes[SchemaTable].Add(dr[1].ToString(),
                                new Index(dr[1].ToString(), Convert.ToInt32(dr[3]), Convert.ToBoolean(dr[4]),  
                                new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase) { { dr[2].ToString(), Convert.ToBoolean(dr[6]) } }, dr[7].ToString()));
                        }
                        else
                        {
                            Index index = dictIndexes[SchemaTable][dr[1].ToString()];
                            index.columns.Add(dr[2].ToString(), Convert.ToBoolean(dr[6]));
                        }
                    }
                    else
                    {

                        dictIndexes.Add(SchemaTable,
                            new Dictionary<string, Index>(StringComparer.InvariantCultureIgnoreCase)
                            {
                        { dr[1].ToString(),
                        new Index(dr[1].ToString(), Convert.ToInt32(dr[3]), Convert.ToBoolean(dr[4]), new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase) { { dr[2].ToString(), Convert.ToBoolean(dr[6]) } },dr[7].ToString())
                        }
                            });
                    }
                }
            }

            /**************************************FK*************************************************/
            Dictionary<string, Dictionary<string, ForeignKey>> dictFK = new Dictionary<string, Dictionary<string, ForeignKey>>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow dr in dsTablesItog.Tables[2].Rows)
            {
                string SchemaTable = dr[1].ToString();
                if (dictFK.ContainsKey(SchemaTable))
                {
                    var fkDict = dictFK[SchemaTable];
                    if (fkDict.ContainsKey(dr[0].ToString()))
                    {//этот FK есть
                        if (fkDict[dr[0].ToString()].fkColumnName.Contains(dr[3].ToString()) || fkDict[dr[0].ToString()].pkColumnName.Contains(dr[4].ToString()))
                        {
                            continue;
                        }
                        fkDict[dr[0].ToString()].fkColumnName.Add(dr[3].ToString());
                        fkDict[dr[0].ToString()].pkColumnName.Add(dr[4].ToString());
                    }
                    else
                    {
                        fkDict.Add(dr[0].ToString(), new ForeignKey(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), new List<string>() { dr[3].ToString() },
                            new List<string>() { dr[4].ToString() }, dr[5].ToString(), dr[6].ToString()));
                    }
                }
                else
                {
                    dictFK.Add(SchemaTable, new Dictionary<string, ForeignKey>(StringComparer.InvariantCultureIgnoreCase)
                        {
                        { dr[0].ToString(), new ForeignKey(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), new List<string>() { dr[3].ToString() },
                            new List<string>() { dr[4].ToString() }, dr[5].ToString(), dr[6].ToString())
                                }

                        }
                    );
                }
            }
            /**************************************TABLES*************************************************/
            SortedDictionary<string, Table> dictRet = new SortedDictionary<string, Table>(StringComparer.InvariantCultureIgnoreCase);
            foreach (DataRow dr in dsTablesItog.Tables[0].Rows)
            {
                string SchemaTable = dr[0].ToString();
                if (dictRet.ContainsKey(dr[0].ToString()))
                {
                    Table tmpT = dictRet[SchemaTable];
                    if (!tmpT.columns.ContainsKey(dr[1].ToString()))
                    {
                        tmpT.columns.Add(dr[1].ToString(),
                            new Column(dr[1].ToString(), dr[2].ToString(), Convert.ToInt32(dr[3]),
                                       Convert.ToInt32(dr[4]), Convert.ToInt32(dr[5]), Convert.ToInt32(dr[6]), Convert.ToBoolean(dr[7]), dr[12].ToString(),
                                       dr[13].ToString(), dr[8].ToString(),Convert.ToInt32(dr[9]), Convert.ToInt32(dr[10]), dr[11].ToString()));
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
                    if(dictPK.ContainsKey(SchemaTable))
                        pk = dictPK[SchemaTable];
                    

                    dictRet.Add(SchemaTable,
                        new Table(SchemaTable, new Dictionary<string, Column>(StringComparer.InvariantCultureIgnoreCase)
                        {
                            { dr[1].ToString(),
                            new Column(dr[1].ToString(), dr[2].ToString(), Convert.ToInt32(dr[3]),
                                       Convert.ToInt32(dr[4]), Convert.ToInt32(dr[5]), Convert.ToInt32(dr[6]), Convert.ToBoolean(dr[7]),dr[12].ToString(),
                                       dr[13].ToString(),dr[8].ToString(),Convert.ToInt32(dr[9]), Convert.ToInt32(dr[10]),dr[11].ToString())}
                            
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
