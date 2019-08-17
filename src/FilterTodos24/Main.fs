module FilterTodos24

open Elmish
open Elmish.React
open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Thoth.Json

type Filter =
    | All
    | Incomplete
    | Completed

// Before TodoEdit had an index and a text field, now we're going to be
// using the todo's id to identify which todo we're editing, so I
// switched the field name from Index to Id.
type TodoEdit =
    { Id : int
      Text : string }

// Added an Id field, which will hold each todo's randomly generated Id.
type Todo =
    { Id : int
      Text : string
      Completed : bool }

type Model =
    { Text : string
      Todos : Todo list
      Editing : TodoEdit option
      Filter : Filter }

// Added GenerateTodoId message, which we're going to use to generate a
// random id for newly create todo.
// AddTodo was changed to (AddTodo of int) because it new takes the integer
// id that was randomly generated and uses that to add the new todo.
type Msg =
    | UpdateText of string
    | GenerateTodoId
    | AddTodo of int
    | RemoveTodo of int
    | Edit of int * string
    | EditSave of int * string
    | ToggleTodo of int
    | SetFilter of Filter
    | Failure of string

[<LiteralAttribute>]
let STORAGE_KEY = "todos"

let todosDecoder : Decoder<Todo list> = Decode.Auto.generateDecoder<Todo list>()

let loadTodos() : Todo list =
    Browser.WebStorage.localStorage.getItem STORAGE_KEY
    |> unbox
    |> Decode.fromString todosDecoder
    |> function
    | Ok todos -> todos
    | _ -> []

let saveTodos (todos : Todo list) : Cmd<Msg> =
    let save (todos : Todo list) =
        Encode.Auto.toString (0, todos)
        |> fun json -> (STORAGE_KEY, json)
        |> Browser.WebStorage.localStorage.setItem
    Cmd.OfFunc.attempt save todos (string >> Failure)

let generate : unit -> Cmd<Msg> =
    // We initialize random number generator here.
    let r = System.Random()
    // r.Next(minValue, maxValue) returns an integer random value between minValue and maxValue,
    // then it will pass that integer to AddTodo, which will get passed into the update function.
    fun () ->
        Cmd.ofMsg
        <| AddTodo(r.Next(System.Int32.MinValue, System.Int32.MaxValue))

let init todos : Model * Cmd<Msg> =
    { Text = ""
      Todos = todos
      Editing = None
      Filter = All }, Cmd.none

let update msg model : Model * Cmd<Msg> =
    match msg with
    | UpdateText newText -> { model with Text = newText }, Cmd.none
    // When we submits new todo, it now passws in GenerateTodoId, which will generate
    // a random integetr Id, then the result will be passed into AddedTodo which now accepts
    // an integer.
    | GenerateTodoId -> model, generate()
    // So now after the random Id is generated, it gets passed into AddTodo
    // which gets passed into the udpate function. We take the Id and model.Text value
    // to create new todo and then we append the new todo the end of model.Todos.
    | AddTodo todoId ->
        let newTodos =
            model.Todos @ [ { Id = todoId
                              Text = model.Text
                              Completed = false } ]
        { model with Text = ""
                     Todos = newTodos }, saveTodos newTodos
    // Since we  get passws the Id, we can just use List.where to
    // keep all the todos that don't have the save Id as the one we aremoving.
    | RemoveTodo todoId ->
        let newTodos = model.Todos |> List.where (fun todo -> todo.Id <> todoId)
        { model with Todos = newTodos }, saveTodos newTodos
    // We use Id to track the edited todo instead of the Index.
    | Edit(todoId, todoText) ->
        { model with Editing =
                         Some { Id = todoId
                                Text = todoText } }, Cmd.none
    // We are now saving the todo, so if the todo's Id is the Id that we are editting.
    // then we change the text of that tidi ti the edit text. If its Id isn't the same as
    // the edit todo's Id, we keep it the same because it wasn't the todo that we were editing.
    | EditSave(index, todoText) ->
        let newTodos =
            model.Todos
            |> List.map (fun todo ->
                   if todo.Id = index then { todo with Text = todoText }
                   else todo)
        { model with Editing = None
                     Todos = newTodos }, saveTodos newTodos
    // We map over the Todos and change the completed field of
    // the todo that has the Id that we chose to toggle.
    | ToggleTodo todoId ->
        let newTodos =
            model.Todos
            |> List.map (fun todo ->
                   if todo.Id = todoId then
                       { todo with Completed = not todo.Completed }
                   else todo)
        { model with Todos = newTodos }, saveTodos newTodos
    | SetFilter filter -> { model with Filter = filter }, Cmd.none
    | Failure err ->
        JS.console.error err
        model, Cmd.none

