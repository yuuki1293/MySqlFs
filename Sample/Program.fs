open MySqlFs

let mydb =
    mysql "Server=localhost;Uid=root;Pwd=root"

let mydbt =
    mysql "Server=localhost;Uid=root;Pwd=root;Database=hoge"

let ifExn (result:Result<int,exn>)=
    match result with
    | Ok x -> printfn $"OK %d{x}"
    | Error x ->
        raise x

mydb { create (DataBase "hoge") IfNotExists }
|> ifExn

mydb {
    alter (DataBase "hoge")
    readonly false
}
|> ifExn

mydbt{
    drop (Table "fuga") IfExists
}
|> ifExn

mydbt{
    drop (Table "piyo") IfExists
}
|> ifExn

mydbt{
    create (Table "fuga")
    cols (table{
        col "id" "INTEGER" [PrimaryKey;AutoIncrement]
        col "name" "VARCHAR(20)" [NotNull]
    })
    comment "test table 1"
}
|>ifExn

mydbt {
    create (Table "piyo")
    like (Table "fuga")
}
|> ifExn
