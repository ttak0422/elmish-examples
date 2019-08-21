module App.State

open Elmish
open Elmish.UrlParser
open Elmish.Navigation
open Global
open Types

let pageParser : Parser<Page -> Page, Page> =
    oneOf
        [ map About (s "about")
          map HelloWorld01 (s "helloworld01")
          map HelloWorld01b (s "helloworld01b")
          map HelloWorld02 (s "helloworld02")
          map HelloWorld03 (s "helloworld03")
          map HelloWorld04 (s "helloworld04")
          map Counter05 (s "counter05")
          map Counter06 (s "counter06")
          map Counter07 (s "counter07")
          map Counter08 (s "counter08")
          map Counter09 (s "counter09")
          map Counter10 (s "counter10")
          map Counter11 (s "counter11")
          map Counter12 (s "counter12")
          map InputBox13 (s "inputbox13")
          map Todos14 (s "todos14")
          map Todos15 (s "todos15")
          map Todos16 (s "todos16")
          map Todos17 (s "todos17")
          map EditableTodos18 (s "editabletodos18")
          map EditableTodos19 (s "editabletodos19")
          map EditableTodos20 (s "editabletodos20")
          map LocalStorageEditableTodos21 (s "localstorageeditabletodos21")
          map LocalStorageEditableTodos22 (s "localstorageeditabletodos22")
          map FilterTodos23 (s "filtertodos23")
          map FilterTodos24 (s "filtertodos24")

          map (NavigationTodos25 NavigationTodos25.Filter.All)
              (s "navigationtodos25")

          map (NavigationTodos25 NavigationTodos25.Filter.Incomplete)
              (s "navigationtodos25" </> s "incomplete")

          map (NavigationTodos25 NavigationTodos25.Filter.Completed)
              (s "navigationtodos25" </> s "completed")

          map (NavigationTodos25b NavigationTodos25b.Route.Top)
              (s "navigationtodos25b")

          map (NavigationTodos25b NavigationTodos25b.Route.Incomplete)
              (s "navigationtodos25b" </> s "incomplete") // </> s "incomplete")

          map (NavigationTodos25b NavigationTodos25b.Route.Completed)
              (s "navigationtodos25b" </> s "completed") ]

