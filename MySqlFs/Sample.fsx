#r @"bin\Debug\net6.0\MySqlFs.dll"

open MySqlFs

let mydb = mysql "Server=localhost;Uid=root;Pwd=root"
let mydbt = mysql "Server=localhost;Uid=root;Pwd=root;Database=hoge"

mydb{
    drop (DataBase "hoge")
}

mydb{
    create(DataBase "hoge")
}

mydb{
    alter (DataBase "hoge")
    readonly true
}

mydbt{
    create (Table "fuga")
}