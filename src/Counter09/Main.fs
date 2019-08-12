module HelloWorld01

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

module Types =
    type Model = int list

    type Msg =
        | Increment of int

module State =
    let init _ : Types.Model = [ 0; 0 ]

    let update msg (model : Types.Model) =
        match msg with
        | Types.Increment index ->
            model
            |> List.indexed
            |> List.map (fun (idx, cnt) ->
                if idx = index then
                    cnt + 1
                else
                    cnt)

module View =

    let root (model : Types.Model) dispatch : ReactElement =
        (*
            Elmishではイベントの発火にdispatcherが必要なので
            ローカル関数にしてdispatcherにアクセスできるようにしたり、
            引数として渡したりする必要がある。
        *)
        let viewCount (index, count) : ReactElement =
            div [ ClassName "mb-2" ]
                [ str <| string count
                  button
                    [ ClassName "btn btn-primary"
                      OnClick (fun _ -> dispatch <| Types.Increment index)]
                    [ str "+" ]]
        div [ ClassName "text-center" ]
            (model
             |> List.indexed
             |> List.map viewCount)

Program.mkSimple State.init State.update View.root
|> Program.withReactBatched "elmish-app"
|> Program.run