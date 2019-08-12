// 一行のコメント

(*
    複数行の
    コメント
    (* コメントのネスト *)
*)

// exportするモジュールの宣言
module HelloWorld01

// importするモジュールの宣言
open Elmish
open Elmish.React
open Fable.React

(*
    Elmとは違いElmishではTEAのmodel, update, viewに従う必要がある
    ここではダミーのmodel, update, モデルに依存しないviewを宣言している
*)

module Types =
    type Model = Dummy

    type Msg = Dummy

module State =
    let init _ = Types.Dummy

    let update msg model = model

module View =
    // 型宣言を書く必要はないものの書いておく方がいい場面もある
    // 型宣言を書くにはこう
    let root (model: Types.Model) dispatch : ReactElement =
        str "Hello, World"

Program.mkSimple State.init State.update View.root
|> Program.withReactBatched "elmish-app"
|> Program.run