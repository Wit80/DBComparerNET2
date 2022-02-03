using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public enum ConstraintsTypeEnum
    {
        PRIMARY_KEY,    /*PK*/
        UNIQUE,         /*UQ*/
        FOREIGN_KEY,    /*F*/
        CHECK,          /*C*/
        NONE
    }
}
