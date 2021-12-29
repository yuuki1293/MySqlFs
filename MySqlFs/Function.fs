namespace MySqlFs

open MySql.Data.MySqlClient

[<AutoOpen>]
module PublicTypes =
    type DataBase = DataBase of string
    type IfNotExists = IfNotExists
    type IfExists = IfExists
    type Table = Table of string
    type Temporary = Temporary

module Function =
    type DataBaseCreateOut = DataBaseCreateOut of string
    type DataBaseDropOut = DataBaseDropOut of string
    type DataBaseAlterOut = DataBaseAlterOut of string

    type INextCols =
        abstract member value : string

    type INextTableCreateOut =
        abstract member value : string

    type TableCreateOut =
        | TableCreateOut of string
        interface INextTableCreateOut with
            member this.value =
                match this with
                | TableCreateOut x -> x

    type TableCreateOutWithCols =
        | TableCreateOutWithCols of string
        interface INextCols with
            member this.value = this.Value

        interface INextTableCreateOut with
            member this.value = this.Value

        member this.Value =
            match this with
            | TableCreateOutWithCols x -> x

    type TableCol = TableCol of string

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
        static member createDatabase1 (DataBase database) (conn: MySqlConnection) =
            $"CREATE DATABASE {database}" |> DataBaseCreateOut, conn

        static member createDatabase2 (DataBase database) (_: IfNotExists) (conn: MySqlConnection) =
            $"CREATE DATABASE IF NOT EXISTS {database}"
            |> DataBaseCreateOut,
            conn

        static member createTable1 (Table table) (conn: MySqlConnection) =
            $"CREATE TABLE {table}" |> TableCreateOutWithCols, conn

        static member createTable2 (Table table) (_: IfNotExists) (conn: MySqlConnection) =
            $"CREATE TABLE IF NOT EXISTS {table}"
            |> TableCreateOutWithCols,
            conn

        static member createTable3 (Table table) (_: Temporary) (conn: MySqlConnection) =
            $"CREATE TEMPORARY TABLE {table}"
            |> TableCreateOutWithCols,
            conn

        static member createTable4 (Table table) (_: IfNotExists) (_: Temporary) (conn: MySqlConnection) =
            $"CREATE TEMPORARY TABLE IF NOT EXISTS {table}"
            |> TableCreateOutWithCols,
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

        static member createTable (character: string) (command: INextTableCreateOut, conn: MySqlConnection) =
            $"{command.value} DEFAULT CHARACTER SET = {character}"
            |> TableCreateOut,
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

        static member createTable (collation: string) (command: INextTableCreateOut, conn: MySqlConnection) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> TableCreateOut,
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

    type Cols =
        static member createTable (TableCol tablecol) (command: INextCols, conn: MySqlConnection) =
            $"{command.value} {tablecol}" |> TableCreateOut, conn

    type Engine =
        static member createTable (engineName: string) (command: INextTableCreateOut, conn: MySqlConnection) =
            $"{command.value} ENGINE = {engineName}"
            |> TableCreateOut,
            conn

    type Comment =
        static member createTable (comment: string) (command: INextTableCreateOut, conn: MySqlConnection) =
            $"{command.value} COMMENT = {comment}"
            |> TableCreateOut,
            conn

    type Run =
        static member createDatabase(DataBaseCreateOut command, conn: MySqlConnection) =
            Common.runExecuteNonQuery (command, conn)

        static member dropDatabase(DataBaseDropOut command, conn: MySqlConnection) =
            Common.runExecuteNonQuery (command, conn)

        static member alterDatabase(DataBaseAlterOut command, conn: MySqlConnection) =
            Common.runExecuteNonQuery (command, conn)

        static member createTable(command: INextTableCreateOut, conn: MySqlConnection) =
            Common.runExecuteNonQuery (command.value, conn)
