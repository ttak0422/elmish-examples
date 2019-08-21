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
    | InputBox13Msg of InputBox13.Msg
    | Todos14Msg of Todos14.Msg
    | Todos15Msg of Todos15.Msg
    | Todos16Msg of Todos16.Msg
    | Todos17Msg of Todos17.Msg
    | EditableTodos18Msg of EditableTodos18.Msg
    | EditableTodos19Msg of EditableTodos19.Msg
    | EditableTodos20Msg of EditableTodos20.Msg
    | LocalStorageEditableTodos21Msg of LocalStorageEditableTodos21.Msg
    | LocalStorageEditableTodos22Msg of LocalStorageEditableTodos22.Msg
    | FilterTodos23Msg of FilterTodos23.Msg
    | FilterTodos24Msg of FilterTodos24.Msg
    | NavigationTodos25Msg of NavigationTodos25.Msg
    | NavigationTodos25bMsg of NavigationTodos25b.Msg

type Model =
    { CurrentPage : Page
      HelloWorld01 : HelloWorld01.Model
      HelloWorld01b : HelloWorld01b.Types.Model
      HelloWorld02 : HelloWorld02.Model
      HelloWorld03 : HelloWorld03.Model
      HelloWorld04 : HelloWorld04.Model
      Counter05 : Counter05.Model
      Counter06 : Counter06.Model
      Counter07 : Counter07.Model
      Counter08 : Counter08.Model
      Counter09 : Counter09.Model
      Counter10 : Counter10.Model
      Counter11 : Counter11.Model
      Counter12 : Counter12.Model
      InputBox13 : InputBox13.Model
      Todos14 : Todos14.Model
      Todos15 : Todos15.Model
      Todos16 : Todos16.Model
      Todos17 : Todos17.Model
      EditableTodos18 : EditableTodos18.Model
      EditableTodos19 : EditableTodos19.Model
      EditableTodos20 : EditableTodos20.Model
      LocalStorageEditableTodos21 : LocalStorageEditableTodos21.Model
      LocalStorageEditableTodos22 : LocalStorageEditableTodos22.Model
      FilterTodos23 : FilterTodos23.Model
      FilterTodos24 : FilterTodos24.Model
      NavigationTodos25 : NavigationTodos25.Model
      NavigationTodos25b : NavigationTodos25b.Model }
