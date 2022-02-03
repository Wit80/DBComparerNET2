﻿using DBComparerLibrary.DBSchema;
using System;
using System.Collections.Generic;
using System.Data;
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
        public DataBase RunProcess() 
        {
            wrk.GetDataFromDB();
            if (0 == wrk.dsObjects.Tables[0].Rows.Count)
            {//прочитано 0 объектов
            }
            DataBase db = new DataBase(wrk.dsObjects.Tables[0].Rows[0][(int)SQLObjectFieldsEnum.serverName].ToString(), 
                wrk.dsObjects.Tables[0].Rows[0][(int)SQLObjectFieldsEnum.dbName].ToString());
            foreach (DataRow dr in wrk.dsObjects.Tables[0].Rows)
            {
                RowProcess(db, dr);
                
            }
            return db;


        }


        private void RowProcess(DataBase db, DataRow dr) 
        {
            // проверка есть ли эта схема
            if (!db.schemas.ContainsKey(dr[(int)SQLObjectFieldsEnum.schemaName].ToString()))
            {//схемы нет, добавляем её
                db.schemas.Add(dr[(int)SQLObjectFieldsEnum.schemaName].ToString(),
                    new Schema(dr[(int)SQLObjectFieldsEnum.schemaName].ToString(), Convert.ToInt32(dr[(int)SQLObjectFieldsEnum.schemaId])));
            }
            RowSpliter(db.schemas[dr[(int)SQLObjectFieldsEnum.schemaName].ToString()], dr);
        }
        private void RowSpliter(Schema sch, DataRow dr) 
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
                                new Table(objId, dr[(int)SQLObjectFieldsEnum.objectName].ToString(),
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
                                new View(objId, dr[(int)SQLObjectFieldsEnum.objectName].ToString(),
                                    Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.dtCreate]),
                                    Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.dtModif])));
                        }
                    }
                    break;
                case "PK"://ограничение PRIMARY KEY
                    {
                        SetConstraint(sch.tables, dr, ConstraintsTypeEnum.PRIMARY_KEY);
                    }
                    break;
                case "UQ"://ограничение UNIQUE
                    {
                        SetConstraint(sch.tables, dr, ConstraintsTypeEnum.UNIQUE);
                    }
                    break;
                case "F"://ограничение FOREIGN KEY
                    {
                        SetConstraint(sch.tables, dr, ConstraintsTypeEnum.FOREIGN_KEY);
                    }
                    break;
                case "C"://ограничение CHECK
                    {
                        SetConstraint(sch.tables, dr, ConstraintsTypeEnum.CHECK);
                    }
                    break;
                default: 
                    {
                        throw new ComparerException("Неизвестный тип объекта - " + objType);
                    }
            }
        }
        private void SetConstraint(Dictionary<string, Table> tbls, DataRow dr, ConstraintsTypeEnum constrType) 
        {
            string parentObjName = dr[(int)SQLObjectFieldsEnum.tblName].ToString();
            if (!tbls.ContainsKey(parentObjName))
            {//этой таблицы нет
                UInt64 objId = Convert.ToUInt64(dr[(int)SQLObjectFieldsEnum.parentObjId]);
                tbls.Add(parentObjName,
                    new Table(objId, parentObjName,
                    wrk.GetRowCount(objId), Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.tblCreate]),
                    Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.tblModify]), wrk.GetColums(objId), wrk.GetIndexes(objId)));
            }
            tbls[parentObjName]
                    .constraints.Add(new Constraints(dr[(int)SQLObjectFieldsEnum.objectName].ToString(),
                            Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.dtCreate]),
                            Convert.ToDateTime(dr[(int)SQLObjectFieldsEnum.dtModif]), constrType));
        }        
    }
}
