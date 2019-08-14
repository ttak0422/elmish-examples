module Counter11

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props


type Model = int list

// We've added the (Decrement of int) value to the Msg union type.
// (Decrement of int) will work in a similar way that (Increment of int) works
// except it will decrement the counter at the specified index instead of incrementing it.
type Msg =
    | Increment of int
    | Decrement of int
    | AddCount


let init _ : Model = [ 0; 0 ]

let update msg (model : Model) =
    match msg with
    | Increment index ->
        model
        |> List.indexed
        |> List.map (fun (idx, cnt) ->
            if idx = index then
                cnt + 1
            else
                cnt)
    // We added an expression that handles the (Decrement of int) message value,
    // which decrements the counter at the index that we care about.
    | Decrement index ->
        model
        |> List.indexed
        |> List.map (fun (idx, cnt) ->
            if idx = index then
                cnt - 1
            else
                cnt)
    | AddCount ->
        model @ [ 0 ]


let root (model : Model) dispatch : ReactElement =
    let viewCount (index, count) : ReactElement =
        div [ ClassName "mb-2" ]
            [ str <| string count
              button
                [ ClassName "btn btn-primary ml-2"
                  OnClick (fun _ -> dispatch <| Increment index) ]
                [ str "+" ]
              // We added a button that will trigger pass a (Decrement of int) message
              // to the update function when it's clicked.
              button
                [ ClassName "btn btn-primary ml-2"
                  OnClick (fun _ -> dispatch <| Decrement index) ]
                [ str "-" ] ]
    div [ ClassName "text-center" ]
        [ div [ ClassName "mb-2" ]
            [ button
                [ ClassName "btn btn-primary"
                  OnClick (fun _ -> dispatch AddCount) ]
                [ str "Add Count" ] ]
          div [] (model |> List.indexed |> List.map viewCount) ]


Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run