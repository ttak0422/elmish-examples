﻿module Counter10

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

module Types =
    type Model = int list

    type Msg =
        | Increment of int
        | AddCount

module State =
    open Types

    let init _ : Types.Model = [ 0; 0 ]

    let update msg (model : Types.Model) =
        match msg with
        | Increment index ->
            model
            |> List.indexed
            |> List.map (fun (idx, cnt) ->
                if idx = index then
                    cnt + 1
                else
                    cnt)
        | AddCount ->
            model @ [ 0 ]

module View =
    open Types

    let root (model : Model) dispatch : ReactElement =
        let viewCount (index, count) : ReactElement =
            div [ ClassName "mb-2" ]
                [ str <| string count
                  button
                    [ ClassName "btn btn-primary"
                      OnClick (fun _ -> dispatch <| Increment index)]
                    [ str "+" ] ]
        div [ ClassName "text-center" ]
            [ div [ ClassName "mb-2" ]
                [ button
                    [ ClassName "btn btn-primary"
                      OnClick (fun _ -> dispatch AddCount) ]
                    [ str "Add Count" ] ]
              div [] (model |> List.indexed |> List.map viewCount) ]

open Types
open State
open View

Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run