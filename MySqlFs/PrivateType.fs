namespace MySqlFs

open MySqlFs.PrivateInterface

module PrivateType =
    type NextOptDatabaseCreate =
        | NextOptDatabaseCreate of string
        interface IEndExecuteNonQuery with
            member this.value = this.Value this

        interface IOptDatabaseCreate with
            member this.value = this.Value this

        member _.Value(NextOptDatabaseCreate value) = value

    type NextEndDatabaseDrop =
        | NextEndDatabaseDrop of string
        interface IEndExecuteNonQuery with
            member this.value = this.Value this

        member _.Value(NextEndDatabaseDrop value) = value

    type NextOptDatabaseAlter =
        | NextOptDatabaseAlter of string
        interface IEndExecuteNonQuery with
            member this.value = this.Value this

        interface IOptDatabaseAlter with
            member this.value = this.Value this

        member _.Value(NextOptDatabaseAlter value) = value

    type NextOptTableCreate =
        | NextOptTableCreate of string
        interface IEndExecuteNonQuery with
            member this.value = this.Value this
        interface IOptTableCreate with
            member this.value = this.Value this

        member _.Value(NextOptTableCreate value) = value

    type NextCols'Like'OptTableCreate =
        | NextCols'Like'OptTableCreate of string
        interface IColsTableCreate with
            member this.value = this.Value this
        
        interface ILikeTableCreate with
            member this.value = this.Value this
        interface IOptTableCreate with
            member this.value = this.Value this

        interface IEndExecuteNonQuery with
            member this.value = this.Value this

        member _.Value(NextCols'Like'OptTableCreate value) = value

    type NextEndTableCreate =
        | NextEndTableCreate of string
        interface IEndExecuteNonQuery with
            member this.value = this.Value this

        member _.Value(NextEndTableCreate value) = value

    type NextEndTableDrop =
        | NextEndTableDrop of string
        interface IEndExecuteNonQuery with
            member this.value = this.Value this

        member _.Value(NextEndTableDrop value) = value

    type TableCol = TableCol of string
