module Todos17


open Elmish
open Elmish.React
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props


type Model =
    { Text : string
      Todos : string list }

type Msg =
    | UpdateText of string
    | AddTodo
    | RemoveTodo of int


let init() : Model =
    { Text = ""
      Todos = [] }

let update msg model : Model =
    match msg with
    | UpdateText newText ->
        { model with Text = newText }
    | AddTodo ->
        { model with
            Text = ""
            Todos = model.Todos @ [ model.Text] }
    | RemoveTodo index ->
        let beforeTodos = List.take index model.Todos
        let afterTodos = List.skip (index + 1) model.Todos
        let newTodos = beforeTodos @ afterTodos
        { model with Todos = newTodos }


// We made it so you can now hit enter to add a todo instead of having to click on the button.
// To accomplish this, we wrapped the input and button in a form and made the AddTodo message
// get passed whenever an OnKeyDown event gets triggered.
// The OnKeyDonw event will get triggered whenever the user hits emter in the box on the button.
let root model dispatch : ReactElement =
    let viewTodo (index, todo) : ReactElement =
        div [ ClassName "card" ]
            [ div [ ClassName "card-block" ]
                [ str todo
                  span
                    [ OnClick (fun _ -> dispatch (RemoveTodo index))
                      ClassName "float-right" ]
                      [ str "✖" ] ] ]

    let onEnter msg =
        function
        | (ev : Browser.Types.KeyboardEvent) when ev.keyCode = 13. ->
            if model.Text.Length > 0 then
                dispatch msg
            else
                ()
        | _ -> ()
        |> OnKeyDown


    div [ ClassName "col-12 col-sm-6 offset-sm-3" ]
        [ div [ ClassName "row" ]
            [ div [ ClassName "col-9" ]
                [ input
                    [ OnInput (fun e -> !!e.target?value |> UpdateText |> dispatch)
                      valueOrDefault model.Text
                      AutoFocus true
                      ClassName "form-control"
                      Placeholder "Enter a todo"
                      onEnter AddTodo ] ]
              div [ ClassName "col-3" ]
                [ button
                    [ OnClick (fun _ -> dispatch AddTodo); ClassName "btn btn-primary form-control" ]
                    [ str "+" ] ] ]
          div [] (model.Todos |> List.indexed |> List.map viewTodo) ]


Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run