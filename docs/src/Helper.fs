module Helper

module Nav25 =
    open Fable.Core.JsInterop
    open Fable.React
    open Fable.React.Props
    open NavigationTodos25

    let filterTodos filter todos =
        match filter with
        | All -> todos
        | Incomplete -> List.filter (fun todo -> not todo.Completed) todos
        | Completed -> List.filter (fun todo -> todo.Completed) todos

    let root model dispatch : ReactElement =
        let onEnter msg : DOMAttr =
            function
            | (ev : Browser.Types.KeyboardEvent) when ev.keyCode = 13. ->
                dispatch msg
            | _ -> ()
            |> OnKeyDown

        let viewFilter filter isFilter filterText filterPath =
            if isFilter then span [ ClassName "mr-3" ] [ str filterText ]
            else
                a [ ClassName "text-primary mr-3"
                    Href("#" + filterPath)
                    Style [ Cursor "pointer" ] ] [ str filterText ]

        let viewFilters filter =
            div []
                [ viewFilter All (filter = All) "All" "navigationtodos25"

                  viewFilter Incomplete (filter = Incomplete)
                      "incomplete" "navigationtodos25/incomplete"

                  viewFilter Completed (filter = Completed)
                      "completed" "navigationtodos25/completed"]

        let viewEditTodo (todoEdit : TodoEdit) : ReactElement =
            div [ ClassName "card" ]
                [ div [ ClassName "card-block" ]
                      [ div [ onEnter (EditSave(todoEdit.Id, todoEdit.Text)) ]
                            [ input [ OnInput(fun e ->
                                          !!e.target?value
                                          |> fun x ->
                                              (todoEdit.Id, x)
                                              |> Edit
                                              |> dispatch)
                                      ClassName "form-control"
                                      Value todoEdit.Text ] ] ] ]

        let viewNormalTodo (todo : Todo) : ReactElement =
            div [ ClassName "card" ]
                [ div [ ClassName "card-block" ]
                      [ input [ OnClick(fun _ -> dispatch <| ToggleTodo todo.Id)
                                Type "checkbox"
                                Checked todo.Completed
                                ClassName "mr-3" ]

                        span [ OnDoubleClick
                                   (fun _ ->
                                   dispatch <| Edit(todo.Id, todo.Text))
                               Style [ TextDecoration(if todo.Completed then
                                                          "line-through"
                                                      else "none") ] ]
                            [ str todo.Text ]
                        span [ OnClick(fun _ -> dispatch <| RemoveTodo todo.Id)
                               ClassName "float-right" ] [ str "✖" ] ] ]

        let viewTodo (editing : TodoEdit option) todo : ReactElement =
            match editing with
            | Some todoEdit ->
                if todoEdit.Id = todo.Id then viewEditTodo todoEdit
                else viewNormalTodo todo
            | _ -> viewNormalTodo todo

        div [ ClassName "col-12 col-sm-6 offset-sm-3" ]
            [ div [ ClassName "row" ]
                  [ div [ ClassName "col-9" ]
                        [ input [ OnInput(fun e ->
                                      !!e.target?value
                                      |> UpdateText
                                      |> dispatch)
                                  Value model.Text
                                  AutoFocus true
                                  ClassName "form-control"
                                  Placeholder "Enter a todo"
                                  onEnter GenerateTodoId ] ]

                    div [ ClassName "col-3" ]
                        [ button [ OnClick(fun _ -> dispatch GenerateTodoId)
                                   ClassName "btn btn-primary form-control" ]
                              [ str "+" ] ] ]
              viewFilters model.Filter
              div [] (model.Todos
                      |> filterTodos model.Filter
                      |> List.map (fun todo -> viewTodo model.Editing todo)) ]

module Nav25b =
    open Fable.React
    open Fable.React.Props
    open NavigationTodos25b    
    open NavigationTodos25b.Datas
    open NavigationTodos25b.View

    let viewFilter (filter : Filter) (isFilter : bool) (fitlerText : string) (filterPath : string) : ReactElement =
        if isFilter then span [ ClassName "mr-3" ] [ str fitlerText ]
        else
            a [ ClassName "text-primary mr-3"
                Href <| "#" + filterPath
                Style [ Cursor "pointer" ] ] [ str fitlerText ]

    let viewFilters (filter : Filter) : ReactElement =
        div [] [ viewFilter All (filter = All) "All" "navigationtodos25b"
                 viewFilter Incomplete (filter = Incomplete) "Incomplete" "navigationtodos25b/incomplete"
                 viewFilter Completed (filter = Completed) "Completed" "navigationtodos25b/completed" ]

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
                        [ button [ onClick GenerateTodoId dispatch
                                   ClassName "btn btn-primary form-control" ]
                              [ str "+" ] ] ]
              viewFilters model.Filter

              div []
                  (model.Todos
                   |> filterTodos model.Filter
                   |> List.map
                          (fun todo -> viewTodo model.Editing todo dispatch)) ]
