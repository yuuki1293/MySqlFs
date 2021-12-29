namespace MySqlFs

open MySql.Data.MySqlClient
open MySqlFs
open MySqlFs.Function

[<AutoOpen>]
module MySqlBuilder =
    type MySqlBuilder(conn: string) =

        member _.Yield _ = Common.open' conn

        //CREATE DATABASE
        member _.Run(v: DataBaseCreateOut * MySqlConnection) = Run.createDatabase v

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, database: DataBase) = Original.createDatabase1 database v

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, database: DataBase, ifNotExists: IfNotExists) =
            Original.createDatabase2 database ifNotExists v

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseCreateOut * MySqlConnection, character: string) = CharSet.createDatabase character v

        [<CustomOperation("collate")>]
        member _.Collate(v: DataBaseCreateOut * MySqlConnection, collation: string) = Collate.createDatabase collation v

        [<CustomOperation("encryption")>]
        member _.Encryption(v: DataBaseCreateOut * MySqlConnection, enable: bool) = Encryption.createDatabase enable v

        //DROP DATABASE
        member _.Run(v: DataBaseDropOut * MySqlConnection) = Run.dropDatabase v

        [<CustomOperation("drop")>]
        member _.Drop(v: MySqlConnection, database: DataBase) = Original.drop1 database v

        [<CustomOperation("drop")>]
        member _.Drop(v: MySqlConnection, database: DataBase, ifExists: IfExists) = Original.drop2 database ifExists v

        //ALTER DATABASE
        member _.Run(v: DataBaseAlterOut * MySqlConnection) = Run.alterDatabase v

        [<CustomOperation("alter")>]
        member _.Alter(v: MySqlConnection, database: DataBase) = Original.alter database v

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseAlterOut * MySqlConnection, character: string) = CharSet.alterDatabase character v

        [<CustomOperation("collate")>]
        member _.Collate(v: DataBaseAlterOut * MySqlConnection, collation: string) = Collate.alterDatabase collation v

        [<CustomOperation("encryption")>]
        member _.Encryption(v: DataBaseAlterOut * MySqlConnection, enable: bool) = Encryption.alterDatabase enable v

        //CREATE TABLE
        member _.Run(v: INextTableCreateOut * MySqlConnection) = Run.createTable v

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, table: Table) = Original.createTable1 table v

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, table: Table, ifNotExists: IfNotExists) =
            Original.createTable2 table ifNotExists v

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, table: Table, temporary: Temporary) =
            Original.createTable3 table temporary v

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, table: Table, ifNotExists: IfNotExists, temporary: Temporary) =
            Original.createTable4 table ifNotExists temporary v

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, table: Table, temporary: Temporary, ifNotExists: IfNotExists) =
            Original.createTable4 table ifNotExists temporary v

        [<CustomOperation("cols")>]
        member _.Cols(v: INextCols * MySqlConnection, tablecol: TableCol) = Cols.createTable tablecol v

        [<CustomOperation("charset")>]
        member _.CharSet(v: INextTableCreateOut * MySqlConnection, character: string) = CharSet.createTable character v

        [<CustomOperation("collate")>]
        member _.Collate(v: INextTableCreateOut * MySqlConnection, collation: string) = Collate.createTable collation v

        [<CustomOperation("engine")>]
        member _.Engine(v: INextTableCreateOut * MySqlConnection, engineName: string) = Engine.createTable engineName v

        [<CustomOperation("comment")>]
        member _.Comment(v: INextTableCreateOut * MySqlConnection, comment: string) = Comment.createTable comment v

    let mysql connection = MySqlBuilder connection
