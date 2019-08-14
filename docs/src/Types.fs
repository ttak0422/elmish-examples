module App.Types

open Global

type Msg =
    | HelloWorld01Msg of HelloWorld01.Msg // dummy
    | HelloWorld01bMsg of HelloWorld01b.Types.Msg // dummy
    | HelloWorld02Msg of HelloWorld02.Msg // dummy
    | HelloWorld03Msg of HelloWorld03.Msg // dummy
    | HelloWorld04Msg of HelloWorld04.Msg // dummy
    | Counter05Msg of Counter05.Msg
    | Counter06Msg of Counter06.Msg
    | Counter07Msg of Counter07.Msg
    | Counter08Msg of Counter08.Msg
    | Counter09Msg of Counter09.Msg
    | Counter10Msg of Counter10.Msg
    | Counter11Msg of Counter11.Msg
    | Counter12Msg of Counter12.Msg


type Model =
    { CurrentPage: Page
      HelloWorld01: HelloWorld01.Model
      HelloWorld01b: HelloWorld01b.Types.Model
      HelloWorld02: HelloWorld02.Model
      HelloWorld03: HelloWorld03.Model
      HelloWorld04: HelloWorld04.Model
      Counter05: Counter05.Model
      Counter06: Counter06.Model
      Counter07: Counter07.Model
      Counter08: Counter08.Model
      Counter09: Counter09.Model
      Counter10: Counter10.Model
      Counter11: Counter11.Model
      Counter12: Counter12.Model }
