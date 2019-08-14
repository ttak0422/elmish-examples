module HelloWorld01b.Main

(*
    Examples of splitting a file.
    Constraints based on F # forward references become 
    more effective as the project grows.

    <Compile Include="Types.fs" />
    <Compile Include="State.fs" />
    <Compile Include="View.fs" />
    <Compile Include="Main.fs" />

*)

open Types
open State
open View
open Elmish
open Elmish.React

Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run