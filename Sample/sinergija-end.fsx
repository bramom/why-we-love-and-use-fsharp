// Card: Single choice union type s11-s17
type CardType = CardType of string
let cardType = CardType "VISA"
printfn "%A" cardType
let (CardType ct) = cardType
printfn "%s" ct
//cardType + ""
type CardNumber = CardNumber of string

// Tuples s21-s23
let card = (CardType "Master", CardNumber "5555 5555 5555 4444")
let printCard (CardType cardType, CardNumber cardNumber) =
    printfn "%s %s" (cardType.ToUpper()) cardNumber

printCard card

// Unions and match s31-s35
type PaymentMethod =
    | Cash
    | Card of CardType * CardNumber
    | Check of int

let printPayment paymentMethod =
    match paymentMethod with
    | Cash ->
        printfn "(Cash)"
    | Card (CardType cardType, CardNumber cardNumber) ->
        printfn "(%s '%s')" cardType cardNumber
    | _ ->
        printfn "unknown"

printPayment Cash

printPayment (Card card)

//Exhaustive pattern matching | Check of int

// Record type s41-s45
type Order =
    { OrderNo: int //OrderNumber?
      PaymentMethod: PaymentMethod option
      Note: string option }

let order1 = { OrderNo = 1; PaymentMethod = Some (Check 10); Note = None }
printfn "%A" order1

let order2 = { OrderNo = 1; PaymentMethod = Some (Check 10); Note = None }
let order3 = { order2 with Note = Some "note" }

order1 = order3


// Single case union including validation sinergija-emailtype.fsx