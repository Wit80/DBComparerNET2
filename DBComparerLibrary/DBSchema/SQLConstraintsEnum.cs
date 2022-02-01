using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    public enum SQLConstraintsEnum
    {
        check,
        foreign_key,
        primary_key,
        unique,
        none
    }
}
