module Global

type Page =
    | About
    | HelloWorld01
    | HelloWorld01b
    | HelloWorld02
    | HelloWorld03
    | HelloWorld04
    | Counter05
    | Counter06
    | Counter07
    | Counter08
    | Counter09
    | Counter10
    | Counter11
    | Counter12
    | InputBox13
    | Todos14
    | Todos15
    | Todos16
    | Todos17
    | EditableTodos18
    | EditableTodos19
    | EditableTodos20
    | LocalStorageEditableTodos21
    | LocalStorageEditableTodos22
    | FilterTodos23
    | FilterTodos24
    | NavigationTodos25 of NavigationTodos25.Filter
    | NavigationTodos25b of NavigationTodos25b.Route

let toHash page =
    match page with
    | About -> "about"
    | HelloWorld01 -> "helloworld01"
    | HelloWorld01b -> "helloworld01b"
    | HelloWorld02 -> "helloworld02"
    | HelloWorld03 -> "helloworld03"
    | HelloWorld04 -> "helloworld04"
    | Counter05 -> "counter05"
    | Counter06 -> "counter06"
    | Counter07 -> "counter07"
    | Counter08 -> "counter08"
    | Counter09 -> "counter09"
    | Counter10 -> "counter10"
    | Counter11 -> "counter11"
    | Counter12 -> "counter12"
    | InputBox13 -> "inputbox13"
    | Todos14 -> "todos14"
    | Todos15 -> "todos15"
    | Todos16 -> "todos16"
    | Todos17 -> "todos17"
    | EditableTodos18 -> "editabletodos18"
    | EditableTodos19 -> "editabletodos19"
    | EditableTodos20 -> "editabletodos20"
    | LocalStorageEditableTodos21 -> "localstorageeditabletodos21"
    | LocalStorageEditableTodos22 -> "localstorageeditabletodos22"
    | FilterTodos23 -> "filtertodos23"
    | FilterTodos24 -> "filtertodos24"
    | NavigationTodos25 filter ->
        let nested =
            match filter with
            | NavigationTodos25.Filter.All -> ""
            | NavigationTodos25.Filter.Incomplete -> "/incomplete"
            | NavigationTodos25.Filter.Completed -> "/completed"
        "navigationtodos25" + nested
    | NavigationTodos25b route ->
        let nested =
            match route with
            | NavigationTodos25b.Route.Top -> ""
            | NavigationTodos25b.Route.Incomplete -> "/incomplete"
            | NavigationTodos25b.Route.Completed -> "/completed"
        "navigationtodos25b" + nested
    |> fun url -> "#" + url
