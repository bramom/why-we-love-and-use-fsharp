// F# interactive
// <Alt> + Enter

(* 5. COMPLETENESS *)

// impure code when needed
let mutable counter = 0
counter <- 4

// create C# compatible classes and interfaces
type IEnumerator<'a> =
    abstract member Current : 'a
    abstract MoveNext : unit -> bool

// extension methods
type System.Int32 with
    member this.IsEven = this % 2 = 0

(2).IsEven

let i=20

if i.IsEven then printfn "'%i' is even" i