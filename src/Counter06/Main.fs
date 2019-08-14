module Counter06

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props


type Model = int

type Msg = Increment


let init _ : Model = 0
// The type declaration for the update function means that the update function 
// takes a Msg and a Model and then returns a Model Type.
let update (msg : Msg) (model : Model) : Model =
    match msg with
    | Increment -> model + 1


// I add the type declaration to the view function so it's easier to understand.
// The view function takes Model and Msg -> unittype and returns ReactElement.
let root (model : Model) (dispatch : Msg -> unit) : ReactElement =
    div [ ClassName "text-center" ]
        [ div [] [ str <| string model ]
          button [ ClassName "btn btn-primary"
                   OnClick (fun _ -> dispatch Increment) ]
            [ str "+" ] ]


Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run