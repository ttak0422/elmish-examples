namespace NavigationTodos25b.Datas

open NavigationTodos25b.Utils

type Todo =
    { Id : Id
      Text : string
      Completed : bool }

type TodoEdit =
    { Id : Id
      Text : string }

module Todo =
    open Browser    
    open Thoth.Json

    [<LiteralAttribute>]
    let private STORAGE_KEY = "todos"

    let private todosDecoder : Decoder<List<Todo>> =
        Decode.Auto.generateDecoder<List<Todo>>()

    let loadTodos() : List<Todo> =
        localStorage.getItem STORAGE_KEY
        |> unbox
        |> Decode.fromString todosDecoder
        |> function
        | Ok todos -> todos
        | _ -> []

    let saveTodos (todos : List<Todo>) : unit =
        Encode.Auto.toString (0, todos)
        |> fun json -> (STORAGE_KEY, json)
        |> localStorage.setItem
