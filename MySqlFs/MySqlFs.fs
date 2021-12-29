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
        
        static member runExecuteNonQuery(commandS, conn: MySqlConnection) =
            try
                conn.Open()
                use mutable command = conn.CreateCommand()
                command.CommandText <- commandS
                command.Connection <- conn
                command.ExecuteNonQuery() |> Ok
            with
            | x -> Error x
            
    type Original=
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
        
        static member drop1 (DataBase database) (conn: MySqlConnection) =
            $"DROP DATABASE {database} " |> DataBaseDropOut, conn

        static member drop2 (DataBase database) (ifExists: bool) (conn: MySqlConnection) =
            let ifExistsV = if ifExists then "IF EXISTS" else ""

            $"DROP DATABASE {ifExistsV} {database}"
            |> DataBaseDropOut,
            conn
        
        static member alter (DataBase database) (conn: MySqlConnection) = $"ALTER DATABASE {database}"|>DataBaseAlterOut, conn

        
    type DefaultCharSet=
        static member createDatabase (character: string) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character}"
            |> DataBaseCreateOut,
            conn
        
        static member alterDatabase (character: string) (DataBaseAlterOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character}"
            |> DataBaseAlterOut,
            conn
            
    type DefaultCollate=
        static member createDatabase (collation: string) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> DataBaseCreateOut,
            conn
        
        static member alterDatabase (collation: string) (DataBaseAlterOut command, conn: MySqlConnection) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> DataBaseAlterOut,
            conn
            
    type DefaultEncryption=
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
            
    type Run=
        static member createDatabase(DataBaseCreateOut command, conn: MySqlConnection)=
            Common.runExecuteNonQuery(command,conn)
            
        static member dropDatabase(DataBaseDropOut command, conn: MySqlConnection)=
            Common.runExecuteNonQuery(command,conn)
        
        static member runAlterDatabase(DataBaseAlterOut command, conn: MySqlConnection)=
            Common.runExecuteNonQuery(command,conn)
            
    type MySql =
        [<Obsolete "Use Common.open' instead">]
        static member open'(connectionString: string) = new MySqlConnection(connectionString)
        
        [<Obsolete "Use Common.RunExecuteNonQuery instead">]
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
        [<Obsolete "Use Original.create1 instead">]
        static member create1 (DataBase database) (conn: MySqlConnection) =
            $"CREATE DATABASE {database} "
            |> DataBaseCreateOut,
            conn

        [<Obsolete "Use Original.create2 instead">]
        static member create2 (DataBase database) (ifNotExists: bool) (conn: MySqlConnection) =
            let ifNotExistsV =
                if ifNotExists then
                    "IF NOT EXISTS"
                else
                    ""

            $"CREATE DATABASE {ifNotExistsV} {database}"
            |> DataBaseCreateOut,
            conn

        [<Obsolete "Use DefaultCharSet.CreateDatabase instead">]
        static member defaultCharacterSetCreateDatabaseS (character: string) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character}"
            |> DataBaseCreateOut,
            conn

        [<Obsolete "Don't use">]
        static member defaultCharacterSetCreateDatabaseE (character: Encoding) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character.BodyName}"
            |> DataBaseCreateOut,
            conn
        
        [<Obsolete "Use DefaultCollate.CreateDatabase">]
        static member defaultCollateCreateDatabase (collation: string) (DataBaseCreateOut command, conn: MySqlConnection) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> DataBaseCreateOut,
            conn

        [<Obsolete "Use DefaultEncryption.CreateDatabase instead">]
        static member defaultEncryptionCreateDatabase (enable: bool) (DataBaseCreateOut command, conn: MySqlConnection) =
            let ins = if enable then "Y" else "N"

            $"{command} DEFAULT ENCRYPTION = '{ins}'"
            |> DataBaseCreateOut,
            conn
        
        [<Obsolete "Use Run.CreateDatabase instead">]
        static member runCreateDatabase(DataBaseCreateOut command, conn: MySqlConnection)=
            MySql.RunExecuteNonQuery(command,conn)
        
        //DROP DATABASE
        [<Obsolete "Use Original.drop1 instead">]
        static member drop1 (DataBase database) (conn: MySqlConnection) =
            $"DROP DATABASE {database} " |> DataBaseDropOut, conn

        [<Obsolete "Use Original.drop2 instead">]
        static member drop2 (DataBase database) (ifExists: bool) (conn: MySqlConnection) =
            let ifExistsV = if ifExists then "IF EXISTS" else ""

            $"DROP DATABASE {ifExistsV} {database}"
            |> DataBaseDropOut,
            conn
        
        [<Obsolete "Use Original.dropDatabase instead">]
        static member runDropDatabase(DataBaseDropOut command, conn: MySqlConnection)=
            MySql.RunExecuteNonQuery(command,conn)
            
        //ALTER DATABASE
        [<Obsolete "Use Original.alter instead">]
        static member alter (DataBase database) (conn: MySqlConnection) = $"ALTER DATABASE {database}"|>DataBaseAlterOut, conn
        
        [<Obsolete "Use DefaultCharSet.alterDatabase instead">]
        static member defaultCharacterSetAlterDatabaseS (character: string) (DataBaseAlterOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character}"
            |> DataBaseAlterOut,
            conn

        [<Obsolete "Don't use">]
        static member defaultCharacterSetAlterDatabaseE (character: Encoding) (DataBaseAlterOut command, conn: MySqlConnection) =
            $"{command} DEFAULT CHARACTER SET = {character.BodyName}"
            |> DataBaseAlterOut,
            conn
        
        [<Obsolete "Use DefaultCollate.alterDatabase instead">]
        static member defaultCollateSetAlterDatabase (collation: string) (DataBaseAlterOut command, conn: MySqlConnection) =
            $"{command} DEFAULT COLLATE = {collation}"
            |> DataBaseAlterOut,
            conn

        [<Obsolete "Use DefaultEncryption.alterDatabase instead">]
        static member defaultEncryptionSetAlterDatabase (enable: bool) (DataBaseAlterOut command, conn: MySqlConnection) =
            let ins = if enable then "Y" else "N"

            $"{command} DEFAULT ENCRYPTION = '{ins}'"
            |> DataBaseAlterOut,
            conn
            
        [<Obsolete "Use Run.alterDatabase instead">]
        static member runAlterDatabase(DataBaseAlterOut command, conn: MySqlConnection)=
            MySql.RunExecuteNonQuery(command,conn)