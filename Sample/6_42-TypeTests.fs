namespace Core.ValueTypes.Tests

open Test.Helpers
open Xunit
open Swensen.Unquote

[<Trait (Trait.Category, Category.UnitTest)>]
module TypeTests =
    open System
    open FsCheck.Xunit
    open OpenLimo.Core.Library
    open OpenLimo.Core.Serialization
    open OpenLimo.Core.ValueTypes
    open TestDataGen

    let serializeDeserialize<'a> (x: 'a) =
        let serialized = Json.serialize x
        Json.deserialize<'a> serialized

    [<Property(Arbitrary=[|typeof<AddressGen.Valid>|])>]
    let ``given valid Address, expect success`` (info, location) =
        let expected = Ok (Address.create (info, location))
        let result = Address.tryCreate (info, location)
        result =! expected

    [<Property(Arbitrary=[|typeof<AddressGen.Invalid>|])>]
    let ``given invalid Address, expect error`` (info, location, error) =
        let expected = Error error
        let result = Address.tryCreate (info, location)
        result =! expected

    [<Property(Arbitrary=[|typeof<AddressGen.Instances>|])>]
    let ``Address serialization roundtrip`` (value) =
        let result = serializeDeserialize<Address.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<AddressGen.InvalidJsons>|])>]
    let ``Address deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<Address.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<AirportCodeGen.Valid>|])>]
    let ``given valid AirportCode, expect success`` (value) =
        let expected = Ok (AirportCode.create value)
        let result = AirportCode.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AirportCodeGen.Invalid>|])>]
    let ``given invalid AirportCode, expect error`` (value, error) =
        let expected = Error error
        let result = AirportCode.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AirportCodeGen.Instances>|])>]
    let ``AirportCode serialization roundtrip`` (value) =
        let result = serializeDeserialize<AirportCode.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<AirportCodeGen.InvalidJsons>|])>]
    let ``AirportCode deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<AirportCode.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<AirportGen.Valid>|])>]
    let ``given valid Airport, expect success`` (airport, location) =
        let expected = Ok (Airport.create (airport, location))
        let result = Airport.tryCreate (airport, location)
        result =! expected

    [<Property(Arbitrary=[|typeof<AirportGen.Invalid>|])>]
    let ``given invalid Airport, expect error`` (airport, location, error) =
        let expected = Error error
        let location = Location.create location
        let result = Airport.tryCreate (airport, location)
        result =! expected

    [<Property(Arbitrary=[|typeof<AirportGen.Instances>|])>]
    let ``Airport serialization roundtrip`` (value) =
        let result = serializeDeserialize<Airport.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<AirportGen.InvalidJsons>|])>]
    let ``Airport deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<Airport.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<FlightNumberGen.Valid>|])>]
    let ``given valid FlightNumber, expect success`` (info) =
        let expected = Ok (FlightNumber.create info)
        let result = FlightNumber.tryCreate info
        result =! expected

    [<Property(Arbitrary=[|typeof<FlightNumberGen.Invalid>|])>]
    let ``given invalid FlightNumber, expect error`` (info, error) =
        let expected = Error error
        let result = FlightNumber.tryCreate info
        result =! expected

    [<Property(Arbitrary=[|typeof<FlightNumberGen.Instances>|])>]
    let ``FlightNumber serialization roundtrip`` (value) =
        let result = serializeDeserialize<FlightNumber.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<FlightNumberGen.InvalidJsons>|])>]
    let ``FlightNumber deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<FlightNumber.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<AircraftTailNumberGen.Valid>|])>]
    let ``given valid AircraftTailNumber, expect success`` value =
        let expected = Ok (AircraftTailNumber.create value)
        let result = AircraftTailNumber.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AircraftTailNumberGen.Invalid>|])>]
    let ``given invalid AircraftTailNumber, expect error`` value =
        let expected = Error [Error.InvalidAircraftTailNumber]
        let result = AircraftTailNumber.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AircraftTailNumberGen.Instances>|])>]
    let ``AircraftTailNumber serialization roundtrip`` value =
        let result = serializeDeserialize<AircraftTailNumber.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``AircraftTailNumber deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<AircraftTailNumber.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<FlightTimeGen.Instances>|])>]
    let ``FlightTime serialization roundtrip`` (value) =
        let result = serializeDeserialize<FlightTime> value
        result =! value

    [<Property(Arbitrary=[|typeof<FlightTimeGen.InvalidJsons>|])>]
    let ``FlightTime deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<FlightTime> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<FlightStatusGen.Instances>|])>]
    let ``FlightStatus serialization roundtrip`` (value) =
        let result = serializeDeserialize<FlightStatus> value
        result =! value

    [<Property(Arbitrary=[|typeof<FlightStatusGen.InvalidJsons>|])>]
    let ``FlightStatus deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<FlightStatus> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<FlightInfoGen.Instances>|])>]
    let ``FlightInfo serialization roundtrip`` (value) =
        let result = serializeDeserialize<FlightInfo> value
        result =! value

    [<Property(Arbitrary=[|typeof<FlightInfoGen.InvalidJsons>|])>]
    let ``FlightInfo deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<FlightInfo> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<FlightDepartureDateGen.ValidStrings>|])>]
    let ``given valid string representation of FlightDepartureDate, expect success`` (value) =
        let expected = Ok (FlightDepartureDate.create value)
        let result = FlightDepartureDate.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<FlightDepartureDateGen.ValidInts>|])>]
    let ``given valid integer representation of FlightDepartureDate, expect success`` (year, month, day) =
        let expected =
            DateTimeOffset (year, month, day, 1, 1, 0, TimeSpan (0, 0, 0))
            |> stringf "yyyyMMdd"
            |> FlightDepartureDate.create

        let result = FlightDepartureDate.T.Create (year, month, day)
        result =! expected

    [<Property(Arbitrary=[|typeof<FlightDepartureDateGen.InvalidStrings>|])>]
    let ``given invalid string represenation of FlightDepartureDate, expect error`` (value, error) =
        let expected = Error error
        let result = FlightDepartureDate.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<FlightDepartureDateGen.InvalidInts>|])>]
    let ``given invalid integer represenation of FlightDepartureDate, expect error`` (year, month, day, error) =
        let expected = Error error
        let result = FlightDepartureDate.T.TryCreate (year, month, day)
        result =! expected

    [<Property(Arbitrary=[|typeof<FlightDepartureDateGen.Instances>|])>]
    let ``FlightDepartureDate serialization roundtrip`` (value) =
        let result = serializeDeserialize<FlightDepartureDate.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<FlightDepartureDateGen.InvalidJsons>|])>]
    let ``FlightDepartureDate deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<FlightDepartureDate.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<CustomerIdGen.Valid>|])>]
    let ``given valid CustomerId, expect success`` value =
        let expected = Ok (CustomerId.create value)
        let result = CustomerId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<CustomerIdGen.Invalid>|])>]
    let ``given invalid CustomerId, expect error`` value =
        let expected = Error [Error.InvalidCustomerId]
        let result = CustomerId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<CustomerIdGen.Instances>|])>]
    let ``CustomerId serialization roundtrip`` value =
        let result = serializeDeserialize<CustomerId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``CustomerId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<CustomerId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<FirstNameGen.Valid>|])>]
    let ``given valid FirstName, expect success`` value =
        let expected = Ok (FirstName.create value)
        let result = FirstName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<FirstNameGen.Invalid>|])>]
    let ``given invalid FirstName, expect error`` value =
        let expected = Error [Error.InvalidFirstName]
        let result = FirstName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<FirstNameGen.Instances>|])>]
    let ``FirstName serialization roundtrip`` value =
        let result = serializeDeserialize<FirstName.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``FirstName deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<FirstName.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<LastNameGen.Valid>|])>]
    let ``given valid LastName, expect success`` value =
        let expected = Ok (LastName.create value)
        let result = LastName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<LastNameGen.Invalid>|])>]
    let ``given invalid LastName, expect error`` value =
        let expected = Error [Error.InvalidLastName]
        let result = LastName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<LastNameGen.Instances>|])>]
    let ``LastName serialization roundtrip`` value =
        let result = serializeDeserialize<LastName.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``LastName deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<LastName.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<EmailAddressGen.Valid>|])>]
    let ``given valid EmailAddress, expect success`` value =
        let expected = Ok (EmailAddress.create value)
        let result = EmailAddress.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<EmailAddressGen.Invalid>|])>]
    let ``given invalid EmailAddress, expect error`` value =
        let expected = Error [Error.InvalidEmailAddress]
        let result = EmailAddress.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<EmailAddressGen.Instances>|])>]
    let ``EmailAddress serialization roundtrip`` value =
        let result = serializeDeserialize<EmailAddress.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``EmailAddress deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<EmailAddress.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<EmailAddressGen.Valid>|])>]
    let ``given valid EmailAddressContact, expect success`` value =
        let expected = Ok (EmailAddressContact.create value)
        let result = EmailAddressContact.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<EmailAddressGen.Invalid>|])>]
    let ``given invalid EmailAddressContact, expect error`` value =
        let expected = Error [Error.InvalidEmailAddress]
        let result = EmailAddressContact.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<EmailAddressGen.Instances>|])>]
    let ``EmailAddressContact serialization roundtrip`` value =
        let result = serializeDeserialize<EmailAddressContact.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``EmailAddressContact deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<EmailAddressContact.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<CellPhoneGen.Valid>|])>]
    let ``given valid CellPhone, expect success`` value =
        let expected = Ok (CellPhone.create value)
        let result = CellPhone.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<CellPhoneGen.Invalid>|])>]
    let ``given invalid CellPhone, expect error`` value =
        let expected = Error [Error.InvalidCellPhone]
        let result = CellPhone.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<CellPhoneGen.Instances>|])>]
    let ``CellPhone serialization roundtrip`` value =
        let result = serializeDeserialize<CellPhone.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<CellPhoneGen.InvalidJsons>|])>]
    let ``CellPhone deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<CellPhone.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<CellPhoneGen.Valid>|])>]
    let ``given valid CellPhoneContact, expect success`` value =
        let expected = Ok (CellPhoneContact.create value)
        let result = CellPhoneContact.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<CellPhoneGen.Invalid>|])>]
    let ``given invalid CellPhoneContact, expect error`` value =
        let expected = Error [Error.InvalidCellPhone]
        let result = CellPhoneContact.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<CellPhoneGen.Instances>|])>]
    let ``CellPhoneContact serialization roundtrip`` value =
        let result = serializeDeserialize<CellPhoneContact.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<LandlineGen.Valid>|])>]
    let ``given valid Landline, expect success`` value =
        let expected = Ok (Landline.create value)
        let result = Landline.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<LandlineGen.Invalid>|])>]
    let ``given invalid Landline, expect error`` value =
        let expected = Error [Error.InvalidLandline]
        let result = Landline.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<LandlineGen.Instances>|])>]
    let ``Landline serialization roundtrip`` value =
        let result = serializeDeserialize<Landline.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<LandlineGen.InvalidJsons>|])>]
    let ``Landline deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<Landline.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<LandlineGen.Valid>|])>]
    let ``given valid LandlineContact, expect success`` value =
        let expected = Ok (LandlineContact.create value)
        let result = LandlineContact.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<LandlineGen.Invalid>|])>]
    let ``given invalid LandlineContact, expect error`` value =
        let expected = Error [Error.InvalidLandline]
        let result = LandlineContact.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<LandlineGen.Instances>|])>]
    let ``LandlineContact serialization roundtrip`` value =
        let result = serializeDeserialize<LandlineContact.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<PasswordGen.Valid>|])>]
    let ``given valid Password, expect success`` value =
        let expected = Ok (Password.create PasswordGen.complexityCheck PasswordGen.hash value)
        let result = Password.tryCreate PasswordGen.complexityCheck PasswordGen.hash value
        result =! expected

    [<Property(Arbitrary=[|typeof<PasswordGen.Invalid>|])>]
    let ``given invalid Password, expect error`` value =
        let expected = Error [Error.InvalidPassword]
        let result = Password.tryCreate PasswordGen.complexityCheck PasswordGen.hash value
        result =! expected

    [<Property(Arbitrary=[|typeof<PasswordGen.Weak>|])>]
    let ``given weak Password, expect error`` value =
        let expected = Error [Error.WeakPasswordComplexity PasswordGen.compexityErrorMessage]
        let result = Password.tryCreate PasswordGen.complexityCheck PasswordGen.hash value
        result =! expected

    [<Property(Arbitrary=[|typeof<PasswordGen.Instances>|])>]
    let ``Password serialization roundtrip`` value =
        let result = serializeDeserialize<Password.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<PasswordHashGen.Valid>|])>]
    let ``given valid PasswordHash, expect success`` value =
        let expected = Ok (Password.createFromHash value)
        let result = Password.tryCreateFromHash value
        result =! expected

    [<Property(Arbitrary=[|typeof<PasswordHashGen.Invalid>|])>]
    let ``given invalid PasswordHash, expect error`` value =
        let expected = Error [Error.InvalidPasswordHash]
        let result = Password.tryCreateFromHash value
        result =! expected

    [<Property(Arbitrary=[|typeof<PasswordHashGen.Instances>|])>]
    let ``Password created from hash serialization roundtrip`` value =
        let result = serializeDeserialize<Password.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<ExternalLoginGen.Valid>|])>]
    let ``given valid ExternalLogin, expect success`` value =
        let expected = Ok (ExternalLogin.create value)
        let result = ExternalLogin.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ExternalLoginGen.Invalid>|])>]
    let ``given invalid ExternalLogin, expect error`` value =
        let expected = Error [Error.InvalidExternalLogin]
        let result = ExternalLogin.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ExternalLoginGen.Instances>|])>]
    let ``ExternalLogin serialization roundtrip`` value =
        let result = serializeDeserialize<ExternalLogin.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<ExternalLoginGen.InvalidJsons>|])>]
    let ``ExternalLogin deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<ExternalLogin.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<GuidIdGen.Valid>|])>]
    let ``given valid CreditCardId, expect success`` (cardId) =
        let expected = Ok (CreditCardId.create (cardId))
        let result = CreditCardId.tryCreate (cardId)
        result =! expected

    [<Property(Arbitrary=[|typeof<GuidIdGen.Invalid>|])>]
    let ``given CreditCardId, expect error`` (cardId) =
        let expected = Error [Error.InvalidCreditCardId]
        let result = CreditCardId.tryCreate (cardId)
        result =! expected

    [<Property(Arbitrary=[|typeof<CreditCardIdGen.Instances>|])>]
    let ``CreditCardId serialization roundtrip`` (value) =
        let result = serializeDeserialize<CreditCardId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``CreditCardId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<CreditCardId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<GuidIdGen.Valid>|])>]
    let ``given valid TransactionId, expect success`` (cardId) =
        let expected = Ok (TransactionId.create (cardId))
        let result = TransactionId.tryCreate (cardId)
        result =! expected

    [<Property(Arbitrary=[|typeof<GuidIdGen.Invalid>|])>]
    let ``given TransactionId, expect error`` (cardId) =
        let expected = Error [Error.InvalidTransactionId]
        let result = TransactionId.tryCreate (cardId)
        result =! expected

    [<Property(Arbitrary=[|typeof<TransactionIdGen.Instances>|])>]
    let ``TransactionId serialization roundtrip`` (value) =
        let result = serializeDeserialize<TransactionId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``TransactionId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<TransactionId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<TransactionGen.InvalidJsons>|])>]
    let ``Transaction deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<Payment.Transaction> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<CreditCardExpirationDateGen.Valid>|])>]
    let ``given valid CreditCardExpirationDate, expect success`` (expires) =
        let expected = Ok (CreditCardExpirationDate.create (expires))
        let result = CreditCardExpirationDate.tryCreate (expires)
        result =! expected

    [<Property(Arbitrary=[|typeof<CreditCardExpirationDateGen.Invalid>|])>]
    let ``given invalid CreditCardExpirationDate, expect error`` (expires, error) =
        let expected = Error error
        let result = CreditCardExpirationDate.tryCreate (expires)
        result =! expected

    [<Property(Arbitrary=[|typeof<CreditCardExpirationDateGen.Instances>|])>]
    let ``CreditCardExpirationDate serialization roundtrip`` (value) =
        let result = serializeDeserialize<CreditCardExpirationDate.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<CreditCardExpirationDateGen.InvalidJsons>|])>]
    let ``CreditCardExpirationDate deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<UnmaskedCreditCard.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<CreditCardTypeGen.Instances>|])>]
    let ``CreditCardType serialization roundtrip`` (value) =
        let result = serializeDeserialize<CreditCardType> value
        result =! value

    [<Property(Arbitrary=[|typeof<CreditCardTypeGen.InvalidJsons>|])>]
    let ``CreditCardType deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<CreditCardType> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<UnmaskedCreditCardGen.Valid>|])>]
    let ``given valid UnmaskedCreditCard, expect success`` (info) =
        let expected = Ok (UnmaskedCreditCard.create (info))
        let result = UnmaskedCreditCard.tryCreate (info)
        result =! expected

    [<Property(Arbitrary=[|typeof<UnmaskedCreditCardGen.Invalid>|])>]
    let ``given invalid UnmaskedCreditCard, expect error`` (info, error) =
        let expected = Error error
        let result = UnmaskedCreditCard.tryCreate (info)
        result =! expected

    [<Property(Arbitrary=[|typeof<UnmaskedCreditCardGen.Instances>|])>]
    let ``UnmaskedCreditCard serialization roundtrip`` (value) =
        let result = serializeDeserialize<UnmaskedCreditCard.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<UnmaskedCreditCardGen.InvalidJsons>|])>]
    let ``UnmaskedCreditCard deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<UnmaskedCreditCard.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<MaskedCreditCardGen.Valid>|])>]
    let ``given valid MaskedCreditCard, expect success`` (info) =
        let expected = Ok (MaskedCreditCard.create (info))
        let result = MaskedCreditCard.tryCreate (info)
        result =! expected

    [<Property(Arbitrary=[|typeof<MaskedCreditCardGen.Invalid>|])>]
    let ``given invalid MaskedCreditCard, expect error`` (info, error) =
        let expected = Error error
        let result = MaskedCreditCard.tryCreate (info)
        result =! expected

    [<Property(Arbitrary=[|typeof<MaskedCreditCardGen.Instances>|])>]
    let ``MaskedCreditCard serialization roundtrip`` (value) =
        let result = serializeDeserialize<MaskedCreditCard.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<MaskedCreditCardGen.InvalidJsons>|])>]
    let ``MaskedCreditCard deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<MaskedCreditCard.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<UserDefinedIdGen.Valid>|])>]
    let ``given valid AccountId, expect success`` value =
        let expected = Ok (AccountId.create value)
        let result = AccountId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<UserDefinedIdGen.Invalid>|])>]
    let ``given invalid AccountId, expect error`` value =
        let expected = Error [Error.InvalidAccountId]
        let result = AccountId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AccountIdGen.Instances>|])>]
    let ``AccountId serialization roundtrip`` value =
        let result = serializeDeserialize<AccountId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``AccountId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<AccountId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<AccountNameGen.Valid>|])>]
    let ``given valid AccountName, expect success`` value =
        let expected = Ok (AccountName.create value)
        let result = AccountName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AccountNameGen.Invalid>|])>]
    let ``given invalid AccountName, expect error`` value =
        let expected = Error [Error.InvalidAccountName]
        let result = AccountName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AccountNameGen.Instances>|])>]
    let ``AccountName serialization roundtrip`` value =
        let result = serializeDeserialize<AccountName.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``AccountName deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<AccountName.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<UserDefinedIdGen.Valid>|])>]
    let ``given invalid ServiceClassId, expect success`` value =
        let expected = Ok (ServiceClassId.create value)
        let result = ServiceClassId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<UserDefinedIdGen.Invalid>|])>]
    let ``given valid ServiceClassId, expect error`` value =
        let expected = Error [Error.InvalidServiceClassId]
        let result = ServiceClassId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ServiceClassIdGen.Instances>|])>]
    let ``ServiceClassId serialization roundtrip`` value =
        let result = serializeDeserialize<ServiceClassId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``ServiceClassId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<ServiceClassId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<ServiceClassNameGen.Valid>|])>]
    let ``given valid ServiceClassName, expect success`` value =
        let expected = Ok (ServiceClassName.create value)
        let result = ServiceClassName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ServiceClassNameGen.Invalid>|])>]
    let ``given invalid ServiceClassName, expect error`` value =
        let expected = Error [Error.InvalidServiceClassName]
        let result = ServiceClassName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ServiceClassNameGen.Instances>|])>]
    let ``ServiceClassName serialization roundtrip`` value =
        let result = serializeDeserialize<ServiceClassName.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``ServiceClassName deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<ServiceClassName.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<ServiceClassDescriptionGen.Valid>|])>]
    let ``given valid ServiceClassDescription, expect success`` value =
        let expected = Ok (ServiceClassDescription.create value)
        let result = ServiceClassDescription.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ServiceClassDescriptionGen.Invalid>|])>]
    let ``given invalid ServiceClassDescription, expect error`` value =
        let expected = Error [Error.InvalidServiceClassDescription]
        let result = ServiceClassDescription.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ServiceClassDescriptionGen.Instances>|])>]
    let ``ServiceClassDescription serialization roundtrip`` value =
        let result = serializeDeserialize<ServiceClassDescription.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``ServiceClassDescription deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<ServiceClassDescription.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<ServiceClassPaxCapacityGen.Valid>|])>]
    let ``given valid ServiceClassPaxCapacity, expect success`` value =
        let expected = Ok (ServiceClassPaxCapacity.create value)
        let result = ServiceClassPaxCapacity.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ServiceClassPaxCapacityGen.Invalid>|])>]
    let ``given invalid ServiceClassPaxCapacity, expect error`` value =
        let expected = Error [Error.InvalidServiceClassPaxCapacity]
        let result = ServiceClassPaxCapacity.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ServiceClassPaxCapacityGen.Instances>|])>]
    let ``ServiceClassPaxCapacity serialization roundtrip`` value =
        let result = serializeDeserialize<ServiceClassPaxCapacity.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``ServiceClassPaxCapacity deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<ServiceClassPaxCapacity.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<ServiceClassLuggageCapacityGen.Valid>|])>]
    let ``given valid ServiceClassLuggageCapacity, expect success`` value =
        let expected = Ok (ServiceClassLuggageCapacity.create value)
        let result = ServiceClassLuggageCapacity.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ServiceClassLuggageCapacityGen.Invalid>|])>]
    let ``given invalid ServiceClassLuggageCapacity, expect error`` value =
        let expected = Error [Error.InvalidServiceClassLuggageCapacity]
        let result = ServiceClassLuggageCapacity.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ServiceClassLuggageCapacityGen.Instances>|])>]
    let ``ServiceClassLuggageCapacity serialization roundtrip`` value =
        let result = serializeDeserialize<ServiceClassLuggageCapacity.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``ServiceClassLuggageCapacity deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<ServiceClassLuggageCapacity.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<GuidIdGen.Valid>|])>]
    let ``given valid OfferId, expect success`` value =
        let expected = Ok (OfferId.create value)
        let result = OfferId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<GuidIdGen.Invalid>|])>]
    let ``given invalid OfferId, expect error`` value =
        let expected = Error [Error.InvalidOfferId]
        let result = OfferId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<OfferIdGen.Instances>|])>]
    let ``OfferId serialization roundtrip`` value =
        let result = serializeDeserialize<OfferId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``OfferId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<OfferId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<GuidIdGen.Valid>|])>]
    let ``given valid ResourceId, expect success`` value =
        let expected = Ok (ResourceId.create value)
        let result = ResourceId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<GuidIdGen.Invalid>|])>]
    let ``given invalid ResourceId, expect error`` value =
        let expected = Error [Error.InvalidResourceId]
        let result = ResourceId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ResourceIdGen.Instances>|])>]
    let ``ResourceId serialization roundtrip`` value =
        let result = serializeDeserialize<ResourceId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``ResourceId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<ResourceId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<AreaIdGen.Valid>|])>]
    let ``given AreaId, expect success`` value =
        let expected = Ok (AreaId.create value)
        let result = AreaId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AreaIdGen.Invalid>|])>]
    let ``given invalid AreaId, expect error`` value =
        let expected = Error [Error.InvalidAreaId]
        let result = AreaId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AreaIdGen.Instances>|])>]
    let ``AreaId serialization roundtrip`` value =
        let result = serializeDeserialize<AreaId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``AreaId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<AreaId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<AreaNameGen.Valid>|])>]
    let ``given valid AreaName, expect success`` value =
        let expected = Ok (AreaName.create value)
        let result = AreaName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AreaNameGen.Invalid>|])>]
    let ``given invalid AreaName, expect error`` value =
        let expected = Error [Error.InvalidAreaName]
        let result = AreaName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AreaNameGen.Instances>|])>]
    let ``AreaName serialization roundtrip`` value =
        let result = serializeDeserialize<AreaName.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``AreaName deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<AreaName.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<AreaTypeGen.Instances>|])>]
    let ``AreaType serialization roundtrip`` value =
        let result = serializeDeserialize<AreaType> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``AreaType deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<AreaType> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<ExtraRateGen.Valid>|])>]
    let ``given valid ExtraRate, expect success`` value =
        let expected = Ok (ExtraRate.create value)
        let result = ExtraRate.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ExtraRateGen.Invalid>|])>]
    let ``given invalid ExtraRate, expect error`` value =
        let expected = Error [Error.InvalidExtraRate]
        let result = ExtraRate.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<ExtraRateGen.Instances>|])>]
    let ``ExtraRate serialization roundtrip`` value =
        let result = serializeDeserialize<ExtraRate.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``ExtraRate deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<ExtraRate.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<AreaToAreaGroupNameGen.Valid>|])>]
    let ``given valid AreaToAreaGroupName, expect success`` value =
        let expected = Ok (AreaToAreaGroupName.create value)
        let result = AreaToAreaGroupName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AreaToAreaGroupNameGen.Invalid>|])>]
    let ``given invalid AreaToAreaGroupName, expect error`` value =
        let expected = Error [Error.InvalidAreaToAreaGroupName]
        let result = AreaToAreaGroupName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AreaToAreaGroupNameGen.Instances>|])>]
    let ``AreaToAreaGroupName serialization roundtrip`` value =
        let result = serializeDeserialize<AreaToAreaGroupName.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``AreaToAreaGroupName deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<AreaToAreaGroupName.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``Area deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<AreaToAreaGroupName.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<GuidIdGen.Valid>|])>]
    let ``given valid AddressId, expect success`` (addressId) =
        let expected = Ok (AddressId.create (addressId))
        let result = AddressId.tryCreate (addressId)
        result =! expected

    [<Property(Arbitrary=[|typeof<GuidIdGen.Invalid>|])>]
    let ``given AddressId, expect error`` (addressId) =
        let expected = Error [Error.InvalidAddressId]
        let result = AddressId.tryCreate (addressId)
        result =! expected

    [<Property(Arbitrary=[|typeof<AddressIdGen.Instances>|])>]
    let ``AddressId serialization roundtrip`` (value) =
        let result = serializeDeserialize<AddressId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``AddressId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<AddressId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<GuidIdGen.Valid>|])>]
    let ``given valid CellPhoneId, expect success`` (cellPhoneId) =
        let expected = Ok (CellPhoneId.create (cellPhoneId))
        let result = CellPhoneId.tryCreate (cellPhoneId)
        result =! expected

    [<Property(Arbitrary=[|typeof<GuidIdGen.Invalid>|])>]
    let ``given CellPhoneId, expect error`` (cellPhoneId) =
        let expected = Error [Error.InvalidCellPhoneId]
        let result = CellPhoneId.tryCreate (cellPhoneId)
        result =! expected

    [<Property(Arbitrary=[|typeof<CellPhoneIdGen.Instances>|])>]
    let ``CellPhoneId serialization roundtrip`` (value) =
        let result = serializeDeserialize<CellPhoneId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``CellPhoneId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<CellPhoneId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<GuidIdGen.Valid>|])>]
    let ``given valid EmailId, expect success`` (emailId) =
        let expected = Ok (EmailId.create (emailId))
        let result = EmailId.tryCreate (emailId)
        result =! expected

    [<Property(Arbitrary=[|typeof<GuidIdGen.Invalid>|])>]
    let ``given EmailId, expect error`` (emailId) =
        let expected = Error [Error.InvalidEmailId]
        let result = EmailId.tryCreate (emailId)
        result =! expected

    [<Property(Arbitrary=[|typeof<EmailIdGen.Instances>|])>]
    let ``EmailId serialization roundtrip`` (value) =
        let result = serializeDeserialize<EmailId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``EmailId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<EmailId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<BookingReferenceGen.Valid>|])>]
    let ``given valid BookingReference, expect success`` (bookingReference) =
        let expected = Ok (BookingReference.create (bookingReference))
        let result = BookingReference.tryCreate (bookingReference)
        result =! expected

    [<Property(Arbitrary=[|typeof<BookingReferenceGen.Invalid>|])>]
    let ``given BookingReference, expect error`` (bookingReference) =
        let expected = Error [Error.InvalidBookingReference]
        let result = BookingReference.tryCreate (bookingReference)
        result =! expected

    [<Property(Arbitrary=[|typeof<BookingReferenceGen.Instances>|])>]
    let ``BookingReference serialization roundtrip`` (value) =
        let result = serializeDeserialize<BookingReference.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``BookingReference deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<BookingReference.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<AddressNameGen.Valid>|])>]
    let ``given valid AddressName, expect success`` value =
        let expected = Ok (AddressName.create value)
        let result = AddressName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AddressNameGen.Invalid>|])>]
    let ``given invalid AddressName, expect error`` value =
        let expected = Error [Error.InvalidAddressName]
        let result = AddressName.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<AddressNameGen.Instances>|])>]
    let ``AddressName serialization roundtrip`` value =
        let result = serializeDeserialize<AddressName.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``AddressName deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<AddressName.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<CustomerBookingNoteGen.Valid>|])>]
    let ``given valid CustomerBookingNote, expect success`` value =
        let expected = Ok (CustomerBookingNote.create value)
        let result = CustomerBookingNote.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<CustomerBookingNoteGen.Invalid>|])>]
    let ``given invalid BookingCustomerNote, expect error`` value =
        let expected = Error [Error.InvalidCustomerBookingNote]
        let result = CustomerBookingNote.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<CustomerBookingNoteGen.Instances>|])>]
    let ``CustomerBookingNote serialization roundtrip`` value =
        let result = serializeDeserialize<CustomerBookingNote.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``CustomerBookingNote deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<CustomerBookingNote.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<VehicleMakeGen.Valid>|])>]
    let ``given valid VehicleMake, expect success`` value =
        let expected = Ok (VehicleMake.create value)
        let result = VehicleMake.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<VehicleMakeGen.Invalid>|])>]
    let ``given invalid VehicleMake, expect error`` value =
        let expected = Error [Error.InvalidVehicleMake]
        let result = VehicleMake.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<VehicleMakeGen.Instances>|])>]
    let ``VehicleMake serialization roundtrip`` value =
        let result = serializeDeserialize<VehicleMake.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``VehicleMake deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<VehicleMake.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<VehicleModelGen.Valid>|])>]
    let ``given valid VehicleModel, expect success`` value =
        let expected = Ok (VehicleModel.create value)
        let result = VehicleModel.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<VehicleModelGen.Invalid>|])>]
    let ``given invalid VehicleModel, expect error`` value =
        let expected = Error [Error.InvalidVehicleModel]
        let result = VehicleModel.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<VehicleModelGen.Instances>|])>]
    let ``VehicleModel serialization roundtrip`` value =
        let result = serializeDeserialize<VehicleModel.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``VehicleModel deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<VehicleModel.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<VehicleLicensePlateGen.Valid>|])>]
    let ``given valid VehicleLicensePlate, expect success`` value =
        let expected = Ok (VehicleLicensePlate.create value)
        let result = VehicleLicensePlate.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<VehicleLicensePlateGen.Invalid>|])>]
    let ``given invalid VehicleLicensePlate, expect error`` value =
        let expected = Error [Error.InvalidVehicleLicensePlate]
        let result = VehicleLicensePlate.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<VehicleLicensePlateGen.Instances>|])>]
    let ``VehicleLicensePlate serialization roundtrip`` value =
        let result = serializeDeserialize<VehicleLicensePlate.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``VehicleLicensePlate deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<VehicleLicensePlate.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<VehicleColorGen.Valid>|])>]
    let ``given valid VehicleColor, expect success`` value =
        let expected = Ok (VehicleColor.create value)
        let result = VehicleColor.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<VehicleColorGen.Invalid>|])>]
    let ``given invalid VehicleColor, expect error`` value =
        let expected = Error [Error.InvalidVehicleColor]
        let result = VehicleColor.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<VehicleColorGen.Instances>|])>]
    let ``VehicleColor serialization roundtrip`` value =
        let result = serializeDeserialize<VehicleColor.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``VehicleColor deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<VehicleColor.T> json |> ignore
        ) |> ignore


    [<Property(Arbitrary=[|typeof<EmployeeIdGen.Valid>|])>]
    let ``given valid EmployeeId, expect success`` value =
        let expected = Ok (EmployeeId.create value)
        let result = EmployeeId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<EmployeeIdGen.Invalid>|])>]
    let ``given invalid EmployeeId, expect error`` value =
        let expected = Error [Error.InvalidEmployeeId]
        let result = EmployeeId.tryCreate value
        result =! expected

    [<Property(Arbitrary=[|typeof<EmployeeIdGen.Instances>|])>]
    let ``EmployeeId serialization roundtrip`` value =
        let result = serializeDeserialize<EmployeeId.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<InvalidJsonsGen>|])>]
    let ``EmployeeId deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<EmployeeId.T> json |> ignore
        ) |> ignore

    [<Property(Arbitrary=[|typeof<RoleGen.Instances>|])>]
    let ``Role serialization roundtrip`` (value) =
        let result = serializeDeserialize<Role> value
        result =! value

    [<Property(Arbitrary=[|typeof<RoleGen.InvalidJsons>|])>]
    let ``Role deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<Role> json |> ignore
        ) |> ignore
