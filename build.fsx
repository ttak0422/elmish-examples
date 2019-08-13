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

let webpack path =
    let cmd = sprintf "webpack --env.entry=%s --mode=development" path
    Yarn.exec cmd id

let webpackDevServer path =
    let cmd = sprintf "webpack-dev-server --env.entry=%s --mode=development" path
    Yarn.exec cmd id
   

Target.create "build01" <| fun _ -> webpack "./src/HelloWorld01/HelloWorld01.fsproj"
Target.create "build03" <| fun _ -> webpack "./src/HelloWorld03/HelloWorld03.fsproj"
Target.create "build04" <| fun _ -> webpack "./src/HelloWorld04/HelloWorld04.fsproj"
Target.create "build05" <| fun _ -> webpack "./src/Counter05/Counter05.fsproj"
Target.create "build07" <| fun _ -> webpack "./src/Counter07/Counter07.fsproj"
Target.create "build08" <| fun _ -> webpack "./src/Counter08/Counter08.fsproj"
Target.create "build09" <| fun _ -> webpack "./src/Counter09/Counter09.fsproj"
Target.create "build10" <| fun _ -> webpack "./src/Counter10/Counter10.fsproj"
Target.create "build11" <| fun _ -> webpack "./src/Counter11/Counter11.fsproj"
Target.create "build12" <| fun _ -> webpack "./src/Counter12/Counter12.fsproj"
Target.create "build01b" <| fun _ -> webpack "./src/HelloWorld01b/HelloWorld01b.fsproj"

Target.create "watch01" <| fun _ -> webpackDevServer "./src/HelloWorld01/HelloWorld01.fsproj"
Target.create "watch03" <| fun _ -> webpackDevServer "./src/HelloWorld03/HelloWorld03.fsproj"
Target.create "watch04" <| fun _ -> webpackDevServer "./src/HelloWorld04/HelloWorld04.fsproj"
Target.create "watch05" <| fun _ -> webpackDevServer "./src/Counter05/Counter05.fsproj"
Target.create "watch07" <| fun _ -> webpackDevServer "./src/Counter07/Counter07.fsproj"
Target.create "watch08" <| fun _ -> webpackDevServer "./src/Counter08/Counter08.fsproj"
Target.create "watch09" <| fun _ -> webpackDevServer "./src/Counter09/Counter09.fsproj"
Target.create "watch10" <| fun _ -> webpackDevServer "./src/Counter10/Counter10.fsproj"
Target.create "watch11" <| fun _ -> webpackDevServer "./src/Counter11/Counter11.fsproj"
Target.create "watch12" <| fun _ -> webpackDevServer "./src/Counter12/Counter12.fsproj"
Target.create "watch01b" <| fun _ -> webpackDevServer "./src/HelloWorld01b/HelloWorld01b.fsproj"

"Clean"
    ==> "YarnInstall"
    ==> "DotnetRestore"
    ==> "PreProcessing"

"PreProcessing" ==> "build01"
"PreProcessing" ==> "build01b"
"PreProcessing" ==> "build03"
"PreProcessing" ==> "build04"
"PreProcessing" ==> "build05"
"PreProcessing" ==> "build07"
"PreProcessing" ==> "build08"
"PreProcessing" ==> "build09"
"PreProcessing" ==> "build10"
"PreProcessing" ==> "build11"
"PreProcessing" ==> "build12"

"PreProcessing" ==> "watch01"
"PreProcessing" ==> "watch01b"
"PreProcessing" ==> "watch03"
"PreProcessing" ==> "watch04"
"PreProcessing" ==> "watch05"
"PreProcessing" ==> "watch07"
"PreProcessing" ==> "watch08"
"PreProcessing" ==> "watch09"
"PreProcessing" ==> "watch10"
"PreProcessing" ==> "watch11"
"PreProcessing" ==> "watch12"

Target.runOrDefault "PreProcessing"