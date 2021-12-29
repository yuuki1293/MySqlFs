namespace MySqlFs

open MySql.Data.MySqlClient
open System.Text
open MySqlFs
open MySqlFs.Function

[<AutoOpen>]
module MySqlBuilder =

    type MySqlBuilder() =

        member _.Yield _ = ()

        static member RunExecuteNonQuery(commandS, conn: MySqlConnection) =
            try
                conn.Open()
                use mutable command = conn.CreateCommand()
                command.CommandText <- commandS
                command.Connection <- conn
                command.ExecuteNonQuery() |> Ok
            with
            | x -> Error x

        [<CustomOperation("open'")>]
        member _.Open(_, conn) = MySql.open' (conn)
        
        //CREATE DATABASE
        member _.Run(v: DataBaseCreateOut * MySqlConnection) =
            let DataBaseCreateOut command, conn = v
            MySqlBuilder.RunExecuteNonQuery(command, conn)

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, database: DataBase) = MySql.create1 database v

        [<CustomOperation("create")>]
        member _.Create(v: MySqlConnection, database: DataBase, ifNotExists: bool) =
            MySql.create2 database ifNotExists v

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseCreateOut * MySqlConnection, character: string) =
            MySql.defaultCharacterSetS character v

        [<CustomOperation("charset")>]
        member _.CharSet(v: DataBaseCreateOut * MySqlConnection, character: Encoding) =
            MySql.defaultCharacterSetE character v

        [<CustomOperation("collate")>]
        member _.Collate(v: DataBaseCreateOut * MySqlConnection, collation: string) = MySql.defaultCollate collation v

        [<CustomOperation("encryption")>]
        member _.Encryption(v: DataBaseCreateOut * MySqlConnection, enable: bool) = MySql.defaultEncryption enable v
        
        //DROP DATABASE
        member _.Run(v: DataBaseDropOut * MySqlConnection) =
            let DataBaseDropOut command, conn = v
            MySqlBuilder.RunExecuteNonQuery(command, conn)
         
        [<CustomOperation("drop")>]
        member _.Drop(v: MySqlConnection, database: DataBase)=
            MySql.drop1 database v
        
        [<CustomOperation("drop")>]
        member _.Drop(v: MySqlConnection, database: DataBase, ifExists:bool)=
            MySql.drop2 database ifExists v
        
    let mysql = MySqlBuilder()
