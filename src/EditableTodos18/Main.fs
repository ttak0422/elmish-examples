module Todos17

open Elmish
open Elmish.React
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props


// We are creating a new type alias which is just a record that will represent the editing state.
// The index property is the index of the todo we're editing and the text is that's string value.
type TodoEdit =
    { Index : int
      Text : string }

// We are creating a new property called Editing, which will keep track of the editing state.
// It's type is TodoEdit option instead of just TodoEdit.
// We are usin a option type which is similar to nullable values in JavaScript.
// The option equivalent of null is None value. When a Option value is not None,
// then it is Some something. For example,
// if I have a int option (Option<int>) type, that means it can be None or it can be
// Some 1 or Some 2 or Some 3, etc. Don't worry if it doesn't make complete sense yet.
// Just think of Option type as nullable types in JavaScript.
type Model =
    { Text : string
      Todos : string list
      Editing : TodoEdit option } // Option<TodoEdit>

// We added 2 new values to the Msg union type, (Edit of int * string) and (EditSave of int * string).
// These values will be used to represent editing a todo.
// The (Edit of int * string) value is used for keeping track of the current todo being edited.
// The int represents the index of the todo and string represents the text for that todo.
// The (EditSave of int * string) value will be used to save the new value of the todo.
// The int is todo index, the string is the string we want to save that todo as.
type Msg =
    | UpdateText of string
    | AddTodo
    | RemoveTodo of int
    | Edit of int * string
    | EditSave of int * string


// I set todos property of the model to the list [ "Laundry"; "Dished" ] in the begining.
// The editing property os set to None in the begining since we aren't editing in the begining.
let init() : Model =
    { Text = ""
      Todos = [ "Laundry"; "Dishes" ]
      Editing = None }

let update msg model : Model =
    match msg with
    | UpdateText newText -> { model with Text = newText }
    | AddTodo ->
        { model with Text = ""
                     Todos = model.Todos @ [ model.Text ] }
    | RemoveTodo index ->
        let beforeTodos = List.take index model.Todos
        let afterTodos = List.skip (index + 1) model.Todos
        let newTodos = beforeTodos @ afterTodos
        { model with Todos = newTodos }
    // We are just setting the editing property in the model the editing value
    // that we can use to represent the edit.
    | Edit(index, todoText) ->
        { model with Editing =
                         Some { Index = index
                                Text = todoText } }
    // We use a let expression to create the new todos. We use List.indexed and List.map
    // and change the string of the todo at the editing index to the new string.
    // We also set the editing property to None because we aren't editing anymore.
    | EditSave(index, todoText) ->
        let newTodos =
            model.Todos
            |> List.indexed
            |> List.map (fun (i, todo) ->
                   if i = index then todoText
                   else todo)
        { model with Editing = None
                     Todos = newTodos }


let root model dispatch : ReactElement =
    let onEnter msg : DOMAttr =
        function
        | (ev : Browser.Types.KeyboardEvent) when ev.keyCode = 13. ->
            dispatch msg            
        | _ -> ()
        |> OnKeyDown

    let viewEditTodo index todoEdit : ReactElement =
        div [ ClassName "card" ]
            [ div [ ClassName "card-block" ]
                  [ div [ onEnter (EditSave(todoEdit.Index, todoEdit.Text)) ]
                        [ input [ OnInput(fun e ->
                                      !!e.target?value
                                      |> fun x ->
                                          (index, x)
                                          |> Edit
                                          |> dispatch)
                                  ClassName "form-control"
                                  Value todoEdit.Text ] ] ] ]

    let viewNormalTodo index todo : ReactElement =
        div [ ClassName "card" ]
            [ div [ ClassName "card-block" ]
                  [ span
                        [ OnDoubleClick(fun _ -> dispatch <| Edit(index, todo)) ]
                        [ str todo ]
                    span [ OnClick(fun _ -> dispatch <| RemoveTodo index)
                           ClassName "float-right" ] [ str "✖" ] ] ]

    let viewTodo editing index todo : ReactElement =
        match editing with
        | Some todoEdit ->
            if todoEdit.Index = index then viewEditTodo index todoEdit
            else viewNormalTodo index todo
        | _ -> viewNormalTodo index todo

    div [ ClassName "col-12 col-sm-6 offset-sm-3" ]
        [ div [ ClassName "row" ]
              [ div [ ClassName "col-9" ] [ input [ OnInput(fun e ->
                                                        !!e.target?value
                                                        |> UpdateText
                                                        |> dispatch)
                                                    Value model.Text
                                                    AutoFocus true
                                                    ClassName "form-control"
                                                    Placeholder "Enter a todo"
                                                    onEnter AddTodo ] ]

                div [ ClassName "col-3" ]
                    [ button [ OnClick(fun _ -> dispatch AddTodo)
                               ClassName "btn btn-primary form-control" ]
                          [ str "+" ] ] ]
          div [] (model.Todos
                  |> List.indexed
                  |> List.map (fun (i, todo) -> viewTodo model.Editing i todo)) ]

Program.mkSimple init update root
|> Program.withReactBatched "elmish-app"
|> Program.run
