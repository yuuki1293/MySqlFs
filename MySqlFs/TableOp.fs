namespace MySqlFs

open MySqlFs.Function

[<AutoOpen>]
module TableBuilder =
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

    type TableBuilder() =
        member _.Yield _ = ""
        member _.Run (x: string) =
            let trimmed = x.TrimEnd(',')
            TableCol $"({trimmed})"

        [<CustomOperation("col")>]
        member _.col(v: string, colName: string, dataType: string) =
            $"{v} {colName} {dataType},"
        
        [<CustomOperation("col")>]
        member _.col(v:string,colName:string,dataType :string,colOps:ColOption list)=
            let colOpsS=
                seq{
                    for colOp in colOps do colOp
                }
                |>Seq.fold (fun x y-> $"{x} {y.Get}" ) ""
            $"{v} {colName} {dataType} {colOpsS},"
        
    let table = TableBuilder()
