type CardType = CardType of string
type CardNumber = CardNumber of string

type PaymentMethod =
  | Cash
  | Cheque of int
  | Card of CardType * CardNumber

let printPayment paymentMethod =
    match paymentMethod with
    | Cash ->
        printfn "Paid in cash"
    | Cheque checkNo ->
        printfn "Paid by cheque: %i" checkNo
    | Card (cardType,cardNo) ->
        printfn "Paid with %A %A" cardType cardNo

let cashPayment = Cash
let chequePayment  = Cheque 123
let cardPayment  = Card (CardType "Visa", CardNumber "123")

printPayment cashPayment
printPayment chequePayment
printPayment cardPayment