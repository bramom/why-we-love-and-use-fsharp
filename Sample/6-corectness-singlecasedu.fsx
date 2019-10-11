//what's wrong with this code?
let name = "John"

//single case discriminated union
type Name = Name of string

let name2 = Name "John"
let (Name value) = name2

printfn "%s" value

//add validation
type Email = private Email of string
    with
        static member Create s =
            if System.Text.RegularExpressions.Regex.IsMatch(s,@"^\S+@\S+\.\S+$")
                then Email s
                else invalidArg "s" "Invalid email adress"
        static member GetValue (Email e) = e
        member x.Value =
            let (Email e) = x
            e

//wrap
let email = Email.Create "fsharp@sinerija.live"

//unwrap
email.Value
|> printfn "%s"

email |> Email.GetValue
|> printfn "%s"

// code that uses the internals of the type fails to compile
Email "bad email"
