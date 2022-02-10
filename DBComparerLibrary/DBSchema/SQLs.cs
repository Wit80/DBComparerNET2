using System;
using System.Collections.Generic;
using System.Text;

namespace DBComparerLibrary.DBSchema
{
    static public class SQLs
    {
        public static string GetSQLViews_WithScript()
        {
            return @"
select schema_name(v.schema_id) + '.' + v.name as view_name,
       m.definition
from sys.views v
join sys.sql_modules m 
     on m.object_id = v.object_id";
        }

        public static string GetSQLViews_WithColums()
        {
            return @"
select schema_name(v.schema_id) + '.' + object_name(c.object_id) as view_name,
	   c.name AS column_name,
	   case 
		when t.is_user_defined = 1 then s.name + '.' + type_name(c.user_type_id) 
		else type_name(c.user_type_id)
		end as data_type,
       c.max_length as max_len,
       c.precision as precision,
	   c.scale as scale,
	   columnproperty(c.object_id, c.name, 'Precision') as maxSymbols,
	   c.is_nullable as isnullable	
from sys.columns c
join sys.views v     on v.object_id = c.object_id
join sys.types t on c.user_type_id = t.user_type_id
join sys.schemas s on t.schema_id = s.schema_id ";
        }

        public static string GetSQLSchemas()
        {
            return @"
select s.name as schema_name, 
    u.name as schema_owner
from sys.schemas s
    inner join sys.sysusers u
        on u.uid = s.principal_id";
        }
        public static string GetSQLTables_WithColumns() 
        {
            return @"
select schema_name(tab.schema_id) + '.' + tab.name as table_name, 
    col.name as column_name, 
    case 
		when t.is_user_defined = 1 then s.name + '.' + type_name(col.user_type_id) 
		else type_name(col.user_type_id)
		end as data_type,   
    col.max_length,
    col.precision,
	col.scale as scale,
	columnproperty(col.object_id, col.name, 'Precision') as maxSymbols,
	col.is_nullable as isnullable
from sys.tables as tab
    inner join sys.columns as col on tab.object_id = col.object_id
    left join sys.types as t on col.user_type_id = t.user_type_id
	join sys.schemas s on t.schema_id = s.schema_id";
        }

        public static string GetSQLTables_WithDefaults()
        {
            return @"
select schema_name(t.schema_id) + '.' + t.name table_name,
    col.name as column_name,
    con.definition,
    con.name as constraint_name
from sys.default_constraints con
    left outer join sys.objects t
        on con.parent_object_id = t.object_id
    left outer join sys.all_columns col
        on con.parent_column_id = col.column_id
        and con.parent_object_id = col.object_id";
        }

        public static string GetSQLIndexes()
        {
            return @"
select AAA.table_view as table_view, AAA.index_name as index_name, AAA.columns as columns, AAA.index_type as index_type, AAA.isunique as is_uniq, AAA.PKtype as ind_type, DDD.is_descending_key as is_descending
from
(select i.name as index_name,
    substring(column_names, 1, len(column_names)-1) as columns,
    i.type as index_type,
    i.is_unique as isunique,
	i.is_primary_key as PKtype,
    schema_name(t.schema_id) + '.' + t.name as table_view
from sys.objects t
    inner join sys.indexes i
        on t.object_id = i.object_id
    cross apply (select col.name+ ', '
                    from sys.index_columns ic
                        inner join sys.columns col
                            on ic.object_id = col.object_id
                            and ic.column_id = col.column_id
                    where ic.object_id = t.object_id
                        and ic.index_id = i.index_id
                            order by key_ordinal
                            for xml path ('') ) D (column_names)
where t.is_ms_shipped <> 1
and index_id > 0
) as AAA
INNER JOIN (select schema_name(tab.schema_id) + '.'  + tab.name as table_view, 
    pk.name as pk_name,
    ic.index_column_id as column_id,
    col.name as column_name, 
    tab.name as table_name,
	ic.is_descending_key
from sys.tables tab
    inner join sys.indexes pk
        on tab.object_id = pk.object_id 
    inner join sys.index_columns ic
        on ic.object_id = pk.object_id
        and ic.index_id = pk.index_id
    inner join sys.columns col
        on pk.object_id = col.object_id
        and col.column_id = ic.column_id
) as DDD on AAA.index_name = DDD.pk_name and AAA.table_view = DDD.table_view";
        }

        public static string GetSQLForeignKeys()
        {
            return @"
select fk.name as fk_constraint_name,
	schema_name(fk_tab.schema_id) + '.' + fk_tab.name as foreign_table,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name as primary_table,
    fk_col.name as fk_column_name,
    pk_col.name as pk_column_name,
	fk.delete_referential_action_desc,
	fk.update_referential_action_desc
from sys.foreign_keys fk
    inner join sys.tables fk_tab
        on fk_tab.object_id = fk.parent_object_id
    inner join sys.tables pk_tab
        on pk_tab.object_id = fk.referenced_object_id
    inner join sys.foreign_key_columns fk_cols
        on fk_cols.constraint_object_id = fk.object_id
    inner join sys.columns fk_col
        on fk_col.column_id = fk_cols.parent_column_id
        and fk_col.object_id = fk_tab.object_id
    inner join sys.columns pk_col
        on pk_col.column_id = fk_cols.referenced_column_id
        and pk_col.object_id = pk_tab.object_id";
        }
    }
}
