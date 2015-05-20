module Reload.Project

#I "packages/FAKE/tools"
#r "FakeLib.dll"

open Fake.FscHelper

type ProjectDef =
    {
        BaseDir: string
        Version: string
        Files: string list
        WithParams: FscParams -> FscParams
        GenerateAssemblyInfo: bool
    }
    static member Default =
        {
            BaseDir = System.IO.Path.GetFullPath "."
            Version = "1.0.0"
            Files = []
            WithParams = id
            GenerateAssemblyInfo = true
        }

// project.fsx should define a project value.
let project: ProjectDef =
    { ProjectDef.Default with
        Files = ["Main.fs"]
        WithParams = fun (defaults: FscParams) ->
            { defaults with
                Output = "main.exe"
                // TODO: create a DSL for specifying library and target fx
                References = ["packages/FSharp.Core/lib/net40/FSharp.Core.dll"] }
    }

