module InputBox13


open Elmish
open Elmish.React
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props


// We made our model type a record that has a property called Text.
// The text property has a string type.
// For example, our model can be the value { Text = "Hello" }
// Records are similar to objects in JavaScript.
type Model = 
    { Text : string }

// We have a Msg that can be the value (UpdateText of string)
type Msg = 
    | UpdateText of string


// We set the initial model value to { Text = "" }, so the input box calue
// is initially an empty string.
let init() : Model =
    { Text = "" }

// We just have to handle one case for our message.
// All we do is set the text property in the model to the string that is currently in the input box.
let update msg model : Model =
    match msg with
    | UpdateText newText ->
        { model with Text = newText }


// We have an input box that listens for an OnInput event.
// When a user types in the input box, an OnInput event will get triggered and the input
// box's text value will get passed with the UpdateText as a string.
// That's why the Msg type has the value (UpdateText of string).
// The string that gets passed along with the message to the update function is the
// string of text that's in the input box.
// We display the model.Text value in a div element underneath the input box.
let root model dispatch : ReactElement =
    div [ ClassName "text-center" ]
        [ input 
            [ OnInput(fun e -> !!e.target?value |> UpdateText |> dispatch) 
              valueOrDefault model.Text ]
          div [] [ str model.Text ] ]


Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run