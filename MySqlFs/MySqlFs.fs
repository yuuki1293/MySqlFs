namespace MySqlFs

open System.Text
open MySql.Data.MySqlClient

[<AutoOpen>]
module Function =
    type DataBase = DataBase of string
    type DataBaseCreateOut = DataBaseCreateOut of string
    

    type MySql =
        static member open'(connectionString: string) = new MySqlConnection(connectionString)
        
        //CREATE DATABASE
        static member create1 (DataBase database) (conn: MySqlConnection) =
            $"CREATE DATABASE {database} " |> DataBaseCreateOut, conn

        static member create2 (DataBase database) (ifNotExists: bool) (conn: MySqlConnection) =
            let ifNotExistsV =
                if ifNotExists then
                    "IF NOT EXISTS"
                else
                    ""

            $"CREATE DATABASE {ifNotExistsV} {database}"
            |> DataBaseCreateOut,
            conn

        static member defaultCharacterSetS (character: string) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character}"
            |> DataBaseCreateOut,
            conn

        static member defaultCharacterSetE (character: Encoding) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character.BodyName}"
            |> DataBaseCreateOut,
            conn

        static member defaultCollate (collation: string) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> DataBaseCreateOut,
            conn

        static member defaultEncryption (enable: bool) (DataBaseCreateOut command, conn: MySqlConnection) =
            let ins = if enable then "Y" else "N"

            $"{command} DEFAULT ENCRYPTION = '{ins}'"
            |> DataBaseCreateOut,
            conn
        