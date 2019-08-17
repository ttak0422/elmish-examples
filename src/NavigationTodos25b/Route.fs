namespace NavigationTodos25b

type Route =
    | Top
    | Incomplete
    | Completed
    member __.ToHash() =
        match __ with
        | Top -> "top"
        | Incomplete -> "incomplete"
        | Completed -> "completed"
        |> fun url -> "#" + url

module Route =
    open Elmish.UrlParser

    let urlParser : Parser<Route -> Route, Route> =
        oneOf [ map Top top
                map Top (s "all")
                map Incomplete (s "incomplete")
                map Completed (s "completed") ]
