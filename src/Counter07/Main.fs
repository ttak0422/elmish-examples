﻿module Counter07

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

module Types =
    type Model = int

    // We've added a new value called Decrement that is type Msg.
    // Think of the Msg type as a type that can either be Increment or it can be Decrement.
    // We use "|" between all the possible values a Msg type can be.
    type Msg =
        | Increment
        | Decrement

module State =
    let init _ : Types.Model = 0

    // Now that there are 2 possible Msg values, we added a new entry to the case expression
    // that deals with messages that are equal to Decrement value.
    // When the message is a Decrement value, 
    // the new model value that's returned is one less than what it was.
    // After the new models state is returned, 
    // the view function will get passed the new modek value and return the new HTML,
    // which will get displayed for the user to see.
    let update msg (model : Types.Model) =
        match msg with
        | Types.Increment -> model + 1
        | Types.Decrement -> model - 1

module View =    
    let root (model : Types.Model) dispatch : ReactElement =
        div [ ClassName "text-center" ]
            [ div [] [ str <| string model ]
              div [ ClassName "btn-group" ]
                [ button [ ClassName "btn btn-primary"
                           OnClick (fun _ -> dispatch Types.Increment) ]
                    [ str "+" ]
                  button [ ClassName "btn btn-danger"
                           OnClick (fun _ -> dispatch Types.Decrement) ]
                    [ str "-" ] ] ]

Program.mkSimple State.init State.update View.root
|> Program.withReactBatched "elmish-app"
|> Program.run