namespace MySqlFs

open Function
open MySqlFs
open MySqlFs.PrivateInterface
open PrivateType

[<AutoOpen>]
module MySqlBuilder =
    type MySqlBuilder(conn: string) =
        member _.Yield _ = ()
        member _.Run(v: IEndExecuteNonQuery) = Run.executeNonQuery v conn

        //CREATE DATABASE
        [<CustomOperation("create")>]
        member _.Create(_: unit, database: DataBase) = Original.createDatabase1 database

        [<CustomOperation("create")>]
        member _.Create(_: unit, database: DataBase, ifNotExists: IfNotExists) =
            Original.createDatabase2 database ifNotExists

        [<CustomOperation("charset")>]
        member _.CharSet(v: IOptDatabaseCreate, character: string) = CharSet.createDatabase character v

        [<CustomOperation("collate")>]
        member _.Collate(v: IOptDatabaseCreate, collation: string) = Collate.createDatabase collation v

        [<CustomOperation("encryption")>]
        member _.Encryption(v: IOptDatabaseCreate, enable: bool) = Encryption.createDatabase enable v

        //DROP DATABASE
        [<CustomOperation("drop")>]
        member _.Drop(_: unit, database: DataBase) = Original.dropDatabase1 database

        [<CustomOperation("drop")>]
        member _.Drop(_: unit, database: DataBase, ifExists: IfExists) =
            Original.dropDatabase2 database ifExists

        //ALTER DATABASE
        [<CustomOperation("alter")>]
        member _.Alter(_: unit, database: DataBase) = Original.alterDatabase database

        [<CustomOperation("charset")>]
        member _.CharSet(v: IOptDatabaseAlter, character: string) = CharSet.alterDatabase character v

        [<CustomOperation("collate")>]
        member _.Collate(v: IOptDatabaseAlter, collation: string) = Collate.alterDatabase collation v

        [<CustomOperation("encryption")>]
        member _.Encryption(v: IOptDatabaseAlter, enable: bool) = Encryption.alterDatabase enable v

        [<CustomOperation("readonly")>]
        member _.ReadOnly(v: IOptDatabaseAlter, readonly: bool) = ReadOnly.alterDatabase readonly v

        //CREATE TABLE
        [<CustomOperation("create")>]
        member _.Create(_: unit, table: Table) = Original.createTable1 table

        [<CustomOperation("create")>]
        member _.Create(_: unit, table: Table, ifNotExists: IfNotExists) = Original.createTable2 table ifNotExists

        [<CustomOperation("create")>]
        member _.Create(_: unit, table: Table, temporary: Temporary) = Original.createTable3 table temporary

        [<CustomOperation("create")>]
        member _.Create(_: unit, table: Table, ifNotExists: IfNotExists, temporary: Temporary) =
            Original.createTable4 table ifNotExists temporary

        [<CustomOperation("create")>]
        member _.Create(_: unit, table: Table, temporary: Temporary, ifNotExists: IfNotExists) =
            Original.createTable4 table ifNotExists temporary

        [<CustomOperation("like")>]
        member _.Like(v: ILikeTableCreate, table: Table) = Like.createTable table v

        [<CustomOperation("cols")>]
        member _.Cols(v: IColsTableCreate, tableCol: TableCol) = Cols.createTable tableCol v

        [<CustomOperation("charset")>]
        member _.CharSet(v: IOptTableCreate, character: string) = CharSet.createTable character v

        [<CustomOperation("collate")>]
        member _.Collate(v: IOptTableCreate, collation: string) = Collate.createTable collation v

        [<CustomOperation("engine")>]
        member _.Engine(v: IOptTableCreate, engineName: string) = Engine.createTable engineName v

        [<CustomOperation("comment")>]
        member _.Comment(v: IOptTableCreate, comment: string) = Comment.createTable comment v

        //DROP TABLE
        [<CustomOperation("drop")>]
        member _.Drop(_: unit, table: Table) = Original.dropTable1 table

        [<CustomOperation("drop")>]
        member _.Drop(_: unit, table: Table, ifExists: IfExists) = Original.dropTable2 table ifExists

        [<CustomOperation("drop")>]
        member _.Drop(_: unit, table: Table, temporary: Temporary) = Original.dropTable3 table temporary

        [<CustomOperation("drop")>]
        member _.Drop(_: unit, table: Table, ifExists: IfExists, temporary: Temporary) =
            Original.dropTable4 table ifExists temporary

        [<CustomOperation("drop")>]
        member _.Drop(_: unit, table: Table, temporary: Temporary, ifExists: IfExists) =
            Original.dropTable4 table ifExists temporary

        //ALTER TABLE
        [<CustomOperation("alter")>]
        member _.Alter(_:unit, table:Table)=
            Original.alterTable table
            
        [<CustomOperation("opt")>]
        member _.Opt(command:IAlterSpecification, tableOptions:TableOption list) =
            Opt.alterTable tableOptions command
    let mysql connection = MySqlBuilder connection
