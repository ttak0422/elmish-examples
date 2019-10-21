module Todos16


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


// We added some nice little touces to the web app.
// We added a placeholder attribute that's similar to placeholder
// attribute you're used to with native HTML.
let root model dispatch : ReactElement =
    // We made the styling nicer by taking advantage of Bootstrap classes.
    let viewTodo (index, todo) : ReactElement =
        div [ ClassName "card" ]
            [ div [ ClassName "card-block" ]
                [ str todo
                  span
                    [ OnClick (fun _ -> dispatch (RemoveTodo index))
                      ClassName "float-right" ]
                      [ str "✖" ] ] ]

    div [ ClassName "col-12 col-sm-6 offset-sm-3" ]
        [ div [ ClassName "row" ]
            [ div [ ClassName "col-9" ]
                [ input
                    [ OnInput (fun e -> !!e.target?value |> UpdateText |> dispatch)
                      valueOrDefault model.Text
                      AutoFocus true
                      ClassName "form-control"
                      Placeholder "Enter a todo" ] ]
              div [ ClassName "col-3" ]
                [ button
                    [ OnClick (fun _ -> dispatch AddTodo); ClassName "btn btn-primary form-control" ]
                    [ str "+" ] ] ]
          div [] (model.Todos |> List.indexed |> List.map viewTodo) ]


Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run