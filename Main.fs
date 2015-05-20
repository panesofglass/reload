module Program

let work() =
    printfn "Hello, world!"

[<EntryPoint>]
let main args =
    printfn "Doing work with %A" args
    work()
    0
