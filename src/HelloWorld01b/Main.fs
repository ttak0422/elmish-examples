module HelloWorld01b.Main

(*
    単一のファイルにModel, Update, Viewを書くことはまずない。
    ファイルを分割するならこんな感じ。
    F#の前方参照を基本とする制約はプロジェクトが大きくなるにつれて
    その効果を発揮する。

    <Compile Include="Types.fs" />
    <Compile Include="State.fs" />
    <Compile Include="View.fs" />
    <Compile Include="Main.fs" />

    *.fsprojのファイルの順番がそのまま参照の順番になる。
*)

open Types
open State
open View
open Elmish
open Elmish.React

Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run