// F# interactive

// #help;;
// #quit;;
1 + 5

printfn "Hello World!"
printfn "%d" (1 + 5)

[1;2;3]

(1,2)

// # FSX
// Think of .FSX files as a single-file project

(* 1. CONCISENESS *)

// one-liners
[1..100] |> List.sum |> printfn "sum=%d"

// no curly braces, semicolons or parentheses
let square x = x * x
let sq = square 42

// simple types in one line
type Person = {First:string; Last:string}

// complex types in a few lines
type Employee =
  | Worker of Person
  | Manager of Employee list

// type inference
let jdoe = {First="John"; Last="Doe"}
let worker = Worker jdoe

// automatic equality and comparison
let person1 = {First="John"; Last="Doe"}
let person2 = {First="John"; Last="Doe"}
printfn "Equal? %A"  (person1 = person2)

// easy composition of functions
let add2 x = x + 2
let mul3 x = x * 3
let add2times3 = add2 >> mul3

add2times3 5

//do you want some more?

(* 2. CORRECTNESS *)

// strict type checking
printfn "print string %s" 123 //compile error

// all values immutable by default
person1.First <- "new name"  //assignment error

// never have to check for nulls
let personFullName person =
    //person properties can be used safely
    person.First + " " + person.Last

// units of measure
[<Measure>] type m
[<Measure>] type ft

5<m> + 2<m>
let distance = 10<m> + 10<ft> // error!

(* 4. CONCURRENCY *)

// easy async logic with "async" keyword
let! result = async {something}

// easy parallelism
Async.Parallel [ for i in 0..40 ->
      async { return fib(i) } ]

// message queues
MailboxProcessor.Start(fun inbox-> async{
	let! msg = inbox.Receive()
	printfn "message is: %s" msg
	})

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

// UI code
open System.Windows.Forms
let form = new Form(Width= 400, Height = 300,
   Visible = true, Text = "Hello World")
form.TopMost <- true
form.Click.Add (fun args-> printfn "clicked!")
form.Show()