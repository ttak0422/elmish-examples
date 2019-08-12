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

"Clean"
    ==> "YarnInstall"
    ==> "DotnetRestore"
    ==> "PreProcessing"

Target.runOrDefault "PreProcessing"