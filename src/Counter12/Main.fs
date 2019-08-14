module Counter12

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props


type Model = int list

// We've added a (Remove of int) value to our Msg union type.
// The (Remove of int) value will be represent removing a counter
// at the specified index.
type Msg =
    | Increment of int
    | Decrement of int
    | Remove of int
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
    | Decrement index ->
        model
        |> List.indexed
        |> List.map (fun (idx, cnt) ->
            if idx = index then
                cnt - 1
            else
                cnt)
    | Remove index ->
        // We're using List.take to get the values in the list that are before the index in the list.
        // The List.take function will take the first n elements from the list that it gets passed.
        // For example:
        // List.take 2 [ 1; 3; 5; 4 ] = [ 1; 3 ]
        // List.take 1 [ 3; 2; 1 ] = [ 3 ]
        // List.take  0 [ 5; 6; 7 ] = []
        // Since we want to get all the values in the list that are before index,
        // we can write List.take index model
        let before = List.take index model
        // We're using List.skip to get the value in the list that are after the index in the list.
        // The List.skip function will skip the first n elements form the list that it gets passed.
        // For example:
        // List.skip 2 [ 1; 3; 5; 4 ] = [ 5; 4 ]
        // List.drop 1 [ 3; 2; 1 ] = [ 2; 1 ]
        // List.drop 0 [ 5; 6; 7 ] = [ 5; 6; 7 ]
        // Since we want to get all the values after the index,
        // we can skip the first (index + 1) by writing List.skip (index + 1) model.
        let after = List.skip (index+1) model
        // Since we have the list of values before the removed index and 
        // the list of values after the removed index,
        // we can concatenate them together and that will be the new value that we use as our model
        before @ after
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
              button
                [ ClassName "btn btn-primary ml-2"
                  OnClick (fun _ -> dispatch <| Decrement index) ]
                [ str "-" ]
              button
                [ ClassName "btn btn-primary ml-2"
                  OnClick (fun _ -> dispatch <| Remove index) ]
                [ str "X" ] ]
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