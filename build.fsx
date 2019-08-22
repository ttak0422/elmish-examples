#r "paket: groupref Build //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.IO.FileSystemOperators
open Fake.Tools.Git
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
    ++ "docs/**/*.fsproj"
    |> Seq.iter (fun proj ->
        DotNet.restore id proj)

Target.create "Setup" ignore

Target.create "SetupQuick" ignore

let webpack path =
    let cmd = sprintf "webpack --env.entry=%s --mode=development" path
    Yarn.exec cmd id

let webpackDevServer path =
    let cmd = sprintf "webpack-dev-server --env.entry=%s --mode=development" path
    Yarn.exec cmd id

let quick buildFn path =
    DotNet.restore id path
    buildFn path

let quickBuild path =
    quick webpack path

let quickWatch path =
    quick webpackDevServer path

Target.create "build01"  <| fun _ ->  quickBuild "./src/HelloWorld01/HelloWorld01.fsproj"
Target.create "build03"  <| fun _ ->  quickBuild "./src/HelloWorld03/HelloWorld03.fsproj"
Target.create "build04"  <| fun _ ->  quickBuild "./src/HelloWorld04/HelloWorld04.fsproj"
Target.create "build05"  <| fun _ ->  quickBuild "./src/Counter05/Counter05.fsproj"
Target.create "build07"  <| fun _ ->  quickBuild "./src/Counter07/Counter07.fsproj"
Target.create "build08"  <| fun _ ->  quickBuild "./src/Counter08/Counter08.fsproj"
Target.create "build09"  <| fun _ ->  quickBuild "./src/Counter09/Counter09.fsproj"
Target.create "build10"  <| fun _ ->  quickBuild "./src/Counter10/Counter10.fsproj"
Target.create "build11"  <| fun _ ->  quickBuild "./src/Counter11/Counter11.fsproj"
Target.create "build12"  <| fun _ ->  quickBuild "./src/counter12/counter12.fsproj"
Target.create "build13"  <| fun _ ->  quickBuild "./src/InputBox13/InputBox13.fsproj"
Target.create "build14"  <| fun _ ->  quickBuild "./src/Todos14/Todos14.fsproj"
Target.create "build15"  <| fun _ ->  quickBuild "./src/Todos15/Todos15.fsproj"
Target.create "build16"  <| fun _ ->  quickBuild "./src/Todos16/Todos16.fsproj"
Target.create "build17"  <| fun _ ->  quickBuild "./src/Todos17/Todos17.fsproj"
Target.create "build18"  <| fun _ ->  quickBuild "./src/EditableTodos18/EditableTodos18.fsproj"
Target.create "build19"  <| fun _ ->  quickBuild "./src/EditableTodos19/EditableTodos19.fsproj"
Target.create "build20"  <| fun _ ->  quickBuild "./src/EditableTodos20/EditableTodos20.fsproj"
Target.create "build21"  <| fun _ ->  quickBuild "./src/LocalStorageEditableTodos21/LocalStorageEditable21.fsproj"
Target.create "build22"  <| fun _ ->  quickBuild "./src/LocalStorageEditableTodos22/LocalStorageEditable22.fsproj"
Target.create "build23"  <| fun _ ->  quickBuild "./src/FilterTodos23/FilterTodos23.fsproj"
Target.create "build24"  <| fun _ ->  quickBuild "./src/FilterTodos24/FilterTodos24.fsproj"
Target.create "build25"  <| fun _ ->  quickBuild "./src/NavigationTodos25/NavigationTodos25.fsproj"
Target.create "build01b" <| fun _ -> quickBuild "./src/HelloWorld01b/HelloWorld01b.fsproj"
Target.create "build25b" <| fun _ -> quickBuild "./src/NavigationTodos25b/NavigationTodos25b.fsproj"

