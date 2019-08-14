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
    ++ "docs/**/bin"
    ++ "docs/**/obj"
    ++ "docs/**/deploy"
    |> Shell.cleanDirs

Target.create "YarnInstall" <| fun _ ->
    Yarn.install id

Target.create "DotnetRestore" <| fun _ ->
    !! "src/**/*.fsproj"
    |> Seq.iter (fun proj ->
        DotNet.restore id proj)

Target.create "DotnetRestoreDocs" <| fun _ ->
    !! "docs/**/*.fsproj"
    |> Seq.iter (fun proj ->
        DotNet.restore id proj)

Target.create "Setup" ignore

Target.create "SetupDocs" ignore

let webpack path =
    let cmd = sprintf "webpack --env.entry=%s" path
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

let inline yarnWorkDir (ws : string) (yarnParams : Yarn.YarnParams) =
    { yarnParams with WorkingDirectory = ws }

Target.create "BuildDocs" <| fun _ ->
    Yarn.exec "webpack --config docs/webpack.config.js" (yarnWorkDir "docs")

Target.create "WatchDocs" <| fun _ ->
    Yarn.exec "webpack-dev-server --config docs/webpack.config.js" (yarnWorkDir "docs")

"Clean"
    ==> "YarnInstall"
    ==> "DotnetRestore"
    ==> "Setup"

"Clean"
    ==> "YarnInstall"
    ==> "DotnetRestoreDocs"
    ==> "SetupDocs"

"Setup" ==> "build01"
"Setup" ==> "build01b"
"Setup" ==> "build03"
"Setup" ==> "build04"
"Setup" ==> "build05"
"Setup" ==> "build07"
"Setup" ==> "build08"
"Setup" ==> "build09"
"Setup" ==> "build10"
"Setup" ==> "build11"
"Setup" ==> "build12"

"Setup" ==> "watch01"
"Setup" ==> "watch01b"
"Setup" ==> "watch03"
"Setup" ==> "watch04"
"Setup" ==> "watch05"
"Setup" ==> "watch07"
"Setup" ==> "watch08"
"Setup" ==> "watch09"
"Setup" ==> "watch10"
"Setup" ==> "watch11"
"Setup" ==> "watch12"

"BuildDocs"
    <== [ "SetupDocs" ]

"WatchDocs"
    <== [ "SetupDocs" ]    

Target.runOrDefault "PreProcessing"