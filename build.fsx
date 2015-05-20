#I "packages/FAKE/tools"
#r "FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.ChangeWatcher
open Fake.EnvironmentHelper
open Fake.FscHelper
open Fake.ProcessHelper

#load "project.fsx"

let ensureProjectLoaded() =
    tracefn "Project definition loaded for directory: %s" project.BaseDir

let assemblyInfo = "AssemblyInfo.fs"

let createAssemblyInfo() =
    CreateFSharpAssemblyInfo (project.BaseDir + assemblyInfo)
        [Attribute.Title "Reload Demo"
         Attribute.Description "Demonstration of compiling and watching F# tools with FAKE"
         Attribute.Version version
         Attribute.FileVersion version]

let compile() =
    if project.GenerateAssemblyInfo then
        createAssemblyInfo()
        assemblyInfo::project.Files
    else files
    |> Fsc project.FscParams

let run() =
    Shell.Exec output |> ignore

let watch() =
    use watcher =
        { BaseDirectory = project.BaseDir
          Includes = project.Files
          Excludes = [assemblyInfo] }
        |> WatchChanges (fun changes ->
            tracefn "%A" changes
            // TODO: restore target running so as not to duplicate the dependency chain. https://github.com/fsharp/FAKE/issues/791
            //Run "Run")
            compile()
            run())

    // TODO: This appears to block.
    System.Console.ReadLine() |> ignore
    watcher.Dispose()

Target "ProjectLoaded" ensureProjectLoaded
Target "AssemblyInfo" createAssemblyInfo
Target "Compile" compile
Target "Run" run
Target "Watch" watch

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

"EnsureProjectLoaded"
==> "AssemblyInfo"

"EnsureProjectLoaded"
==> "Compile"
==> "Run"
==> "Watch"

RunTargetOrDefault "Help"
