namespace MySqlFs

open MySql.Data.MySqlClient
open System.Text

[<AutoOpen>]
module MySqlBuilder=
    type MysqlBuilder() =
        member _.Yield _=
            ()
            
        member _.Run (DataBaseCreateOut commandS,conn:MySqlConnection)=
            try
                conn.Open()
                use mutable command = conn.CreateCommand()
                command.CommandText <- commandS
                command.Connection <- conn
                command.ExecuteNonQuery()
                |> Ok
            with
            | x -> Error x
            
        [<CustomOperation("open'")>]
        member _.Open(_,conn)=
            MySql.open'(conn)
        
        [<CustomOperation("create")>]
        member _.Create(v:MySqlConnection,database:DataBase)=
            MySql.create1 database v
        
        [<CustomOperation("create")>]
        member _.Create(v:MySqlConnection,database:DataBase,ifNotExists: bool)=
            MySql.create2 database ifNotExists v 
        
        [<CustomOperation("charset")>]
        member _.CharSet(v:DataBaseCreateOut * MySqlConnection,character:string)=
            MySql.defaultCharacterSetS character v
            
        [<CustomOperation("charset")>]
        member _.CharSet(v:DataBaseCreateOut * MySqlConnection,character:Encoding)=
            MySql.defaultCharacterSetE character v
            
        [<CustomOperation("collate")>]
        member _.Collate(v:DataBaseCreateOut * MySqlConnection,collation: string)=
            MySql.defaultCollate collation v
            
        [<CustomOperation("encryption")>]
        member _.Encryption(v:DataBaseCreateOut * MySqlConnection,enable: bool)=
            MySql.defaultEncryption enable v
        
    let mysql = MysqlBuilder()