Target.create "watch01"  <| fun _ ->  quickWatch "./src/HelloWorld01/HelloWorld01.fsproj"
Target.create "watch03"  <| fun _ ->  quickWatch "./src/HelloWorld03/HelloWorld03.fsproj"
Target.create "watch04"  <| fun _ ->  quickWatch "./src/HelloWorld04/HelloWorld04.fsproj"
Target.create "watch05"  <| fun _ ->  quickWatch "./src/Counter05/Counter05.fsproj"
Target.create "watch07"  <| fun _ ->  quickWatch "./src/Counter07/Counter07.fsproj"
Target.create "watch08"  <| fun _ ->  quickWatch "./src/Counter08/Counter08.fsproj"
Target.create "watch09"  <| fun _ ->  quickWatch "./src/Counter09/Counter09.fsproj"
Target.create "watch10"  <| fun _ ->  quickWatch "./src/Counter10/Counter10.fsproj"
Target.create "watch11"  <| fun _ ->  quickWatch "./src/Counter11/Counter11.fsproj"
Target.create "watch12"  <| fun _ ->  quickWatch "./src/Counter12/Counter12.fsproj"
Target.create "watch13"  <| fun _ ->  quickWatch "./src/InputBox13/InputBox13.fsproj"
Target.create "watch14"  <| fun _ ->  quickWatch "./src/Todos14/Todos14.fsproj"
Target.create "watch15"  <| fun _ ->  quickWatch "./src/Todos15/Todos15.fsproj"
Target.create "watch16"  <| fun _ ->  quickWatch "./src/Todos16/Todos16.fsproj"
Target.create "watch17"  <| fun _ ->  quickWatch "./src/Todos17/Todos17.fsproj"
Target.create "watch18"  <| fun _ ->  quickWatch "./src/EditableTodos18/EditableTodos18.fsproj"
Target.create "watch19"  <| fun _ ->  quickWatch "./src/EditableTodos19/EditableTodos19.fsproj"
Target.create "watch20"  <| fun _ ->  quickWatch "./src/EditableTodosw20/EditableTodos20.fsproj"
Target.create "watch21"  <| fun _ ->  quickWatch "./src/LocalStorageEditableTodos21/LocalStorageEditable21.fsproj"
Target.create "watch22"  <| fun _ ->  quickWatch "./src/LocalStorageEditableTodos22/LocalStorageEditable22.fsproj"
Target.create "watch23"  <| fun _ ->  quickWatch "./src/FilterTodos23/FilterTodos23.fsproj"
Target.create "watch24"  <| fun _ ->  quickWatch "./src/FilterTodos24/FilterTodos24.fsproj"
Target.create "watch25"  <| fun _ ->  quickWatch "./src/NavigationTodos25/NavigationTodos25.fsproj"
Target.create "watch01b" <| fun _ -> quickWatch "./src/HelloWorld01b/HelloWorld01b.fsproj"
Target.create "watch25b" <| fun _ -> quickWatch "./src/NavigationTodos25b/NavigationTodos25b.fsproj"

let inline yarnWorkDir (ws : string) (yarnParams : Yarn.YarnParams) =
    { yarnParams with WorkingDirectory = ws }

Target.create "BuildDocs" <| fun _ ->
    Yarn.exec "webpack --config docs/webpack.config.js" (yarnWorkDir "docs")

Target.create "WatchDocs" <| fun _ ->
    Yarn.exec "webpack-dev-server --config docs/webpack.config.js" (yarnWorkDir "docs")

let root = __SOURCE_DIRECTORY__
let repositoryLink = "https://github.com/ttak0422/elmish-examples.git"
let ghPagesBranch = "gh-pages"
let ghPagesClone = root </> "temp"
let docsOutput = root </> "docs" </> "deploy"

Target.create "PublishDocs" <| fun _ ->
    Shell.cleanDir ghPagesClone
    Repository.cloneSingleBranch "" repositoryLink ghPagesBranch ghPagesClone
    Shell.copyRecursive docsOutput ghPagesClone true |> Trace.tracefn "%A"
    Staging.stageAll ghPagesClone
    Commit.exec ghPagesClone "Update gh-pages"
    Branches.push ghPagesClone

Target.create "BuildAll" <| fun _ ->
    !! "./src/**/*.fsproj"
    |> Seq.iter webpack

"Clean"
    ==> "YarnInstall"
    ==> "SetupQuick"

"Clean"
    ==> "YarnInstall"
    ==> "DotnetRestore"
    ==> "Setup"

"SetupQuick" ==> "build01"
"SetupQuick" ==> "build01b"
"SetupQuick" ==> "build03"
"SetupQuick" ==> "build04"
"SetupQuick" ==> "build05"
"SetupQuick" ==> "build07"
"SetupQuick" ==> "build08"
"SetupQuick" ==> "build09"
"SetupQuick" ==> "build10"
"SetupQuick" ==> "build11"
"SetupQuick" ==> "build12"
"SetupQuick" ==> "build13"
"SetupQuick" ==> "build14"
"SetupQuick" ==> "build15"
"SetupQuick" ==> "build16"
"SetupQuick" ==> "build17"
"SetupQuick" ==> "build18"
"SetupQuick" ==> "build19"
"SetupQuick" ==> "build20"
"SetupQuick" ==> "build21"

"SetupQuick" ==> "watch01"
"SetupQuick" ==> "watch01b"
"SetupQuick" ==> "watch03"
"SetupQuick" ==> "watch04"
"SetupQuick" ==> "watch05"
"SetupQuick" ==> "watch07"
"SetupQuick" ==> "watch08"
"SetupQuick" ==> "watch09"
"SetupQuick" ==> "watch10"
"SetupQuick" ==> "watch11"
"SetupQuick" ==> "watch12"
"SetupQuick" ==> "watch13"
"SetupQuick" ==> "watch14"
"SetupQuick" ==> "watch15"
"SetupQuick" ==> "watch16"
"SetupQuick" ==> "watch17"
"SetupQuick" ==> "watch18"
"SetupQuick" ==> "watch19"
"SetupQuick" ==> "watch21"

"BuildDocs"
    <== [ "Setup" ]

"WatchDocs"
    <== [ "Setup" ]

"PublishDocs"
    <== [ "BuildDocs" ]

"BuildAll"
    <== [ "Setup" ]

Target.runOrDefault "PreProcessing"