let subscriptions dispatch : Cmd<Msg> = Cmd.ofSub ignore

let filterTodos filter todos =
    match filter with
    | All -> todos
    | Incomplete -> List.filter (fun todo -> not todo.Completed) todos
    | Completed -> List.filter (fun todo -> todo.Completed) todos

let root model dispatch : ReactElement =
    let onEnter msg : DOMAttr =
        function
        | (ev : Browser.Types.KeyboardEvent) when ev.keyCode = 13. ->
            dispatch msg
        | _ -> ()
        |> OnKeyDown

    let viewFilter filter isFilter filterText =
        if isFilter then span [ ClassName "mr-3" ] [ str filterText ]
        else
            span [ ClassName "text-primary mr-3"
                   OnClick(fun _ -> dispatch <| SetFilter filter)
                   Style [ Cursor "pointer" ] ] [ str filterText ]

    let viewFilters filter =
        div [] [ viewFilter All (filter = All) "All"
                 viewFilter Incomplete (filter = Incomplete) "Incomplete"
                 viewFilter Completed (filter = Completed) "Completed" ]

    // vieweditTodo has one less argument because it doesn't need the Index for editing,
    // it use the todo's Id to identify the tidi that is being edited.
    // It passes in todoEdit.Id into Edit and EditSave now instead of the Index.
    let viewEditTodo (todoEdit : TodoEdit) : ReactElement =
        div [ ClassName "card" ]
            [ div [ ClassName "card-block" ]
                  [ div [ onEnter (EditSave(todoEdit.Id, todoEdit.Text)) ]
                        [ input [ OnInput(fun e ->
                                      !!e.target?value
                                      |> fun x ->
                                          (todoEdit.Id, x)
                                          |> Edit
                                          |> dispatch)
                                  ClassName "form-control"
                                  Value todoEdit.Text ] ] ] ]

    // viewNormalTodo has one less argument because it doesn't need the Index.
    // It now passes todo.Id into ToggleTodo, Edit, and RemoveTodo instead of the Index.
    let viewNormalTodo (todo : Todo) : ReactElement =
        div [ ClassName "card" ]
            [ div [ ClassName "card-block" ]
                  [ input [ OnClick(fun _ -> dispatch <| ToggleTodo todo.Id)
                            Type "checkbox"
                            Checked todo.Completed
                            ClassName "mr-3" ]

                    span [ OnDoubleClick
                               (fun _ -> dispatch <| Edit(todo.Id, todo.Text))
                           Style [ TextDecoration(if todo.Completed then
                                                      "line-through"
                                                  else "none") ] ]
                        [ str todo.Text ]
                    span [ OnClick(fun _ -> dispatch <| RemoveTodo todo.Id)
                           ClassName "float-right" ] [ str "✖" ] ] ]

    // viewTodo now has one less argument because it doesn't need to have the Index
    // passed in anymore since it uses the Id for editing and removing each todo.
    // It also doesn't pass the Index into viewEditTodo and viewNormalTodo anymore.
    let viewTodo (editing : TodoEdit option) todo : ReactElement =
        match editing with
        | Some todoEdit ->
            if todoEdit.Id = todo.Id then viewEditTodo todoEdit
            else viewNormalTodo todo
        | _ -> viewNormalTodo todo

    div [ ClassName "col-12 col-sm-6 offset-sm-3" ]
        [ div [ ClassName "row" ]
              [ div [ ClassName "col-9" ] [ input [ OnInput(fun e ->
                                                        !!e.target?value
                                                        |> UpdateText
                                                        |> dispatch)
                                                    Value model.Text
                                                    AutoFocus true
                                                    ClassName "form-control"
                                                    Placeholder "Enter a todo"
                                                    onEnter GenerateTodoId ] ]

                div [ ClassName "col-3" ]
                    [ button [ OnClick(fun _ -> dispatch GenerateTodoId)
                               ClassName "btn btn-primary form-control" ]
                          [ str "+" ] ] ]
          viewFilters model.Filter
          div [] (model.Todos
                  |> filterTodos model.Filter
                  |> List.map (fun todo -> viewTodo model.Editing todo)) ]

Program.mkProgram (loadTodos >> init) update root
|> Program.withReactBatched "elmish-app"
|> Program.withSubscription subscriptions
|> Program.run
