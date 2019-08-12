module HelloWorld01

open Elmish
open Elmish.React
open Fable.React

module Types =
    type Model = Dummy

    type Msg = Dummy

module State =
    let init _ = Types.Dummy

    let update msg model = model

module View =
    let root model dispatch =
        str "Hello, World"

Program.mkSimple State.init State.update View.root
|> Program.withReactBatched "elmish-app"
|> Program.run