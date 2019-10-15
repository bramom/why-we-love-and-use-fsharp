namespace Core.ValueTypes.Tests

module TestDataGen =
    open System
    open System.Text.RegularExpressions
    open LanguagePrimitives
    open FsCheck
    open Test.Helpers
    open OpenLimo.Core.Library
    open OpenLimo.Core.Library.Measures
    open OpenLimo.Core.Library.TypeExtensions
    open OpenLimo.Core.Serialization
    open OpenLimo.Core.ValueTypes
    open OpenLimo.Core.ValueTypes.Amount
    open OpenLimo.Core.ValueTypes.Booking
    open OpenLimo.Core.ValueTypes.BookingPolicy
    open OpenLimo.Core.ValueTypes.Error

    let private nullEmptyAndWhiteSpaceStrings = [""; "   "; " "; "\t"; null]

    let private invalidJsons = [null; ""; "null"; "foo"; "asd\asd"]

    type InvalidJsonsGen =
        static member Values =
            Gen.elements invalidJsons
            |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module UserDefinedIdGen =
        let private firstLastLetterGen =
            let allowedChars = UPPERCASE_CHARS + LOWERCASE_CHARS
            Gen.elements allowedChars
            |> Gen.map string

        let private middlePartGen =
            let allowedChars =
                UPPERCASE_CHARS + LOWERCASE_CHARS + NUMERIC_CHARS + "_.-"
            Gen.choose (0, 100)
            |> Gen.map (randomString allowedChars 0)

        type Valid =
            static member Values =
                Gen.map3 (fun a b c -> a + b + c)
                    firstLastLetterGen
                    middlePartGen
                    firstLastLetterGen
                |> Arb.fromGen

        type Invalid =
            static member Values =
                let invalidValues =
                    [null; ""; "   "; "*bar"; "(foo)"; "džej";
                     "johndoe."; "j"; "john_"; "."; "-" ;"_" ;"_john";
                     "-john"; ".john"]
                invalidValues
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module GuidIdGen =
        type Valid =
            static member Values =
                Arb.Default.Guid().Generator
                |> Gen.map string
                |> Arb.fromGen

        type Invalid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (Guid.TryParse >> fst >> not)

    [<RequireQualifiedAccess>]
    module AggregateIdGen =
        type Instances =
            static member Values =
                GuidIdGen.Valid.Values.Generator
                |> Gen.map Aggregate.Id.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CellPhoneIdGen =
        type Instances =
            static member Values =
                GuidIdGen.Valid.Values.Generator
                |> Gen.map CellPhoneId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module LandlineIdGen =
        type Instances =
            static member Values =
                GuidIdGen.Valid.Values.Generator
                |> Gen.map LandlineId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AddressIdGen =
        type Instances =
            static member Values =
                GuidIdGen.Valid.Values.Generator
                |> Gen.map AddressId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module OfferIdGen =
        type Instances =
            static member Values =
                GuidIdGen.Valid.Values.Generator
                |> Gen.map OfferId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ResourceIdGen =
        type Instances =
            static member Values =
                GuidIdGen.Valid.Values.Generator
                |> Gen.map ResourceId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CreditCardIdGen =
        type Instances =
            static member Values =
                GuidIdGen.Valid.Values.Generator
                |> Gen.map CreditCardId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module TransactionIdGen =
        type Instances =
            static member Values =
                GuidIdGen.Valid.Values.Generator
                |> Gen.map TransactionId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module EmailIdGen =
        type Instances =
            static member Values =
                GuidIdGen.Valid.Values.Generator
                |> Gen.map EmailId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ExpectedVersionGen =
        type Valid =
            static member Values =
                Arb.Default.Int64().Generator
                |> Gen.filter(fun r -> r >= -1L && r <= 1000L)
                |> Arb.fromGen

        type Invalid =
            static member Values =
                Arb.Default.Int64().Generator
                |> Gen.filter(fun r -> r >= -1000L && r <= -2L)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module FirstNameGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map FirstName.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module LastNameGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map LastName.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module EmailAddressGen =
        let private validEmailAddresses =
            ["email@example.com"
             "firstname.lastname@example.com"
             "email@subdomain.example.com"
             "firstname+lastname@example.com"
             "email@123.123.123.123"
             "email@[123.123.123.123]"
             "“email”@example.com"
             "1234567890@example.com"
             "email@example-one.com"
             "_______@example.com"
             "email@example.name"
             "email@example.museum"
             "email@example.co.jp"
             "firstname-lastname@example.com"]

        type Valid =
            static member Values =
                validEmailAddresses
                |> Gen.elements
                |> Arb.fromGen

        type Invalid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (fun x ->
                    if isNull x then true
                    else not (x.Contains "@")
                )

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map EmailAddress.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CustomerIdGen =
        type Valid =
            static member Values =
                Gen.oneof
                    [UserDefinedIdGen.Valid.Values.Generator
                     EmailAddressGen.Valid.Values.Generator]
                |> Arb.fromGen

        type Invalid =
            static member Values =
                UserDefinedIdGen.Invalid.Values

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map CustomerId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module EmployeeIdGen =
        type Valid =
            static member Values =
                Gen.oneof
                    [UserDefinedIdGen.Valid.Values.Generator
                     EmailAddressGen.Valid.Values.Generator]
                |> Arb.fromGen

        type Invalid =
            static member Values =
                UserDefinedIdGen.Invalid.Values

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map EmployeeId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CellPhoneGen =
        let private numericStringGen =
            Gen.choose (2, 20)
            |> Gen.map (randomString NUMERIC_CHARS 2)

        let private invalidCellPhones =
            [(null, null); ("", ""); ("", "555333"); ("123d", ""); ("123d", "123456789"); ("123", ".1234567")]

        type Valid =
            static member Values =
                Gen.map2
                    (fun c p ->
                        { CountryCode = c
                          PhoneNumber = p } : CellPhone.Info
                    )
                    numericStringGen
                    numericStringGen
                |> Arb.fromGen

        type Invalid =
            static member Values =
                invalidCellPhones
                |> Gen.elements
                |> Gen.map (fun (c, p) ->
                    { CountryCode = c
                      PhoneNumber = p } : CellPhone.Info
                )
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map CellPhone.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module LandlineGen =
        let private nonNumericStringGen =
            Arb.Default.String().Generator
            |> Gen.filter (fun x ->
                if isNull x then true
                elif String.IsNullOrWhiteSpace x then false
                else not (Seq.forall Char.IsDigit x)
            )

        let private numericStringGen =
            Gen.choose (2, 20)
            |> Gen.map (randomString NUMERIC_CHARS 2)

        let private nonWhiteSpaceStringGen =
            Arb.Default.String().Generator
            |> Gen.filterNot String.IsNullOrWhiteSpace

        type Valid =
            static member Values =
                Gen.map4
                    (fun c a p e ->
                        { CountryCode = c
                          AreaCode = a
                          PhoneNumber = p
                          Extension = e } : Landline.Info)
                    numericStringGen
                    numericStringGen
                    numericStringGen
                    (Gen.optionOf nonWhiteSpaceStringGen)
                |> Arb.fromGen

        type Invalid =
            static member Values =
                Gen.map4
                    (fun c a p e ->
                        { CountryCode = c
                          AreaCode = a
                          PhoneNumber = p
                          Extension = e } : Landline.Info)
                    nonNumericStringGen
                    nonNumericStringGen
                    nonNumericStringGen
                    (Gen.optionOf (Arb.Default.String().Generator))
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map Landline.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module PasswordHashGen =
        type Valid =
            static member Values =
                Arb.Default.Byte().Generator
                |> Gen.arrayOf
                |> Gen.filter (fun x -> x.Length > 0)
                |> Gen.map (Convert.ToBase64String)
                |> Arb.fromGen

        type Invalid =
            static member Values =
                let trimmer = Regex("[ \n\r\t]");
                Arb.Default.String().Generator
                |> Gen.filter (fun x ->
                    if isNull x then true
                    else trimmer.Replace(x, "").Length % 4 <> 0
                )
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map Password.createFromHash
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module PasswordGen =

        let compexityErrorMessage = "Password must be at least 6 characters long"

        let complexityCheck (x: string) =
            if x.Length > 5 then Ok ()
            else Error compexityErrorMessage

        let hash (x: string) =
            let algo = new System.Security.Cryptography.SHA256Managed ()
            let bytes = System.Text.Encoding.UTF8.GetBytes (x)
            let hash = algo.ComputeHash (bytes)
            Convert.ToBase64String (hash)

        type Valid =
            static member Values =
                let allowedChars =
                    NUMERIC_CHARS + SPECIAL_CHARS
                    + UPPERCASE_CHARS + LOWERCASE_CHARS
                    + CYRILLIC_LOWERCASE_CHARS + CYRILLIC_UPPERCASE_CHARS
                Gen.choose (6, 100)
                |> Gen.map (randomString allowedChars 6)
                |> Arb.fromGen

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Weak =
            static member Values =
                let allowedChars =
                    NUMERIC_CHARS + SPECIAL_CHARS
                    + UPPERCASE_CHARS + LOWERCASE_CHARS
                    + CYRILLIC_LOWERCASE_CHARS + CYRILLIC_UPPERCASE_CHARS
                Gen.choose (1, 5)
                |> Gen.map (randomString allowedChars 1)
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map (Password.create complexityCheck hash)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ExternalLoginGen =
        let private allowedChars =
            NUMERIC_CHARS + SPECIAL_CHARS
            + UPPERCASE_CHARS + LOWERCASE_CHARS
            + CYRILLIC_LOWERCASE_CHARS + CYRILLIC_UPPERCASE_CHARS

        let private randomStringGen =
            Gen.choose (1, 50)
            |> Gen.map (randomString allowedChars 1)

        let private invalidExternalLogins =
            [(null, null, Some null)
             ("", "", Some null)
             ("  ", "  ", Some null)
             ("foo", "", Some null)
             ("", "bar", Some null)
             (null, "foo", Some null)
             ("foo", null, Some null)
             ("", null, Some null)
             (null, "foo", Some null)
             (null, " ", Some null)]

        type Valid =
            static member Values =
                Gen.map3
                    (fun p k n ->
                        { Provider = p
                          Key = k
                          Name = n } : ExternalLogin.Info
                    )
                    randomStringGen
                    randomStringGen
                    (Gen.optionOf randomStringGen)
                |> Arb.fromGen

        type Invalid =
            static member Values =
                invalidExternalLogins
                |> Gen.elements
                |> Gen.map (fun (p, k, n) ->
                    { Provider = p
                      Key = k
                      Name = n } : ExternalLogin.Info
                )
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map ExternalLogin.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AccountIdGen =
        type Instances =
            static member Values =
                UserDefinedIdGen.Valid.Values.Generator
                |> Gen.map AccountId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AccountNameGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map AccountName.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ServiceClassIdGen =
        type Instances =
            static member Values =
                UserDefinedIdGen.Valid.Values.Generator
                |> Gen.map ServiceClassId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ServiceClassNameGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map ServiceClassName.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ServiceClassDescriptionGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map ServiceClassDescription.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ServiceClassPaxCapacityGen =
        type Valid =
            static member Values =
                Arb.Default.PositiveInt().Generator
                |> Gen.map (fun x -> x.Get)
                |> Arb.fromGen

        type Invalid =
            static member Values =
                Arb.Default.NegativeInt().Generator
                |> Gen.map (fun x -> x.Get)
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map ServiceClassPaxCapacity.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ServiceClassLuggageCapacityGen =
        type Valid =
            static member Values =
                Arb.Default.PositiveInt().Generator
                |> Gen.map (fun x -> x.Get)
                |> Arb.fromGen

        type Invalid =
            static member Values =
                Arb.Default.NegativeInt().Generator
                |> Gen.map (fun x -> x.Get)
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map ServiceClassLuggageCapacity.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AddressNameGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map AddressName.create
                |> Arb.fromGen

    module CustomerBookingNoteGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << isNull)

        type Invalid =
            static member Values =
                let x: string = null
                Gen.constant x
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map CustomerBookingNote.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CreditCardExpirationDateGen =

        let private validExpirationGen =
            let months = [1..12]
            let years = [0..99]

            (months, years)
            ||> Seq.allPairs
            |> Seq.map (fun (month, year) -> sprintf "%02i/%02i" month year)
            |> Gen.elements

        let private invalidExpirationGen =
            Arb.Default.DateTime().Generator
            |> Gen.map string

        type Valid =
            static member Values =
                validExpirationGen
                |> Arb.fromGen

        type Invalid =
            static member Values =
                let mkGen =
                    Gen.map2 (fun expires error -> expires, error)

                [mkGen invalidExpirationGen (Gen.constant [Error.InvalidCreditCardExpirationDate])]
                |> Gen.oneof
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map CreditCardExpirationDate.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    type CreditCardOwnerGen =
        static member Values =
            Gen.map2
                (fun ownerKind ownerId -> ownerKind ownerId)
                (Gen.elements [Customer; Account])
                (AggregateIdGen.Instances.Values.Generator)
            |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CreditCardTypeGen =
        type Instances =
            static member Values =
                let americanExpress = Gen.constant (CreditCardType.AmericanExpress)
                let discover        = Gen.constant (CreditCardType.Discover)
                let masterCard      = Gen.constant (CreditCardType.MasterCard)
                let maestro         = Gen.constant (CreditCardType.Maestro)
                let visa            = Gen.constant (CreditCardType.Visa)
                let unionPay        = Gen.constant (CreditCardType.UnionPay)
                let unknown =
                    Arb.Default.NonEmptyString().Generator
                    |> Gen.map (fun value -> CreditCardType.Unknown value.Get)

                [americanExpress
                 discover
                 masterCard
                 maestro
                 visa
                 unionPay
                 unknown]
                |> Gen.oneof
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module UnmaskedCreditCardGen =
        let private validNameGen =
            LastNameGen.Valid.Values.Generator
            |> Gen.zip FirstNameGen.Valid.Values.Generator
            |> Gen.map (fun (f, l) -> f + " " + l)

        let private invalidNameGen = Gen.elements nullEmptyAndWhiteSpaceStrings

        let private validCreditCardNumbers =
            ["4485588122863384" // Visa
             "4783616269826374" // Visa
             "4556712983198185" // Visa
             "4539443650178936" // Visa
             "4539029699864584" // Visa
             "4485678408065620" // Visa
             "4532944261948333" // Visa
             "4024007190383553" // Visa
             "4111111111111111" // Visa
             "4012888888881881" // Visa
             "4222222222222"    // Visa
             "378282246310005"  // American Express
             "371449635398431"  // American Express
             "378734493671000"  // American Express Corporate
             "30569309025904"   // Diners Club
             "38520000023237"   // Diners Club
             "6011111111111117" // Discover
             "6011000990139424" // Discover
             "3530111333300000" // JCB
             "3566002020360505" // JCB
             "5555555555554444" // MasterCard
             "5105105105105100" // MasterCard
            ]

        let private invalidCreditCardNumbers =
            [null; ""; "4111111111111112"; "6011111111111117a"; "5105105105105101"]

        let private validPanGen =
            Gen.elements validCreditCardNumbers

        let private invalidPanGen =
            Gen.elements invalidCreditCardNumbers

        type Valid =
            static member Values =
                Gen.map3
                    (fun pan name expires ->
                        let info : UnmaskedCreditCard.Info =
                            { Pan = pan
                              NameOnCard = name
                              Expires = expires }
                        info
                    )
                    validPanGen
                    validNameGen
                    (CreditCardExpirationDateGen.Instances.Values.Generator)
                |> Arb.fromGen

        type Invalid =
            static member Values =
                let mkGen =
                    Gen.map4
                        (fun pan name expires error ->
                            let info : UnmaskedCreditCard.Info =
                                { Pan = pan
                                  NameOnCard = name
                                  Expires = expires }
                            info, error
                        )

                [mkGen invalidPanGen validNameGen (CreditCardExpirationDateGen.Instances.Values.Generator) (Gen.constant [Error.InvalidCreditCardPan])
                 mkGen invalidPanGen invalidNameGen (CreditCardExpirationDateGen.Instances.Values.Generator) (Gen.constant [Error.InvalidCreditCardPan; Error.InvalidCreditCardNameOnCard])]
                |> Gen.oneof
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map UnmaskedCreditCard.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module MaskedCreditCardGen =
        let validCreditCardTypeGen =
            CreditCardTypeGen.Instances.Values
            |> Arb.toGen

        let validStringGen =
            Arb.Default.String().Generator
            |> Gen.filterNot (String.IsNullOrWhiteSpace)

        let private invalidStringGen =
            Gen.elements nullEmptyAndWhiteSpaceStrings

        type Valid =
            static member Values =
                Gen.map3
                    (fun scrambledPan expires cardType ->
                        let info : MaskedCreditCard.Info =
                            { ScrambledPan = scrambledPan
                              Expires = expires
                              CardType = cardType }
                        info
                    )
                    validStringGen
                    (CreditCardExpirationDateGen.Instances.Values.Generator)
                    validCreditCardTypeGen
                |> Arb.fromGen

        type Invalid =
            static member Values =
                let mkGen =
                    Gen.map4
                        (fun scrambledPan expires cardType error ->
                            let info : MaskedCreditCard.Info =
                                { ScrambledPan = scrambledPan
                                  Expires = expires
                                  CardType = cardType }
                            info, error
                        )

                [mkGen invalidStringGen (CreditCardExpirationDateGen.Instances.Values.Generator) validCreditCardTypeGen (Gen.constant [Error.InvalidCreditCardScrambledPan])]
                |> Gen.oneof
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map MaskedCreditCard.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module LocationGen =
        let private validLatGen =
            Arb.Default.Decimal().Generator
            |> Gen.filter (fun x -> x > -90m && x < 90m)
            |> Gen.map toDeg

        let private validLngGen =
            Arb.Default.Decimal().Generator
            |> Gen.filter (fun x -> x > -180m && x < 180m)
            |> Gen.map toDeg

        let private invalidLatGen =
            Arb.Default.Decimal().Generator
            |> Gen.filter (fun x -> x < -90m || x > 90m)
            |> Gen.map toDeg

        let private invalidLngGen =
            Arb.Default.Decimal().Generator
            |> Gen.filter (fun x -> x < -180m || x > 180m)
            |> Gen.map toDeg

        type Valid =
            static member Values =
                Gen.zip validLatGen validLngGen
                |> Arb.fromGen

        type Invalid =
            static member Values =
                [Gen.zip3 invalidLatGen invalidLngGen (Gen.constant [InvalidLatitude; InvalidLongitude])
                 Gen.zip3 validLatGen invalidLngGen (Gen.constant [InvalidLongitude])
                 Gen.zip3 invalidLatGen validLngGen (Gen.constant [InvalidLatitude])]
                |> Gen.oneof
                |> Arb.fromGen

        type Instances =
            static member Values =
                Gen.zip validLatGen validLngGen
                |> Gen.map Location.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                ["""{ "latitude": 0.0, "longitude": null }"""
                 """{ "latitude": null, "longitude": null }"""
                 """{ "latitude": "foo", "longitude": true }"""
                 """{ "latitudeee": "asdas", "dummy": null }"""]
                 @ invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AddressGen =

        let private nullStringGen =
            Gen.elements nullEmptyAndWhiteSpaceStrings

        let private nonNullOrWhitespaceStringGen =
            Arb.Default.String().Generator
            |> Gen.filterNot String.IsNullOrWhiteSpace

        let private validCountryCodeGen =
            nonNullOrWhitespaceStringGen
            |> Gen.filter (fun s -> s.Length = 2)

        let private invalidCountryCodeGen =
            Arb.Default.String().Generator
            |> Gen.filter (fun s ->
                String.IsNullOrEmpty (s)
                || s.Length <> 2
            )

        let private validAddressInfoGen =
            Gen.map3
                (fun nonNull str code ->
                    { StreetName = nonNull
                      StreetNumber = nonNull
                      Apartment = str
                      City = nonNull
                      Landmark = str
                      State = str
                      PostalCode = str
                      CountryCode = code
                      Formatted = str } : Address.Info
                )
                nonNullOrWhitespaceStringGen
                (Arb.Default.String().Generator)
                validCountryCodeGen

        let private emptyInfo =
            // empty fields here are mutated, because they're the subject of validation and, thus, tests
            ({ StreetName = ""
               StreetNumber = "42"
               Apartment = "foo_apartment"
               City = ""
               Landmark = ""
               State = "foo_Arizona"
               PostalCode = "foo_zip"
               CountryCode = ""
               Formatted = "" } : Address.Info)
            |> Gen.constant


        let private setStreetName generator =
            generator
            |> Gen.map (fun value ->
                fun addressInfo -> { addressInfo with StreetName = value } : Address.Info
            )

        let private setStreetNumber generator =
            generator
            |> Gen.map (fun value ->
                fun addressInfo -> { addressInfo with StreetNumber = value } : Address.Info
            )

        let private setPostalCode generator =
            generator
            |> Gen.map (fun value ->
                fun addressInfo -> { addressInfo with PostalCode = value } : Address.Info
            )

        let private setCountryCode generator =
            generator
            |> Gen.map (fun value ->
                fun addressInfo -> { addressInfo with CountryCode = value } : Address.Info
            )

        let private setCity generator =
            generator
            |> Gen.map (fun value ->
                fun addressInfo -> { addressInfo with City = value } : Address.Info
            )

        type Valid =
            static member Values =
                LocationGen.Instances.Values.Generator
                |> Gen.zip validAddressInfoGen
                |> Arb.fromGen

        type Invalid =
            static member Values =
                let invalidStreet =
                    emptyInfo
                    |> Gen.apply (setStreetName   nullStringGen)
                    |> Gen.apply (setStreetNumber nullStringGen)
                    |> Gen.apply (setPostalCode   nullStringGen)
                    |> Gen.apply (setCountryCode  validCountryCodeGen)
                    |> Gen.apply (setCity         nonNullOrWhitespaceStringGen)
                    |> Gen.zip3
                        (Gen.constant [InvalidAddressStreet])
                        LocationGen.Instances.Values.Generator

                let invalidCity =
                    emptyInfo
                    |> Gen.apply (setStreetName   nonNullOrWhitespaceStringGen)
                    |> Gen.apply (setCountryCode  validCountryCodeGen)
                    |> Gen.apply (setCity         nullStringGen)
                    |> Gen.zip3
                        (Gen.constant [InvalidAddressCity])
                        LocationGen.Instances.Values.Generator

                let invalidCountryCode =
                    emptyInfo
                    |> Gen.apply (setStreetName   nonNullOrWhitespaceStringGen)
                    |> Gen.apply (setCountryCode  invalidCountryCodeGen)
                    |> Gen.apply (setCity         nonNullOrWhitespaceStringGen)
                    |> Gen.zip3
                        (Gen.constant [InvalidCountryCode])
                        LocationGen.Instances.Values.Generator

                let invalidAll =
                    emptyInfo
                    |> Gen.apply (setStreetName   nullStringGen)
                    |> Gen.apply (setStreetNumber nullStringGen)
                    |> Gen.apply (setCountryCode  nullStringGen)
                    |> Gen.apply (setCity         nullStringGen)
                    |> Gen.apply (setPostalCode   nullStringGen)
                    |> Gen.zip3
                        (Gen.constant [InvalidCountryCode; InvalidAddressCity; InvalidAddressStreet])
                        LocationGen.Instances.Values.Generator

                [invalidStreet; invalidCity; invalidCountryCode; invalidAll]
                |> Gen.oneof
                |> Gen.map (fun (errors, lat, info) -> (info, lat, errors))
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map Address.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                ["""{ "location": { "latitude": 23.3323, "longitude": 34.3434 } }"""
                 """{ "location": null }"""
                 """{
                        "streetName": "",
                        "streetNumber": "",
                        "apartment": "",
                        "landmark": "",
                        "city": "",
                        "state": "",
                        "postalCode": "",
                        "countryCode": "",
                        "formatted": "",
                        "location": null
                    }"""
                 """{
                        "streetName": "",
                        "streetNumber": "",
                        "apartment": "",
                        "landmark": "",
                        "city": "",
                        "state": "",
                        "postalCode": "",
                        "countryCode": "",
                        "formatted": "",
                        "location": {
                            "latitude": null,
                            "longitude": 34.3434
                        }
                    }"""]
                 @ invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AirportCodeGen =
        type Valid =
            static member Values =
                Arb.Default.NonEmptyString().Generator
                |> Gen.map (fun s -> s.Get.RemoveWhitespaces ())
                |> Gen.filter (fun s -> s.Length = 3)
                |> Arb.fromGen

        type Invalid =
            static member Values =
                let invalidCodes =
                    Arb.Default.String().Generator
                    |> Gen.filter (fun s ->
                        isNull s || s.RemoveWhitespaces().Length <> 3
                    )

                Gen.zip
                    invalidCodes
                    (Gen.constant [InvalidAirportCode])
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map AirportCode.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AirportGen =
        let private invalidNameGen =
            Gen.elements nullEmptyAndWhiteSpaceStrings

        let private validNameGen =
            Arb.Default.String().Generator
            |> Gen.filterNot String.IsNullOrWhiteSpace

        let validTerminalGen =
            Arb.Default.NonEmptyString().Generator
            |> Gen.map (fun s -> s.Get)
            |> Gen.filterNot String.IsNullOrWhiteSpace
            |> Gen.optionOf

        let invalidTerminalGen =
            Gen.elements nullEmptyAndWhiteSpaceStrings
            |> Gen.map Some

        type Valid =
            static member Values =
                Gen.map4
                    (fun code name terminal location ->
                        let info : Airport.Info =
                            { Code = code
                              Name = name
                              Terminal = terminal }
                        info, location
                    )
                    AirportCodeGen.Instances.Values.Generator
                    validNameGen
                    validTerminalGen
                    LocationGen.Instances.Values.Generator

                |> Arb.fromGen

        type Invalid =
            static member Values =
                let mkAirport code name terminal location error =
                    let info : Airport.Info =
                        { Code = code
                          Name = name
                          Terminal = terminal }
                    info, location, error

                [Gen.map5 mkAirport AirportCodeGen.Instances.Values.Generator validNameGen invalidTerminalGen LocationGen.Valid.Values.Generator (Gen.constant [InvalidAirportTerminal] )
                 Gen.map5 mkAirport AirportCodeGen.Instances.Values.Generator invalidNameGen invalidTerminalGen LocationGen.Valid.Values.Generator (Gen.constant [InvalidAirportName; InvalidAirportTerminal] )
                 Gen.map5 mkAirport AirportCodeGen.Instances.Values.Generator invalidNameGen validTerminalGen LocationGen.Valid.Values.Generator (Gen.constant [InvalidAirportName] ) ]
                |> Gen.oneof
                |> Arb.fromGen
                |> Arb.mapFilter id (fun _ -> true)

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map Airport.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                ["""{ "code": "BEG", "name": "Aerodrom Nikola Tesla", "terminal": null, "location": null }"""
                 """{ "location": { "latitude": 23.3323, "longitude": 34.3434 } }"""
                 """{ "location": null }"""
                 """{ "code": "BEG", "name": null, "terminal": "2", "location": { "latitude": null, "longitude": 34.3434 } }"""]
                 @ invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module FlightNumberGen =
        let private validAirlineGen =
            Arb.Default.NonEmptyString().Generator
            |> Gen.map (fun s -> s.Get.RemoveWhitespaces ())
            |> Gen.filter (fun s -> s.Length = 2)

        let private invalidAirlineGen =
            Arb.Default.String().Generator
            |> Gen.filter (fun s ->
                isNull s || s.RemoveWhitespaces().Length <> 2
            )

        let private validFlightNumberGen =
            Arb.Default.NonEmptyString().Generator
            |> Gen.map (fun s -> s.Get.RemoveWhitespaces ())
            |> Gen.filterNot String.IsNullOrWhiteSpace

        let private invalidFlightNumberGen =
            Gen.elements nullEmptyAndWhiteSpaceStrings

        type Valid =
            static member Values =
                Gen.map2
                    (fun a n ->
                        { Airline = a
                          FlightNumber = n } : FlightNumber.Info
                    )
                    validAirlineGen
                    validFlightNumberGen
                |> Arb.fromGen

        type Invalid =
            static member Values =
                let mkGen =
                    Gen.map3
                        (fun a n e ->
                            let info : FlightNumber.Info =
                                { Airline = a
                                  FlightNumber = n }
                            info, e
                        )

                [mkGen invalidAirlineGen validFlightNumberGen (Gen.constant [InvalidAirlineCode])
                 mkGen validAirlineGen invalidFlightNumberGen (Gen.constant [InvalidFlightNumber])
                 mkGen invalidAirlineGen invalidFlightNumberGen (Gen.constant [InvalidAirlineCode;InvalidFlightNumber])]
                |> Gen.oneof
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map FlightNumber.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AircraftTailNumberGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map AircraftTailNumber.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module FlightTimeGen =
        type Instances =
            static member Values =
                let scheduledGen =
                    Arb.Default.DateTimeOffset().Generator
                    |> Gen.map FlightTime.Scheduled

                let estimatedGen =
                    Arb.Default.DateTimeOffset().Generator
                    |> Gen.map FlightTime.Estimated

                let actualGen =
                    Arb.Default.DateTimeOffset().Generator
                    |> Gen.map FlightTime.Actual

                Gen.oneof [scheduledGen; estimatedGen; actualGen]
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module FlightStatusGen =
        type Instances =
            static member Values =
                [FlightStatus.Scheduled
                 FlightStatus.Departed
                 FlightStatus.InAir
                 FlightStatus.Landed
                 FlightStatus.Arrived
                 FlightStatus.Expected
                 FlightStatus.Delayed
                 FlightStatus.Canceled
                 FlightStatus.Unknown]
                |> Gen.elements
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module FlightInfoGen =
        type Instances =
            static member Values =
                Gen.map5
                    (fun aa da dt at s ->
                        { ArrivingAirport = aa
                          DepartingAirport = da
                          DepartureTime = dt
                          ArrivalTime = at
                          Status = s }
                    )
                    AirportGen.Instances.Values.Generator
                    AirportGen.Instances.Values.Generator
                    FlightTimeGen.Instances.Values.Generator
                    FlightTimeGen.Instances.Values.Generator
                    FlightStatusGen.Instances.Values.Generator
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module FlightDepartureDateGen =
        type ValidStrings =
            static member Values =
                Arb.Default.DateTimeOffset ()
                |> Arb.toGen
                |> Gen.map (stringf "yyyyMMdd")
                |> Arb.fromGen

        type ValidInts =
            static member Values =
                Arb.Default.DateTimeOffset ()
                |> Arb.toGen
                |> Gen.map (fun date -> date.Year, date.Month, date.Day)
                |> Arb.fromGen

        let private invalidYears =
            Arb.Default.Int32 ()
            |> Arb.toGen
            |> Gen.filter (fun x -> x < 1 || x > 9999)

        let private invalidMonths =
            Arb.Default.Int32 ()
            |> Arb.toGen
            |> Gen.filter (fun x -> x < 1 || x > 12)

        let private invalidDays =
            Arb.Default.Int32 ()
            |> Arb.toGen
            |> Gen.filter (fun x -> x < 1 || x > 31)

        type InvalidInts =
            static member Values =
                Gen.map4
                    (fun y m d error -> (y, m, d, error))
                    invalidYears
                    invalidMonths
                    invalidDays
                    (Gen.constant [InvalidFlightDepartureDate])
                |> Arb.fromGen

        type InvalidStrings =
            static member Values =
                InvalidInts.Values
                |> Arb.toGen
                |> Gen.map (fun (year, month, day, error) -> (sprintf "%04i%02i%02i" year month day), error)
                |> Arb.fromGen

        type Instances =
            static member Values =
                ValidStrings.Values.Generator
                |> Gen.map FlightDepartureDate.create
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen


    [<RequireQualifiedAccess>]
    module AmountGen =

        let private supportedCurrencies = ["USD"; "EUR"; "GBP"]

        let private validCurrencies = Gen.elements supportedCurrencies

        let private invalidCurrencies =
            Arb.Default.String().Generator
            |> Gen.filterNot (fun s -> List.exists ((=) s) supportedCurrencies)

        let decimalAmountGen =
            Arb.Default.Decimal().Generator
            // very big amount of money in almost any currency
            // limited because we sum base and extras and can overflow with unlimited decimal generator
            |> Gen.filterNot (fun x -> x < -100_000_000_000_000_000m || x > 100_000_000_000_000_000m)
            |> Gen.map (fun x -> Math.Round(x, 4))

        let positiveDecimalAmountGen =
            Arb.Default.Decimal().Generator
            |> Gen.filterNot (fun x -> x > 100_000_000_000_000_000m || x <= 0m)
            |> Gen.map (fun x -> Math.Round(x, 4))

        let negativeDecimalAmountGen =
            // using NegativeInt generator as Decimal with filter takes forever to execute
            Arb.Default.NegativeInt().Generator
            |> Gen.map (fun x -> decimal x.Get)
            |> Gen.filterNot (fun x -> x < -100_000_000_000_000_000m)
            |> Gen.map (fun x -> Math.Round(x, 4))

        let positiveInstancesGen =
            positiveDecimalAmountGen
            |> Gen.zip validCurrencies
            |> Gen.map Amount.create

        let negativeInstancesGen =
            negativeDecimalAmountGen
            |> Gen.zip validCurrencies
            |> Gen.map Amount.create

        let zeroInstancesGen =
            Gen.constant 0m
            |> Gen.zip validCurrencies
            |> Gen.map Amount.create


        type Valid =
            static member Values =
                decimalAmountGen
                |> Gen.zip validCurrencies
                |> Arb.fromGen

        type Invalid =
            static member Values =
                Gen.zip3
                    invalidCurrencies
                    decimalAmountGen
                    (Gen.constant [InvalidCurrencyCode])
                |> Arb.fromGen

        type Instances =
            static member Values =
                Gen.oneof
                    [positiveInstancesGen
                     negativeInstancesGen
                     zeroInstancesGen]
                |> Arb.fromGen

        type PositiveInstances =
            static member Values =
                positiveInstancesGen
                |> Arb.fromGen

        type ZeroOrPositiveInstances =
            static member Values =
                Gen.oneof
                    [positiveInstancesGen
                     zeroInstancesGen]
                |> Arb.fromGen

        type ZeroOrNegativeInstances =
            static member Values =
                Gen.oneof
                    [negativeInstancesGen
                     zeroInstancesGen]
                |> Arb.fromGen

        type NegativeInstances =
            static member Values =
                negativeInstancesGen
                |> Arb.fromGen

        type RoundedAmounts =
            static member Values =
                [(1.0m, 1.0m)
                 (9.99499999m, 9.99m)
                 (9.995m, 10.00m)
                 (10.005m, 10.00m)
                 (10.00500001m, 10.01m)]
                |> List.map (fun (given, expected) ->
                    (USD given, USD expected)
                )
                |> Gen.elements
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                ["\"USD 123ss\""
                 "USD 123"
                 "123"
                 "USD"]
                 @ invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module EmailAddressContactGen =
        type Instances =
            static member Values =
                EmailAddressGen.Valid.Values.Generator
                |> Gen.map EmailAddressContact.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CellPhoneContactGen =
        type Instances =
            static member Values =
                CellPhoneGen.Valid.Values.Generator
                |> Gen.map CellPhoneContact.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module LandlineContactGen =
        type Instances =
            static member Values =
                LandlineGen.Valid.Values.Generator
                |> Gen.map LandlineContact.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module TripEstimateGen =
        type Instances =
            static member Values =
                Gen.map2
                    (fun l d ->
                        { Length = l * 1.<kilometer>
                          Duration = d * 1.<second> }
                    )
                    (Arb.Default.NormalFloat().Generator |> Gen.map (fun x -> x.Get))
                    (Arb.Default.NormalFloat().Generator |> Gen.map (fun x -> x.Get))
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module TripPickupTimeGen =
        type Instances =
            static member Values =
                let exactGen =
                    Arb.Default.DateTimeOffset().Generator
                    |> Gen.map TripPickupTime.Exact

                let estimatedGen =
                    Arb.Default.DateTimeOffset().Generator
                    |> Gen.map TripPickupTime.Estimated

                Gen.oneof [exactGen; estimatedGen]
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CustomerInfoGen =
        type Instances =
            static member Values =
                Gen.map3
                    (fun id firstName lastName ->
                        { Id = id
                          FirstName = firstName
                          LastName = lastName }
                    )
                    AggregateIdGen.Instances.Values.Generator
                    FirstNameGen.Instances.Values.Generator
                    LastNameGen.Instances.Values.Generator
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AccountInfoGen =
        type Instances =
            static member Values =
                Gen.map2
                    (fun id name ->
                        { Id = id
                          Name = name }
                    )
                    AggregateIdGen.Instances.Values.Generator
                    AccountNameGen.Instances.Values.Generator
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module BookerGen =
        type Instances =
            static member Values =
                let privateBookerGen =
                    CustomerInfoGen.Instances.Values.Generator
                    |> Gen.map Private
                let corporateBookerGen =
                    AccountInfoGen.Instances.Values.Generator
                    |> Gen.zip CustomerInfoGen.Instances.Values.Generator
                    |> Gen.map Corporate

                Gen.oneof [privateBookerGen; corporateBookerGen]
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ItineraryGen =

        // PickupTime

        let private exactPickupTimeGen =
            Arb.Default.DateTimeOffset().Generator
            |> Gen.map PickupTime.Exact

        let private asapPickupTimeGen =
            Arb.Default.DateTimeOffset().Generator
            |> Gen.map PickupTime.ASAP

        let private arriveAtPickupTimeGen =
            Arb.Default.DateTimeOffset().Generator
            |> Gen.map PickupTime.ArriveAt

        let private validAfterLandingPickupTimeGen =
            Arb.Default.PositiveInt().Generator
            |> Gen.map (fun x -> PickupTime.AfterLanding (x.Get * 1<minute>))

        let private validArriveBeforeTakeoffPickupTimeGen =
            Arb.Default.PositiveInt().Generator
            |> Gen.map (fun x -> PickupTime.ArriveBeforeTakeoff (x.Get * 1<minute>))

        let private invalidRelativePickupTimeGen =
            let invalidAfterLandingPickupTimeGen =
                Arb.Default.NegativeInt().Generator
                |> Gen.map (fun x -> PickupTime.AfterLanding (x.Get * 1<minute>))

            let invalidArriveBeforeTakeoffPickupTimeGen =
                Arb.Default.NegativeInt().Generator
                |> Gen.map (fun x -> PickupTime.ArriveBeforeTakeoff (x.Get * 1<minute>))

            Gen.oneof
                [invalidAfterLandingPickupTimeGen
                 invalidArriveBeforeTakeoffPickupTimeGen]

        // AirportMeetingPoing

        let private meetingPointGen =
            let baggageClaimGen = Gen.constant AirportMeetingPoint.BaggageClaim
            let curbsideGen =
                Arb.Default.String().Generator
                |> Gen.map AirportMeetingPoint.Curbside
            Gen.oneof [baggageClaimGen; curbsideGen]

        // Flight

        let private trackableCommercialFlightGen =
            Gen.map3
                (fun departureDate flightNumber flightInfo ->
                    TrackableCommercial (departureDate, flightNumber, flightInfo), flightInfo
                )
                FlightDepartureDateGen.Instances.Values.Generator
                FlightNumberGen.Instances.Values.Generator
                FlightInfoGen.Instances.Values.Generator

        let private untrackableFlightGen =
            let noFlightGen = Gen.constant Flight.NoFlight

            let untrackableCommercialFlightGen =
                Gen.map2
                    (fun departureDate flightNumber ->
                        UntrackableCommercial (departureDate, flightNumber)
                    )
                    FlightDepartureDateGen.Instances.Values.Generator
                    FlightNumberGen.Instances.Values.Generator

            let untrackablePrivateFlightGen =
                AircraftTailNumberGen.Instances.Values.Generator
                |> Gen.map UntrackablePrivate

            Gen.oneof
                [noFlightGen
                 untrackableCommercialFlightGen
                 untrackablePrivateFlightGen]

        // Pickup

        let private validPickupWithTrackableFlightGen =
            Gen.map2
                (fun meetingPoint (flight, flightInfo) ->
                    Pickup.Airport (flightInfo.ArrivingAirport, meetingPoint, flight)
                )
                meetingPointGen
                trackableCommercialFlightGen

        let private invalidPickupWithTrackableFlightGen =
            let fooAirport =
                Gen.sample 1 1 AirportGen.Instances.Values.Generator
                |> Seq.head

            Gen.map2
                (fun meetingPoint (flight, flightInfo) ->
                    Pickup.Airport (fooAirport, meetingPoint, flight)
                )
                meetingPointGen
                (trackableCommercialFlightGen
                 |> Gen.filterNot (fun (_, x) -> Airport.code x.ArrivingAirport = Airport.code fooAirport))

        let private pickupWithoutTrackableFlightGen =
            let withoutTrackableFlightGen =
                Gen.map3
                    (fun airport meetingPoint flight ->
                        Pickup.Airport (airport, meetingPoint, flight)
                    )
                    AirportGen.Instances.Values.Generator
                    meetingPointGen
                    untrackableFlightGen

            let addressGen =
                AddressGen.Instances.Values.Generator
                |> Gen.map Pickup.Address

            Gen.oneof
                [withoutTrackableFlightGen
                 addressGen]

        // Waypoint

        let private waypointGen =
            let addressGen =
                AddressGen.Instances.Values.Generator
                |> Gen.map Waypoint.Address

            let airportGen =
                AirportGen.Instances.Values.Generator
                |> Gen.map Waypoint.Airport

            Gen.oneof
                [addressGen
                 airportGen]

        // DropoffPoint

        let private validDropoffPointWithTrackableFlightGen =
            trackableCommercialFlightGen
            |> Gen.map (fun (flight, flightInfo) ->
                DropoffPoint.Airport (flightInfo.DepartingAirport, flight)
            )

        let private invalidDropoffPointWithTrackableFlightGen =
            let fooAirport =
                Gen.sample 1 1 AirportGen.Instances.Values.Generator
                |> Seq.head

            trackableCommercialFlightGen
            |> Gen.filterNot (fun (_, x) -> Airport.code x.ArrivingAirport = Airport.code fooAirport)
            |> Gen.map (fun (flight, flightInfo) ->
                DropoffPoint.Airport (fooAirport, flight)
            )

        let private dropoffPointWithoutTrackableFlightGen =
            let withoutTrackableFlightGen =
                Gen.map2
                    (fun airport flight ->
                        DropoffPoint.Airport (airport, flight)
                    )
                    AirportGen.Instances.Values.Generator
                    untrackableFlightGen

            let addressGen =
                AddressGen.Instances.Values.Generator
                |> Gen.map DropoffPoint.Address

            Gen.oneof
                [withoutTrackableFlightGen
                 addressGen]

        // Dropoff

        let private dropoffWithoutTrackableFlightGen =
            dropoffPointWithoutTrackableFlightGen
            |> Gen.map Point

        let private validDropoffWithTrackableFlightGen =
            validDropoffPointWithTrackableFlightGen
            |> Gen.map Point

        let private invalidDropoffWithTrackableFlightGen =
            invalidDropoffPointWithTrackableFlightGen
            |> Gen.map Point

        let private validRoundTripWithoutTrackableFlightGen =
            Gen.map2
                (fun point wt ->
                    RoundTrip (point, wt * 1<minute>)
                )
                dropoffPointWithoutTrackableFlightGen
                (Arb.Default.PositiveInt().Generator
                 |> Gen.map (fun x -> x.Get))

        let private validRoundTripWithTrackableFlightGen =
            Gen.map2
                (fun point wt ->
                    RoundTrip (point, wt * 1<minute>)
                )
                validDropoffPointWithTrackableFlightGen
                (Arb.Default.PositiveInt().Generator
                 |> Gen.map (fun x -> x.Get))

        let private invalidRoundTripWithTrackableFlightGen =
            Gen.map2
                (fun point wt ->
                    RoundTrip (point, wt * 1<minute>)
                )
                invalidDropoffPointWithTrackableFlightGen
                (Arb.Default.PositiveInt().Generator
                 |> Gen.map (fun x -> x.Get))

        let private validDropoffHourlyGen =
            Arb.Default.PositiveInt().Generator
            |> Gen.map (fun x -> Hourly (x.Get * 1<minute>))

        let private invalidRoundTripWaitingTimeDurationGen =
            let invalidRoundTripWithoutTrackableFlightGen =
                Gen.map2
                    (fun point wt ->
                        RoundTrip (point, wt * 1<minute>)
                    )
                    dropoffPointWithoutTrackableFlightGen
                    (Arb.Default.NegativeInt().Generator
                     |> Gen.map (fun x -> x.Get))

            let invalidRoundTripWithTrackableFlightGen =
                Gen.map2
                    (fun point wt ->
                        RoundTrip (point, wt * 1<minute>)
                    )
                    validDropoffPointWithTrackableFlightGen
                    (Arb.Default.NegativeInt().Generator
                     |> Gen.map (fun x -> x.Get))

            Gen.oneof
                [invalidRoundTripWithoutTrackableFlightGen
                 invalidRoundTripWithTrackableFlightGen]

        let private invalidDropoffHourlyGen =
            let zeroGen = Gen.constant (Hourly 0<minute>)

            let negativeGen =
                Arb.Default.NegativeInt().Generator
                |> Gen.map (fun x -> Hourly (x.Get * 1<minute>))

            Gen.oneof [zeroGen; negativeGen]

        (*
            // do not remove this.. it is easier to have all in one place for reference
            let allGen' =
                Gen.map4
                    (fun pickupTime pickup waypoints dropoff ->
                        { PickupTime = pickupTime
                          Pickup = pickup
                          Waypoints = waypoints
                          Dropoff = dropoff } : Itinerary.Info
                    )
                    (// pickup time
                     Gen.oneof
                        [exactPickupTimeGen
                         asapPickupTimeGen
                         arriveAtPickupTimeGen
                         afterLandingPickupTimeGen
                         arriveBeforeTakeoffPickupTimeGen])
                    (// pickup
                     Gen.oneof
                        [pickupWithTrackableFlightGen
                         pickupWithoutTrackableFlightGen])
                    (// waypoints
                     Gen.listOf waypointGen)
                    (// dropoff
                     Gen.oneof
                        [dropoffWithoutTrackableFlightGen
                         dropoffWithTrackableFlightGen
                         roundTripWithoutTrackableFlightGen
                         roundTripWithTrackableFlightGen
                         dropoffHourlyGen])
        *)

        type Valid =
            static member Values =
                let afterLandingGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            { PickupTime = pickupTime
                              Pickup = pickup
                              Waypoints = waypoints
                              Dropoff = dropoff } : Itinerary.Info
                        )
                        (// pickup time
                         validAfterLandingPickupTimeGen)
                        (// pickup
                         validPickupWithTrackableFlightGen)
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         Gen.oneof
                            [dropoffWithoutTrackableFlightGen
                             validDropoffWithTrackableFlightGen
                             validRoundTripWithoutTrackableFlightGen
                             validRoundTripWithTrackableFlightGen
                             validDropoffHourlyGen])

                let arriveBeforeTakeoffGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            { PickupTime = pickupTime
                              Pickup = pickup
                              Waypoints = waypoints
                              Dropoff = dropoff } : Itinerary.Info
                        )
                        (// pickup time
                         validArriveBeforeTakeoffPickupTimeGen)
                        (// pickup
                         Gen.oneof
                            [validPickupWithTrackableFlightGen
                             pickupWithoutTrackableFlightGen])
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         Gen.oneof
                            [validDropoffWithTrackableFlightGen
                             validRoundTripWithTrackableFlightGen])

                let arriveAtGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            { PickupTime = pickupTime
                              Pickup = pickup
                              Waypoints = waypoints
                              Dropoff = dropoff } : Itinerary.Info
                        )
                        (// pickup time
                         arriveAtPickupTimeGen)
                        (// pickup
                         Gen.oneof
                            [validPickupWithTrackableFlightGen
                             pickupWithoutTrackableFlightGen])
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         Gen.oneof
                            [dropoffWithoutTrackableFlightGen
                             validDropoffWithTrackableFlightGen
                             validRoundTripWithoutTrackableFlightGen
                             validRoundTripWithTrackableFlightGen])

                let exactAndAsapGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            { PickupTime = pickupTime
                              Pickup = pickup
                              Waypoints = waypoints
                              Dropoff = dropoff } : Itinerary.Info
                        )
                        (// pickup time
                         Gen.oneof
                            [exactPickupTimeGen
                             asapPickupTimeGen])
                        (// pickup
                         Gen.oneof
                            [validPickupWithTrackableFlightGen
                             pickupWithoutTrackableFlightGen])
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         Gen.oneof
                            [dropoffWithoutTrackableFlightGen
                             validDropoffWithTrackableFlightGen
                             validRoundTripWithoutTrackableFlightGen
                             validRoundTripWithTrackableFlightGen
                             validDropoffHourlyGen])

                Gen.oneof
                    [afterLandingGen
                     arriveBeforeTakeoffGen
                     arriveAtGen
                     exactAndAsapGen]
                |> Arb.fromGen

        type Invalid =
            static member Values =
                let noTrackableArrivingFlightGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            let info : Itinerary.Info =
                                { PickupTime = pickupTime
                                  Pickup = pickup
                                  Waypoints = waypoints
                                  Dropoff = dropoff }
                            info, [PickupTimeRequiresTrackableArrivingFlight]
                        )
                        (// pickup time
                         validAfterLandingPickupTimeGen)
                        (// pickup
                         pickupWithoutTrackableFlightGen)
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         Gen.oneof
                            [dropoffWithoutTrackableFlightGen
                             validDropoffWithTrackableFlightGen
                             validRoundTripWithoutTrackableFlightGen
                             validRoundTripWithTrackableFlightGen
                             validDropoffHourlyGen])

                let noTrackableDepartingFlightGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            let info : Itinerary.Info =
                                { PickupTime = pickupTime
                                  Pickup = pickup
                                  Waypoints = waypoints
                                  Dropoff = dropoff }
                            info, [PickupTimeRequiresTrackableDepartingFlight]
                        )
                        (// pickup time
                         validArriveBeforeTakeoffPickupTimeGen)
                        (// pickup
                         Gen.oneof
                            [validPickupWithTrackableFlightGen
                             pickupWithoutTrackableFlightGen])
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         Gen.oneof
                            [dropoffWithoutTrackableFlightGen
                             validRoundTripWithoutTrackableFlightGen])

                let pickupTimeNotAllowedGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            let info : Itinerary.Info =
                                { PickupTime = pickupTime
                                  Pickup = pickup
                                  Waypoints = waypoints
                                  Dropoff = dropoff }
                            info, [PickupTimeNotAllowedForHourly]
                        )
                        (// pickup time
                         Gen.oneof
                            [arriveAtPickupTimeGen
                             validArriveBeforeTakeoffPickupTimeGen])
                        (// pickup
                         Gen.oneof
                            [validPickupWithTrackableFlightGen
                             pickupWithoutTrackableFlightGen])
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         validDropoffHourlyGen)

                let invalidArrivingFlightInfoGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            let info : Itinerary.Info =
                                { PickupTime = pickupTime
                                  Pickup = pickup
                                  Waypoints = waypoints
                                  Dropoff = dropoff }
                            info, [InvalidArrivingFlightInfo]
                        )
                        (// pickup time
                         validAfterLandingPickupTimeGen)
                        (// pickup
                         invalidPickupWithTrackableFlightGen)
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         Gen.oneof
                            [dropoffWithoutTrackableFlightGen
                             validDropoffWithTrackableFlightGen
                             validRoundTripWithoutTrackableFlightGen
                             validRoundTripWithTrackableFlightGen
                             validDropoffHourlyGen])

                let invalidHourlyDurationGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            let info : Itinerary.Info =
                                { PickupTime = pickupTime
                                  Pickup = pickup
                                  Waypoints = waypoints
                                  Dropoff = dropoff }
                            info, [InvalidHorlyDuration]
                        )
                        (// pickup time
                         validAfterLandingPickupTimeGen)
                        (// pickup
                         validPickupWithTrackableFlightGen)
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         invalidDropoffHourlyGen)

                let invalidRoundTripWaitingDurationGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            let info : Itinerary.Info =
                                { PickupTime = pickupTime
                                  Pickup = pickup
                                  Waypoints = waypoints
                                  Dropoff = dropoff }
                            info, [InvalidWaitingTimeDuration]
                        )
                        (// pickup time
                         validAfterLandingPickupTimeGen)
                        (// pickup
                         validPickupWithTrackableFlightGen)
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         invalidRoundTripWaitingTimeDurationGen)

                let invalidRelativePickupTimeGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            let info : Itinerary.Info =
                                { PickupTime = pickupTime
                                  Pickup = pickup
                                  Waypoints = waypoints
                                  Dropoff = dropoff }
                            info, [InvalidRelativePickupTime]
                        )
                        (// pickup time
                         invalidRelativePickupTimeGen)
                        (// pickup
                         validPickupWithTrackableFlightGen)
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         Gen.oneof
                            [dropoffWithoutTrackableFlightGen
                             validDropoffWithTrackableFlightGen
                             validRoundTripWithoutTrackableFlightGen
                             validRoundTripWithTrackableFlightGen])

                let invalidRelativePickupTimeGen =
                    Gen.map4
                        (fun pickupTime pickup waypoints dropoff ->
                            let info : Itinerary.Info =
                                { PickupTime = pickupTime
                                  Pickup = pickup
                                  Waypoints = waypoints
                                  Dropoff = dropoff }
                            info, [InvalidDepartingFlightInfo]
                        )
                        (// pickup time
                         Gen.oneof
                            [exactPickupTimeGen
                             asapPickupTimeGen
                             arriveAtPickupTimeGen
                             validAfterLandingPickupTimeGen])
                        (// pickup
                         validPickupWithTrackableFlightGen)
                        (// waypoints
                         Gen.listOf waypointGen)
                        (// dropoff
                         Gen.oneof
                            [invalidDropoffWithTrackableFlightGen
                             invalidRoundTripWithTrackableFlightGen])

                Gen.oneof
                    [noTrackableArrivingFlightGen
                     noTrackableDepartingFlightGen
                     pickupTimeNotAllowedGen
                     invalidArrivingFlightInfoGen
                     invalidHourlyDurationGen
                     invalidRoundTripWaitingDurationGen
                     invalidRelativePickupTimeGen]
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map Itinerary.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module PaymentMethodGen =
        type Instances =
            static member Values =
                let directBill = Gen.constant PaymentMethod.DirectBill
                let cash = Gen.constant PaymentMethod.Cash

                let customerCreditCardGen =
                    Gen.map3
                        (fun owner creditCardId maskedCreditCard ->
                            PaymentMethod.CreditCard
                                { Owner = Customer owner
                                  CreditCardId = creditCardId
                                  CreditCard = maskedCreditCard }
                        )
                        AggregateIdGen.Instances.Values.Generator
                        CreditCardIdGen.Instances.Values.Generator
                        MaskedCreditCardGen.Instances.Values.Generator

                let accountCreditCardGen =
                    Gen.map3
                        (fun owner creditCardId maskedCreditCard ->
                            PaymentMethod.CreditCard
                                { Owner = Account owner
                                  CreditCardId = creditCardId
                                  CreditCard = maskedCreditCard }
                        )
                        AggregateIdGen.Instances.Values.Generator
                        CreditCardIdGen.Instances.Values.Generator
                        MaskedCreditCardGen.Instances.Values.Generator


                [directBill; cash; customerCreditCardGen; accountCreditCardGen]
                |> Gen.oneof
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ExtraRateGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map ExtraRate.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module RateGen =
        let mkGen (amountGen: Gen<Amount.T>) =
            // let consistentCurrenciesGen =
                // amountGen
                // |> Gen.zip amountGen
                // |> Gen.filter (fun (x, y) -> x.AsCurrencyIso4217 = y.AsCurrencyIso4217)
                // |> Gen.map (fun (x, y) -> x.Round (), y.Round ())

            Gen.map2
                (fun rate extraRate ->
                    { BaseRate = rate
                      Extras =
                        [{ ExtraRate = extraRate
                           Amount = rate }] })
                amountGen
                ExtraRateGen.Instances.Values.Generator

        type Instances =
            static member Values =
                mkGen AmountGen.ZeroOrPositiveInstances.Values.Generator
                |> Arb.fromGen

        type NegativeInstances =
            static member Values =
                mkGen AmountGen.NegativeInstances.Values.Generator
                |> Arb.fromGen

        type InconsistentCurrencyInstances =
            static member Values =
                let inconsistentCurrenciesGen =
                    AmountGen.Instances.Values.Generator
                    |> Gen.zip (AmountGen.Instances.Values.Generator)
                    |> Gen.filterNot (fun (x, y) -> x.AsCurrencyIso4217 = y.AsCurrencyIso4217)
                    |> Gen.map (fun (x, y) -> x.Round (), y.Round ())

                Gen.map2
                    (fun (baseRate, extraAmount) extraRate ->
                        { BaseRate = baseRate
                          Extras =
                            [{ ExtraRate = extraRate
                               Amount = extraAmount }] })
                    inconsistentCurrenciesGen
                    ExtraRateGen.Instances.Values.Generator
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module RateDetailsGen =
        type Instances =
            static member Values =
                Gen.map4
                    (fun token rate rateEngineId calculatedAt ->
                        { Token = token
                          Rate = rate
                          RateEngineId = rateEngineId
                          CalculatedAt = calculatedAt }
                    )
                    (Arb.Default.NonEmptyString().Generator |> Gen.map (fun x -> x.Get))
                    RateGen.Instances.Values.Generator
                    (Arb.Default.NonEmptyString().Generator |> Gen.map (fun x -> x.Get))
                    (Arb.Default.DateTimeOffset().Generator)
                |> Arb.fromGen

        type NegativeInstances =
            static member Values =
                Gen.map4
                    (fun token rate rateEngineId calculatedAt ->
                        { Token = token
                          Rate = rate
                          RateEngineId = rateEngineId
                          CalculatedAt = calculatedAt }
                    )
                    (Arb.Default.NonEmptyString().Generator |> Gen.map (fun x -> x.Get))
                    RateGen.NegativeInstances.Values.Generator
                    (Arb.Default.NonEmptyString().Generator |> Gen.map (fun x -> x.Get))
                    (Arb.Default.DateTimeOffset().Generator)
                |> Arb.fromGen

        type InconsistentCurrencyInstances =
            static member Values =
                Gen.map4
                    (fun token rate rateEngineId calculatedAt ->
                        { Token = token
                          Rate = rate
                          RateEngineId = rateEngineId
                          CalculatedAt = calculatedAt }
                    )
                    (Arb.Default.NonEmptyString().Generator |> Gen.map (fun x -> x.Get))
                    RateGen.InconsistentCurrencyInstances.Values.Generator
                    (Arb.Default.NonEmptyString().Generator |> Gen.map (fun x -> x.Get))
                    (Arb.Default.DateTimeOffset().Generator)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module OfferRateGen =
        let [<Literal>] RateEngineSigningKey = "9/f34+GruOEkZ5shKbX249eXmpJzbly1dNyVvUxyvUxRm87D1jYBwLvterwVFhw9r0Di2LzpuA34YZiHQvRQiw=="

        type Instances =
            static member Values =
                let ``manual rate`` =
                    Gen.map
                        Manual
                        RateGen.Instances.Values.Generator

                let ``rate engine (tm) rate`` =
                    Gen.map
                        (fun rate ->
                            let signer = DigiSigi.HMAC (RateEngineSigningKey)
                            let signature = signer.Sign (Json.serialize rate)
                            RateEngine (rate, signature)
                        )
                        RateDetailsGen.Instances.Values.Generator

                Gen.oneof
                    [``manual rate``
                     ``rate engine (tm) rate``]
                |> Arb.fromGen

        type NegativeInstances =
            static member Values =
                let ``manual rate`` =
                    Gen.map
                        Manual
                        RateGen.NegativeInstances.Values.Generator

                let ``rate engine (tm) rate`` =
                    Gen.map
                        (fun rate ->
                            let signer = DigiSigi.HMAC (RateEngineSigningKey)
                            let signature = signer.Sign (Json.serialize rate)
                            RateEngine (rate, signature)
                        )
                        RateDetailsGen.NegativeInstances.Values.Generator

                Gen.oneof
                    [``manual rate``
                     ``rate engine (tm) rate``]
                |> Arb.fromGen

        type InconsistentCurrencyInstances =
            static member Values =
                let ``manual rate`` =
                    Gen.map
                        Manual
                        RateGen.InconsistentCurrencyInstances.Values.Generator

                let ``rate engine (tm) rate`` =
                    Gen.map
                        (fun rate ->
                            let signer = DigiSigi.HMAC (RateEngineSigningKey)
                            let signature = signer.Sign (Json.serialize rate)
                            RateEngine (rate, signature)
                        )
                        RateDetailsGen.InconsistentCurrencyInstances.Values.Generator

                Gen.oneof
                    [``manual rate``
                     ``rate engine (tm) rate``]
                |> Arb.fromGen


    [<RequireQualifiedAccess>]
    module OfferGen =

        type Instances =
            static member Values =
                Gen.map4
                    (fun offer serviceClassId serviceClassPaxCapacity rate ->
                        { Id = offer
                          ServiceClassId = serviceClassId
                          ServiceClassPaxCapacity = serviceClassPaxCapacity
                          Rate = rate }
                    )
                    OfferIdGen.Instances.Values.Generator
                    AggregateIdGen.Instances.Values.Generator
                    ServiceClassPaxCapacityGen.Instances.Values.Generator
                    OfferRateGen.Instances.Values.Generator
                |> Arb.fromGen

        type NegativeInstances =
            static member Values =
                Gen.map4
                    (fun offer serviceClassId serviceClassPaxCapacity rate ->
                        { Id = offer
                          ServiceClassId = serviceClassId
                          ServiceClassPaxCapacity = serviceClassPaxCapacity
                          Rate = rate }
                    )
                    OfferIdGen.Instances.Values.Generator
                    AggregateIdGen.Instances.Values.Generator
                    ServiceClassPaxCapacityGen.Instances.Values.Generator
                    OfferRateGen.NegativeInstances.Values.Generator
                |> Arb.fromGen

        type InconsistentCurrencyInstances =
            static member Values =
                Gen.map4
                    (fun offer serviceClassId serviceClassPaxCapacity rate ->
                        { Id = offer
                          ServiceClassId = serviceClassId
                          ServiceClassPaxCapacity = serviceClassPaxCapacity
                          Rate = Manual rate }
                    )
                    OfferIdGen.Instances.Values.Generator
                    AggregateIdGen.Instances.Values.Generator
                    ServiceClassPaxCapacityGen.Instances.Values.Generator
                    RateGen.InconsistentCurrencyInstances.Values.Generator
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AreaIdGen =
        let private firstLastLetterGen =
            let allowedChars = UPPERCASE_CHARS + LOWERCASE_CHARS
            Gen.elements allowedChars
            |> Gen.map string

        let private middlePartGen =
            let allowedChars =
                UPPERCASE_CHARS + LOWERCASE_CHARS + NUMERIC_CHARS + "_.-"
            Gen.choose (0, 100)
            |> Gen.map (randomString allowedChars 0)

        type Valid =
            static member Values =
                Gen.map3 (fun a b c -> a + b + c)
                    firstLastLetterGen
                    middlePartGen
                    firstLastLetterGen
                |> Arb.fromGen

        type Invalid =
            static member Values =
                let invalidValues =
                    [null; ""; "   "; "*us"; "(foo)"; "čajetina";
                     "manhattan."; "m"; "man_"; "."; "-" ;"_" ;"_man";
                     "-man"; ".man"]
                invalidValues
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map AreaId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AreaNameGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map AreaName.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AreaTypeGen =
        type Instances =
            static member Values =
                [AreaType.Area
                 AreaType.City
                 AreaType.State
                 AreaType.ZipCode
                 AreaType.Airport]
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AreaToAreaGroupNameGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map AreaToAreaGroupName.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module PriceListIdGen =
        type Instances =
            static member Values =
                UserDefinedIdGen.Valid.Values.Generator
                |> Gen.map PriceListId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module BookingReferenceGen =
        type Valid =
            static member Values =
                let allowedChars =
                    UPPERCASE_CHARS + NUMERIC_CHARS

                Gen.map2 (fun x y -> randomString allowedChars (min x y) (max x y))
                    (Gen.choose (6, 10))
                    (Gen.choose (6, 10))
                |> Arb.fromGen

        type Invalid =
            static member Values =
                let invalidLengthGen =
                    let allowedChars =
                        NUMERIC_CHARS + SPECIAL_CHARS
                        + UPPERCASE_CHARS + LOWERCASE_CHARS
                        + CYRILLIC_LOWERCASE_CHARS + CYRILLIC_UPPERCASE_CHARS
                    Gen.choose (0, 5)
                    |> Gen.map (randomString allowedChars 0)

                let invalidCharsGen =
                    let allowedChars =
                        SPECIAL_CHARS + LOWERCASE_CHARS
                        + CYRILLIC_LOWERCASE_CHARS + CYRILLIC_UPPERCASE_CHARS
                    Gen.map2 (fun x y -> randomString allowedChars (min x y) (max x y))
                        (Gen.choose (6, 10))
                        (Gen.choose (6, 10))

                Gen.oneof [invalidLengthGen; invalidCharsGen]
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map BookingReference.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module BookingPolicyNameGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map BookingPolicyName.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module BookingPolicyIdGen =
        type Instances =
            static member Values =
                GuidIdGen.Valid.Values.Generator
                |> Gen.map BookingPolicyId.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module AccountPolicyScopeGen =
        let setGen =
            Gen.listOf AggregateIdGen.Instances.Values.Generator
            |> Gen.map AccountPolicyScope.Set

        let allExceptGen =
            Gen.listOf AggregateIdGen.Instances.Values.Generator
            |> Gen.map AccountPolicyScope.AllExcept

        type Instances =
            static member Values =
                Gen.oneof
                    [Gen.constant AccountPolicyScope.No
                     Gen.constant AccountPolicyScope.All
                     setGen
                     allExceptGen]
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CustomerPolicyScopeGen =
        type Instances =
            static member Values =
                [CustomerPolicyScope.No
                 CustomerPolicyScope.All]
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module PolicyScopeGen =
        type Instances =
            static member Values =
                Gen.map2
                    (fun cust acc ->
                        { Customers = cust
                          Accounts = acc }
                    )
                    (CustomerPolicyScopeGen.Instances.Values.Generator)
                    (AccountPolicyScopeGen.Instances.Values.Generator)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ServiceClassLeadTimeGen =
        let minutesGen =
            Arb.Default.Int32().Generator
            |> Gen.map (fun x -> ServiceClassLeadTime.Minutes (x * 1<minute>))

        type Instances =
            static member Values =
                Gen.oneof
                    [Gen.constant ServiceClassLeadTime.ASAP
                     minutesGen]
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ServiceClassFlagGen =
        type Instances =
            static member Values =
                [ServiceClassFlag.NoFlag
                 ServiceClassFlag.Featured]
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ServiceTypeGen =
        type Instances =
            static member Values =
                Gen.map3
                    (fun sc lt f ->
                        { ServiceClassId = sc
                          LeadTime = lt
                          Flag = f }
                    )
                    (AggregateIdGen.Instances.Values.Generator)
                    (ServiceClassLeadTimeGen.Instances.Values.Generator)
                    (ServiceClassFlagGen.Instances.Values.Generator)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module ServiceTypesGen =
        type Instances =
            static member Values =
                Gen.map2
                    (fun t h ->
                        { Transfer = t
                          Hourly = h }
                    )
                    (Gen.listOf ServiceTypeGen.Instances.Values.Generator)
                    (Gen.listOf ServiceTypeGen.Instances.Values.Generator)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module PaymentMethodPermissionGen =
        let allowedUpToGen =
            AmountGen.Instances.Values.Generator
            |> Gen.map PaymentMethodPermission.AllowedUpTo

        type Instances =
            static member Values =
                Gen.oneof
                    [Gen.constant PaymentMethodPermission.Allowed
                     Gen.constant PaymentMethodPermission.Forbidden
                     allowedUpToGen]
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module PaymentMethodPermissionsGen =
        type Instances =
            static member Values =
                Gen.map4
                    (fun cust acc db cash ->
                        { CustomerCreditCard = cust
                          AccountCreditCard = acc
                          DirectBill = db
                          Cash = cash }
                    )
                    (PaymentMethodPermissionGen.Instances.Values.Generator)
                    (PaymentMethodPermissionGen.Instances.Values.Generator)
                    (PaymentMethodPermissionGen.Instances.Values.Generator)
                    (PaymentMethodPermissionGen.Instances.Values.Generator)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module PrepaymentGen =
        let percentageGen =
            Arb.Default.Decimal().Generator
            |> Gen.map (fun x -> Prepayment.Percentage (x * 1m<percent>))

        let fixedAmountGen =
            AmountGen.Instances.Values.Generator
            |> Gen.map Prepayment.FixedAmount

        type Instances =
            static member Values =
                Gen.oneof [percentageGen; fixedAmountGen]
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module PrepaymentPolicyGen =
        let withPrepaymentGen =
            PrepaymentGen.Instances.Values.Generator
            |> Gen.map PrepaymentPolicy.WithPrepayment

        type Instances =
            static member Values =
                Gen.oneof
                    [Gen.constant PrepaymentPolicy.NoPrepayment
                     withPrepaymentGen]
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module PaymentPolicyGen =
        type Instances =
            static member Values =
                Gen.map2
                    (fun apm pp ->
                        { AllowedPaymentMethods = apm
                          PrepaymentPolicy = pp }
                    )
                    (PaymentMethodPermissionsGen.Instances.Values.Generator)
                    (PrepaymentPolicyGen.Instances.Values.Generator)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CancellationChargeRuleGen =
        type Instances =
            static member Values =
                Gen.map2
                    (fun bp percentage ->
                        { BeforePickup = bp * 1<minute>
                          Percentage = percentage * 1m<percent> }
                    )
                    (Arb.Default.Int32().Generator)
                    (Arb.Default.Decimal().Generator)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CancellationPolicyGen =
        type Instances =
            static member Values =
                Gen.listOf CancellationChargeRuleGen.Instances.Values.Generator
                |> Gen.map (fun x -> { ChargeRules = x })
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module BookingPolicyGen =
        open FsCheck.Gen

        let private map8 f a b c d e g h i = apply (apply (apply (apply (apply (apply (apply (map f a) b) c) d) e) g) h) i

        type Instances =
            static member Values =
                map8
                    (fun policyId policyName scope areas serviceTypes paymentPolicy cancellationPolicy priceListId ->
                        { Id = policyId
                          Name = policyName
                          Scope = scope
                          ServiceAreas = areas
                          ServiceTypes = serviceTypes
                          PaymentPolicy = paymentPolicy
                          CancellationPolicy = cancellationPolicy
                          PriceListId = priceListId }
                    )
                    (BookingPolicyIdGen.Instances.Values.Generator)
                    (BookingPolicyNameGen.Instances.Values.Generator)
                    (PolicyScopeGen.Instances.Values.Generator)
                    (Gen.listOf AggregateIdGen.Instances.Values.Generator)
                    (ServiceTypesGen.Instances.Values.Generator)
                    (PaymentPolicyGen.Instances.Values.Generator)
                    (CancellationPolicyGen.Instances.Values.Generator)
                    (Gen.optionOf PriceListIdGen.Instances.Values.Generator)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module TransactionTypeGen =
        type Instances =
            static member Values =
                [Payment.Debit; Payment.Credit]
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module TransactionStatusGen =
        type Instances =
            static member Values =
                [Payment.Pending
                 Payment.Authorized
                 Payment.SubmittedForSettlement
                 Payment.Settled
                 Payment.SettlementDeclined
                 Payment.Voided
                 Payment.Failed]
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module TransactionStatusAndExpiresAtGen =
        type Instances =
            static member Values =
                Gen.map2 (fun status expiresAt ->
                        if status = Payment.Authorized then
                            (status, Some expiresAt)
                        else
                            (status, None)
                    )
                    (TransactionStatusGen.Instances.Values.Generator)
                    (Arb.Default.DateTimeOffset().Generator)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module TransactionGen =
        open FsCheck.Gen

        let private map7 f a b c d e g h = apply (apply (apply (apply (apply (apply (map f a) b) c) d) e) g) h

        type Instances =
            static member Values =
                map7
                    (fun id cardId owner (status, expiresAt) tranType amount dateTime ->
                        { Id = id
                          CreditCardId = cardId
                          Owner = owner
                          Status = status
                          Type = tranType
                          Amount = amount
                          CreatedAt = dateTime
                          AuthorizationExpiresAt = expiresAt } : Payment.Transaction
                    )
                    (TransactionIdGen.Instances.Values.Generator)
                    (CreditCardIdGen.Instances.Values.Generator)
                    (CreditCardOwnerGen.Values.Generator)
                    (TransactionStatusAndExpiresAtGen.Instances.Values.Generator)
                    (TransactionTypeGen.Instances.Values.Generator)
                    (AmountGen.Instances.Values.Generator)
                    (Arb.Default.DateTimeOffset().Generator)
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CommsChannelGen =
        type Instances =
            static member Values =
                Gen.oneof
                    [ EmailAddressGen.Instances.Values.Generator |> Gen.map Comms.Email
                      CellPhoneGen.Instances.Values.Generator |> Gen.map Comms.SMS ]
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CommsBookingNotificationGen =
        type Instances =
            static member Values =
                [Comms.BookingConfirmation
                 Comms.BookingCancellation
                 Comms.DriverEnRoute
                 Comms.DriverArrived
                 Comms.DriverCircling
                 Comms.RideCompleted]
                |> Gen.elements
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module CommsBookingNotificationPreferenceGen =
        type Instances =
            static member Values =
                Gen.map2
                    (fun notification channels ->
                        { Notification = notification
                          Channels = channels } : Comms.BookingNotificationPreference
                    )
                    CommsBookingNotificationGen.Instances.Values.Generator
                    (Gen.listOf CommsChannelGen.Instances.Values.Generator)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module VehicleMakeGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map VehicleMake.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module VehicleModelGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map VehicleModel.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module VehicleLicensePlateGen =
        type Valid =
            static member Values =
                Arb.Default.String()
                |> Arb.mapFilter id (not << String.IsNullOrWhiteSpace)

        type Invalid =
            static member Values =
                nullEmptyAndWhiteSpaceStrings
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map VehicleLicensePlate.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module VehicleColorGen =
        type Valid =
            static member Values =
                [null; ""; "#FFF"; "white"; "yellow"; "red"; "#AACC99"]
                |> Gen.elements
                |> Arb.fromGen

        type Invalid =
            static member Values =
                ["asdas"; "асдгаасгаклс"]
                |> Gen.elements
                |> Arb.fromGen

        type Instances =
            static member Values =
                Valid.Values.Generator
                |> Gen.map VehicleColor.create
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module DriverDataGen =
        type Instances =
            static member Values =
                Gen.map4
                    (fun driverId firstName lastName cellPhone ->
                        { Id = DriverId driverId
                          FirstName = firstName
                          LastName = lastName
                          CellPhone = cellPhone }
                    )
                    (Arb.Default.NonEmptyString().Generator |> Gen.map (fun x -> x.Get))
                    FirstNameGen.Instances.Values.Generator
                    LastNameGen.Instances.Values.Generator
                    (Gen.optionOf CellPhoneGen.Instances.Values.Generator)
                |> Arb.fromGen

    [<RequireQualifiedAccess>]
    module VehicleDataGen =
        type Instances =
            static member Values =
                Gen.map6
                    (fun vehicleId make model color serviceClassId licensePlate ->
                        { Id = VehicleId vehicleId
                          Make = make
                          Model = model
                          Color = color
                          ServiceClassId = serviceClassId
                          LicensePlate = licensePlate }
                    )
                    (Arb.Default.NonEmptyString().Generator |> Gen.map (fun x -> x.Get))
                    (Gen.optionOf VehicleMakeGen.Instances.Values.Generator)
                    (Gen.optionOf VehicleModelGen.Instances.Values.Generator)
                    (Gen.optionOf VehicleColorGen.Instances.Values.Generator)
                    (Gen.optionOf AggregateIdGen.Instances.Values.Generator)
                    (Gen.optionOf VehicleLicensePlateGen.Instances.Values.Generator)
                |> Arb.fromGen

     [<RequireQualifiedAccess>]
    module RoleGen =
        type Instances =
            static member Values =
                [Role.Operator
                 Role.Administrator]
                |> Gen.elements
                |> Arb.fromGen

        type InvalidJsons =
            static member Values =
                invalidJsons
                |> Gen.elements
                |> Arb.fromGen
