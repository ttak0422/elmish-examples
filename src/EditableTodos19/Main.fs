module EditableTodos19

// Let's use mkProgram from now on instead of mkSimple.
// This will allow us to do side effects with commands and subscriptions.
// I'll explain more about commands and subscriptions later,
// but for now just think of them as the 2 only ways to do side effects in Elmish.
// Having side effects be really controled like this is really nive for large projects
// because side effects can basically happen in 2 locations,
// so it makes programs easier to understand.
open Elmish
open Elmish.React
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props


type TodoEdit =
    { Index : int
      Text : string }

type Model =
    { Text : string
      Todos : string list
      Editing : TodoEdit option } // Option<TodoEdit>

type Msg =
    | UpdateText of string
    | AddTodo
    | RemoveTodo of int
    | Edit of int * string
    | EditSave of int * string


let init() : Model * Cmd<Msg> =
    { Text = ""
      Todos = [ "Laundry"; "Dishes" ]
      Editing = None }, Cmd.none

// We're using mkProgram now instead of mkSimple, so our update function is slightly different.
// The update function is mostly the same as before but now instead of just returning the model,
// we now return a tuple containing the new model value and a command which can perform side effects.
// We don't need to do any side effects, so we've just added Cmd.none as the command for
// each returning value of the match expression. You'll see how they work later.
// Just think of commands as a way of asking for some side effect to happen and think of
// subscriptions as a way of listening or subscribing to the result of some side effect.
// Commands get returned from the update function and the resulting values
// produced from subscriptions get passed as a message to the update function.
let update msg model : Model * Cmd<Msg> =
    match msg with
    | UpdateText newText -> { model with Text = newText }, Cmd.none
    | AddTodo ->
        { model with Text = ""
                     Todos = model.Todos @ [ model.Text ] }
        , Cmd.none
    | RemoveTodo index ->
        let beforeTodos = List.take index model.Todos
        let afterTodos = List.skip (index + 1) model.Todos
        let newTodos = beforeTodos @ afterTodos
        { model with Todos = newTodos }, Cmd.none
    | Edit(index, todoText) ->
        { model with Editing =
                         Some { Index = index
                                Text = todoText } }, Cmd.none
    | EditSave(index, todoText) ->
        let newTodos =
            model.Todos
            |> List.indexed
            |> List.map (fun (i, todo) ->
                   if i = index then todoText
                   else todo)
        { model with Editing = None
                     Todos = newTodos }, Cmd.none

// We don't need subscriptions, so we're just going to have the subscription function return
// Cmd.ofSub ignore, which indicates we have no subscriptions.
// I'll explain subscriptions more in the future when we use them,
// so don't worry about them right now.
let subscriptions dispatch : Cmd<Msg> =
    Cmd.ofSub ignore


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
                                                    valueOrDefault model.Text
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


// We are now using mkProgram instead of mkSimple,
// which takes a record with the properties: init, update, root and subscriptions.
// The init property is similar to the init property in mkSimple except that it takes a
// function that takes a function that takes in flags and return a tuple of type Model * Cmd<Msg>
// The Cmd<Msg> is useful for if you want to perform any side effects in the beginning of the program.
// You usually don't need to perform any side effects, so you just put the value Cmd.none
// as the command value whenever you don't need to do any commands.
Program.mkProgram init update root
|> Program.withReactBatched "elmish-app"
|> Program.withSubscription subscriptions
|> Program.run
