// Learn more about F# at http://fsharp.org

open System

module MyModule =
    let MyFun x y =
        x + y

type MyRecord =
    { Name: string }

type MyClass (name) =
    member this.Name = name

type IMyInterface =
   // abstract method
   abstract member Add: int -> int -> int

[<AbstractClass>]
type AbstractBaseClass() =
   // abstract method
   abstract member Add: int -> int -> int

type MyInterfaceImplementation () =
    do printfn "my interface implementation"

    interface IMyInterface with
        member this.Add x y =
            x + y

    interface System.IDisposable with
        member this.Dispose() =
            printfn "disposed"

[<EntryPoint>]
let main argv =    
    printfn "Hello World from F#!"

    argv.[1]
    |> MyModule.MyFun argv.[0]
    |> printfn "%s"

    printfn ""

    let myRecord = { Name = "jack" }
    printfn "%A" myRecord

    printfn ""

    let myClass = MyClass ("john")
    printfn "%A" myClass

    let testClassBasedOnInterface () =
        use myClassBasedOnInterface = new MyInterfaceImplementation()
        let adder = myClassBasedOnInterface :> IMyInterface
        let sum = adder.Add 4 5
        printfn "%d" sum

    testClassBasedOnInterface ()

    // create a new object that implements IDisposable
    let makeResource name =
       { new System.IDisposable
         with member this.Dispose() = printfn "%s disposed" name }

    let useAndDisposeResources () =
        use r1 = makeResource "first resource"
        printfn "using first resource"
        for i in [1..3] do
            let resourceName = sprintf "\tinner resource %d" i
            use temp = makeResource resourceName
            printfn "\tdo something with %s" resourceName
        use r2 = makeResource "second resource"
        printfn "using second resource"
        printfn "done."

    useAndDisposeResources ()

    printfn "ende"
    0 // return an integer exit code
