namespace MySqlFs

open System
open System.Text
open MySql.Data.MySqlClient

[<AutoOpen>]
module PublicTypes =
    type DataBase = DataBase of string

module Function =
    type DataBaseCreateOut = DataBaseCreateOut of string
    type DataBaseDropOut = DataBaseDropOut of string
    type DataBaseAlterOut = DataBaseAlterOut of string
    
    type Common=
        static member open'(connectionString: string) = new MySqlConnection(connectionString)
        
        static member RunExecuteNonQuery(commandS, conn: MySqlConnection) =
            try
                conn.Open()
                use mutable command = conn.CreateCommand()
                command.CommandText <- commandS
                command.Connection <- conn
                command.ExecuteNonQuery() |> Ok
            with
            | x -> Error x
           
    type MySql =
        [<Obsolete("Use Common.open' instead")>]
        static member open'(connectionString: string) = new MySqlConnection(connectionString)
        
        [<Obsolete("Use Common.RunExecuteNonQuery instead")>]
        static member RunExecuteNonQuery(commandS, conn: MySqlConnection) =
            try
                conn.Open()
                use mutable command = conn.CreateCommand()
                command.CommandText <- commandS
                command.Connection <- conn
                command.ExecuteNonQuery() |> Ok
            with
            | x -> Error x
        
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

        static member defaultCharacterSetCreateDatabaseS (character: string) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character}"
            |> DataBaseCreateOut,
            conn

        static member defaultCharacterSetCreateDatabaseE (character: Encoding) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character.BodyName}"
            |> DataBaseCreateOut,
            conn

        static member defaultCollateCreateDatabase (collation: string) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> DataBaseCreateOut,
            conn

        static member defaultEncryptionCreateDatabase (enable: bool) (DataBaseCreateOut command, conn: MySqlConnection) =
            let ins = if enable then "Y" else "N"

            $"{command} DEFAULT ENCRYPTION = '{ins}'"
            |> DataBaseCreateOut,
            conn
        
        static member runCreateDatabase(DataBaseCreateOut command, conn: MySqlConnection)=
            MySql.RunExecuteNonQuery(command,conn)
        
        //DROP DATABASE
        static member drop1 (DataBase database) (conn: MySqlConnection) =
            $"DROP DATABASE {database} " |> DataBaseDropOut, conn

        static member drop2 (DataBase database) (ifExists: bool) (conn: MySqlConnection) =
            let ifExistsV = if ifExists then "IF EXISTS" else ""

            $"DROP DATABASE {ifExistsV} {database}"
            |> DataBaseDropOut,
            conn
        
        static member runDropDatabase(DataBaseDropOut command, conn: MySqlConnection)=
            MySql.RunExecuteNonQuery(command,conn)
            
        //ALTER DATABASE
        static member alter (DataBase database) (conn: MySqlConnection) = $"ALTER DATABASE {database}"|>DataBaseAlterOut, conn
        
        static member defaultCharacterSetAlterDatabaseS (character: string) (DataBaseAlterOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character}"
            |> DataBaseAlterOut,
            conn

        static member defaultCharacterSetAlterDatabaseE (character: Encoding) (DataBaseAlterOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character.BodyName}"
            |> DataBaseAlterOut,
            conn
        
        static member defaultCollateSetAlterDatabase (collation: string) (DataBaseAlterOut command, conn: MySqlConnection) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> DataBaseAlterOut,
            conn

        static member defaultEncryptionSetAlterDatabase (enable: bool) (DataBaseAlterOut command, conn: MySqlConnection) =
            let ins = if enable then "Y" else "N"

            $"{command} DEFAULT ENCRYPTION = '{ins}'"
            |> DataBaseAlterOut,
            conn
            
        static member runAlterDatabase(DataBaseAlterOut command, conn: MySqlConnection)=
            MySql.RunExecuteNonQuery(command,conn)