module App.State

open Elmish
open Elmish.UrlParser
open Elmish.Navigation
open Global
open Types

let pageParser: Parser<Page->Page,Page> =
    oneOf [
        map About (s "about")
        map Counter (s "counter")
        map Home (s "home")
        map HelloWorld01 (s "helloworld01")
        map HelloWorld03 (s "helloworld03")
        map HelloWorld04 (s "helloworld04")
        map Counter05 (s "counter05")
        map Counter07 (s "counter07")
        map Counter08 (s "counter08")
        map Counter09 (s "counter09")
        map Counter10 (s "counter10")
        map Counter11 (s "counter11")
        map Counter12 (s "counter12")
    ]

let urlUpdate (result : Page option) model =
    match result with
    | None ->
        model, Navigation.modifyUrl (toHash model.CurrentPage)
    | Some page ->
        { model with CurrentPage = page }, []

let init result =
    let (counter, counterCmd) = Counter.State.init()
    let (home, homeCmd) = Home.State.init()
    let h01 = HelloWorld01.State.init()
    let h03 = HelloWorld03.State.init()
    let h04 = HelloWorld04.State.init()
    let c05 = Counter05.State.init()
    let c07 = Counter07.State.init()
    let c08 = Counter08.State.init()
    let c09 = Counter09.State.init()
    let c10 = Counter10.State.init()
    let c11 = Counter11.State.init()
    let c12 = Counter12.State.init()
    let (model, cmd) =
        urlUpdate result
          { CurrentPage = Home
            Counter = counter
            Home = home
            HelloWorld01 = h01
            HelloWorld03 = h03
            HelloWorld04 = h04
            Counter05 = c05
            Counter07 = c07
            Counter08 = c08
            Counter09 = c09
            Counter10 = c10
            Counter11 = c11
            Counter12 = c12}

    model, Cmd.batch [ cmd
                       Cmd.map CounterMsg counterCmd
                       Cmd.map HomeMsg homeCmd ]

let update msg model =
    match msg with
    | CounterMsg msg ->
        let (counter, counterCmd) = Counter.State.update msg model.Counter
        { model with Counter = counter }, Cmd.map CounterMsg counterCmd
    | HomeMsg msg ->
        let (home, homeCmd) = Home.State.update msg model.Home
        { model with Home = home }, Cmd.map HomeMsg homeCmd
    | HelloWorld01Msg _
    | HelloWorld03Msg _
    | HelloWorld04Msg _ -> failwith "This is dummy msg."
    | Counter05Msg msg ->
        let counterModel = Counter05.State.update msg model.Counter05
        { model with Counter05 = counterModel }, []
    | Counter07Msg msg ->
        let counterModel = Counter07.State.update msg model.Counter07
        { model with Counter07 = counterModel }, []
    | Counter08Msg msg ->
        let counterModel = Counter08.State.update msg model.Counter08
        { model with Counter08 = counterModel }, []
    | Counter09Msg msg ->
        let counterModel = Counter09.State.update msg model.Counter09
        { model with Counter09 = counterModel }, []
    | Counter10Msg msg ->
        let counterModel = Counter10.State.update msg model.Counter10
        { model with Counter10 = counterModel }, []
    | Counter11Msg msg ->
        let counterModel = Counter11.State.update msg model.Counter11
        { model with Counter11 = counterModel }, []
    | Counter12Msg msg ->
        let counterModel = Counter12.State.update msg model.Counter12
        { model with Counter12 = counterModel }, []