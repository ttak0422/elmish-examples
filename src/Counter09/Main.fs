module Counter09

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

module Types =
    type Model = int list

    // Brefore, we had just one counter, but now we are working with multiple counters.
    // Before, we had the Msg type as Increment, which was enough information for us
    // to know that we can increment our counter. Now that we have multiple counters, 
    // we need to pass information about which counter we want to Increment.
    // So now we have (Increment Int) as out Msg type, 
    // where the Int value will indicate the index of the counter we want to increment.
    // So if we want to increment the counter at index 0, 
    // we can have the OnClick event trigger the message (Increment 0).
    // If we want to increment the counter at index 1, we can trigger the message (Increment 1).
    type Msg =
        | Increment of int

module State =
    // We set the model value to initially be [ 0; 0 ], so our view will display
    // 2 counters that each have the value 0 in the beginning.
    let init _ : Types.Model = [ 0; 0 ]

    let update msg (model : Types.Model) =
        match msg with
        | Types.Increment index ->
            model
            |> List.indexed
            |> List.map (fun (idx, cnt) ->
                if idx = index then
                    cnt + 1
                else
                    cnt)

module View =

    // The view function returns a div element that has a list of counters as its child.
    // The List.indexed functino takes a list and returns a indexed list.
    // The List.map function takes a function and a list and returns a mapped version of the list 
    // that it took as an argument.
    // List.map is similar to Array.prototype.map in JavaScript.
    // The list index will get passed as the first argument to the function and
    // the list value will get passed as the second argument to the function.
    let root (model : Types.Model) dispatch : ReactElement =        
        // This is a function that takes the index value, the count value,
        // and returns the HTML that represents the counter.
        // When the button is clicked, it will trigger an OnClick event which will pass (Increment index)
        // as the message to the update function.
        // The index value is the index that the button is at in the list of buttons.
        // In Elmish, since dispatcher is required for the trigger of the event,
        // it's neccessary to make view function a local function to access it or pass it as an argument.
        let viewCount (index, count) : ReactElement =
            div [ ClassName "mb-2" ]
                [ str <| string count
                  button
                    [ ClassName "btn btn-primary"
                      OnClick (fun _ -> dispatch <| Types.Increment index)]
                    [ str "+" ]]
        div [ ClassName "text-center" ]
            (model
             |> List.indexed
             |> List.map viewCount)

Program.mkSimple State.init State.update View.root
|> Program.withReactBatched "elmish-app"
|> Program.run