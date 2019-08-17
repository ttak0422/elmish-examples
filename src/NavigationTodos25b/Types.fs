namespace NavigationTodos25b

open NavigationTodos25b.Utils
open NavigationTodos25b.Datas

type Model =
    { Text : string
      Todos : Todo list
      Editing : Option<TodoEdit>
      Filter : Filter }

type Msg =
    | UpdateText of string
    | GenerateTodoId
    | AddTodo of Id
    | RemoveTodo of Id
    | Edit of Id * string
    | EditSave of Id * string
    | ToggleTodo of Id
    | Failure of string
