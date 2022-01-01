namespace MySqlFs

open Microsoft.FSharp.Core

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
    
    type NoFirstLast =
        | No | First | Last
        member this.Value =
            match this with
            | No -> "NO"
            | First -> "FIRST"
            | Last -> "LAST"
            
    [<RequireQualifiedAccess>]
    type PackKeys =
        | Zero | One | Default
        member this.Value =
            match this with
            | Zero -> "0"
            | One -> "1"
            | Default -> "DEFAULT"
            
    [<RequireQualifiedAccess>]
    type StatsAutoRecalc =
        | Zero | One | Default
        member this.Value =
            match this with
            | Zero -> "0"
            | One -> "1"
            | Default -> "DEFAULT"
            
    [<RequireQualifiedAccess>]
    type StatsPersistent =
        | Zero | One | Default
        member this.Value =
            match this with
            | Zero -> "0"
            | One -> "1"
            | Default -> "DEFAULT"
            
    [<RequireQualifiedAccess>]
    type RowFormat =
        | Default | Dynamic | Fixed | Compressed | Redundant | Compact
        member this.Value =
            match this with
            | Default -> "DEFAULT"
            | Dynamic -> "DYNAMIC"
            | Fixed -> "FIXED"
            | Compressed -> "COMPRESSED"
            | Redundant -> "REDUNDANT"
            | Compact -> "COMPACT"
            
    [<RequireQualifiedAccess>]
    type Storage =
        | Disk | Memory | Default
        member this.Value =
            match this with
            | Disk -> "STORAGE DISK"
            | Memory -> "STORAGE MEMORY"
            | Default -> "STORAGE DEFAULT"
            
    type TableOption=
        | Engine of string
        | AutoIncrementValue of int
        | AvgRowLength of int
        | CharacterSet of string
        | Checksum of bool
        | Collate of string
        | Comment of string
        | Connection of string
        | DataDirectory of absolutePath:string
        | DelayKeyWrite of bool
        | IndexDirectory of absolutePath:string
        | InsertMethod of  NoFirstLast
        | KeyBlockSize of int
        | MaxRows of int
        | MinRows of int
        | PackKeys of PackKeys
        | Password of string
        | RowFormat of RowFormat
        | StatsAutoRecalc of StatsAutoRecalc
        | StatsPersistent of StatsPersistent
        | StatsSamplePages of value:string
        | Tablespace of tablespace_name:string
        | TablespaceStorage of tablespace_name:string * Storage 
        | Union of Table list
        member this.Value=
            match this with
            | Engine x -> $"Engine = {x}"
            | AutoIncrementValue x -> $"AUTO_INCREMENT = {string x}"
            | AvgRowLength x -> $"AVG_ROW_LENGTH = {string x}"
            | CharacterSet x -> $"CHARACTER SET = {x}"
            | Checksum x -> $"CHECKSUM {if x then '1' else '0'}"
            | Collate x -> $"COLLATE = {x}"
            | Comment x -> $"COMMENT = {x}"
            | Connection x -> $"CONNECTION = {x}"
            | DataDirectory x -> $"DATA DIRECTORY = {x}"
            | DelayKeyWrite x -> $"DELAY_KEY_WRITE = {if x then '1' else '0'}"
            | IndexDirectory x -> $"INDEX DIRECTORY = {x}"
            | InsertMethod x -> $"INSERT_METHOD = {x.Value}"
            | KeyBlockSize x -> $"KEY_BLOCK_SIZE = {string x}"
            | MaxRows x -> $"MAX_ROWS = {string x}"
            | MinRows x -> $"MIN_ROWS = {string x}"
            | PackKeys x -> $"PACK_KEYS = {x.Value}"
            | Password x -> $"PASSWORD = {x}"
            | RowFormat x -> $"ROW_FORMAT = {x.Value}"
            | StatsAutoRecalc x -> $"STATUS_AUTO_RECALC = {x.Value}"
            | StatsPersistent x -> $"STATS_PERSISTENT = {x.Value}"
            | StatsSamplePages x -> $"STATS_SAMPLE_PAGES = {x}"
            | Tablespace x -> $"{x}"
            | TablespaceStorage (x,y) -> $"{x} {y.Value}"
            | Union x ->
                let concat = 
                    seq {
                        for i in x do
                            yield
                                match i with
                                | Table table -> table
                    }
                    |>Seq.fold (fun x y -> x + ", " + y ) ""
                let trimmed = concat.TrimStart ','
                $"({trimmed})"
                
    let a = [PackKeys PackKeys.Default]