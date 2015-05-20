#I "packages/FAKE/tools"
#r "FakeLib.dll"

open Fake
open Fake.FscHelper

Target "Compile" <| fun _ ->
    ["Main.fs"]
    |> Fsc (fun p ->
            { p with References = ["packages/FSharp.Core/lib/net40/FSharp.Core.dll"] })

Target "Watch" <| fun _ ->
    use watcher =
        !!"*.fs"
        |> WatchChanges (fun changes ->
            tracefn "%A" changes
            Run "Compile")

    System.Console.ReadLine() |> ignore
    watcher.Dispose()

Target "Default" DoNothing

"Compile"
==> "Default"

RunTargetOrDefault "Default"
