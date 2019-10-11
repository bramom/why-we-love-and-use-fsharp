open System
// F# interactive dotnet fsi
// #help;;
// #quit;;
// <Alt> + Enter

1 + 5

printfn "Hello World!"

printfn "%d" (1 + 5)

(1,2)

let l = [1;2;3]

for el in l do
    printfn "%d" el

(* 1. CONCISENESS *)

// no curly braces, semicolons or parentheses
let square x = x * x
let sq = square 42

// one-liners
printfn "sum=%d" (List.sum [1..100])
// Pipe-forward operator (|>), let (|>) x f = f x
[1..100] |> List.sum |> printfn "sum=%d"

// simple types in one line
type Person =
    { First:string
      Last:string }

// type inference
let jdoe =
    { First="John"
      Last="Doe" }

// automatic equality and comparison
let john = {First="John"; Last="Doe"}
printfn "Equal? %A"  (jdoe = john)

let isValid, result = Int32.TryParse("10")

// easy composition of functions
// Forward composition operator (>>), let (>>) f g x = g (f x)
let add2 x = x + 2
let mul3 x = x * 3
let add2times3 = add2 >> mul3

add2times3 5
