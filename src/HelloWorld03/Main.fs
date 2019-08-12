module HelloWorld03

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
    let root model dispatch : ReactElement =
        (*
            １つ目のリストには属性を、２つ目のリストにはHTML要素のリストが入る。
        *)
        div [] [ str "Hello, World" ]

Program.mkSimple State.init State.update View.root
|> Program.withReactBatched "elmish-app"
|> Program.run