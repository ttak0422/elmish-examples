// More "Elmish"ish
module NavigationTodos25b

open Elmish
open Elmish.Navigation
open Elmish.React
open Elmish.UrlParser
open NavigationTodos25b.Route
open NavigationTodos25b.Datas
open NavigationTodos25b.State
open NavigationTodos25b.View

let todos = Todo.loadTodos()

Program.mkProgram (init todos) update root
|> Program.withSubscription subscriptions
|> Program.toNavigable (parseHash urlParser) urlUpdate
|> Program.withReactBatched "elmish-app"
|> Program.run