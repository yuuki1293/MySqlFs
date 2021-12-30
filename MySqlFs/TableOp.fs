namespace MySqlFs

open MySqlFs.PrivateType

[<AutoOpen>]
module TableBuilder =
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
