#I "packages/FAKE/tools"
#r "FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.ChangeWatcher
open Fake.EnvironmentHelper
open Fake.FscHelper
open Fake.ProcessHelper

let version = environVarOrDefault "version" "1.0.0"
let exeName = environVarOrDefault "exeName" "main.exe"

Target "AssemblyInfo" <| fun _ ->
    CreateFSharpAssemblyInfo "./AssemblyInfo.fs"
        [Attribute.Title "Reload Demo"
         Attribute.Description "Demonstration of compiling and watching F# tools with FAKE"
         Attribute.Version version
         Attribute.FileVersion version]

Target "Compile" <| fun _ ->
    !!"*.fs"
    |> List.ofSeq
    |> Fsc (fun p ->
            { p with
                Output = exeName
                References = ["packages/FSharp.Core/lib/net40/FSharp.Core.dll"] })

Target "Run" <| fun _ ->
    Shell.Exec exeName |> ignore

Target "Watch" <| fun _ ->
    use watcher =
        !!"*.fs"
        --"AssemblyInfo.fs"
        |> WatchChanges (fun changes ->
            tracefn "%A" changes
            Run "Run")

    // TODO: This appears to block.
    System.Console.ReadLine() |> ignore
    watcher.Dispose()

Target "Help" <| fun _ ->
    printfn "build.(cmd|sh) [<target>] [options]"
    printfn @"for FAKE help: packages\FAKE\tools\FAKE.exe --help"
    printfn "targets:"
    printfn "  * AssemblyInfo - generates an `AssemblyInfo.fs`"
    printfn "  * Compile - builds a `.exe` from any `.fs` files in the root directory"
    printfn "  * Help - displays this message"
    printfn "  * Run - runs the generated `.exe`"
    printfn "  * Watch - recompiles and runs the `.exe` for any changes to any `.fs` files in the directory"

"AssemblyInfo"
==> "Compile"
==> "Run"

RunTargetOrDefault "Help"
