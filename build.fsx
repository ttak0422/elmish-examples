#r "paket: groupref Build //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.JavaScript

Target.create "Clean" <| fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs

Target.create "YarnInstall" <| fun _ ->
    Yarn.install id

Target.create "DotnetRestore" <| fun _ ->
    DotNet.restore
        (DotNet.Options.withWorkingDirectory __SOURCE_DIRECTORY__)
        "elmish-examples.sln"

Target.create "PreProcessing" ignore

Target.create "01" <| fun _ ->
    Yarn.exec "start01" id

Target.create "01b" <| fun _ ->
    Yarn.exec "start01b" id

Target.create "03" <| fun _ ->
    Yarn.exec "start03" id

Target.create "04" <| fun _ ->
    Yarn.exec "start04" id

Target.create "05" <| fun _ ->
    Yarn.exec "start05" id

Target.create "07" <| fun _ ->
    Yarn.exec "start07" id


"Clean"
    ==> "YarnInstall"
    ==> "DotnetRestore"
    ==> "PreProcessing"

"PreProcessing" ==> "01"
"PreProcessing" ==> "01b"
"PreProcessing" ==> "03"
"PreProcessing" ==> "04"
"PreProcessing" ==> "05"
"PreProcessing" ==> "07"

Target.runOrDefault "PreProcessing"