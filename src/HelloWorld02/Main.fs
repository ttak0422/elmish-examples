module HelloWorld02

open Elmish
open Elmish.React
open Fable.React


type Model = DummyModel

type Msg = DummyMsg


let init _ = DummyModel

let update msg model = model


// In F#, we can explicitly say what any value's type is.
// Since the root value is just an HTML node, it has the type (ReactElement).
let root (model: Model) dispatch : ReactElement =
    str "Hello, World"


Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run