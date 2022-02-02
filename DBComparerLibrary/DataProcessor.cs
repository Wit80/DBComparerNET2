using DBComparerLibrary.DBSchema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DBComparerLibrary
{
    public class DataProcessor
    {
        public void RowProcess(DataBase db, DataRow dr) 
        {
            // проверка есть ли эта схема
            if (!db.schemas.ContainsKey(dr[(int)SQLObjectFieldsEnum.schemaName].ToString()))
            {//схемы нет, добавляем её
                db.schemas.Add(dr[(int)SQLObjectFieldsEnum.schemaName].ToString(),
                    new Schema(dr[(int)SQLObjectFieldsEnum.schemaName].ToString(), Convert.ToInt32(dr[(int)SQLObjectFieldsEnum.schemaId])));
            }
            fun1(db.schemas[dr[(int)SQLObjectFieldsEnum.schemaName].ToString()], dr);
        }
        private void fun1(Schema sch, DataRow dr) 
        {
            switch (dr[(int)SQLObjectFieldsEnum.objectType].ToString()) 
            {
                case "U":
                    {//таблица
                        if (!sch.tables.ContainsKey(dr[(int)SQLObjectFieldsEnum.objectName].ToString())) 
                        {//этой таблицы нет
                            //sch.tables.Add(dr[(int)SQLObjectFieldsEnum.objectName].ToString(),new Table());
                        }
                    }
                    break;
                case "V":
                    { }
                    break;
                case "PK":
                    { }
                    break;
                case "UQ":
                    { }
                    break;
                case "F":
                    { }
                    break;
                case "C":
                    { }
                    break;
                default: 
                    {
                    }break;



            }
        }

        
    }
}
