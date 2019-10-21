module EditableTodos20

open Elmish
open Elmish.React
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props


type TodoEdit =
    { Index : int
      Text : string }

type Model =
    { Text : string
      Todos : string list
      Editing : TodoEdit option } // Option<TodoEdit>

type Msg =
    | UpdateText of string
    | AddTodo
    | RemoveTodo of int
    | Edit of int * string
    | EditSave of int * string


let init() : Model * Cmd<Msg> =
    // F# doesn't have record type constructors now.
    // https://github.com/fsharp/fslang-suggestions/issues/722
    { Text = ""
      Todos = [ "Laundry"; "Dishes" ]
      Editing = None }, Cmd.none

let update msg model : Model * Cmd<Msg> =
    match msg with
    | UpdateText newText -> { model with Text = newText }, Cmd.none
    | AddTodo ->
        { model with Text = ""
                     Todos = model.Todos @ [ model.Text ] }
        , Cmd.none
    | RemoveTodo index ->
        let beforeTodos = List.take index model.Todos
        let afterTodos = List.skip (index + 1) model.Todos
        let newTodos = beforeTodos @ afterTodos
        { model with Todos = newTodos }, Cmd.none
    | Edit(index, todoText) ->
        { model with Editing =
                         Some { Index = index
                                Text = todoText } }, Cmd.none
    | EditSave(index, todoText) ->
        let newTodos =
            model.Todos
            |> List.indexed
            |> List.map (fun (i, todo) ->
                   if i = index then todoText
                   else todo)
        { model with Editing = None
                     Todos = newTodos }, Cmd.none

let subscriptions dispatch : Cmd<Msg> =
    Cmd.ofSub ignore


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
                  [ span
                        [ OnDoubleClick(fun _ -> dispatch <| Edit(index, todo)) ]
                        [ str todo ]
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
                                                    valueOrDefault model.Text
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


Program.mkProgram init update root
|> Program.withReactBatched "elmish-app"
|> Program.withSubscription subscriptions
|> Program.run
