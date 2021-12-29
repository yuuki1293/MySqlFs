namespace MySqlFs

open MySql.Data.MySqlClient

[<AutoOpen>]
module PublicTypes =
    type DataBase = DataBase of string
    type IfNotExists = IfNotExists
    type IfExists = IfExists

module Function =
    type DataBaseCreateOut = DataBaseCreateOut of string
    type DataBaseDropOut = DataBaseDropOut of string
    type DataBaseAlterOut = DataBaseAlterOut of string

    type Common =
        static member open'(connectionString: string) = new MySqlConnection(connectionString)

        static member runExecuteNonQuery(commandS, conn: MySqlConnection) =
            try
                conn.Open()
                use mutable command = conn.CreateCommand()
                command.CommandText <- commandS
                command.Connection <- conn
                command.ExecuteNonQuery() |> Ok
            with
            | x -> Error x

    type Original =
        static member create1 (DataBase database) (conn: MySqlConnection) =
            $"CREATE DATABASE {database} "
            |> DataBaseCreateOut,
            conn

        static member create2 (DataBase database) (_: IfNotExists) (conn: MySqlConnection) =
            $"CREATE DATABASE IF NOT EXISTS {database}"
            |> DataBaseCreateOut,
            conn

        static member drop1 (DataBase database) (conn: MySqlConnection) =
            $"DROP DATABASE {database} " |> DataBaseDropOut, conn

        static member drop2 (DataBase database) (_: IfExists) (conn: MySqlConnection) =
            $"DROP DATABASE IF EXISTS {database}"
            |> DataBaseDropOut,
            conn

        static member alter (DataBase database) (conn: MySqlConnection) =
            $"ALTER DATABASE {database}" |> DataBaseAlterOut, conn

    type CharSet =
        static member createDatabase (character: string) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character}"
            |> DataBaseCreateOut,
            conn

        static member alterDatabase (character: string) (DataBaseAlterOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character}"
            |> DataBaseAlterOut,
            conn

    type Collate =
        static member createDatabase (collation: string) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> DataBaseCreateOut,
            conn

        static member alterDatabase (collation: string) (DataBaseAlterOut command, conn: MySqlConnection) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> DataBaseAlterOut,
            conn

    type Encryption =
        static member createDatabase (enable: bool) (DataBaseCreateOut command, conn: MySqlConnection) =
            let ins = if enable then "Y" else "N"

            $"{command} DEFAULT ENCRYPTION = '{ins}'"
            |> DataBaseCreateOut,
            conn

        static member alterDatabase (enable: bool) (DataBaseAlterOut command, conn: MySqlConnection) =
            let ins = if enable then "Y" else "N"

            $"{command} DEFAULT ENCRYPTION = '{ins}'"
            |> DataBaseAlterOut,
            conn

    type Run =
        static member createDatabase(DataBaseCreateOut command, conn: MySqlConnection) =
            Common.runExecuteNonQuery (command, conn)

        static member dropDatabase(DataBaseDropOut command, conn: MySqlConnection) =
            Common.runExecuteNonQuery (command, conn)

        static member runAlterDatabase(DataBaseAlterOut command, conn: MySqlConnection) =
            Common.runExecuteNonQuery (command, conn)
