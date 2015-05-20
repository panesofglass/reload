# Reload

Demonstrate building and watching F# compilations using only FAKE.

## Targets

* AssemblyInfo - generates an `AssemblyInfo.fs`
* Compile - builds a `.exe` from any `.fs` files in the root directory
* Default - runs the `Compile` target
* Run - Runs the generated `.exe`
* Watch - should recompile the `.exe` for any changes to any `.fs` files in the directory, but currently appears to blocked by `System.Console.ReadLine`
