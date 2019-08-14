module Counter10

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

module Types =
    type Model = int list

    // We added an AddCount value as our Msg type, which we'll use to indicate
    // that we want to add a counter to our list of counters.
    type Msg =
        | Increment of int
        | AddCount

module State =
    open Types

    let init _ : Types.Model = [ 0; 0 ]

    let update msg (model : Types.Model) =
        match msg with
        | Increment index ->
            model
            |> List.indexed
            |> List.map (fun (idx, cnt) ->
                if idx = index then
                    cnt + 1
                else
                    cnt)
        // When the message type is AddCount, we will append a 0 to the end of the list.
        // We cant get this effect by concatenating 2 lists toghether.
        // The @ operator is the concatenate operator, which takes 2 lists and
        // puts them together.
        | AddCount ->
            model @ [ 0 ]

module View =
    open Types

    let root (model : Model) dispatch : ReactElement =
        let viewCount (index, count) : ReactElement =
            div [ ClassName "mb-2" ]
                [ str <| string count
                  button
                    [ ClassName "btn btn-primary"
                      OnClick (fun _ -> dispatch <| Increment index)]
                    [ str "+" ] ]
        div [ ClassName "text-center" ]
            [ div [ ClassName "mb-2" ]
                // We added a new button that we can click and it will make it
                // so that we add a new counter to the list.
                // When the user clicks on the "Add Count" button,
                // it triggers an OnClick event, which will pass the AddCount value 
                // into the update funciton.
                // The update function will return a list which has an extra integer value at the end of the list,
                // which represents the new counter.
                [ button
                    [ ClassName "btn btn-primary"
                      OnClick (fun _ -> dispatch AddCount) ]
                    [ str "Add Count" ] ]
              div [] (model |> List.indexed |> List.map viewCount) ]

open Types
open State
open View

Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run