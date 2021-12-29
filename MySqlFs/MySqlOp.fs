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
        member _.Create(v: MySqlConnection, database: DataBase) = Original.create1 database v

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, database: DataBase, ifNotExists: IfNotExists) =
            Original.create2 database ifNotExists v

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseCreateOut * MySqlConnection, character: string) = CharSet.createDatabase character v

        [<CustomOperation("collate")>]
        member _.Collate(v: DataBaseCreateOut * MySqlConnection, collation: string) = Collate.createDatabase collation v

        [<CustomOperation("encryption")>]
        member _.Encryption(v: DataBaseCreateOut * MySqlConnection, enable: bool) = Encryption.createDatabase enable v

        //DROP DATABASE
        member _.Run(v: DataBaseDropOut * MySqlConnection) = Run.dropDatabase (v)

        [<CustomOperation("drop")>]
        member _.Drop(v: MySqlConnection, database: DataBase) = Original.drop1 database v

        [<CustomOperation("drop")>]
        member _.Drop(v: MySqlConnection, database: DataBase, ifExists: IfExists) = Original.drop2 database ifExists v

        //ALTER DATABASE
        member _.Run(v: DataBaseAlterOut * MySqlConnection) = Run.runAlterDatabase (v)

        [<CustomOperation("alter")>]
        member _.Alter(v: MySqlConnection, database: DataBase) = Original.alter database v

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseAlterOut * MySqlConnection, character: string) = CharSet.alterDatabase character v

        [<CustomOperation("collate")>]
        member _.Collate(v: DataBaseAlterOut * MySqlConnection, collation: string) = Collate.alterDatabase collation v

        [<CustomOperation("encryption")>]
        member _.Encryption(v: DataBaseAlterOut * MySqlConnection, enable: bool) = Encryption.alterDatabase enable v

    let mysql connection= MySqlBuilder connection
