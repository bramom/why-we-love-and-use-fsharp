// Learn more about F# at http://fsharp.org

open System

[<EntryPoint>]
let main argv =
    printfn "%A" argv

    printfn "What's your name:"
    let name = Console.ReadLine ()
    printfn "Hello %s" name

    let x = 5 + 1
    printfn "x: %d" x
    0 // return an integer exit code