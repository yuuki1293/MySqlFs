namespace MySqlFs

open System
open MySql.Data.MySqlClient
open System.Text
open MySqlFs
open MySqlFs.Function

[<AutoOpen>]
module MySqlBuilder =

    type MySqlBuilder() =

        member _.Yield _ = ()

        [<CustomOperation("open'")>]
        member _.Open(_, conn) = MySql.open' (conn)
        
        //CREATE DATABASE
        member _.Run(v: DataBaseCreateOut * MySqlConnection) =
            MySql.runCreateDatabase(v)

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, database: DataBase) = MySql.create1 database v

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, database: DataBase, ifNotExists: bool) =
            MySql.create2 database ifNotExists v

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseCreateOut * MySqlConnection, character: string) =
            MySql.defaultCharacterSetCreateDatabaseS character v

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseCreateOut * MySqlConnection, character: Encoding) =
            MySql.defaultCharacterSetCreateDatabaseE character v

        [<CustomOperation("collate")>]
        member _.Collate(v: DataBaseCreateOut * MySqlConnection, collation: string) = MySql.defaultCollateCreateDatabase collation v

        [<CustomOperation("encryption")>]
        member _.Encryption(v: DataBaseCreateOut * MySqlConnection, enable: bool) = MySql.defaultEncryptionCreateDatabase enable v
        
        //DROP DATABASE
        member _.Run(v: DataBaseDropOut * MySqlConnection) =
            MySql.runDropDatabase(v)
         
        [<CustomOperation("drop")>]
        member _.Drop(v: MySqlConnection, database: DataBase)=
            MySql.drop1 database v
        
        [<CustomOperation("drop")>]
        member _.Drop(v: MySqlConnection, database: DataBase, ifExists:bool)=
            MySql.drop2 database ifExists v
        
        //ALTER DATABASE
        member _.Run(v: DataBaseAlterOut * MySqlConnection) =
            MySql.runAlterDatabase(v)

        [<CustomOperation("alter")>]
        member _.Alter(v: MySqlConnection, database: DataBase) = MySql.alter database v

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseAlterOut * MySqlConnection, character: string) =
            MySql.defaultCharacterSetAlterDatabaseS character v

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseAlterOut * MySqlConnection, character: Encoding) =
            MySql.defaultCharacterSetAlterDatabaseE character v

        [<CustomOperation("collate")>]
        member _.Collate(v: DataBaseAlterOut * MySqlConnection, collation: string) = MySql.defaultCollateSetAlterDatabase collation v

        [<CustomOperation("encryption")>]
        member _.Encryption(v: DataBaseAlterOut * MySqlConnection, enable: bool) = MySql.defaultEncryptionSetAlterDatabase enable v
    let mysql = MySqlBuilder()
