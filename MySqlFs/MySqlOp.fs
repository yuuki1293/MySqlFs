﻿namespace MySqlFs

open MySql.Data.MySqlClient
open MySqlFs
open MySqlFs.Function

[<AutoOpen>]
module MySqlBuilder =
    type MySqlBuilder(conn: string) =

        member _.Yield _ = ()

        //CREATE DATABASE
        member _.Run(v: DataBaseCreateOut) =
            Run.createDatabase (v, conn)

        [<CustomOperation("create")>]
        member _.Create(_:unit, database: DataBase) = Original.createDatabase1 database

        [<CustomOperation("create")>]
        member _.Create(_:unit, database: DataBase, ifNotExists: IfNotExists) =
            Original.createDatabase2 database ifNotExists

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseCreateOut, character: string) = CharSet.createDatabase character v

        [<CustomOperation("collate")>]
        member _.Collate(v: DataBaseCreateOut, collation: string) = Collate.createDatabase collation v

        [<CustomOperation("encryption")>]
        member _.Encryption(v: DataBaseCreateOut, enable: bool) = Encryption.createDatabase enable v

        //DROP DATABASE
        member _.Run(v: DataBaseDropOut) = Run.dropDatabase (v, conn)

        [<CustomOperation("drop")>]
        member _.Drop(_:unit, database: DataBase) = Original.drop1 database

        [<CustomOperation("drop")>]
        member _.Drop(_:unit, database: DataBase, ifExists: IfExists) = Original.drop2 database ifExists

        //ALTER DATABASE
        member _.Run(v: DataBaseAlterOut) = Run.alterDatabase (v,conn)

        [<CustomOperation("alter")>]
        member _.Alter(_:unit, database: DataBase) = Original.alter database

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseAlterOut, character: string) = CharSet.alterDatabase character v

        [<CustomOperation("collate")>]
        member _.Collate(v: DataBaseAlterOut, collation: string) = Collate.alterDatabase collation v

        [<CustomOperation("encryption")>]
        member _.Encryption(v: DataBaseAlterOut, enable: bool) = Encryption.alterDatabase enable v

        //CREATE TABLE
        member _.Run(v: INextTableCreateOut) = Run.createTable (v, conn)

        [<CustomOperation("create")>]
        member _.Create(_:unit, table: Table) = Original.createTable1 table

        [<CustomOperation("create")>]
        member _.Create(_:unit, table: Table, ifNotExists: IfNotExists) =
            Original.createTable2 table ifNotExists

        [<CustomOperation("create")>]
        member _.Create(_:unit, table: Table, temporary: Temporary) =
            Original.createTable3 table temporary

        [<CustomOperation("create")>]
        member _.Create(_:unit, table: Table, ifNotExists: IfNotExists, temporary: Temporary) =
            Original.createTable4 table ifNotExists temporary 

        [<CustomOperation("create")>]
        member _.Create(_:unit, table: Table, temporary: Temporary, ifNotExists: IfNotExists) =
            Original.createTable4 table ifNotExists temporary 

        [<CustomOperation("cols")>]
        member _.Cols(v: INextCols, tablecol: TableCol) = Cols.createTable tablecol v

        [<CustomOperation("charset")>]
        member _.CharSet(v: INextTableCreateOut, character: string) = CharSet.createTable character v

        [<CustomOperation("collate")>]
        member _.Collate(v: INextTableCreateOut, collation: string) = Collate.createTable collation v

        [<CustomOperation("engine")>]
        member _.Engine(v: INextTableCreateOut, engineName: string) = Engine.createTable engineName v

        [<CustomOperation("comment")>]
        member _.Comment(v: INextTableCreateOut, comment: string) = Comment.createTable comment v

    let mysql connection = MySqlBuilder connection
