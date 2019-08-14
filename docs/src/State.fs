module App.State

open Elmish
open Elmish.UrlParser
open Elmish.Navigation
open Global
open Types

let pageParser: Parser<Page->Page,Page> =
    oneOf [
        map About (s "about")
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
    ]

let urlUpdate (result : Page option) model =
    match result with
    | None ->
        model, Navigation.modifyUrl (toHash model.CurrentPage)
    | Some page ->
        { model with CurrentPage = page }, []

let init result =
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
    let (model, cmd) =
        urlUpdate result
          { CurrentPage = About
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
            Counter12 = c12}

    model, Cmd.batch [ cmd ]

let update msg model =
    match msg with
    | HelloWorld01Msg _
    | HelloWorld01bMsg _
    | HelloWorld02Msg _
    | HelloWorld03Msg _
    | HelloWorld04Msg _ -> failwith "This is dummy msg."
    | Counter05Msg msg ->
        let counterModel = Counter05.update msg model.Counter05
        { model with Counter05 = counterModel }, []
    | Counter06Msg msg ->
        let counterModel = Counter06.update msg model.Counter06
        { model with Counter06 = counterModel }, []
    | Counter07Msg msg ->
        let counterModel = Counter07.update msg model.Counter07
        { model with Counter07 = counterModel }, []
    | Counter08Msg msg ->
        let counterModel = Counter08.update msg model.Counter08
        { model with Counter08 = counterModel }, []
    | Counter09Msg msg ->
        let counterModel = Counter09.update msg model.Counter09
        { model with Counter09 = counterModel }, []
    | Counter10Msg msg ->
        let counterModel = Counter10.update msg model.Counter10
        { model with Counter10 = counterModel }, []
    | Counter11Msg msg ->
        let counterModel = Counter11.update msg model.Counter11
        { model with Counter11 = counterModel }, []
    | Counter12Msg msg ->
        let counterModel = Counter12.update msg model.Counter12
        { model with Counter12 = counterModel }, []