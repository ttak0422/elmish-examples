// Routing in Elmish is very different from Elm.
module NavigationTodos25

open Elmish
open Elmish.Navigation
open Elmish.React
open Elmish.UrlParser
open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Thoth.Json

type Filter =
    | All
    | Incomplete
    | Completed

type TodoEdit =
    { Id : int
      Text : string }

type Todo =
    { Id : int
      Text : string
      Completed : bool }

type Model =
    { Text : string
      Todos : Todo list
      Editing : TodoEdit option
      Filter : Filter }

type Msg =
    | UpdateText of string
    | GenerateTodoId
    | AddTodo of int
    | RemoveTodo of int
    | Edit of int * string
    | EditSave of int * string
    | ToggleTodo of int
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
    let r = System.Random()
    fun () ->
        Cmd.ofMsg
        <| AddTodo(r.Next(System.Int32.MinValue, System.Int32.MaxValue))

// Whenever the URL changes, the current URL gets passed to url parser that we defined,
// which gets passed to url update function that we defined.
let urlParser : Parser<Filter -> Filter, Filter> =
    // Since namespace is conflicted, we have to a specific declaration.
    // We should modularize as much as possible.
    oneOf [ UrlParser.map All UrlParser.top
            UrlParser.map All (UrlParser.s "all")
            UrlParser.map Incomplete (UrlParser.s "incomplete")
            UrlParser.map Completed (UrlParser.s "completed") ]

// This function set filter instead of SetFilter.
let urlUpdate (result : Filter option) (model : Model) : Model * Cmd<Msg> =
    match result with
    | None -> { model with Filter = All }, Cmd.none
    | Some filter -> { model with Filter = filter }, Cmd.none

let init todos result : Model * Cmd<Msg> =
    { Text = ""
      Todos = todos
      Editing = None
      Filter = All }, Cmd.none

let update msg model : Model * Cmd<Msg> =
    match msg with
    | UpdateText newText -> { model with Text = newText }, Cmd.none
    | GenerateTodoId -> model, generate()
    | AddTodo todoId ->
        let newTodos =
            model.Todos @ [ { Id = todoId
                              Text = model.Text
                              Completed = false } ]
        { model with Text = ""
                     Todos = newTodos }, saveTodos newTodos
    | RemoveTodo todoId ->
        let newTodos = model.Todos |> List.where (fun todo -> todo.Id <> todoId)
        { model with Todos = newTodos }, saveTodos newTodos
    | Edit(todoId, todoText) ->
        { model with Editing =
                         Some { Id = todoId
                                Text = todoText } }, Cmd.none
    | EditSave(index, todoText) ->
        let newTodos =
            model.Todos
            |> List.map (fun todo ->
                   if todo.Id = index then { todo with Text = todoText }
                   else todo)
        { model with Editing = None
                     Todos = newTodos }, saveTodos newTodos
    | ToggleTodo todoId ->
        let newTodos =
            model.Todos
            |> List.map (fun todo ->
                   if todo.Id = todoId then
                       { todo with Completed = not todo.Completed }
                   else todo)
        { model with Todos = newTodos }, saveTodos newTodos
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
            a [ ClassName "text-primary mr-3"
                Href("#" + filterText.ToLower())
                Style [ Cursor "pointer" ] ] [ str filterText ]

    let viewFilters filter =
        div [] [ viewFilter All (filter = All) "All"
                 viewFilter Incomplete (filter = Incomplete) "Incomplete"
                 viewFilter Completed (filter = Completed) "Completed" ]

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
                                                    valueOrDefault model.Text
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

Program.mkProgram (init <| loadTodos()) update root
|> Program.withSubscription subscriptions
// We set url parser and url update function here.
|> Program.toNavigable (parseHash urlParser) urlUpdate
|> Program.withReactBatched "elmish-app"
|> Program.run
