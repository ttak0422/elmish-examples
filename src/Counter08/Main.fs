module Counter08

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props


type Model = int

// We've added another new Msg value that we're going to call Reset.
type Msg =
    | Increment
    | Decrement
    | Reset


let init _ : Model = 0

// We added a new entry in the case expression that checks for if the messae is Reset.
// If it is, then the new model value will be 0.
let update msg (model : Model) =
    match msg with
    | Increment -> model + 1
    | Decrement -> model - 1
    | Reset -> 0


let root (model : Model) dispatch : ReactElement =
    div [ ClassName "text-center" ]
        [ div [] [ str <| string model ]
          div [ ClassName "btn-group" ]
            [ button [ ClassName "btn btn-primary"
                       OnClick (fun _ -> dispatch Increment) ]
                [ str "+" ]
              button [ ClassName "btn btn-danger"
                       OnClick (fun _ -> dispatch Decrement) ]
                [ str "-" ]
              // We added a new button that will trigger an event 
              // that will pass the Reset value as a message to the update function.
              button [ ClassName "btn btn-default"
                       OnClick (fun _ -> dispatch Reset) ]
                [ str "Reset" ] ] ]


Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run