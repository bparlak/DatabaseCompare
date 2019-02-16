using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace DBTest
{
    class CreateScript//script oluşturma
    {

        public string CreateTableScript(string connectionString, string tableName)
        {
            string script;
            object result;        
            #region table query
            string sql = @"DECLARE @table_name SYSNAME
                            SELECT @table_name = 'dbo."+tableName+@"'

                            DECLARE 
                                  @object_name SYSNAME
                                , @object_id INT

                            SELECT 
                                  @object_name = '[' + s.name + '].[' + o.name + ']'
                                , @object_id = o.[object_id]
                            FROM sys.objects o WITH (NOWAIT)
                            JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
                            WHERE s.name + '.' + o.name = @table_name
                                AND o.[type] = 'U'
                                AND o.is_ms_shipped = 0

                            DECLARE @SQL NVARCHAR(MAX) = ''

                            ;WITH index_column AS 
                            (
                                SELECT 
                                      ic.[object_id]
                                    , ic.index_id
                                    , ic.is_descending_key
                                    , ic.is_included_column
                                    , c.name
                                FROM sys.index_columns ic WITH (NOWAIT)
                                JOIN sys.columns c WITH (NOWAIT) ON ic.[object_id] = c.[object_id] AND ic.column_id = c.column_id
                                WHERE ic.[object_id] = @object_id
                            ),
                            fk_columns AS 
                            (
                                 SELECT 
                                      k.constraint_object_id
                                    , cname = c.name
                                    , rcname = rc.name
                                FROM sys.foreign_key_columns k WITH (NOWAIT)
                                JOIN sys.columns rc WITH (NOWAIT) ON rc.[object_id] = k.referenced_object_id AND rc.column_id = k.referenced_column_id 
                                JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = k.parent_object_id AND c.column_id = k.parent_column_id
                                WHERE k.parent_object_id = @object_id
                            ),
--!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
							ck_columns AS 
                            (
                                 SELECT 
                                      k.[object_id]
                                    , cname = k.definition
                                FROM sys.check_constraints k WITH (NOWAIT)
                                JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = k.parent_object_id AND c.column_id = k.parent_column_id
                                WHERE k.parent_object_id = @object_id
                            )
--!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            SELECT @SQL = 'CREATE TABLE ' + @object_name + CHAR(13) + '(' + CHAR(13) + STUFF((
                                SELECT CHAR(9) + ', [' + c.name + '] ' + 
                                    CASE WHEN c.is_computed = 1
                                        THEN 'AS ' + cc.[definition] 
                                        ELSE UPPER(tp.name) + 
                                            CASE WHEN tp.name IN ('varchar', 'char', 'varbinary', 'binary', 'text')
                                                   THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length AS VARCHAR(5)) END + ')'
                                                 WHEN tp.name IN ('nvarchar', 'nchar')
                                                   THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length / 2 AS VARCHAR(5)) END + ')'
                                                 WHEN tp.name IN ('ntext')
                                                   THEN ' '
                                                 WHEN tp.name IN ('datetime2', 'time2', 'datetimeoffset') 
                                                   THEN '(' + CAST(c.scale AS VARCHAR(5)) + ')'
                                                 WHEN tp.name = 'decimal' 
                                                   THEN '(' + CAST(c.[precision] AS VARCHAR(5)) + ',' + CAST(c.scale AS VARCHAR(5)) + ')'
                                                ELSE ''
                                            END +
                                            CASE WHEN c.collation_name IS NOT NULL THEN ' COLLATE ' + c.collation_name ELSE '' END +
                                            CASE WHEN c.is_nullable = 1 THEN ' NULL' ELSE ' NOT NULL' END +
                                            CASE WHEN dc.[definition] IS NOT NULL THEN ' DEFAULT' + dc.[definition] ELSE '' END + 
                                            CASE WHEN ic.is_identity = 1 THEN ' IDENTITY(' + CAST(ISNULL(ic.seed_value, '0') AS CHAR(1)) + ',' + CAST(ISNULL(ic.increment_value, '1') AS CHAR(1)) + ')' ELSE '' END 
                                    END + CHAR(13)
                                FROM sys.columns c WITH (NOWAIT)
                                JOIN sys.types tp WITH (NOWAIT) ON c.user_type_id = tp.user_type_id
                                LEFT JOIN sys.computed_columns cc WITH (NOWAIT) ON c.[object_id] = cc.[object_id] AND c.column_id = cc.column_id
                                LEFT JOIN sys.default_constraints dc WITH (NOWAIT) ON c.default_object_id != 0 AND c.[object_id] = dc.parent_object_id AND c.column_id = dc.parent_column_id
                                LEFT JOIN sys.identity_columns ic WITH (NOWAIT) ON c.is_identity = 1 AND c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id
                                WHERE c.[object_id] = @object_id
                                ORDER BY c.column_id

                                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, CHAR(9) + ' ')
                                + ISNULL((SELECT CHAR(9) + ', CONSTRAINT [' + k.name + '] PRIMARY KEY (' + 
								--pkbaşlangıç
                                                (SELECT STUFF((
                                                     SELECT ', [' + c.name + '] ' + CASE WHEN ic.is_descending_key = 1 THEN 'DESC' ELSE 'ASC' END
                                                     FROM sys.index_columns ic WITH (NOWAIT)
                                                     JOIN sys.columns c WITH (NOWAIT) ON c.[object_id] = ic.[object_id] AND c.column_id = ic.column_id
                                                     WHERE ic.is_included_column = 0
                                                         AND ic.[object_id] = k.parent_object_id 
                                                         AND ic.index_id = k.unique_index_id     
                                                     FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, ''))
                                        + ')' + CHAR(13)
                                        FROM sys.key_constraints k WITH (NOWAIT)
                                        WHERE k.parent_object_id = @object_id 
                                            AND k.[type] = 'PK'), '') + ')'  + CHAR(13)
                                + ISNULL((SELECT (
                                    SELECT CHAR(13) +
                                         'ALTER TABLE ' + @object_name + ' WITH' 
                                        + CASE WHEN fk.is_not_trusted = 1 
                                            THEN ' NOCHECK' 
                                            ELSE ' CHECK' 
                                          END + 
                                          ' ADD CONSTRAINT [' + fk.name  + '] FOREIGN KEY(' 
                                          + STUFF((
                                            SELECT ', [' + k.cname + ']'
                                            FROM fk_columns k
                                            WHERE k.constraint_object_id = fk.[object_id]
                                            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
                                           + ')' +
                                          ' REFERENCES [' + SCHEMA_NAME(ro.[schema_id]) + '].[' + ro.name + '] ('
                                          + STUFF((
                                            SELECT ', [' + k.rcname + ']'
                                            FROM fk_columns k
                                            WHERE k.constraint_object_id = fk.[object_id]
                                            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
                                           + ')'
                                        + CASE 
                                            WHEN fk.delete_referential_action = 1 THEN ' ON DELETE CASCADE' 
                                            WHEN fk.delete_referential_action = 2 THEN ' ON DELETE SET NULL'
                                            WHEN fk.delete_referential_action = 3 THEN ' ON DELETE SET DEFAULT' 
                                            ELSE '' 
                                          END
                                        + CASE 
                                            WHEN fk.update_referential_action = 1 THEN ' ON UPDATE CASCADE'
                                            WHEN fk.update_referential_action = 2 THEN ' ON UPDATE SET NULL'
                                            WHEN fk.update_referential_action = 3 THEN ' ON UPDATE SET DEFAULT'  
                                            ELSE '' 
                                          END 
                                        + CHAR(13) + 'ALTER TABLE ' + @object_name + ' CHECK CONSTRAINT [' + fk.name  + ']' + CHAR(13)
                                    FROM sys.foreign_keys fk WITH (NOWAIT)
                                    JOIN sys.objects ro WITH (NOWAIT) ON ro.[object_id] = fk.referenced_object_id
                                    WHERE fk.parent_object_id = @object_id
                                    FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)')), '')
																		
--!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
									 + CHAR(13)
                                + ISNULL((SELECT (
                                    SELECT CHAR(13) +
                                         'ALTER TABLE ' + @object_name + ' WITH' 
                                        + CASE WHEN ck.is_not_trusted = 1 
                                            THEN ' NOCHECK' 
                                            ELSE ' CHECK' 
                                          END + 
                                          ' ADD CONSTRAINT [' + ck.name  + '] CHECK(' 
                                          + STUFF((
                                            SELECT ', ' + k.cname 
                                            FROM ck_columns k
                                            WHERE k.[object_id] = ck.[object_id]
                                            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
                                           + ')' +
                                        + CHAR(13) + 'ALTER TABLE ' + @object_name + ' CHECK CONSTRAINT [' + ck.name  + ']' + CHAR(13)
                                    FROM sys.check_constraints ck WITH (NOWAIT)
                                    JOIN sys.objects ro WITH (NOWAIT) ON ro.[object_id] = ck.[object_id]
                                    WHERE ck.parent_object_id = @object_id
                                    FOR XML PATH(N''), TYPE).value('.', 'NVARCHAR(MAX)')), '')									
--!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                                + ISNULL(((SELECT
                                     CHAR(13) + 'CREATE' + CASE WHEN i.is_unique = 1 THEN ' UNIQUE' ELSE '' END 
                                            + ' NONCLUSTERED INDEX [' + i.name + '] ON ' + @object_name + ' (' +
                                            STUFF((
                                            SELECT ', [' + c.name + ']' + CASE WHEN c.is_descending_key = 1 THEN ' DESC' ELSE ' ASC' END
                                            FROM index_column c
                                            WHERE c.is_included_column = 0
                                                AND c.index_id = i.index_id
                                            FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')'  
                                            + ISNULL(CHAR(13) + 'INCLUDE (' + 
                                                STUFF((
                                                SELECT ', [' + c.name + ']'
                                                FROM index_column c
                                                WHERE c.is_included_column = 1
                                                    AND c.index_id = i.index_id
                                                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') + ')', '')  + CHAR(13)
                                    FROM sys.indexes i WITH (NOWAIT)
                                    WHERE i.[object_id] = @object_id
                                        AND i.is_primary_key = 0
                                        AND i.[type] = 2
                                    FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')
                                ), '')	
	                           select @sql";
            #endregion
            result = ExecuteScalar(sql,connectionString);
            script = Convert.ToString(result);
            return script;          
        }      
        public string CreateColumnScript(string connectionString, string tableName, string columnName)
        {
            string script;
            object result;
            #region column script 
            string sql= @"DECLARE 
                        @tableName varchar(100),
                        @columnName varchar(100),
                        @dataType varchar(100),
                        @dataLength varchar(100),
                        @nullable varchar(100),
                        @scale varchar(100),
                        @precision varchar(100),
                        @collation varchar(100),
                        @computedDef varchar(100),
                        @computedName varchar(100),
                        @iscomputed varchar(100),
                        @script NVARCHAR(MAX) = '';

                        SET @tableName = '"+tableName+@"';
                        SET @columnName = '"+columnName+@"';

                        select @dataType = DATA_TYPE, @dataLength = CHARACTER_MAXIMUM_LENGTH, @nullable = IS_NULLABLE, @scale = NUMERIC_SCALE, @precision = NUMERIC_PRECISION, @collation = COLLATION_NAME  from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @tableName and COLUMN_NAME = @columnName

                        SELECT  @computedName=name, @computedDef=definition FROM sys.computed_columns 

                        SELECT @iscomputed=col.[is_computed],@computedDef=comp.[definition] FROM sys.tables tab inner join sys.columns col on tab.[object_id]=col.[object_id] inner join sys.computed_columns comp on tab.[object_id]=comp.[object_id] where tab.name=@tableName and col.name=@columnName and col.column_id=comp.column_id

                        SET @script = 'ALTER TABLE [' + @tableName + '] ADD '+
                        case when @iscomputed='1' 
                        then (+'['+@computedName+']' +' as '+@computedDef) 
                        else(+'[' + @columnName + ']'+' [' + @dataType +']'+
	                        +( case WHEN @dataType IN('varchar', 'char', 'varbinary', 'binary', 'text') THEN '(' + CASE WHEN @dataLength = -1 THEN ' ' ELSE(@dataLength) END + ')'
			                        WHEN @dataType IN('nvarchar', 'nchar') THEN '(' + CASE WHEN @dataLength = -1 THEN ' ' ELSE(@dataLength) END + ')'
			                        WHEN @dataType IN('ntext') THEN ' '
			                        WHEN @dataType IN('datetime2', 'time2', 'datetimeoffset') THEN '(' + (@dataLength) + ')'
			                        WHEN @dataType IN('decimal') THEN '(' + cast(@precision as varchar(5)) + ',' + cast(@scale as varchar(5)) + ')' ELSE ' 'end)--cast dönüşümü olmazsa + rakamlarda matematiksel işlem yapabilir
	                        +(CASE WHEN @collation IS NOT NULL THEN ' COLLATE ' + @collation ELSE '' END )
	                        +(CASE WHEN @nullable = 'YES' then ' NULL' else ' NOT NULL' end) 
	                        )
                        end;

	                        select @script";
            #endregion
            result = ExecuteScalar(sql, connectionString);
            script = Convert.ToString(result);
            return script;
        }
        public string CreateIndexScript(string connectionString, string tableName,string indexName)
        {
            string script;
            object result;
            #region index script sql code
            string sql = @"SET NOCOUNT ON
                            DECLARE
                            @idxTableName SYSNAME,
                            @idxTableID INT,
                            @idxname SYSNAME,
                            @idxid INT,
                            @colCount INT,
                            @colCountMinusIncludedColumns INT,
                            @IxColumn SYSNAME,
                            @IxFirstColumn BIT,
                            @ColumnIDInTable INT,
                            @ColumnIDInIndex INT,
                            @IsIncludedColumn INT,
                            @sIncludeCols VARCHAR(MAX),
                            @sIndexCols VARCHAR(MAX),
                            @sSQL VARCHAR(MAX),
                            @sParamSQL VARCHAR(MAX),
                            @sFilterSQL VARCHAR(MAX),
                            @location SYSNAME,
                            @IndexCount INT,
                            @CurrentIndex INT,
                            @CurrentCol INT,
                            @Name VARCHAR(128),
                            @IsPrimaryKey TINYINT,
                            @Fillfactor INT,
                            @FilterDefinition VARCHAR(MAX),
                            @IsClustered BIT -- used solely for putting information into the result table

                            IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects WHERE id = object_id(N'[tempdb].[dbo].[#IndexSQL]'))
                            DROP TABLE [dbo].[#IndexSQL]

                            CREATE TABLE #IndexSQL
                            ( TableName VARCHAR(128) NOT NULL
                             ,IndexName VARCHAR(128) NOT NULL
                             ,IsClustered BIT NOT NULL
                             ,IsPrimaryKey BIT NOT NULL
                             ,IndexCreateSQL VARCHAR(max) NOT NULL
                            )

                            IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects WHERE id = object_id(N'[tempdb].[dbo].[#IndexListing]'))
                            DROP TABLE [dbo].[#IndexListing]

                            CREATE TABLE #IndexListing
                            (
                            [IndexListingID] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
                            [TableName] SYSNAME COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
                            [ObjectID] INT NOT NULL,
                            [IndexName] SYSNAME COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
                            [IndexID] INT NOT NULL,
                            [IsPrimaryKey] TINYINT NOT NULL,
                            [FillFactor] INT,
                            [FilterDefinition] NVARCHAR(MAX) NULL
                            )

                            IF EXISTS (SELECT * FROM tempdb.dbo.sysobjects WHERE id = object_id(N'[tempdb].[dbo].[#ColumnListing]'))
                            DROP TABLE [dbo].[#ColumnListing]

                            CREATE TABLE #ColumnListing
                            (
                            [ColumnListingID] INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
                            [ColumnIDInTable] INT NOT NULL,
                            [Name] SYSNAME COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
                            [ColumnIDInIndex] INT NOT NULL,
                            [IsIncludedColumn] BIT NULL
                            )

                            INSERT INTO #IndexListing( [TableName], [ObjectID], [IndexName], [IndexID], [IsPrimaryKey], [FILLFACTOR], [FilterDefinition] )
                            SELECT OBJECT_NAME(si.object_id), si.object_id, si.name, si.index_id, si.Is_Primary_Key, si.Fill_Factor, si.filter_definition
                            FROM sys.indexes si
                            LEFT OUTER JOIN information_schema.table_constraints tc ON si.name = tc.constraint_name AND OBJECT_NAME(si.object_id) = tc.table_name
                            WHERE OBJECTPROPERTY(si.object_id, 'IsUserTable') = 1
                            ORDER BY OBJECT_NAME(si.object_id), si.index_id

                            SELECT @IndexCount = @@ROWCOUNT, @CurrentIndex = 1

                            WHILE @CurrentIndex <= @IndexCount
                            BEGIN

                               SELECT @idxTableName = [TableName],
                               @idxTableID = [ObjectID],
                               @idxname = [IndexName],
                               @idxid = [IndexID],
                               @IsPrimaryKey = [IsPrimaryKey],
                               @FillFactor = [FILLFACTOR],
                               @FilterDefinition = [FilterDefinition]
                               FROM #IndexListing
                               WHERE [IndexListingID] = @CurrentIndex

                               -- So - it is either an index or a constraint
                               -- Check if the index is unique
                               IF (@IsPrimaryKey = 1)
                               BEGIN
                                SET @sSQL = 'ALTER TABLE [dbo].[' + @idxTableName + '] ADD CONSTRAINT [' + @idxname + '] PRIMARY KEY '
                                -- Check if the index is clustered
                                IF (INDEXPROPERTY(@idxTableID, @idxname, 'IsClustered') = 0)
                                BEGIN
                                SET @sSQL = @sSQL + 'NON'
                                SET @IsClustered = 0
                                END
                                ELSE
                                BEGIN
                                SET @IsClustered = 1
                                END
                                SET @sSQL = @sSQL + 'CLUSTERED' + CHAR(13) + '(' + CHAR(13)
                               END
                               ELSE
                               BEGIN
                                SET @sSQL = 'CREATE '
                                -- Check if the index is unique
                                IF (INDEXPROPERTY(@idxTableID, @idxname, 'IsUnique') = 1)
                                BEGIN
                                SET @sSQL = @sSQL + 'UNIQUE '
                                END
                                -- Check if the index is clustered
                                IF (INDEXPROPERTY(@idxTableID, @idxname, 'IsClustered') = 1)
                                BEGIN
                                SET @sSQL = @sSQL + 'CLUSTERED '
                                SET @IsClustered = 1
                                END
                                ELSE
                                BEGIN
                                SET @IsClustered = 0
                                END

                                SELECT
                                @sSQL = @sSQL + 'INDEX [' + @idxname + '] ON [dbo].[' + @idxTableName + ']' + CHAR(13) + '(' + CHAR(13),
                                @colCount = 0,
                                @colCountMinusIncludedColumns = 0
                               END

                               -- Get the nuthe mber of cols in the index
                               SELECT @colCount = COUNT(*),
                                      @colCountMinusIncludedColumns = SUM(CASE ic.is_included_column WHEN 0 THEN 1 ELSE 0 END)
                               FROM sys.index_columns ic
                               INNER JOIN sys.columns sc ON ic.object_id = sc.object_id AND ic.column_id = sc.column_id
                               WHERE ic.object_id = @idxtableid AND index_id = @idxid

                               -- Get the file group info
                               SELECT @location = f.[name]
                               FROM sys.indexes i
                               INNER JOIN sys.filegroups f ON i.data_space_id = f.data_space_id
                               INNER JOIN sys.all_objects o ON i.[object_id] = o.[object_id]
                               WHERE o.object_id = @idxTableID AND i.index_id = @idxid

                               -- Get all columns of the index
                               INSERT INTO #ColumnListing( [ColumnIDInTable], [Name], [ColumnIDInIndex],[IsIncludedColumn] )
                               SELECT sc.column_id, sc.name, ic.index_column_id, ic.is_included_column
                               FROM sys.index_columns ic
                               INNER JOIN sys.columns sc ON ic.object_id = sc.object_id AND ic.column_id = sc.column_id
                               WHERE ic.object_id = @idxTableID AND index_id = @idxid
                               ORDER BY ic.index_column_id

                               IF @@ROWCOUNT > 0
                               BEGIN

                               SELECT @CurrentCol = 1
                               SELECT @IxFirstColumn = 1, @sIncludeCols = '', @sIndexCols = ''
   
                               WHILE @CurrentCol <= @ColCount
                               BEGIN
                                  SELECT @ColumnIDInTable = ColumnIDInTable,
                                  @Name = Name,
                                  @ColumnIDInIndex = ColumnIDInIndex,
                                  @IsIncludedColumn = IsIncludedColumn
                                  FROM #ColumnListing
                                  WHERE [ColumnListingID] = @CurrentCol

                                  IF @IsIncludedColumn = 0
                                  BEGIN

                                     SELECT @sIndexCols = CHAR(9) + @sIndexCols + '[' + @Name + '] '

                                     -- Check the sort order of the index cols ????????
                                     IF (INDEXKEY_PROPERTY (@idxTableID,@idxid,@ColumnIDInIndex,'IsDescending')) = 0
                                        BEGIN
                                        SET @sIndexCols = @sIndexCols + ' ASC '
                                        END
                                     ELSE
                                        BEGIN
                                        SET @sIndexCols = @sIndexCols + ' DESC '
                                        END

                                     IF @CurrentCol < @colCountMinusIncludedColumns
                                        BEGIN
                                        SET @sIndexCols = @sIndexCols + ', '
                                        END

                                  END
                                  ELSE
                                  BEGIN
                                     -- Check for any include columns
                                     IF LEN(@sIncludeCols) > 0
                                        BEGIN
                                        SET @sIncludeCols = @sIncludeCols + ','
                                        END

                                     SELECT @sIncludeCols = @sIncludeCols + '[' + @Name + ']'

                                  END

                                     SET @CurrentCol = @CurrentCol + 1
                               END

                               TRUNCATE TABLE #ColumnListing
                               --append to the result
                               IF LEN(@sIncludeCols) > 0
                                  SET @sIndexCols = @sSQL + @sIndexCols + CHAR(13) + ') ' + ' INCLUDE ( ' + @sIncludeCols + ' ) '
                               ELSE
                                  SET @sIndexCols = @sSQL + @sIndexCols + CHAR(13) + ') '

                               -- Add filtering
                               IF @FilterDefinition IS NOT NULL
                                  SET @sFilterSQL = ' WHERE ' + @FilterDefinition + ' ' + CHAR(13)
                               ELSE
                                  SET @sFilterSQL = ''

                               -- Build the options
                               SET @sParamSQL = 'WITH ( PAD_INDEX = '

                               IF INDEXPROPERTY(@idxTableID, @idxname, 'IsPadIndex') = 1
                                  SET @sParamSQL = @sParamSQL + 'ON,'
                               ELSE
                                  SET @sParamSQL = @sParamSQL + 'OFF,'

                               SET @sParamSQL = @sParamSQL + ' ALLOW_PAGE_LOCKS = '


                               IF INDEXPROPERTY(@idxTableID, @idxname, 'IsPageLockDisallowed') = 0
                                  SET @sParamSQL = @sParamSQL + 'ON,'
                               ELSE
                                  SET @sParamSQL = @sParamSQL + 'OFF,'

                               SET @sParamSQL = @sParamSQL + ' ALLOW_ROW_LOCKS = '

                               IF INDEXPROPERTY(@idxTableID, @idxname, 'IsRowLockDisallowed') = 0
                                  SET @sParamSQL = @sParamSQL + 'ON,'
                               ELSE
                                  SET @sParamSQL = @sParamSQL + 'OFF,'


                               SET @sParamSQL = @sParamSQL + ' STATISTICS_NORECOMPUTE = '

                               -- THIS DOES NOT WORK PROPERLY; IsStatistics only says what generated the last set, not what it was set to do.
                               IF (INDEXPROPERTY(@idxTableID, @idxname, 'IsStatistics') = 1)
                                  SET @sParamSQL = @sParamSQL + 'ON'
                               ELSE
                                  SET @sParamSQL = @sParamSQL + 'OFF'

                               -- Fillfactor 0 is actually not a valid percentage on SQL 2008 R2
                               IF ISNULL( @FillFactor, 90 ) <> 0 
                                SET @sParamSQL = @sParamSQL + ' ,FILLFACTOR = ' + CAST( ISNULL( @FillFactor, 90 ) AS VARCHAR(3) )


                               IF (@IsPrimaryKey = 1) -- DROP_EXISTING isn't valid for PK's
                                  BEGIN
                                   SET @sParamSQL = @sParamSQL + ' ) '
                                  END
                               ELSE
                                  BEGIN
                                   SET @sParamSQL = @sParamSQL + ' ,DROP_EXISTING = ON ) '
                                  END

                               SET @sSQL = @sIndexCols + CHAR(13) + @sFilterSQL + CHAR(13) + @sParamSQL

                               -- 2008 R2 allows ON [filegroup] for primary keys as well, negating the old (IF THE INDEX IS NOT A PRIMARY KEY -ADD THIS - ELSE DO NOT) IsPrimaryKey IF statement
                               SET @sSQL = @sSQL + ' ON [' + @location + ']'

                               --PRINT @sIndexCols +CHAR(13)
                               INSERT INTO #IndexSQL (TableName, IndexName, IsClustered, IsPrimaryKey, IndexCreateSQL) VALUES (@idxTableName, @idxName, @IsClustered, @IsPrimaryKey, @sSQL)

                               END

                               SET @CurrentIndex = @CurrentIndex + 1
                            END

                            SELECT IndexCreateSQL FROM #IndexSQL where TableName='"+tableName+@"' and IndexName='"+indexName+@"'
                            ";
            #endregion
            result = ExecuteScalar(sql,connectionString);
            script = Convert.ToString(result);
            return script;
        }
        public void ExecuteQuery(string sql, string connectingString)
        {
            int a;
            SqlConnection con = new SqlConnection(connectingString);
            try
            {
                SqlCommand cmd = new SqlCommand(sql,con);
                con.Open();
               a= cmd.ExecuteNonQuery();
                if(a!=0)MessageBox.Show("Created.");
            }
            catch (SqlException Ex)
            {                                                                                            //soruna çözüm bul!!!!!!!!!!!!!!!
                MessageBox.Show(Ex.ToString());//eğer oluşturulan key referans aldığı tabloyu bulamazsa hata veriyor,tablo ekleniyor fakat ref gerçekleşmiyor
            }
            finally
            {
                con.Close();
            }
        }

        public object ExecuteScalar(string sql,string connectionString)
        {
            object result = new object();
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql,con);
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return result;
        }

    }
}
