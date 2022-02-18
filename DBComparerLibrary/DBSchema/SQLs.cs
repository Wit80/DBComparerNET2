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
select distinct schema_name(v.schema_id) + '.' + v.name as view_name,
	'' as column_name,
	 '' data_type,
       '' as max_len,
       '' as precision,
	   '' as scale,
	   '' as maxSymbols,
	   '' as isnullable,
	   '' as collation_name,
       schema_name(o.schema_id) + '.' + o.name as referenced_entity_name,
       o.type_desc as entity_type
from sys.views v
join sys.sql_expression_dependencies d
     on d.referencing_id = v.object_id
     and d.referenced_id is not null
join sys.objects o
     on o.object_id = d.referenced_id
union all
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
	   c.is_nullable as isnullable,
	   t.collation_name as collation_name,
	   '' as referenced_entity_name,
       '' as entity_type
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
  col.is_nullable as isnullable,
  t.collation_name as collation_name,
  case when sk.seed_value is NULL then 0
        else sk.seed_value end as seed_value,
  case when sk.increment_value is NULL then 0
        else sk.increment_value end as increment_value,
    c.definition as definit,
  db.text as defaul
from sys.tables as tab
    inner join sys.columns as col on tab.object_id = col.object_id
    left join sys.types as t on col.user_type_id = t.user_type_id
  inner join sys.schemas as s on t.schema_id = s.schema_id
  left join sys.identity_columns as sk on tab.object_id = sk.object_id and col.name = sk.name
  left join sys.objects o ON o.object_id = tab.object_id
  left join sys.computed_columns c ON o.object_id = c.object_id and c.name = col.name
  left join syscomments db  with (nolock) on db.id = col.default_object_id";
        }


        public static string GetSQLIndexes()
        {
            return @"
select schema_name(tab.schema_id) + '.'  + tab.name as table_view, 
    pk.name as index_name,
    col.name as column_name, 
	pk.type as index_type,
	pk.is_unique as isunique,
	pk.is_primary_key as PKtype,
    ic.is_descending_key,
	pk.type_desc as clust
from sys.indexes pk
    inner join sys.tables tab
        on tab.object_id = pk.object_id 
    inner join sys.index_columns ic
        on ic.object_id = pk.object_id
        and ic.index_id = pk.index_id
    inner join sys.columns col
        on pk.object_id = col.object_id
        and col.column_id = ic.column_id
where pk.type in (1,2)";
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
