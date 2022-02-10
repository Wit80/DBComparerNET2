using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    // типы индекса https://docs.microsoft.com/ru-ru/sql/relational-databases/system-catalog-views/sys-indexes-transact-sql?view=sql-server-ver15
    public enum IndexTypeEnum
    {
        heap,                       /*0 = куча*/
        clustered_rowsore,          /*1 = кластеризованный rowstore (сбалансированное дерево)*/
        nonclustered_rowsore,       /*2 = некластеризованный rowstore (сбалансированное дерево)*/
        xml_index,                  /*3 = XML*/
        spatial_index,              /*4 = пространственный*/
        clustered_columnstore,      /*5 = кластеризованный индекс columnstore. Область применения: SQL Server 2014 (12.x) и более поздних версий.*/
        nonclustered_columnstore,   /*6 = некластеризованный индекс columnstore. Область применения: SQL Server 2012 (11.x) и более поздних версий.*/
        nonclustered_hash           /*7 = некластеризованный хэш-индекс. Область применения: SQL Server 2014 (12.x) и более поздних версий.*/
    }
}
