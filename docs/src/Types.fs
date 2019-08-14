module App.Types

open Global

type Msg =
    | HelloWorld01Msg of HelloWorld01.Types.Msg // dummy
    | HelloWorld01bMsg of HelloWorld01b.Types.Msg // dummy
    | HelloWorld03Msg of HelloWorld03.Types.Msg // dummy
    | HelloWorld04Msg of HelloWorld04.Types.Msg // dummy
    | Counter05Msg of Counter05.Types.Msg
    | Counter07Msg of Counter07.Types.Msg
    | Counter08Msg of Counter08.Types.Msg
    | Counter09Msg of Counter09.Types.Msg
    | Counter10Msg of Counter10.Types.Msg
    | Counter11Msg of Counter11.Types.Msg
    | Counter12Msg of Counter12.Types.Msg


type Model =
    { CurrentPage: Page
      HelloWorld01: HelloWorld01.Types.Model
      HelloWorld01b: HelloWorld01b.Types.Model
      HelloWorld03: HelloWorld03.Types.Model
      HelloWorld04: HelloWorld04.Types.Model
      Counter05: Counter05.Types.Model
      Counter07: Counter07.Types.Model
      Counter08: Counter08.Types.Model
      Counter09: Counter09.Types.Model
      Counter10: Counter10.Types.Model
      Counter11: Counter11.Types.Model
      Counter12: Counter12.Types.Model }
