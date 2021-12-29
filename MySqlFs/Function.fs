﻿namespace MySqlFs

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

        static member runExecuteNonQuery(commandS: string, connS: string) =
            let conn = Common.open' connS

            try
                conn.Open()
                use mutable command = conn.CreateCommand()
                command.CommandText <- commandS
                command.Connection <- conn
                command.ExecuteNonQuery() |> Ok
            with
            | x -> Error x

    type Original =
        static member createDatabase1 (DataBase database) =
            $"CREATE DATABASE {database}" |> DataBaseCreateOut

        static member createDatabase2 (DataBase database) (_: IfNotExists) =
            $"CREATE DATABASE IF NOT EXISTS {database}"
            |> DataBaseCreateOut

        static member createTable1 (Table table) =
            $"CREATE TABLE {table}" |> TableCreateOutWithCols

        static member createTable2 (Table table) (_: IfNotExists) =
            $"CREATE TABLE IF NOT EXISTS {table}"
            |> TableCreateOutWithCols

        static member createTable3 (Table table) (_: Temporary) =
            $"CREATE TEMPORARY TABLE {table}"
            |> TableCreateOutWithCols

        static member createTable4 (Table table) (_: IfNotExists) (_: Temporary)=
            $"CREATE TEMPORARY TABLE IF NOT EXISTS {table}"
            |> TableCreateOutWithCols

        static member drop1 (DataBase database)=
            $"DROP DATABASE {database} " |> DataBaseDropOut

        static member drop2 (DataBase database) (_: IfExists) =
            $"DROP DATABASE IF EXISTS {database}"
            |> DataBaseDropOut

        static member alter (DataBase database)=
            $"ALTER DATABASE {database}" |> DataBaseAlterOut

    type CharSet =
        static member createDatabase (character: string) (DataBaseCreateOut command) =
            $"{command} DEFAULT CHARACTER SET = {character}"
            |> DataBaseCreateOut

        static member alterDatabase (character: string) (DataBaseAlterOut command) =
            $"{command} DEFAULT CHARACTER SET = {character}"
            |> DataBaseAlterOut

        static member createTable (character: string) (command: INextTableCreateOut) =
            $"{command.value} DEFAULT CHARACTER SET = {character}"
            |> TableCreateOut

    type Collate =
        static member createDatabase (collation: string) (DataBaseCreateOut command) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> DataBaseCreateOut

        static member alterDatabase (collation: string) (DataBaseAlterOut command) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> DataBaseAlterOut

        static member createTable (collation: string) (command: INextTableCreateOut) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> TableCreateOut

    type Encryption =
        static member createDatabase (enable: bool) (DataBaseCreateOut command) =
            let ins = if enable then "Y" else "N"

            $"{command} DEFAULT ENCRYPTION = '{ins}'"
            |> DataBaseCreateOut

        static member alterDatabase (enable: bool) (DataBaseAlterOut command) =
            let ins = if enable then "Y" else "N"

            $"{command} DEFAULT ENCRYPTION = '{ins}'"
            |> DataBaseAlterOut

    type Cols =
        static member createTable (TableCol tablecol) (command: INextCols) =
            $"{command.value} {tablecol}" |> TableCreateOut

    type Engine =
        static member createTable (engineName: string) (command: INextTableCreateOut) =
            $"{command.value} ENGINE = {engineName}"
            |> TableCreateOut

    type Comment =
        static member createTable (comment: string) (command: INextTableCreateOut) =
            $"{command.value} COMMENT = '{comment}'"
            |> TableCreateOut

    type Run =
        static member createDatabase(DataBaseCreateOut command, conn: string) =
            Common.runExecuteNonQuery (command, conn)

        static member dropDatabase(DataBaseDropOut command, conn: string) =
            Common.runExecuteNonQuery (command, conn)

        static member alterDatabase(DataBaseAlterOut command, conn: string) =
            Common.runExecuteNonQuery (command, conn)

        static member createTable(command: INextTableCreateOut, conn: string) =
            Common.runExecuteNonQuery (command.value, conn)
