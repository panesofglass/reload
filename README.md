# Reload

Demonstrate building and watching F# compilations using only FAKE.

## Targets

* AssemblyInfo - generates an `AssemblyInfo.fs`
* Compile - builds a `.exe` from any `.fs` files in the root directory
* Help - displays the list of available targets
* Run - runs the generated `.exe`
* Watch - recompiles the `.exe` for any changes to any `.fs` files in the directory

## Running Watch on *nix

Add the following `export` to your `.bash_profile`:

```
export MONO_MANAGED_WATCHER=false
```

