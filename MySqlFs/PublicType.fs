namespace MySqlFs

[<AutoOpen>]
module PublicType=
    type DataBase = DataBase of string
    type IfNotExists = IfNotExists
    type IfExists = IfExists
    type Table = Table of string
    type Temporary = Temporary
    type ColOption =
        | NotNull
        | Default of string
        | AutoIncrement
        | Unique
        | PrimaryKey
        | Comment of string
         member this.Get =
            match this with
            | NotNull -> "NOT NULL"
            | Default x -> $"DEFAULT {x}"
            | AutoIncrement -> "AUTO_INCREMENT"
            | Unique -> "UNIQUE"
            | PrimaryKey -> "PRIMARY KEY"
            | Comment x -> $"COMMENT '{x}'"
    