module HelloWorld01

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

module Types =
    type Model = Dummy

    type Msg = Dummy

module State =
    let init _ = Types.Dummy

    let update msg model = model

module View =
    let root model dispatch : ReactElement =
        (*
            Bootstrapを使用しているのでメッセージが中央に位置するようになる。
        *)
        div [ ClassName "text-center" ] [ str "Hello, World" ]

Program.mkSimple State.init State.update View.root
|> Program.withReactBatched "elmish-app"
|> Program.run