module Todo15


open Elmish
open Elmish.React
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props


type Model = 
    { Text : string 
      Todos : string list }

// We added a (RemoveTodo of int) value to the Msg,
// which allo us to remove a todo by index.
type Msg = 
    | UpdateText of string
    | AddTodo
    | RemoveTodo of int


let init() : Model =
    { Text = ""
      Todos = [] }

let update msg model : Model =
    match msg with
    | UpdateText newText ->
        { model with Text = newText }    
    | AddTodo ->
        { model with
            Text = ""
            Todos = model.Todos @ [ model.Text] }    
    | RemoveTodo index ->
        let beforeTodos = List.take index model.Todos
        let afterTodos = List.skip (index + 1) model.Todos
        let newTodos = beforeTodos @ afterTodos
        { model with Todos = newTodos }



let root model dispatch : ReactElement =
    let makeTodo = 
        List.indexed 
        >> List.map (fun (index, todo) -> 
                    div [] 
                        [ str todo 
                          span [ OnClick (fun _ -> dispatch (RemoveTodo index))] [ str " X" ]])
    div [ ClassName "text-center" ]
        [ input 
            [ OnInput(fun e -> !!e.target?value |> UpdateText |> dispatch) 
              valueOrDefault model.Text
              AutoFocus true ]
          button [ OnClick (fun _ -> dispatch AddTodo); ClassName "btn btn-primary" ] [ str "Add Todo" ]
          div [] (makeTodo model.Todos)]


Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run