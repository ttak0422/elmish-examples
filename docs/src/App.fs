module App.View

open Elmish
open Elmish.UrlParser
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open App.Types
open App.State
open Global
open Helper

importAll "../sass/main.sass"

open Fable.React
open Fable.React.Props

let menuItem label page currentPage =
    li [] [ a [ classList [ "is-active", page = currentPage ]
                Href(toHash page) ] [ str label ] ]

let menu currentPage =
    aside [ ClassName "menu" ]
        [ p [ ClassName "menu-label" ] [ str "General" ]

          ul [ ClassName "menu-list" ]
              [ menuItem "About" Page.About currentPage
                menuItem "HelloWorld01" HelloWorld01 currentPage
                menuItem "HelloWorld01b" HelloWorld01b currentPage
                menuItem "HelloWorld02" HelloWorld02 currentPage
                menuItem "HelloWorld03" HelloWorld03 currentPage
                menuItem "HelloWorld04" HelloWorld04 currentPage
                menuItem "Counter05" Counter05 currentPage
                menuItem "Counter06" Counter06 currentPage
                menuItem "Counter07" Counter07 currentPage
                menuItem "Counter08" Counter08 currentPage
                menuItem "Counter09" Counter09 currentPage
                menuItem "Counter10" Counter10 currentPage
                menuItem "Counter11" Counter11 currentPage
                menuItem "Counter12" Counter12 currentPage
                menuItem "InputBox13" InputBox13 currentPage
                menuItem "Todos14" Todos14 currentPage
                menuItem "Todos15" Todos15 currentPage
                menuItem "Todos16" Todos16 currentPage
                menuItem "Todos17" Todos17 currentPage
                menuItem "EditableTodos18" EditableTodos18 currentPage
                menuItem "EditableTodos19" EditableTodos19 currentPage
                menuItem "EditableTodos20" EditableTodos20 currentPage

                menuItem "LocalStorageEditableTodos21"
                    LocalStorageEditableTodos21 currentPage

                menuItem "LocalStorageEditableTodos22"
                    LocalStorageEditableTodos22 currentPage
                menuItem "FilterTodos23" FilterTodos23 currentPage
                menuItem "FilterTodos24" FilterTodos24 currentPage

                menuItem "NavigationTodos25"
                    (NavigationTodos25 NavigationTodos25.Filter.All) currentPage

                menuItem "NavigationTodos25b"
                    (NavigationTodos25b NavigationTodos25b.Route.Top)
                    currentPage ] ]

let root model dispatch =
    let pageHtml page =
        match page with
        | Page.About -> Info.View.root
        | HelloWorld01 ->
            HelloWorld01.root model.HelloWorld01 (HelloWorld01Msg >> dispatch)
        | HelloWorld01b ->
            HelloWorld01b.View.root model.HelloWorld01b
                (HelloWorld01bMsg >> dispatch)
        | HelloWorld02 ->
            HelloWorld02.root model.HelloWorld02 (HelloWorld02Msg >> dispatch)
        | HelloWorld03 ->
            HelloWorld03.root model.HelloWorld03 (HelloWorld03Msg >> dispatch)
        | HelloWorld04 ->
            HelloWorld04.root model.HelloWorld04 (HelloWorld04Msg >> dispatch)
        | Counter05 -> Counter05.root model.Counter05 (Counter05Msg >> dispatch)
        | Counter06 -> Counter06.root model.Counter06 (Counter06Msg >> dispatch)
        | Counter07 -> Counter07.root model.Counter07 (Counter07Msg >> dispatch)
        | Counter08 -> Counter08.root model.Counter08 (Counter08Msg >> dispatch)
        | Counter09 -> Counter09.root model.Counter09 (Counter09Msg >> dispatch)
        | Counter10 -> Counter10.root model.Counter10 (Counter10Msg >> dispatch)
        | Counter11 -> Counter11.root model.Counter11 (Counter11Msg >> dispatch)
        | Counter12 -> Counter12.root model.Counter12 (Counter12Msg >> dispatch)
        | InputBox13 ->
            InputBox13.root model.InputBox13 (InputBox13Msg >> dispatch)
        | Todos14 -> Todos14.root model.Todos14 (Todos14Msg >> dispatch)
        | Todos15 -> Todos15.root model.Todos15 (Todos15Msg >> dispatch)
        | Todos16 -> Todos16.root model.Todos16 (Todos16Msg >> dispatch)
        | Todos17 -> Todos17.root model.Todos17 (Todos17Msg >> dispatch)
        | EditableTodos18 ->
            EditableTodos18.root model.EditableTodos18
                (EditableTodos18Msg >> dispatch)
        | EditableTodos19 ->
            EditableTodos19.root model.EditableTodos19
                (EditableTodos19Msg >> dispatch)
        | EditableTodos20 ->
            EditableTodos20.root model.EditableTodos20
                (EditableTodos20Msg >> dispatch)
        | LocalStorageEditableTodos21 ->
            LocalStorageEditableTodos21.root model.LocalStorageEditableTodos21
                (LocalStorageEditableTodos21Msg >> dispatch)
        | LocalStorageEditableTodos22 ->
            LocalStorageEditableTodos22.root model.LocalStorageEditableTodos22
                (LocalStorageEditableTodos22Msg >> dispatch)
        | FilterTodos23 ->
            FilterTodos23.root model.FilterTodos23
                (FilterTodos23Msg >> dispatch)
        | FilterTodos24 ->
            FilterTodos24.root model.FilterTodos24
                (FilterTodos24Msg >> dispatch)
        | NavigationTodos25 _ ->
            Nav25.root model.NavigationTodos25
                (NavigationTodos25Msg >> dispatch)
        | NavigationTodos25b _ ->
            Nav25b.root model.NavigationTodos25b
                (NavigationTodos25bMsg >> dispatch)
    div []
        [ Navbar.View.root

          div [ ClassName "section" ]
              [ div [ ClassName "container" ]
                    [ div [ ClassName "columns" ]
                          [ div [ ClassName "column is-3" ]
                                [ menu model.CurrentPage ]

                            div [ ClassName "column" ]
                                [ pageHtml model.CurrentPage ] ] ] ] ]

open Elmish.React
open Elmish.Debug
open Elmish.HMR

// App
Program.mkProgram init update root
|> Program.toNavigable (parseHash pageParser) urlUpdate
#if DEBUG
|> Program.withDebugger
#endif

|> Program.withReactBatched "elmish-app"
|> Program.run
