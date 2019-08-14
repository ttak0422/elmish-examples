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
    let root (model: Model) dispatch =
        str "Hello, World"

open State
open View

// `Program.mkSimple` likes `sandbox` in Elm
Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run