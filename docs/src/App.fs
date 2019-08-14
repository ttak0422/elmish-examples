module App.View

open Elmish
open Elmish.UrlParser
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open App.Types
open App.State
open Global

importAll "../sass/main.sass"

open Fable.React
open Fable.React.Props

let menuItem label page currentPage =
    li
      [ ]
      [ a
          [ classList [ "is-active", page = currentPage ]
            Href (toHash page) ]
          [ str label ] ]

let menu currentPage =
  aside
    [ ClassName "menu" ]
    [ p
        [ ClassName "menu-label" ]
        [ str "General" ]
      ul
        [ ClassName "menu-list" ]
        [ menuItem "About" Page.About currentPage
          menuItem "HelloWorld01" Page.HelloWorld01 currentPage
          menuItem "HelloWorld01b" Page.HelloWorld01b currentPage
          menuItem "HelloWorld03" Page.HelloWorld03 currentPage
          menuItem "HelloWorld04" Page.HelloWorld04 currentPage
          menuItem "Counter05" Page.Counter05 currentPage
          menuItem "Counter07" Page.Counter07 currentPage
          menuItem "Counter08" Page.Counter08 currentPage
          menuItem "Counter09" Page.Counter09 currentPage
          menuItem "Counter10" Page.Counter10 currentPage
          menuItem "Counter11" Page.Counter11 currentPage
          menuItem "Counter12" Page.Counter12 currentPage ] ]

let root model dispatch =

  let pageHtml page =
    match page with
    | Page.About -> Info.View.root
    | HelloWorld01 -> HelloWorld01.View.root model.HelloWorld01 (HelloWorld01Msg >> dispatch)
    | HelloWorld01b -> HelloWorld01b.View.root model.HelloWorld01b (HelloWorld01bMsg >> dispatch)
    | HelloWorld03 -> HelloWorld03.View.root model.HelloWorld03 (HelloWorld03Msg >> dispatch)
    | HelloWorld04 -> HelloWorld04.View.root model.HelloWorld04 (HelloWorld04Msg >> dispatch)
    | Counter05 -> Counter05.View.root model.Counter05 (Counter05Msg >> dispatch)
    | Counter07 -> Counter07.View.root model.Counter07 (Counter07Msg >> dispatch)
    | Counter08 -> Counter08.View.root model.Counter08 (Counter08Msg >> dispatch)
    | Counter09 -> Counter09.View.root model.Counter09 (Counter09Msg >> dispatch)
    | Counter10 -> Counter10.View.root model.Counter10 (Counter10Msg >> dispatch)
    | Counter11 -> Counter11.View.root model.Counter11 (Counter11Msg >> dispatch)
    | Counter12 -> Counter12.View.root model.Counter12 (Counter12Msg >> dispatch)

  div
    []
    [ Navbar.View.root
      div
        [ ClassName "section" ]
        [ div
            [ ClassName "container" ]
            [ div
                [ ClassName "columns" ]
                [ div
                    [ ClassName "column is-3" ]
                    [ menu model.CurrentPage ]
                  div
                    [ ClassName "column" ]
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
