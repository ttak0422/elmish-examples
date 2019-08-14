module HelloWorld04

// I'm importing the Fable.React.Props module, which has all the HtmlAttr we need.
// I'm exposing the ClassName attribute, which we can use for adding classes to HTML elements.
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
            Now the View.root value has a div element which has a class of "text-center".
            Since we're using Bootstrap, this will make it so child text node is centered.
            So now the "Hello, World" message is centered.
        *)
        div [ ClassName "text-center" ] [ str "Hello, World" ]

Program.mkSimple State.init State.update View.root
|> Program.withReactBatched "elmish-app"
|> Program.run