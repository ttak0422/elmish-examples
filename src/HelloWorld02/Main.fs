module HelloWorld02

open Elmish
open Elmish.React
open Fable.React

// dummy
module Types =
    type Model = Dummy

    type Msg = Dummy

// dummy
module State =
    let init _ = Types.Dummy

    let update msg model = model


module View =
    open Types    
    // In F#, we can explicitly say what any value's type is.
    // Since the root value is just an HTML node, it has the type (ReactElement).
    let root (model: Model) dispatch : ReactElement =
        str "Hello, World"

open State
open View

Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run