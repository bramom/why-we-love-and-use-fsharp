// type Option<'a> =       // use a generic definition
//    | Some of 'a           // valid value
//    | None                 // missing

let o1: int Option = Some 42
let ``oh no 😢``: int Option = None
let o2 = Some 42

let areEqual = (o1=o2)

Option.map2 (+) o1 o2
Option.map2 (+) o1 None

[1;2;3;4]  |> List.tryFind (fun x-> x = 3)  // Some 3
[1;2;3;4]  |> List.tryFind (fun x-> x = 10) // None

match o1 with
| Some value ->
    printfn "%d" value
| None ->
    printfn "No value"