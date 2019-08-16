module EditableTodos22

open Elmish
open Elmish.React
open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props
open Thoth.Json

type TodoEdit =
    { Index : int
      Text : string }

// We changed the todos so that they aren't just string, they now are records
// with a completed property which represents whether the user has completed that todo.
type Todo =
    { Text : string
      Completed : bool }

// The Todos property is now Todo list instead of string list.
type Model =
    { Text : string
      Todos : Todo list
      Editing : TodoEdit option } // Option<TodoEdit>

type Msg =
    | UpdateText of string
    | AddTodo
    | RemoveTodo of int
    | Edit of int * string
    | EditSave of int * string
    // We added a new (ToggleTodo of int * string) message type.
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

// We changed the type declaration to take a list of Todo records.
let saveTodos (todos : Todo list) : Cmd<Msg> =
    let save (todos : Todo list) =
        Encode.Auto.toString (0, todos)
        |> fun json -> (STORAGE_KEY, json)
        |> Browser.WebStorage.localStorage.setItem
    Cmd.OfFunc.attempt save todos (string >> Failure)

let init todos : Model * Cmd<Msg> =
    { Text = ""
      Todos = todos
      Editing = None }, Cmd.none

let update msg model : Model * Cmd<Msg> =
    match msg with
    | UpdateText newText -> { model with Text = newText }, Cmd.none
    | AddTodo ->
        let newTodos =
            model.Todos @ [ { Text = model.Text
                              Completed = false } ]
        { model with Text = ""
                     Todos = newTodos }, saveTodos newTodos
    | RemoveTodo index ->
        let beforeTodos = List.take index model.Todos
        let afterTodos = List.skip (index + 1) model.Todos
        let newTodos = beforeTodos @ afterTodos
        { model with Todos = newTodos }, saveTodos newTodos
    | Edit(index, todoText) ->
        { model with Editing =
                         Some { Index = index
                                Text = todoText } }, Cmd.none
    | EditSave(index, todoText) ->
        let newTodos =
            model.Todos
            |> List.indexed
            |> List.map (fun (i, todo) ->
                   if i = index then { todo with Text = todoText }
                   else todo)
        { model with Editing = None
                     Todos = newTodos }, saveTodos newTodos
    // Added an extra clause to handle toggling the todo.
    | ToggleTodo index ->
        let newTodos =
            model.Todos
            |> List.indexed
            |> List.map (fun (i, todo) ->
                   if i = index then
                       { todo with Completed = not todo.Completed }
                   else todo)
        { model with Todos = newTodos }, saveTodos newTodos
    | Failure err ->
        JS.console.error err
        model, Cmd.none

let subscriptions dispatch : Cmd<Msg> = Cmd.ofSub ignore

let root model dispatch : ReactElement =
    let onEnter msg : DOMAttr =
        function
        | (ev : Browser.Types.KeyboardEvent) when ev.keyCode = 13. ->
            dispatch msg
        | _ -> ()
        |> OnKeyDown

    let viewEditTodo index todoEdit : ReactElement =
        div [ ClassName "card" ]
            [ div [ ClassName "card-block" ]
                  [ div [ onEnter (EditSave(todoEdit.Index, todoEdit.Text)) ]
                        [ input [ OnInput(fun e ->
                                      !!e.target?value
                                      |> fun x ->
                                          (index, x)
                                          |> Edit
                                          |> dispatch)
                                  ClassName "form-control"
                                  Value todoEdit.Text ] ] ] ]

    let viewNormalTodo index todo : ReactElement =
        div [ ClassName "card" ]
            [ div [ ClassName "card-block" ]
                  [ // Added a checkbox that can be clicked to toggle the todo
                    // between being completed or incomplete.
                    input [ OnClick(fun _ -> dispatch <| ToggleTodo index)
                            Type "checkbox"
                            Checked todo.Completed
                            ClassName "mr-3" ]

                    // Adding styling to indicate a todo is completed.
                    span [ OnDoubleClick
                               (fun _ -> dispatch <| Edit(index, todo.Text))
                           Style [ TextDecoration(if todo.Completed then
                                                      "line-through"
                                                  else "none") ] ]
                        [ str todo.Text ]
                    span [ OnClick(fun _ -> dispatch <| RemoveTodo index)
                           ClassName "float-right" ] [ str "✖" ] ] ]

    let viewTodo editing index todo : ReactElement =
        match editing with
        | Some todoEdit ->
            if todoEdit.Index = index then viewEditTodo index todoEdit
            else viewNormalTodo index todo
        | _ -> viewNormalTodo index todo

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
                                                    onEnter AddTodo ] ]

                div [ ClassName "col-3" ]
                    [ button [ OnClick(fun _ -> dispatch AddTodo)
                               ClassName "btn btn-primary form-control" ]
                          [ str "+" ] ] ]
          div [] (model.Todos
                  |> List.indexed
                  |> List.map (fun (i, todo) -> viewTodo model.Editing i todo)) ]

Program.mkProgram (loadTodos >> init) update root
|> Program.withReactBatched "elmish-app"
|> Program.withSubscription subscriptions
|> Program.run
