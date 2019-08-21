namespace NavigationTodos25b

module View =
    open Fable.Core.JsInterop
    open Fable.React
    open Fable.React.Props
    open NavigationTodos25b.Datas

    let filterTodos (filter : Filter) (todos : List<Todo>) : List<Todo> =
        match filter with
        | All -> todos
        | Incomplete -> List.filter (fun t -> not t.Completed) todos
        | Completed -> List.filter (fun t -> t.Completed) todos

    let onEnter (msg : Msg) (dispatch : Msg -> unit) : DOMAttr =
        function
        | (ev : Browser.Types.KeyboardEvent) when ev.keyCode = 13. ->
            dispatch msg
        | _ -> ()
        |> OnKeyDown

    let onInput (f : string -> Msg) (dispatch : Msg -> unit) : DOMAttr =
        OnInput(fun ev ->
            !!ev.target?value
            |> f
            |> dispatch)

    let onClick (msg : Msg) (dispatch : Msg -> unit) : DOMAttr =
        OnClick(fun _ -> dispatch msg)
    let onDoubleClick (msg : Msg) (dispatch : Msg -> unit) : DOMAttr =
        OnDoubleClick(fun _ -> dispatch msg)

    let viewFilter (filter : Filter) (isFilter : bool) (fitlerText : string) : ReactElement =
        if isFilter then span [ ClassName "mr-3" ] [ str fitlerText ]
        else
            a [ ClassName "text-primary mr-3"
                Href <| "#" + fitlerText.ToLower()
                Style [ Cursor "pointer" ] ] [ str fitlerText ]

    let viewFilters (filter : Filter) : ReactElement =
        div [] [ viewFilter All (filter = All) "All"
                 viewFilter Incomplete (filter = Incomplete) "Incomplete"
                 viewFilter Completed (filter = Completed) "Completed" ]

    let viewEditTodo (todoEdit : TodoEdit) (dispatch : Msg -> unit) : ReactElement =
        div [ ClassName "card" ]
            [ div [ ClassName "card-block" ]
                  [ div
                        [ onEnter (EditSave(todoEdit.Id, todoEdit.Text))
                              dispatch ] [ input [ onInput
                                                       (fun text ->
                                                       (todoEdit.Id, text)
                                                       |> Edit) dispatch
                                                   ClassName "form-control"
                                                   Value todoEdit.Text ] ] ] ]

    let viewNormalTodo (todo : Todo) (dispatch : Msg -> unit) : ReactElement =
        div [ ClassName "card" ]
            [ div [ ClassName "card-block" ]
                  [ input [ onClick (ToggleTodo todo.Id) dispatch
                            Type "checkbox"
                            Checked todo.Completed
                            ClassName "mr-3" ]

                    span [ onDoubleClick (Edit(todo.Id, todo.Text)) dispatch
                           Style [ TextDecoration(if todo.Completed then
                                                      "line-through"
                                                  else "none") ] ]
                        [ str todo.Text ]
                    span [ onClick (RemoveTodo todo.Id) dispatch
                           ClassName "float-right" ] [ str "✖" ] ] ]

    let viewTodo (editing : Option<TodoEdit>) (todo : Todo)
        (dispatch : Msg -> unit) : ReactElement =
        match editing with
        | Some todoEdit ->
            if todoEdit.Id = todo.Id then viewEditTodo todoEdit dispatch
            else viewNormalTodo todo dispatch
        | _ -> viewNormalTodo todo dispatch

    let root (model : Model) (dispatch : Msg -> unit) : ReactElement =
        div [ ClassName "col-12 col-sm-6 offset-sm-3" ]
            [ div [ ClassName "row" ]
                  [ div [ ClassName "col-9" ]
                        [ input [ onInput UpdateText dispatch
                                  Value model.Text
                                  AutoFocus true
                                  ClassName "form-control"
                                  Placeholder "Enter a todo"
                                  onEnter GenerateTodoId dispatch ] ]

                    div [ ClassName "col-3" ]
                              [ button
                                    [ onClick GenerateTodoId dispatch
                                      ClassName "btn btn-primary form-control" ]
                                    [ str "+" ] ] ]
              viewFilters model.Filter

              div []
                  (model.Todos
                   |> filterTodos model.Filter
                   |> List.map
                          (fun todo ->
                          viewTodo model.Editing todo dispatch)) ]  
