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
            We've made it so the view value isn't  just a text node any more.
            It's now a div element with a text node as a child.
            The Fable.React module has all the ReactElement elements you need.
            div takes 2 arguments which are both lists.
            The first list is a list of attributes, the second list is a list of child ReactElement elements.
            We can nest elements the same way we normally do with HTML.
        *)
        div [] [ str "Hello, World" ]

Program.mkSimple State.init State.update View.root
|> Program.withReactBatched "elmish-app"
|> Program.run