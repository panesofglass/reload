# Reload

Demonstrate building and watching F# compilations using only FAKE.

## Targets

* Compile -> builds a `.exe` from any `.fs` files in the root directory
* Default -> runs the `Compile` target
* Watch -> should recompile the `.exe` for any changes to any `.fs` files in the directory
