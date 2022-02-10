using DBComparerLibrary.DBSchema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DBComparerLibrary
{
    public class DataProcessor
    {

        private DBsysWorker wrk;

        public DataProcessor(string connString)
        {
            wrk = new DBsysWorker(connString);
        }
        public ddDataBase RunProcess() 
        {
            wrk.GetDataFromDB();

            return null;
            wrk.GetDataFromDB();
            if (0 == wrk.dsObjects.Tables[0].Rows.Count)
            {//прочитано 0 объектов
                throw new ComparerException("Из БД прочитано 0 объектов ");
            }
            ddDataBase db = new ddDataBase(wrk.dsObjects.Tables[0].Rows[0][(int)SQLObjectFieldsEnum.serverName].ToString(), 
                wrk.dsObjects.Tables[0].Rows[0][(int)SQLObjectFieldsEnum.dbName].ToString());
            foreach (DataRow dr in wrk.dsObjects.Tables[0].Rows)
            {
                try
                {
                    RowProcess(db, dr);
                }
                catch (Exception ex) 
                {
                    throw new ComparerException("Ошибка при обработке строки.Тип исключения: " + ex.GetType() + " : " + ex.Message);
                }
                
            }
            return db;


        }

       


        private void RowProcess(ddDataBase db, DataRow dr) 
        {
            // проверка есть ли эта схема
            if (!db.schemas.ContainsKey(dr[(int)SQLObjectFieldsEnum.schemaName].ToString()))
            {//схемы нет, добавляем её
                db.schemas.Add(dr[(int)SQLObjectFieldsEnum.schemaName].ToString(),
                    new ddSchema(dr[(int)SQLObjectFieldsEnum.schemaName].ToString(), Convert.ToInt32(dr[(int)SQLObjectFieldsEnum.schemaId])));
            }
            RowSpliter(db.schemas[dr[(int)SQLObjectFieldsEnum.schemaName].ToString()], dr);
        }
        private void RowSpliter(ddSchema sch, DataRow dr) 
        {
            string objType = dr[(int)SQLObjectFieldsEnum.objectType].ToString().Trim();
            switch (objType) 
            {
                case "U"://таблица (пользовательская)
                    {
                        if (!sch.tables.ContainsKey(dr[(int)SQLObjectFieldsEnum.objectName].ToString()))
                        {//этой таблицы нет
                            UInt64 objId = Convert.ToUInt64(dr[(int)SQLObjectFieldsEnum.objectId]);
                            sch.tables.Add(dr[(int)SQLObjectFieldsEnum.objectName].ToString(),
                                new ddTable(objId, dr[(int)SQLObjectFieldsEnum.objectName].ToString(),
                                wrk.GetRowCount(objId), Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.dtCreate]),
                                Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.dtModif]), wrk.GetColums(objId), wrk.GetIndexes(objId)));
                        }
                    }
                    break;
                case "V"://представление
                    {
                        if (!sch.tables.ContainsKey(dr[(int)SQLObjectFieldsEnum.objectName].ToString()))
                        {//этого представления нет
                            UInt64 objId = Convert.ToUInt64(dr[(int)SQLObjectFieldsEnum.objectId]);
                            sch.views.Add(dr[(int)SQLObjectFieldsEnum.objectName].ToString(),
                                new ddView(objId, dr[(int)SQLObjectFieldsEnum.objectName].ToString(),
                                    Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.dtCreate]),
                                    Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.dtModif])));
                        }
                    }
                    break;
                case "PK"://ограничение PRIMARY KEY
                    {
                        SetConstraint(sch.tables, dr, ddConstraintsTypeEnum.PRIMARY_KEY);
                    }
                    break;
                case "UQ"://ограничение UNIQUE
                    {
                        SetConstraint(sch.tables, dr, ddConstraintsTypeEnum.UNIQUE);
                    }
                    break;
                case "F"://ограничение FOREIGN KEY
                    {
                        SetConstraint(sch.tables, dr, ddConstraintsTypeEnum.FOREIGN_KEY);
                    }
                    break;
                case "C"://ограничение CHECK
                    {
                        SetConstraint(sch.tables, dr, ddConstraintsTypeEnum.CHECK);
                    }
                    break;
                default: 
                    {
                        throw new ComparerException("Неизвестный тип объекта - " + objType);
                    }
            }
        }
        private void SetConstraint(Dictionary<string, ddTable> tbls, DataRow dr, ddConstraintsTypeEnum constrType) 
        {
            string parentObjName = dr[(int)SQLObjectFieldsEnum.tblName].ToString();
            if (!tbls.ContainsKey(parentObjName))
            {//этой таблицы нет
                UInt64 objId = Convert.ToUInt64(dr[(int)SQLObjectFieldsEnum.parentObjId]);
                tbls.Add(parentObjName,
                    new ddTable(objId, parentObjName,
                    wrk.GetRowCount(objId), Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.tblCreate]),
                    Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.tblModify]), wrk.GetColums(objId), wrk.GetIndexes(objId)));
            }
            tbls[parentObjName]
                    .constraints.Add(dr[(int)SQLObjectFieldsEnum.objectName].ToString(),new ddConstraints(dr[(int)SQLObjectFieldsEnum.objectName].ToString(),
                            Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.dtCreate]),
                            Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.dtModif]), constrType));
        }        
    }
}
