using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    // порядок полей в DataSet Objects
    public enum SQLObjectFieldsEnum
    {
        serverName = 0,
        dbName,
        objectName,
        objectId,
        schemaId,
        schemaName,
        objectType,
        dtCreate,
        dtModif,
        parentObjId,
        tblName,
        tblCreate,
        tblModify
    }
}
