// This is how you write single-line comments in F#.

(*
    This is how you
    write multi-line comments
    in F#.
    (*
        This is how you write nested commts in F#.
    *)
*)

// This is how you declare what your module name is
// The default is public.
module HelloWorld01

// We're importing the Elmish and Elmish.React, Fable.React modules
// the text value available in our file,
// so we can just reference it if we want.
open Elmish
open Elmish.React
open Fable.React

(*
    Unlike Elm, Elmish needs to follow TEA(model, update, view).
    So I declare dummy model and update in this sample.
*)


type Model = DummyModel

type Msg = DummyMsg


let init() = DummyModel

let update msg model = model


let root (model: Model) dispatch =
    str "Hello, World"


// `Program.mkSimple` likes `sandbox` in Elm
Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run