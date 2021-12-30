namespace MySqlFs

module PrivateInterface =
    type IEndExecuteNonQuery =
        abstract member value : string

    type IOptDatabaseCreate =
        abstract member value : string

    type IOptDatabaseAlter =
        abstract member value : string

    type IColsTableCreate =
        abstract member value : string
        
    type ILikeTableCreate=
        abstract member value : string

    type IOptTableCreate =
        abstract member value : string
