namespace MySqlFs

open MySql.Data.MySqlClient
open MySqlFs.PrivateInterface
open PrivateType

module Function =
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
        static member createDatabase1(DataBase database) =
            $"CREATE DATABASE {database}"
            |> NextOptDatabaseCreate

        static member createDatabase2 (DataBase database) (_: IfNotExists) =
            $"CREATE DATABASE IF NOT EXISTS {database}"
            |> NextOptDatabaseCreate

        static member createTable1(Table table) =
            $"CREATE TABLE {table}" |> NextCols'Like'OptTableCreate

        static member createTable2 (Table table) (_: IfNotExists) =
            $"CREATE TABLE IF NOT EXISTS {table}"
            |> NextCols'Like'OptTableCreate

        static member createTable3 (Table table) (_: Temporary) =
            $"CREATE TEMPORARY TABLE {table}"
            |> NextCols'Like'OptTableCreate

        static member createTable4 (Table table) (_: IfNotExists) (_: Temporary) =
            $"CREATE TEMPORARY TABLE IF NOT EXISTS {table}"
            |> NextCols'Like'OptTableCreate

        static member dropDatabase1(DataBase database) =
            $"DROP DATABASE {database} "
            |> NextEndDatabaseDrop

        static member dropDatabase2 (DataBase database) (_: IfExists) =
            $"DROP DATABASE IF EXISTS {database}"
            |> NextEndDatabaseDrop

        static member dropTable1(Table table) =
            $"DROP TABLE {table}" |> NextEndTableDrop

        static member dropTable2 (Table table) (_: IfExists) =
            $"DROP TABLE IF EXISTS {table}"
            |> NextEndTableDrop

        static member dropTable3 (Table table) (_: Temporary) =
            $"DROP TEMPORARY TABLE {table}"
            |> NextEndTableDrop

        static member dropTable4 (Table table) (_: IfExists) (_: Temporary) =
            $"DROP TEMPORARY TABLE IF EXISTS {table}"
            |> NextEndTableDrop

        static member alterDatabase(DataBase database) =
            $"ALTER DATABASE {database}"
            |> NextOptDatabaseAlter

    let inline charSetBuild (command: ^i) (character: string) =
        $"{(^i: (member value : string) command)} DEFAULT CHARACTER SET = {character}"

    type CharSet =
        static member createDatabase (character: string) (command: IOptDatabaseCreate) =
            charSetBuild command character
            |> NextOptDatabaseCreate

        static member alterDatabase (character: string) (command: IOptDatabaseAlter) =
            charSetBuild command character
            |> NextOptDatabaseAlter

        static member createTable (character: string) (command: IOptTableCreate) =
            charSetBuild command character
            |> NextOptTableCreate

    let inline collateBuild (command: ^i) (collation: string) =
        $"{(^i: (member value : string) command)} DEFAULT COLLATE = {collation}"

    type Collate =
        static member createDatabase (collation: string) (command: IOptDatabaseCreate) =
            collateBuild command collation
            |> NextOptDatabaseCreate

        static member alterDatabase (collation: string) (command: IOptDatabaseAlter) =
            collateBuild command collation
            |> NextOptDatabaseAlter

        static member createTable (collation: string) (command: IOptTableCreate) =
            $"{command.value} DEFAULT COLLATE = {collation}"
            |> NextOptTableCreate

    let inline encryptionBuild (command: ^i) (enable: bool) =
        let ins = if enable then "Y" else "N"
        $"{(^i: (member value : string) command)} DEFAULT ENCRYPTION = '{ins}'"

    type Encryption =
        static member createDatabase (enable: bool) (command: IOptDatabaseCreate) =
            encryptionBuild command enable
            |> NextOptDatabaseCreate

        static member alterDatabase (enable: bool) (command: IOptDatabaseAlter) =
            encryptionBuild command enable
            |> NextOptDatabaseAlter

    let inline colsBuild (command: ^i) (tableCol: string) =
        $"{(^i: (member value : string) command)} {tableCol}"

    type Cols =
        static member createTable (TableCol tableCol) (command: IColsTableCreate) =
            colsBuild command tableCol |> NextOptTableCreate

    let inline engineBuild (command: ^i) (engineName: string) =
        $"{(^i: (member value : string) command)} ENGINE = {engineName}"

    type Engine =
        static member createTable (engineName: string) (command: IOptTableCreate) =
            engineBuild command engineName
            |> NextOptTableCreate

    let inline commentBuild (command: ^i) (comment: string) =
        $"{(^i: (member value : string) command)} COMMENT = '{comment}'"

    type Comment =
        static member createTable (comment: string) (command: IOptTableCreate) =
            commentBuild command comment |> NextOptTableCreate

    let inline readOnlyBuild (command: ^i) (readOnly: bool) =
        let readOnlyS = if readOnly then "1" else "0"
        $"{(^i: (member value : string) command)} READ ONLY = {readOnlyS}"

    type ReadOnly =
        static member alterDatabase (readOnly: bool) (command: IOptDatabaseAlter) =
            readOnlyBuild command readOnly
            |> NextOptDatabaseAlter

    let inline likeBuild (command: ^i) (Table table) =
        $"{(^i: (member value : string) command)} LIKE {table}"

    type Like =
        static member createTable (table: Table) (command: ILikeTableCreate) =
            likeBuild command table |> NextEndTableCreate


    type Run =
        static member executeNonQuery (command: IEndExecuteNonQuery) (conn: string) =
            Common.runExecuteNonQuery (command.value, conn)