let urlUpdate (result : Page option) (model : Model) : Model * Cmd<Msg> =
    match result with
    | None -> model, Navigation.modifyUrl (toHash model.CurrentPage)
    | Some(NavigationTodos25 filter) ->
        let navModel' = { model.NavigationTodos25 with Filter = filter }
        { model with CurrentPage = NavigationTodos25 filter
                     NavigationTodos25 = navModel' }, []
    | Some(NavigationTodos25b route) ->
        let filter =
            match route with
            | NavigationTodos25b.Route.Top ->
                NavigationTodos25b.Datas.Filter.All
            | NavigationTodos25b.Route.Incomplete ->
                NavigationTodos25b.Datas.Filter.Incomplete
            | NavigationTodos25b.Route.Completed ->
                NavigationTodos25b.Datas.Filter.Completed

        let navModel' = { model.NavigationTodos25b with Filter = filter }
        { model with CurrentPage = NavigationTodos25b route
                     NavigationTodos25b = navModel' }, []
    | Some page -> { model with CurrentPage = page }, []

let init (result : Page option) : Model * Cmd<Msg> =
    let h01 = HelloWorld01.init()
    let h01b = HelloWorld01b.State.init()
    let h02 = HelloWorld02.init()
    let h03 = HelloWorld03.init()
    let h04 = HelloWorld04.init()
    let c05 = Counter05.init()
    let c06 = Counter06.init()
    let c07 = Counter07.init()
    let c08 = Counter08.init()
    let c09 = Counter09.init()
    let c10 = Counter10.init()
    let c11 = Counter11.init()
    let c12 = Counter12.init()
    let i13 = InputBox13.init()
    let t14 = Todos14.init()
    let t15 = Todos15.init()
    let t16 = Todos16.init()
    let t17 = Todos17.init()
    let e18 = EditableTodos18.init()
    let m19, cmd19 = EditableTodos19.init()
    let m20, cmd20 = EditableTodos20.init()
    let m21, cmd21 =
        LocalStorageEditableTodos21.init
            (LocalStorageEditableTodos21.loadTodos())
    let m22, cmd22 =
        LocalStorageEditableTodos22.init
            (LocalStorageEditableTodos22.loadTodos())
    let m23, cmd23 = FilterTodos23.init (FilterTodos23.loadTodos())
    let m24, cmd24 = FilterTodos24.init (FilterTodos24.loadTodos())
    let m25, cmd25 = NavigationTodos25.init (NavigationTodos25.loadTodos()) None
    let m25b, cmd25b =
        NavigationTodos25b.State.init
            (NavigationTodos25b.Datas.Todo.loadTodos()) None

    let (model, cmd) =
        urlUpdate result { CurrentPage = About
                           HelloWorld01 = h01
                           HelloWorld01b = h01b
                           HelloWorld02 = h02
                           HelloWorld03 = h03
                           HelloWorld04 = h04
                           Counter05 = c05
                           Counter06 = c06
                           Counter07 = c07
                           Counter08 = c08
                           Counter09 = c09
                           Counter10 = c10
                           Counter11 = c11
                           Counter12 = c12
                           InputBox13 = i13
                           Todos14 = t14
                           Todos15 = t15
                           Todos16 = t16
                           Todos17 = t17
                           EditableTodos18 = e18
                           EditableTodos19 = m19
                           EditableTodos20 = m20
                           LocalStorageEditableTodos21 = m21
                           LocalStorageEditableTodos22 = m22
                           FilterTodos23 = m23
                           FilterTodos24 = m24
                           NavigationTodos25 = m25
                           NavigationTodos25b = m25b }
    model,
    Cmd.batch [ cmd
                Cmd.map EditableTodos19Msg cmd19
                Cmd.map EditableTodos20Msg cmd20
                Cmd.map LocalStorageEditableTodos21Msg cmd21
                Cmd.map LocalStorageEditableTodos22Msg cmd22
                Cmd.map FilterTodos23Msg cmd23
                Cmd.map FilterTodos24Msg cmd24
                Cmd.map NavigationTodos25Msg cmd25
                Cmd.map NavigationTodos25bMsg cmd25b ]

let update msg model =
    match msg with
    | HelloWorld01Msg _
    | HelloWorld01bMsg _
    | HelloWorld02Msg _
    | HelloWorld03Msg _
    | HelloWorld04Msg _ -> model, []
    | Counter05Msg msg ->
        let newModel = Counter05.update msg model.Counter05
        { model with Counter05 = newModel }, []
    | Counter06Msg msg ->
        let newModel = Counter06.update msg model.Counter06
        { model with Counter06 = newModel }, []
    | Counter07Msg msg ->
        let newModel = Counter07.update msg model.Counter07
        { model with Counter07 = newModel }, []
    | Counter08Msg msg ->
        let newModel = Counter08.update msg model.Counter08
        { model with Counter08 = newModel }, []
    | Counter09Msg msg ->
        let newModel = Counter09.update msg model.Counter09
        { model with Counter09 = newModel }, []
    | Counter10Msg msg ->
        let newModel = Counter10.update msg model.Counter10
        { model with Counter10 = newModel }, []
    | Counter11Msg msg ->
        let newModel = Counter11.update msg model.Counter11
        { model with Counter11 = newModel }, []
    | Counter12Msg msg ->
        let newModel = Counter12.update msg model.Counter12
        { model with Counter12 = newModel }, []
    | InputBox13Msg msg ->
        let newModel = InputBox13.update msg model.InputBox13
        { model with InputBox13 = newModel }, []
    | Todos14Msg msg ->
        let newModel = Todos14.update msg model.Todos14
        { model with Todos14 = newModel }, []
    | Todos15Msg msg ->
        let newModel = Todos15.update msg model.Todos15
        { model with Todos15 = newModel }, []
    | Todos16Msg msg ->
        let newModel = Todos16.update msg model.Todos16
        { model with Todos16 = newModel }, []
    | Todos17Msg msg ->
        let newModel = Todos17.update msg model.Todos17
        { model with Todos17 = newModel }, []
    | EditableTodos18Msg msg ->
        let newModel = EditableTodos18.update msg model.EditableTodos18
        { model with EditableTodos18 = newModel }, []
    | EditableTodos19Msg msg ->
        let newModel, cmd = EditableTodos19.update msg model.EditableTodos19
        { model with EditableTodos19 = newModel },
        Cmd.map EditableTodos19Msg cmd
    | EditableTodos20Msg msg ->
        let newModel, cmd = EditableTodos20.update msg model.EditableTodos20
        { model with EditableTodos20 = newModel },
        Cmd.map EditableTodos20Msg cmd
    | LocalStorageEditableTodos21Msg msg ->
        let newModel, cmd =
            LocalStorageEditableTodos21.update msg
                model.LocalStorageEditableTodos21
        { model with LocalStorageEditableTodos21 = newModel },
        Cmd.map LocalStorageEditableTodos21Msg cmd
    | LocalStorageEditableTodos22Msg msg ->
        let newModel, cmd =
            LocalStorageEditableTodos22.update msg
                model.LocalStorageEditableTodos22
        { model with LocalStorageEditableTodos22 = newModel },
        Cmd.map LocalStorageEditableTodos22Msg cmd
    | FilterTodos23Msg msg ->
        let newModel, cmd = FilterTodos23.update msg model.FilterTodos23
        { model with FilterTodos23 = newModel }, Cmd.map FilterTodos23Msg cmd
    | FilterTodos24Msg msg ->
        let newModel, cmd = FilterTodos24.update msg model.FilterTodos24
        { model with FilterTodos24 = newModel }, Cmd.map FilterTodos24Msg cmd
    | NavigationTodos25Msg msg ->
        let newModel, cmd = NavigationTodos25.update msg model.NavigationTodos25
        { model with NavigationTodos25 = newModel },
        Cmd.map NavigationTodos25Msg cmd
    | NavigationTodos25bMsg msg ->
        let newModel, cmd =
            NavigationTodos25b.State.update msg model.NavigationTodos25b
        { model with NavigationTodos25b = newModel },
        Cmd.map NavigationTodos25bMsg cmd
