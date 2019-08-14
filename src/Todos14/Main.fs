module InputBox13


open Elmish
open Elmish.React
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props


// We added a new property called Todos, which is list of strings.
type Model = 
    { Text : string 
      Todos : string list }

// We added a new AddTodo message type.
type Msg = 
    | UpdateText of string
    | AddTodo


// We set the todos property so that it's initially an empty list.
let init() : Model =
    { Text = ""
      Todos = [] }

let update msg model : Model =
    match msg with
    | UpdateText newText ->
        { model with Text = newText }
    // We append the model.Text value to the end of our list of todo strings.
    | AddTodo ->
        { model with
            Text = ""
            Todos = model.Todos @ [ model.Text] }


// We added (AutoFocus true), which is like the native HTML autofocus attribute.
// We also added a button that triggers an OnClick event when clicked which
// passes an AddTodo message to the udpate function.
let root model dispatch : ReactElement =
    div [ ClassName "text-center" ]
        [ input 
            [ OnInput(fun e -> !!e.target?value |> UpdateText |> dispatch) 
              valueOrDefault model.Text
              AutoFocus true ]
          button [ OnClick (fun _ -> dispatch AddTodo); ClassName "btn btn-primary" ] [ str "Add Todo" ]
          div [] (List.map (fun todo -> div [] [ str todo ]) model.Todos) ]


Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run