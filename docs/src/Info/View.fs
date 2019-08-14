module Info.View

open Fable.React
open Fable.React.Props

let root =
  div
    [ ClassName "content" ]
    [ h1
        [ ]
        [ str "About" ]
      p
        [ ]
        [ str "Samples of each section are posted. " ] ]
