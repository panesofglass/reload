#I "packages/FAKE/tools"
#r "FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.ChangeWatcher
open Fake.EnvironmentHelper
open Fake.FscHelper
open Fake.ProcessHelper

let assemblyInfo = "AssemblyInfo.fs"
let defaultBaseDir = System.IO.Path.GetFullPath "."
let version = environVarOrDefault "version" "1.0.0"
// TODO: require this parameter
let output = environVarOrDefault "output" "main.exe"
// TODO: require this parameter
let files =
    environVarOrDefault "files" "Main.fs"
    |> (fun str -> str.Split([|','|]))
    |> List.ofArray
    |> (fun xs -> assemblyInfo::xs)

Target "AssemblyInfo" <| fun _ ->
    CreateFSharpAssemblyInfo (defaultBaseDir + assemblyInfo)
        [Attribute.Title "Reload Demo"
         Attribute.Description "Demonstration of compiling and watching F# tools with FAKE"
         Attribute.Version version
         Attribute.FileVersion version]

Target "Compile" <| fun _ ->
    // NOTE: file order matters, so this must be explicit.
    // TODO: allow an environVar to specify the file list.
    files
    |> Fsc (fun p ->
            { p with
                Output = output
                References = ["packages/FSharp.Core/lib/net40/FSharp.Core.dll"] })

Target "Run" <| fun _ ->
    Shell.Exec output |> ignore

Target "Watch" <| fun _ ->
    use watcher =
        { BaseDirectory = defaultBaseDir
          Includes = files |> List.filter ((<>) assemblyInfo)
          Excludes = [assemblyInfo] }
        |> WatchChanges (fun changes ->
            tracefn "%A" changes
            Run "Run")

    // TODO: This appears to block.
    System.Console.ReadLine() |> ignore
    watcher.Dispose()

Target "Help" <| fun _ ->
    // TODO: Include instructions for specifying the output name and files to compile.
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
==> "Watch"

RunTargetOrDefault "Help"
