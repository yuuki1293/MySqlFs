namespace MySqlFs

open System.Text
open MySql.Data.MySqlClient

[<AutoOpen>]
module PublicTypes =
    type DataBase = DataBase of string

module Function =
    type DataBaseCreateOut = DataBaseCreateOut of string
    type DataBaseDropOut = DataBaseDropOut of string
    type DataBaseAlterOut = DataBaseAlterOut of string

    type MySql =
        static member open'(connectionString: string) = new MySqlConnection(connectionString)

        //CREATE DATABASE
        static member create1 (DataBase database) (conn: MySqlConnection) =
            $"CREATE DATABASE {database} "
            |> DataBaseCreateOut,
            conn

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

        //DROP DATABASE
        static member drop1 (DataBase database) (conn: MySqlConnection) =
            $"DROP DATABASE {database} " |> DataBaseDropOut, conn

        static member drop2 (DataBase database) (ifExists: bool) (conn: MySqlConnection) =
            let ifExistsV = if ifExists then "IF EXISTS" else ""

            $"DROP DATABASE {ifExistsV} {database}"
            |> DataBaseDropOut,
            conn
