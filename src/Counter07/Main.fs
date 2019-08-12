module HelloWorld01

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

module Types =
    type Model = int

    type Msg =
        | Increment
        | Decrement

module State =
    let init _ : Types.Model = 0

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