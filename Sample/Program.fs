open MySqlFs

let mydb =
    mysql "Server=localhost;Uid=root;Pwd=root"

let mydbt =
    mysql "Server=localhost;Uid=root;Pwd=root;Database=hoge"

mydb { drop (DataBase "hoge") } |> printfn "%A"
mydb { create (DataBase "hoge") } |> printfn "%A"

mydb {
    alter (DataBase "hoge")
    readonly true
}
|> printfn "%A"

mydbt {
    create (Table "fuga")

    cols (
        table {
            col "id" "INTEGER" [ PrimaryKey; AutoIncrement ]
            col "name" "VARCHAR(20)" [ NotNull ]
        }
    )
}
|> printfn "%A"
