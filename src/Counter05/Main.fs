module Counter05

// We've exposed the button function, which will be dispalayed as a button element.
// We are exposing the OnClick function from the Fable.React module.
// We use this similarly to how we use the onclick attribute in native HTML.

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

module Types =
    // We are creating a type alias called Model.
    // Type aliases don't create a new type, they just make it so the problem is easier to understand.
    type Model = int

    // We created a new type that we're calling  Msg.
    // The Msg type can only be the value Increment.
    type Msg = Increment

module State =
    let init _ : Types.Model = 0
    // The udpate function will get called whenever an event is triggered.
    // The message value will be passed in as the first value and the current model state 
    // will be passed in as the second value.
    // The update function returns the new model state, which will be passed the view function.
    let update msg (model : Types.Model) =
        match msg with
        // Here we're using a case expression, which is similar to a switch statement in JavaScript.
        // We are checking to see what value the msg argument is.        
        // If the msg argument is Increment, then we're going to return the model value plus one.
        // So we are effectively incrementing the model's value by one.
        // This new model value will return the new HTML that gets rendered to the page.
        | Types.Increment -> model + 1

module View =
    open Types
    // This is the view function.
    // The view function takes the model and dispatcher, then return a ReactElement, which gets displayed on the screen.
    // Every time the modek gets updated, the new value for the model will get passed into the view function, 
    // which will output the HTML display.    
    let root model dispatch =
        div [ ClassName "text-center" ]
            [ div [] [ str <| string model ]
              button [ ClassName "btn btn-primary"
                       // The OnClick function takes dispatcher and Increment value and will trigger an event 
                       // whenerver the user clicks on the button.
                       // When an event is triggered, the message value gets passed to the update function,
                       // then the update function returns the new model state.
                       // So whenerver a user clicks the button, the onClick event will get triggered
                       // which will pass the Increment value to the update funciton.
                       OnClick (fun _ -> dispatch Increment) ]
                [ str "+" ] ]

// Program.mkSimple will allow us to write an interactive application 
// instead of just static HTML application.
// mkSimple takes init and root(view) and update.
// the init is the initial value that the model is set to.
// the root is view function which takes the model and duspatcher, 
// the update is a function that takes a message and the model as arguments and returns the new model.
open State
open View

Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run