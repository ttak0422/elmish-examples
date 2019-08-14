module HelloWorld04

// I'm importing the Fable.React.Props module, which has all the HtmlAttr we need.
// I'm exposing the ClassName attribute, which we can use for adding classes to HTML elements.
open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props


type Model = DummyModel

type Msg = DummyMsg


let init _ = DummyModel

let update msg model = model


let root model dispatch : ReactElement =
    (*
        Now the View.root value has a div element which has a class of "text-center".
        Since we're using Bootstrap, this will make it so child text node is centered.
        So now the "Hello, World" message is centered.
    *)
    div [ ClassName "text-center" ] [ str "Hello, World" ]


Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run