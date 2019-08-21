namespace NavigationTodos25b

module State =
    open Elmish
    open Fable.Core
    open NavigationTodos25b
    open NavigationTodos25b.Datas
    open System

    let generate() : Cmd<Msg> =
        Random.next Int32.MinValue Int32.MaxValue
        |> AddTodo
        |> Cmd.ofMsg

    let saveTodos (todos : List<Todo>) : Cmd<Msg> =
        Cmd.OfFunc.attempt Todo.saveTodos todos (string >> Failure)

    let urlUpdate (result : Option<Route>) (model : Model) : Model * Cmd<Msg> =
        match result with
        | None -> 
            JS.console.error "parse failed."
            { model with Filter = All }, Cmd.none
        | Some Route.Top -> { model with Filter = All }, Cmd.none
        | Some Route.Incomplete -> { model with Filter = Incomplete }, Cmd.none
        | Some Route.Completed -> { model with Filter = Completed }, Cmd.none

    let init (todos : List<Todo>) (result : Option<Route>) : Model * Cmd<Msg> =
        { Text = ""
          Todos = todos
          Editing = None
          Filter = All }, []

    let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
        match msg with
        | UpdateText text' -> { model with Text = text' }, []
        | GenerateTodoId -> model, generate()
        | AddTodo id ->
            let todo =
                { Id = id
                  Text = model.Text
                  Completed = false }

            let todos' = model.Todos @ [ todo ]
            { model with Text = ""
                         Todos = todos' }, saveTodos todos'
        | RemoveTodo id ->
            let todos' = model.Todos |> List.where (fun t -> t.Id <> id)
            { model with Todos = todos' }, saveTodos todos'
        | Edit(id, text) ->
            { model with Editing =
                             Some { Id = id
                                    Text = text } }, []
        | EditSave(id, text) ->
            let todos' =
                model.Todos
                |> List.map (fun t ->
                       if t.Id = id then { t with Text = text }
                       else t)
            { model with Editing = None
                         Todos = todos' }, saveTodos todos'
        | ToggleTodo id ->
            let todos' =
                model.Todos
                |> List.map (fun t ->
                       if t.Id = id then { t with Completed = not t.Completed }
                       else t)
            { model with Todos = todos' }, saveTodos todos'
        | Failure err ->
            JS.console.error err
            model, []

    let subscriptions dispatch : Cmd<Msg> = Cmd.ofSub ignore
