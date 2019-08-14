module Global
type Page =
    | About
    | HelloWorld01
    | HelloWorld03
    | HelloWorld04
    | Counter05
    | Counter07
    | Counter08
    | Counter09
    | Counter10
    | Counter11
    | Counter12

let toHash page =
    match page with
    | About -> "about"
    | HelloWorld01 -> "helloworld01"
    | HelloWorld03 -> "helloworld03"
    | HelloWorld04 -> "helloworld04"
    | Counter05 -> "counter05"
    | Counter07 -> "counter07"
    | Counter08 -> "counter08"
    | Counter09 -> "counter09"
    | Counter10 -> "counter10"
    | Counter11 -> "counter11"
    | Counter12 -> "counter12"
    |> fun url -> "#" + url
