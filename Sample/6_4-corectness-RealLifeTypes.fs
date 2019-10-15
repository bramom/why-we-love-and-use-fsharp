namespace OpenLimo.Core.ValueTypes

open System
open System.Net.Mail
open System.Text.RegularExpressions
open OpenLimo.Core.Library
open OpenLimo.Core.Library.TypeExtensions
open OpenLimo.Core.Library.Result
open OpenLimo.Core.Serialization

module private Validation =
    let isValidEmail (email: string) =
        if String.IsNullOrWhiteSpace (email) then
            false
        else
            try
                MailAddress (email) |> ignore
                true
            with
            | :? FormatException ->
                false

[<RequireQualifiedAccess>]
module CustomerId =

    let private isValid customerId =
        not (isNull customerId)
        && (Regex.IsMatch (customerId, "^[a-zA-Z][0-9a-zA-Z_.-]*[0-9a-zA-Z]$")
            || Validation.isValidEmail customerId)

    [<Newtonsoft.Json.JsonConverter(typeof<CustomerIdJsonConverter>)>]
    type T = private CustomerId of string
        with
            static member CanCreate = isValid
            static member TryCreate (customerId) =
                if T.CanCreate customerId
                then Ok <| CustomerId customerId
                else Error [Error.InvalidCustomerId]
            static member Create (customerId) =
                match T.TryCreate customerId with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (CustomerId s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (CustomerId s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and CustomerIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module EmployeeId =

    let private isValid employeeId =
        not (isNull employeeId)
        && (Regex.IsMatch (employeeId, "^[a-zA-Z][0-9a-zA-Z_.-]*[0-9a-zA-Z]$")
            || Validation.isValidEmail employeeId)

    [<Newtonsoft.Json.JsonConverter(typeof<EmployeeIdJsonConverter>)>]
    type T = private EmployeeId of string
        with
            static member CanCreate = isValid
            static member TryCreate (employeeId) =
                if T.CanCreate employeeId
                then Ok <| EmployeeId employeeId
                else Error [Error.InvalidEmployeeId]
            static member Create (employeeId) =
                match T.TryCreate employeeId with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (EmployeeId s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (EmployeeId s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and EmployeeIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module FirstName =

    let private isValid = not << String.IsNullOrWhiteSpace

    [<Newtonsoft.Json.JsonConverter(typeof<FirstNameJsonConverter>)>]
    type T = private FirstName of string
        with
            static member CanCreate = isValid
            static member TryCreate (firstName) =
                if T.CanCreate firstName
                then Ok <| FirstName firstName
                else Error [Error.InvalidFirstName]
            static member Create (firstName) =
                match T.TryCreate firstName with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (FirstName s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (FirstName s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and FirstNameJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module LastName =

    let private isValid = not << String.IsNullOrWhiteSpace

    [<Newtonsoft.Json.JsonConverter(typeof<LastNameJsonConverter>)>]
    type T = private LastName of string
        with
            static member CanCreate = isValid
            static member TryCreate (lastName) =
                if T.CanCreate lastName
                then Ok <| LastName lastName
                else Error [Error.InvalidLastName]
            static member Create (lastName) =
                match T.TryCreate lastName with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (LastName s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (LastName s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and LastNameJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module EmailAddress =

    [<Newtonsoft.Json.JsonConverter(typeof<EmailAddressJsonConverter>)>]
    type T = private EmailAddress of string
        with
            static member CanCreate = Validation.isValidEmail
            static member TryCreate (email) =
                if T.CanCreate email
                then Ok <| EmailAddress (email.ToLowerInvariant ())
                else Error [Error.InvalidEmailAddress]
            static member Create (email) =
                match T.TryCreate email with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (EmailAddress s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (EmailAddress s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and EmailAddressJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module CellPhoneId =

    let private asString = stringf "N"

    let private isValid value =
        Guid.TryParse value
        |> fun (result, _) -> result

    [<Newtonsoft.Json.JsonConverter(typeof<CellPhoneIdJsonConverter>)>]
    type T = private CellPhoneId of Guid
        with
            static member CanCreate = isValid
            static member TryCreate (id) =
                if T.CanCreate id
                then Ok (CellPhoneId (Guid.Parse id))
                else Error [Error.InvalidCellPhoneId]
            static member Create (id) =
                match T.TryCreate id with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member NewId () =
                CellPhoneId (Guid.NewGuid ())
            static member Value (CellPhoneId s) = asString s
            override x.ToString () =
                let (CellPhoneId value) = x
                asString value
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (CellPhoneId s) =
                Encode.string (asString s)
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and CellPhoneIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let newId = T.NewId
    let fromGuid = CellPhoneId
    let asGuid (CellPhoneId value) = value
    let empty = CellPhoneId Guid.Empty

[<RequireQualifiedAccess>]
module EmailId =

    let private asString = stringf "N"

    let private isValid value =
        Guid.TryParse value
        |> fun (result, _) -> result

    [<Newtonsoft.Json.JsonConverter(typeof<EmailIdJsonConverter>)>]
    type T = private EmailId of Guid
        with
            static member CanCreate = isValid
            static member TryCreate (id) =
                if T.CanCreate id
                then Ok (EmailId (Guid.Parse id))
                else Error [Error.InvalidEmailId]
            static member Create (id) =
                match T.TryCreate id with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member NewId () =
                EmailId (Guid.NewGuid ())
            static member Value (EmailId s) = asString s
            override x.ToString () =
                let (EmailId value) = x
                asString value
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (EmailId s) =
                Encode.string (asString s)
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and EmailIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let newId = T.NewId
    let fromGuid = EmailId
    let asGuid (EmailId value) = value
    let empty = EmailId Guid.Empty

[<RequireQualifiedAccess>]
module CellPhone =

    type Info =
        { CountryCode: string
          PhoneNumber: string }
        with
            static member Decoder =
                Decode.map2
                    (fun countryCode phoneNumber ->
                        { CountryCode = countryCode
                          PhoneNumber = phoneNumber })
                    (Decode.field "countryCode" Decode.string)
                    (Decode.field "phoneNumber" Decode.string)
            static member Encoder (instance: Info) =
                Encode.object
                    [ "countryCode", Encode.string instance.CountryCode
                      "phoneNumber", Encode.string instance.PhoneNumber ]
            static member FromJson json = Decode.fromString Info.Decoder json
            static member ToJson instance = Info.Encoder instance |> Encode.toString 4

    let private isNumeric = Seq.forall Char.IsDigit

    let private isValid { CountryCode = countryCode; PhoneNumber = phoneNumber } =
        not (String.IsNullOrWhiteSpace countryCode)
        && not (String.IsNullOrWhiteSpace phoneNumber)
        && isNumeric countryCode
        && isNumeric phoneNumber

    [<Newtonsoft.Json.JsonConverter(typeof<CellPhoneJsonConverter>)>]
    type T = private CellPhone of Info
        with
            static member CanCreate = isValid
            static member TryCreate cellPhone =
                if T.CanCreate cellPhone
                then Ok <| CellPhone cellPhone
                else Error [Error.InvalidCellPhone]
            static member Create cellPhone =
                match T.TryCreate cellPhone with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (CellPhone c) = c
            static member Decoder path token =
                Info.Decoder path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (CellPhone a) = Info.Encoder a
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4

    and CellPhoneJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module LandlineId =

    let private asString = stringf "N"

    let private isValid value =
        Guid.TryParse value
        |> fun (result, _) -> result

    [<Newtonsoft.Json.JsonConverter(typeof<LandlineIdJsonConverter>)>]
    type T = private LandlineId of Guid
        with
            static member CanCreate = isValid
            static member TryCreate (id) =
                if T.CanCreate id
                then Ok (LandlineId (Guid.Parse id))
                else Error [Error.InvalidLandlineId]
            static member Create (id) =
                match T.TryCreate id with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member NewId () =
                LandlineId (Guid.NewGuid ())
            static member Value (LandlineId s) = asString s
            override x.ToString () =
                let (LandlineId value) = x
                asString value
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (LandlineId s) =
                Encode.string (asString s)
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0
    and LandlineIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let newId = T.NewId
    let fromGuid = LandlineId
    let asGuid (LandlineId value) = value
    let empty = LandlineId Guid.Empty

[<RequireQualifiedAccess>]
module Landline =

    type Info =
        { CountryCode: string
          AreaCode: string
          PhoneNumber: string
          Extension: string option }
        with
            static member Decoder =
                Decode.map4
                    (fun countryCode areaCode phoneNumber extension ->
                        { CountryCode = countryCode
                          AreaCode = areaCode
                          PhoneNumber = phoneNumber
                          Extension = extension })
                    (Decode.field "countryCode" Decode.string)
                    (Decode.field "areaCode" Decode.string)
                    (Decode.field "phoneNumber" Decode.string)
                    (Decode.optional "extension" Decode.string)

            static member Encoder (instance: Info) =
                Encode.object
                    [ "countryCode", Encode.string instance.CountryCode
                      "areaCode", Encode.string instance.AreaCode
                      "phoneNumber", Encode.string instance.PhoneNumber
                      "extension", (Encode.option Encode.string) instance.Extension ]
            static member FromJson json = Decode.fromString Info.Decoder json
            static member ToJson instance = Info.Encoder instance |> Encode.toString 4

    let private isNumeric = Seq.forall Char.IsDigit

    let private isValid info =
        not (String.IsNullOrWhiteSpace info.CountryCode)
        && not (String.IsNullOrWhiteSpace info.AreaCode)
        && not (String.IsNullOrWhiteSpace info.PhoneNumber)
        && isNumeric info.CountryCode
        && isNumeric info.AreaCode
        && isNumeric info.PhoneNumber
        && (info.Extension.IsNone
            || not (String.IsNullOrWhiteSpace info.Extension.Value))

    [<Newtonsoft.Json.JsonConverter(typeof<LandlineJsonConverter>)>]
    type T = private Landline of Info
        with
            static member CanCreate = isValid
            static member TryCreate landline =
                if T.CanCreate landline
                then Ok <| Landline landline
                else Error [Error.InvalidLandline]
            static member Create landline =
                match T.TryCreate landline with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (Landline l) = l
            static member Decoder path token =
                Info.Decoder path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (Landline a) = Info.Encoder a
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4

    and LandlineJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module EmailAddressContact =

    type UnverifiedEmailData = EmailAddress.T
    type VerifiedEmailData = EmailAddress.T * DateTimeOffset

    [<Newtonsoft.Json.JsonConverter(typeof<EmailAddressContactJsonConverter>)>]
    type T =
        | UnverifiedEmail of UnverifiedEmailData
        | VerifiedEmail of VerifiedEmailData
        with
            static member Decoder =
                Decode.map2
                    (fun emailAddress -> function
                        | Some verifiedAt -> VerifiedEmail (emailAddress, verifiedAt)
                        | None            -> UnverifiedEmail emailAddress)
                    (Decode.field "email" EmailAddress.T.Decoder)
                    (Decode.optional "verifiedAt" Decode.datetimeOffset)
            static member Encoder = function
                | UnverifiedEmail email ->
                    Encode.object
                        [ "email", EmailAddress.T.Encoder email ]
                | VerifiedEmail (email, verifiedAt) ->
                    Encode.object
                        [ "email", EmailAddress.T.Encoder email
                          "verifiedAt", Encode.datetimeOffset verifiedAt ]
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4

    and EmailAddressContactJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = EmailAddress.canCreate
    let tryCreate email = UnverifiedEmail <!> EmailAddress.tryCreate email
    let create = EmailAddress.create >> UnverifiedEmail

    let unwrap = function
        | VerifiedEmail (e, _) -> e
        | UnverifiedEmail e -> e

    let verify dateVerified = function
        | UnverifiedEmail email -> VerifiedEmail (email, dateVerified)
        | VerifiedEmail _ as email -> email

[<RequireQualifiedAccess>]
module CellPhoneContact =

    type UnverifiedCellPhoneData = CellPhone.T
    type VerifiedCellPhoneData = CellPhone.T * DateTimeOffset

    [<Newtonsoft.Json.JsonConverter(typeof<CellPhoneContactJsonConverter>)>]
    type T =
        | UnverifiedCellPhone of UnverifiedCellPhoneData
        | VerifiedCellPhone of VerifiedCellPhoneData
        with
            static member Decoder =
                Decode.map2
                    (fun cellPhone -> function
                        | Some x -> VerifiedCellPhone (cellPhone, x)
                        | None   -> UnverifiedCellPhone cellPhone)
                    (Decode.field "cellPhone" CellPhone.T.Decoder)
                    (Decode.optional "verifiedAt" Decode.datetimeOffset)
            static member Encoder = function
                | UnverifiedCellPhone cellPhone ->
                    Encode.object
                        [ "cellPhone", CellPhone.T.Encoder cellPhone ]
                | VerifiedCellPhone (cellPhone, verifiedAt) ->
                    Encode.object
                        [ "cellPhone", CellPhone.T.Encoder cellPhone
                          "verifiedAt", Encode.datetimeOffset verifiedAt ]
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4

    and CellPhoneContactJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = CellPhone.canCreate
    let tryCreate cellPhone = UnverifiedCellPhone <!> CellPhone.tryCreate cellPhone
    let create = CellPhone.create >> UnverifiedCellPhone

    let unwrap = function
        | VerifiedCellPhone (c, _) -> c
        | UnverifiedCellPhone c -> c

    let verify dateVerified = function
        | UnverifiedCellPhone cellPhone -> VerifiedCellPhone (cellPhone, dateVerified)
        | VerifiedCellPhone _ as cellPhone -> cellPhone

[<RequireQualifiedAccess>]
module LandlineContact =

    type UnverifiedLandlineData = Landline.T
    type VerifiedLandlineData = Landline.T * DateTimeOffset

    [<Newtonsoft.Json.JsonConverter(typeof<LandlineContactJsonConverter>)>]
    type T =
        | UnverifiedLandline of UnverifiedLandlineData
        | VerifiedLandline of VerifiedLandlineData
        with
            static member Decoder =
                Decode.map2
                    (fun landline -> function
                        | Some x -> VerifiedLandline (landline, x)
                        | None   -> UnverifiedLandline landline)
                    (Decode.field "landline" Landline.T.Decoder)
                    (Decode.optional "verifiedAt" Decode.datetimeOffset)
            static member Encoder = function
                | UnverifiedLandline landline ->
                    Encode.object
                        [ "landline", Landline.T.Encoder landline ]
                | VerifiedLandline (landline, verifiedAt) ->
                    Encode.object
                        [ "landline", Landline.T.Encoder landline
                          "verifiedAt", Encode.datetimeOffset verifiedAt ]
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4

    and LandlineContactJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = Landline.canCreate
    let tryCreate landline = UnverifiedLandline <!> Landline.tryCreate landline
    let create = Landline.create >> UnverifiedLandline

    let unwrap = function
        | VerifiedLandline (l, _) -> l
        | UnverifiedLandline l -> l

    let verify dateVerified = function
        | UnverifiedLandline landline -> VerifiedLandline (landline, dateVerified)
        | VerifiedLandline _ as landline -> landline

[<RequireQualifiedAccess>]
module Password =

    let private isNonEmptyBase64 (str: string) =
        try
            let bytes = Convert.FromBase64String (str)
            bytes.Length > 0
        with
        | _ -> false

    let private validateHash (hash: string) =
        not (String.IsNullOrWhiteSpace hash) && isNonEmptyBase64 hash

    let private validate complexityCheck (password: string) =
        if String.IsNullOrWhiteSpace password then
            [Error.InvalidPassword]
        else
            match complexityCheck password with
            | Ok () -> []
            | Error message -> [Error.WeakPasswordComplexity message]

    [<Newtonsoft.Json.JsonConverter(typeof<PasswordJsonConverter>)>]
    type T = private Password of string
        with
            static member CanCreate (password, complexityCheck) =
                validate complexityCheck password
                |> List.isEmpty
            static member TryCreate (password, complexityCheck, hashFn: HashPassword) =
                match validate complexityCheck password with
                | [] -> Ok (Password (hashFn password))
                | errors -> Error errors
            static member Create (password, complexityCheck, hashFn: HashPassword) =
                match T.TryCreate (password, complexityCheck, hashFn) with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member CanCreateFromHash = validateHash
            static member TryCreateFromHash (hash) =
                if T.CanCreateFromHash hash
                then Ok (Password hash)
                else Error [Error.InvalidPasswordHash]
            static member CreateFromHash hash =
                match T.TryCreateFromHash hash with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (Password s) = s
            member x.VerifyHashedPassword (password: string, verifyFn: VerifyHashedPassword) = verifyFn x password
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreateFromHash x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (Password s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and PasswordJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")
    and
        /// Accepts plain text password, returns hashed representation of password.
        HashPassword = string -> string
    and
        /// Accepts Password.T instance and plain text password, returns a Result indicating the result of a password hash comparison.
        VerifyHashedPassword = T -> string -> Result<unit, string> //string hashedPassword, string providedPassword

    let canCreate complexityCheck password = T.CanCreate (password, complexityCheck)
    let tryCreate complexityCheck hashFn password = T.TryCreate (password, complexityCheck, hashFn)
    let create complexityCheck hashFn password = T.Create (password, complexityCheck, hashFn)
    let canCreateFromHash = T.CanCreateFromHash
    let tryCreateFromHash = T.TryCreateFromHash
    let createFromHash = T.CreateFromHash
    let verifyHashedPassword verifyFn (password: T) passwordToVerify = password.VerifyHashedPassword (passwordToVerify, verifyFn)
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module ExternalLogin =

    type Info =
        { Provider: string
          Key: string
          Name: string option }
        with
            static member Decoder =
                Decode.map3
                    (fun provider key name ->
                        { Provider = provider
                          Key = key
                          Name = name })
                    (Decode.field "provider" Decode.string)
                    (Decode.field "key" Decode.string)
                    (Decode.optional "name" Decode.string)
            static member Encoder (instance: Info) =
                Encode.object
                    [ "provider", Encode.string instance.Provider
                      "key", Encode.string instance.Key
                      "name", (Encode.option Encode.string) instance.Name ]
            static member FromJson json = Decode.fromString Info.Decoder json
            static member ToJson instance = Info.Encoder instance |> Encode.toString 4

    let private nonEmpty = not << String.IsNullOrWhiteSpace
    let private isValid { Provider = provider; Key = key; Name = name } =
        nonEmpty provider && nonEmpty key && (name <> Some null)

    [<Newtonsoft.Json.JsonConverter(typeof<ExternalProviderJsonConverter>)>]
    type T = private ExternalLogin of Info
        with
            static member CanCreate = isValid
            static member TryCreate (externalLogin) =
                if T.CanCreate externalLogin
                then Ok <| ExternalLogin externalLogin
                else Error [Error.InvalidExternalLogin]
            static member Create (externalLogin) =
                match T.TryCreate externalLogin with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (ExternalLogin e) = e
            static member Decoder path token =
                Info.Decoder path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (ExternalLogin i) = Info.Encoder i
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4

    and ExternalProviderJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f


[<RequireQualifiedAccess>]
module TransactionId =

    let private asString = stringf "N"

    let private isValid value =
        Guid.TryParse value
        |> fun (result, _) -> result

    [<Newtonsoft.Json.JsonConverter(typeof<TransactionIdJsonConverter>)>]
    type T = private TransactionId of Guid
        with
            static member CanCreate = isValid
            static member TryCreate (id) =
                if T.CanCreate id
                then Ok (TransactionId (Guid.Parse id))
                else Error [Error.InvalidTransactionId]
            static member Create (id) =
                match T.TryCreate id with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member NewId () =
                TransactionId (Guid.NewGuid ())
            static member Value (TransactionId s) = asString s
            override x.ToString () =
                let (TransactionId value) = x
                asString value
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (TransactionId s) =
                Encode.string (asString s)
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and TransactionIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f
    let newId = T.NewId
    let fromGuid = TransactionId
    let asGuid (TransactionId value) = value
    let empty = TransactionId Guid.Empty

[<RequireQualifiedAccess>]
module CreditCardId =

    let private asString = stringf "N"

    let private isValid value =
        Guid.TryParse value
        |> fun (result, _) -> result

    [<Newtonsoft.Json.JsonConverter(typeof<CreditCardIdJsonConverter>)>]
    type T = private CreditCardId of Guid
        with
            static member CanCreate = isValid
            static member TryCreate (id) =
                if T.CanCreate id
                then Ok (CreditCardId (Guid.Parse id))
                else Error [Error.InvalidCreditCardId]
            static member Create (id) =
                match T.TryCreate id with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member NewId () =
                CreditCardId (Guid.NewGuid ())
            static member Value (CreditCardId s) = asString s
            override x.ToString () =
                let (CreditCardId value) = x
                asString value
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (CreditCardId s) =
                Encode.string (asString s)
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and CreditCardIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f
    let newId = T.NewId
    let fromGuid = CreditCardId
    let asGuid (CreditCardId value) = value
    let empty = CreditCardId Guid.Empty

[<RequireQualifiedAccess>]
module CreditCardExpirationDate =
    open System.Globalization

    let private tryParseExpirationDate value =
         DateTimeOffset.TryParseExact(value, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None)
         |> function
         | true, result -> Ok result
         | _ -> Error [Error.InvalidCreditCardExpirationDate]

    let private isValid value =
        Result.isOk (tryParseExpirationDate value)

    let private asString = stringf "MM/yy"

    [<Newtonsoft.Json.JsonConverter(typeof<CreditCardExpirationDateJsonConverter>)>]
    type T = private CreditCardExpirationDate of DateTimeOffset
        with
            static member CanCreate = isValid
            static member TryCreate (value) =
                if T.CanCreate value
                then
                    tryParseExpirationDate value |> Result.map CreditCardExpirationDate
                else Error [Error.InvalidCreditCardExpirationDate]
            static member Create (value) =
                match T.TryCreate value with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (CreditCardExpirationDate s) = asString s
            override x.ToString () =
                let (CreditCardExpirationDate value) = x
                asString value

            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (CreditCardExpirationDate s) = Encode.string (asString s)
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

            member x.IsExpired (target: DateTimeOffset) =
                let (CreditCardExpirationDate actual) = x
                actual.Year <= target.Year && actual.Month <= target.Month

    and CreditCardExpirationDateJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f
    let isExpired (expirationDate: T) target = expirationDate.IsExpired target

[<RequireQualifiedAccess>]
module UnmaskedCreditCard =

    type Info =
        { Pan: string
          NameOnCard: string
          Expires: CreditCardExpirationDate.T }
        with
            static member Decoder =
                Decode.map3
                    (fun pan nameOnCard expires ->
                        { Pan = pan
                          NameOnCard = nameOnCard
                          Expires = expires })
                    (Decode.field "pan" Decode.string)
                    (Decode.field "nameOnCard" Decode.string)
                    (Decode.field "expires" CreditCardExpirationDate.T.Decoder)
            static member Encoder (instance: Info) =
                Encode.object
                    [ "pan", Encode.string instance.Pan
                      "nameOnCard", Encode.string instance.NameOnCard
                      "expires", CreditCardExpirationDate.T.Encoder instance.Expires ]
            static member FromJson json = Decode.fromString Info.Decoder json
            static member ToJson instance = Info.Encoder instance |> Encode.toString 4

    let private isNumeric = Seq.forall Char.IsDigit

    // https://en.wikipedia.org/wiki/Luhn_algorithm
    let private luhn (s: string) =
        let rec g r c = function
            | 0 -> r
            | i ->
                let d = ((int s.[i - 1]) - 48) <<< c
                g (r + if d < 10 then d else d - 9) (1 - c) (i - 1)
        (g 0 0 s.Length) % 10 = 0

    let private isValidPan pan =
        if (String.IsNullOrWhiteSpace (pan))
           || (pan.Length > 19) // https://en.wikipedia.org/wiki/Payment_card_number
           || (not (isNumeric pan)) then false
        else
            luhn pan

    let private validate (cc: Info) =
        [
            if not (isValidPan cc.Pan) then
                yield Error.InvalidCreditCardPan
            if String.IsNullOrWhiteSpace cc.NameOnCard then
                yield Error.InvalidCreditCardNameOnCard
        ]

    [<Newtonsoft.Json.JsonConverter(typeof<UnmaskedCreditCardJsonConverter>)>]
    type T = private UnmaskedCreditCard of Info
        with
            static member CanCreate = validate >> List.isEmpty
            static member TryCreate (cc) =
                match validate cc with
                | [] -> Ok (UnmaskedCreditCard cc)
                | errors -> Error errors
            static member Create cc =
                match T.TryCreate cc with
                | Ok cc -> cc
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (UnmaskedCreditCard cc) = cc
            static member Decoder path token =
                Info.Decoder path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (UnmaskedCreditCard i) = Info.Encoder i
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4

    and UnmaskedCreditCardJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module CreditCardTypeKind =
    let [<Literal>] AmericanExpress = "AMERICAN_EXPRESS"
    let [<Literal>] Discover = "DISCOVER"
    let [<Literal>] MasterCard = "MASTER_CARD"
    let [<Literal>] Maestro = "MAESTRO"
    let [<Literal>] Visa = "VISA"
    let [<Literal>] UnionPay = "UNION_PAY"

[<Newtonsoft.Json.JsonConverter(typeof<CreditCardTypeJsonConverter>)>]
type CreditCardType =
    | AmericanExpress
    | Discover
    | MasterCard
    | Maestro
    | Visa
    | UnionPay
    | Unknown of string
    with
        member x.AsString =
            match x with
            | AmericanExpress -> CreditCardTypeKind.AmericanExpress
            | Discover -> CreditCardTypeKind.Discover
            | MasterCard -> CreditCardTypeKind.MasterCard
            | Maestro -> CreditCardTypeKind.Maestro
            | Visa -> CreditCardTypeKind.Visa
            | UnionPay -> CreditCardTypeKind.UnionPay
            | Unknown value -> value
        static member FromString = function
            | CreditCardTypeKind.AmericanExpress -> AmericanExpress
            | CreditCardTypeKind.Discover -> Discover
            | CreditCardTypeKind.MasterCard -> MasterCard
            | CreditCardTypeKind.Maestro -> Maestro
            | CreditCardTypeKind.Visa -> Visa
            | CreditCardTypeKind.UnionPay -> UnionPay
            | unknownType -> Unknown unknownType
        static member Decoder path token =
            Decode.string path token
            |> Result.bind (fun x ->
                match x with
                | CreditCardTypeKind.AmericanExpress ->
                    Decode.succeed AmericanExpress x token
                | CreditCardTypeKind.Discover ->
                    Decode.succeed Discover x token
                | CreditCardTypeKind.MasterCard ->
                    Decode.succeed MasterCard x token
                | CreditCardTypeKind.Maestro ->
                    Decode.succeed Maestro x token
                | CreditCardTypeKind.Visa ->
                    Decode.succeed Visa x token
                | CreditCardTypeKind.UnionPay ->
                    Decode.succeed UnionPay x token
                | unknownType ->
                    Decode.succeed (Unknown unknownType) x token
            )
        static member Encoder (x: CreditCardType) = Encode.string x.AsString
        static member FromJson json = Decode.fromString CreditCardType.Decoder json
        static member ToJson instance = CreditCardType.Encoder instance |> Encode.toString 4

and CreditCardTypeJsonConverter () =
    inherit Json.ValueTypeJsonConverter<CreditCardType> (CreditCardType.ToJson, CreditCardType.Decoder "")

[<RequireQualifiedAccess>]
module MaskedCreditCard =
    type Info =
        { ScrambledPan: string
          Expires: CreditCardExpirationDate.T
          CardType: CreditCardType }
        with
            static member Decoder =
                Decode.map3
                    (fun scrambledPan expires cardType ->
                        { ScrambledPan = scrambledPan
                          Expires = expires
                          CardType = cardType })
                    (Decode.field "scrambledPan" Decode.string)
                    (Decode.field "expires" CreditCardExpirationDate.T.Decoder)
                    (Decode.field "cardType" CreditCardType.Decoder)
            static member Encoder (instance: Info) =
                Encode.object
                    [ "scrambledPan", Encode.string instance.ScrambledPan
                      "expires", CreditCardExpirationDate.T.Encoder instance.Expires
                      "cardType", CreditCardType.Encoder instance.CardType ]
            static member FromJson json = Decode.fromString Info.Decoder json
            static member ToJson instance = Info.Encoder instance |> Encode.toString 4

    let private validate (cc: Info) =
        [
            if String.IsNullOrWhiteSpace cc.ScrambledPan then
                yield Error.InvalidCreditCardScrambledPan
        ]

    [<Newtonsoft.Json.JsonConverter(typeof<MaskedCreditCardJsonConverter>)>]
    type T = private MaskedCreditCard of Info
        with
            static member CanCreate = validate >> List.isEmpty
            static member TryCreate (cc) =
                match validate cc with
                | [] -> Ok (MaskedCreditCard cc)
                | errors -> Error errors
            static member Create cc =
                match T.TryCreate cc with
                | Ok cc -> cc
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (MaskedCreditCard cc) = cc
            static member Decoder path token =
                Info.Decoder path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (MaskedCreditCard i) = Info.Encoder i
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4

    and MaskedCreditCardJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module AccountId =

    let private isValid accountId =
        not (isNull accountId)
        && Regex.IsMatch (accountId, "^[a-zA-Z][0-9a-zA-Z_.-]*[0-9a-zA-Z]$")

    [<Newtonsoft.Json.JsonConverter(typeof<AccountIdJsonConverter>)>]
    type T = private AccountId of string
        with
            static member CanCreate = isValid
            static member TryCreate (accountId) =
                if T.CanCreate accountId
                then Ok <| AccountId accountId
                else Error [Error.InvalidAccountId]
            static member Create (accountId) =
                match T.TryCreate accountId with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (AccountId s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (AccountId s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and AccountIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module AccountName =

    let private isValid = not << String.IsNullOrWhiteSpace

    [<Newtonsoft.Json.JsonConverter(typeof<AccountNameJsonConverter>)>]
    type T = private AccountName of string
        with
            static member CanCreate = isValid
            static member TryCreate (name) =
                if T.CanCreate name
                then Ok <| AccountName name
                else Error [Error.InvalidAccountName]
            static member Create (name) =
                match T.TryCreate name with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (AccountName s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (AccountName s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and AccountNameJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module AddressId =

    let private asString = stringf "N"

    let private isValid value =
        Guid.TryParse value
        |> fun (result, _) -> result

    [<Newtonsoft.Json.JsonConverter(typeof<AddressIdJsonConverter>)>]
    type T = private AddressId of Guid
        with
            static member CanCreate = isValid
            static member TryCreate (id) =
                if T.CanCreate id
                then Ok (AddressId (Guid.Parse id))
                else Error [Error.InvalidAddressId]
            static member Create (id) =
                match T.TryCreate id with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member NewId () =
                AddressId (Guid.NewGuid ())
            static member Value (AddressId s) = asString s
            override x.ToString () =
                let (AddressId value) = x
                asString value
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (AddressId s) =
                Encode.string (asString s)
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and AddressIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let newId = T.NewId
    let fromGuid = AddressId
    let asGuid (AddressId value) = value
    let empty = AddressId Guid.Empty

[<RequireQualifiedAccess>]
module AddressName =

    let private isValid = not << String.IsNullOrWhiteSpace

    [<Newtonsoft.Json.JsonConverter(typeof<AddressNameJsonConverter>)>]
    type T = private AddressName of string
        with
            static member CanCreate = isValid
            static member TryCreate (addressName) =
                if T.CanCreate addressName
                then Ok (AddressName addressName)
                else Error [Error.InvalidAddressName]
            static member Create (addressName) =
                match T.TryCreate addressName with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (AddressName s) = s
            override x.ToString () =
                let (AddressName value) = x
                value
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (AddressName s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and AddressNameJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value

[<RequireQualifiedAccess>]
module Address =

    type Info =
        { StreetName: string
          StreetNumber: string
          Apartment: string
          Landmark: string
          City: string
          State: string
          PostalCode: string
          CountryCode: string
          Formatted: string }

    type FullInfo =
        { Info: Info
          Location: Location.T }
        with
            static member Decoder =
                Decode.object
                    (fun data ->
                        { Info =
                            { StreetName =   data.Optional.Field "streetName" Decode.string   |> Option.defaultValue null
                              StreetNumber = data.Optional.Field "streetNumber" Decode.string |> Option.defaultValue null
                              Apartment =    data.Optional.Field "apartment" Decode.string    |> Option.defaultValue null
                              Landmark =     data.Optional.Field "landmark" Decode.string     |> Option.defaultValue null
                              City =         data.Required.Field "city" Decode.string
                              State =        data.Optional.Field "state" Decode.string        |> Option.defaultValue null
                              PostalCode =   data.Optional.Field "postalCode" Decode.string   |> Option.defaultValue null
                              CountryCode =  data.Required.Field "countryCode" Decode.string
                              Formatted =    data.Optional.Field "formatted" Decode.string    |> Option.defaultValue null }
                          Location = data.Required.Field "location" Location.T.Decoder })
            static member Encoder (instance: FullInfo) =
                Encode.object
                    [ "streetName", Encode.string instance.Info.StreetName
                      "streetNumber", Encode.string instance.Info.StreetNumber
                      "apartment", Encode.string instance.Info.Apartment
                      "landmark", Encode.string instance.Info.Landmark
                      "city", Encode.string instance.Info.City
                      "state", Encode.string instance.Info.State
                      "postalCode", Encode.string instance.Info.PostalCode
                      "countryCode", Encode.string instance.Info.CountryCode
                      "formatted", Encode.string instance.Info.Formatted
                      "location", Location.T.Encoder instance.Location ]
            static member FromJson json = Decode.fromString FullInfo.Decoder json
            static member ToJson instance = FullInfo.Encoder instance |> Encode.toString 4

    let private validate address =
        [
            if (String.IsNullOrWhiteSpace (address.CountryCode)) || address.CountryCode.Length <> 2 then
                yield Error.InvalidCountryCode
            if String.IsNullOrWhiteSpace (address.City) then
                yield Error.InvalidAddressCity
            // TODO: should review approach here since postal code only is valid for London even if it does not have street number or name
            if String.IsNullOrWhiteSpace (address.PostalCode)
               && String.IsNullOrWhiteSpace (address.StreetName)
               && String.IsNullOrWhiteSpace (address.StreetNumber) then
                yield Error.InvalidAddressStreet
        ]

    [<Newtonsoft.Json.JsonConverter(typeof<AddressJsonConverter>)>]
    type T = private Address of FullInfo
        with
            static member CanCreate = validate >> List.isEmpty
            static member TryCreate (address, location) =
                match validate address with
                | [] -> Ok <| Address { Info = address; Location = location }
                | errors -> Error errors
            static member Create (address, location) =
                match T.TryCreate (address, location) with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (Address a) = a
            static member Decoder path token =
                FullInfo.Decoder path token
                |> Result.bind (fun x ->
                    T.TryCreate (x.Info, x.Location)
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (Address a) = FullInfo.Encoder a
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4

    and AddressJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f
    let info = T.Value >> fun x -> x.Info
    let location = T.Value >> fun x -> x.Location

[<RequireQualifiedAccess>]
module AirportCode =

    let private normalize (value: string) = value.RemoveWhitespaces ()
    let private isValid value =
        let value = normalize value
        not (String.IsNullOrEmpty value)
        && value.Length = 3

    [<Newtonsoft.Json.JsonConverter(typeof<AirportCodeJsonConverter>)>]
    type T = private AirportCode of string
        with
            static member CanCreate = isValid
            static member TryCreate (value) =
                let value = normalize value
                if T.CanCreate value
                then Ok <| AirportCode value
                else Error [Error.InvalidAirportCode]
            static member Create (value) =
                match T.TryCreate value with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (AirportCode s) = s
            override x.ToString () =
                let (AirportCode value) = x
                value

            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (AirportCode s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and AirportCodeJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module Airport =

    type Info =
        { Code: AirportCode.T
          Name: string
          Terminal: string option }

    type FullInfo =
        { Info: Info
          Location: Location.T }
        with
            static member Decoder =
                Decode.map4
                    (fun code name terminal location ->
                        { Info =
                            { Code = code
                              Name = name
                              Terminal = terminal }
                          Location = location })
                    (Decode.field "code" AirportCode.T.Decoder)
                    (Decode.field "name" Decode.string)
                    (Decode.optional "terminal" Decode.string)
                    (Decode.field "location" Location.T.Decoder)
            static member Encoder (instance: FullInfo) =
                Encode.object
                    [ "code", AirportCode.T.Encoder instance.Info.Code
                      "name", Encode.string instance.Info.Name
                      "terminal", (Encode.option Encode.string) instance.Info.Terminal
                      "location", Location.T.Encoder instance.Location ]
            static member FromJson json = Decode.fromString FullInfo.Decoder json
            static member ToJson instance = FullInfo.Encoder instance |> Encode.toString 4

    let private validate airport =
        [
            if String.IsNullOrEmpty (airport.Name) then
                yield Error.InvalidAirportName
            if Option.isSome airport.Terminal && String.IsNullOrWhiteSpace (airport.Terminal.Value) then
                yield Error.InvalidAirportTerminal
        ]

    let private normalizeName info =
        if isNull (info.Name) then info
        else
            { info with Name = info.Name.Trim () }

    [<Newtonsoft.Json.JsonConverter(typeof<AirportJsonConverter>)>]
    type T = private Airport of FullInfo
        with
            static member CanCreate = validate >> List.isEmpty
            static member TryCreate (airport, location) =
                let airport = normalizeName airport
                match validate airport with
                | [] -> Ok <| Airport { Info=airport; Location=location }
                | errors -> Error errors
            static member Create (airport, location) =
                match T.TryCreate (airport, location) with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (Airport a) = a
            static member Decoder path token =
                FullInfo.Decoder path token
                |> Result.bind (fun x ->
                    T.TryCreate (x.Info, x.Location)
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (Airport a) = FullInfo.Encoder a
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4
    and AirportJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f
    let info = T.Value >> fun x -> x.Info
    let code = T.Value >> fun x -> x.Info.Code
    let location = T.Value >> fun x -> x.Location

[<RequireQualifiedAccess>]
module FlightNumber =

    type Info =
        { Airline: string
          FlightNumber: string }
        with
            static member Decoder =
                Decode.map2
                    (fun airline flightNumber ->
                        { Airline = airline
                          FlightNumber = flightNumber })
                    (Decode.field "airline" Decode.string)
                    (Decode.field "flightNumber" Decode.string)
            static member Encoder (instance: Info) =
                Encode.object
                    [ "airline", Encode.string instance.Airline
                      "flightNumber", Encode.string instance.FlightNumber ]
            static member FromJson json = Decode.fromString Info.Decoder json
            static member ToJson instance = Info.Encoder instance |> Encode.toString 4

    let private validate { Airline=airline; FlightNumber=flightNumber } =
        [
            if (String.IsNullOrEmpty (airline)) || airline.Length <> 2 then
                yield Error.InvalidAirlineCode
            if String.IsNullOrEmpty (flightNumber) then
                yield Error.InvalidFlightNumber
        ]

    let private normalize info =
        { info with
            Airline = info.Airline.RemoveWhitespaces ()
            FlightNumber = info.FlightNumber.RemoveWhitespaces () }

    [<Newtonsoft.Json.JsonConverter(typeof<FlightNumberJsonConverter>)>]
    type T = private FlightNumber of Info
        with
            static member CanCreate = validate >> List.isEmpty
            static member TryCreate (flightNumber) =
                let flightNumber = normalize flightNumber
                match validate flightNumber with
                | [] -> Ok <| FlightNumber flightNumber
                | errors -> Error errors
            static member Create (flightNumber) =
                match T.TryCreate flightNumber with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (FlightNumber i) = i
            static member Decoder path token =
                Info.Decoder path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (FlightNumber i) = Info.Encoder i
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4

    and FlightNumberJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module AircraftTailNumber =

    let private isValid = not << String.IsNullOrWhiteSpace

    [<Newtonsoft.Json.JsonConverter(typeof<AircraftTailNumberJsonConverter>)>]
    type T = private AircraftTailNumber of string
        with
            static member CanCreate = isValid
            static member TryCreate (aircraftTailNumber) =
                if T.CanCreate aircraftTailNumber
                then Ok <| AircraftTailNumber aircraftTailNumber
                else Error [Error.InvalidAircraftTailNumber]
            static member Create (aircraftTailNumber) =
                match T.TryCreate aircraftTailNumber with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (AircraftTailNumber s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (AircraftTailNumber s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and AircraftTailNumberJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module FlightTimeKind =
    let [<Literal>] Scheduled = "SCHEDULED"
    let [<Literal>] Estimated = "ESTIMATED"
    let [<Literal>] Actual = "ACTUAL"

[<Newtonsoft.Json.JsonConverter(typeof<FlightTimeJsonConverter>)>]
type FlightTime =
    | Scheduled of DateTimeOffset
    | Estimated of DateTimeOffset
    | Actual of DateTimeOffset
    with
        member x.Value =
            match x with
            | Scheduled value
            | Estimated value
            | Actual value ->
                value

        static member Decoder =
            Decode.field "kind" Decode.string
            |> Decode.andThen (function
                | FlightTimeKind.Scheduled ->
                    Decode.map
                        Scheduled
                        (Decode.field "time" Decode.datetimeOffset)
                | FlightTimeKind.Estimated ->
                    Decode.map
                        Estimated
                        (Decode.field "time" Decode.datetimeOffset)
                | FlightTimeKind.Actual ->
                    Decode.map
                        Actual
                        (Decode.field "time" Decode.datetimeOffset)
                | unknown ->
                    "Trying to decode kind, but kind " + unknown + " is not supported"
                    |> Decode.fail
            )
        static member Encoder = function
            | Scheduled x ->
                Encode.object
                    [ "kind", Encode.string FlightTimeKind.Scheduled
                      "time", Encode.datetimeOffset x ]
            | Estimated x ->
                Encode.object
                    [ "kind", Encode.string FlightTimeKind.Estimated
                      "time", Encode.datetimeOffset x ]
            | Actual x ->
                Encode.object
                    [ "kind", Encode.string FlightTimeKind.Actual
                      "time", Encode.datetimeOffset x ]
        static member FromJson json = Decode.fromString FlightTime.Decoder json
        static member ToJson instance = FlightTime.Encoder instance |> Encode.toString 4

and FlightTimeJsonConverter () =
    inherit Json.ValueTypeJsonConverter<FlightTime> (FlightTime.ToJson, FlightTime.Decoder "")

[<RequireQualifiedAccess>]
module FlightStatusKind =
    let [<Literal>] Scheduled = "SCHEDULED"
    let [<Literal>] Departed = "DEPARTED"
    let [<Literal>] InAir = "IN_AIR"
    let [<Literal>] Landed = "LANDED"
    let [<Literal>] Arrived = "ARRIVED"
    let [<Literal>] Expected = "EXPECTED"
    let [<Literal>] Delayed = "DELAYED"
    let [<Literal>] Canceled = "CANCELED"
    let [<Literal>] Unknown = "UNKNOWN"

[<Newtonsoft.Json.JsonConverter(typeof<FlightStatusJsonConverter>)>]
type FlightStatus =
    | Scheduled
    | Departed
    | InAir
    | Landed
    | Arrived
    | Expected
    | Delayed
    | Canceled
    | Unknown
    with
        member x.AsString =
            match x with
            | Scheduled -> FlightStatusKind.Scheduled
            | Departed -> FlightStatusKind.Departed
            | InAir -> FlightStatusKind.InAir
            | Landed -> FlightStatusKind.Landed
            | Arrived -> FlightStatusKind.Arrived
            | Expected -> FlightStatusKind.Expected
            | Delayed -> FlightStatusKind.Delayed
            | Canceled -> FlightStatusKind.Canceled
            | Unknown -> FlightStatusKind.Unknown
        static member Decoder path token =
            Decode.string path token
            |> Result.bind (fun x ->
                match x with
                | FlightStatusKind.Scheduled ->
                    Decode.succeed Scheduled x token
                | FlightStatusKind.Departed ->
                    Decode.succeed Departed x token
                | FlightStatusKind.InAir ->
                    Decode.succeed InAir x token
                | FlightStatusKind.Landed ->
                    Decode.succeed Landed x token
                | FlightStatusKind.Arrived ->
                    Decode.succeed Arrived x token
                | FlightStatusKind.Expected ->
                    Decode.succeed Expected x token
                | FlightStatusKind.Delayed ->
                    Decode.succeed Delayed x token
                | FlightStatusKind.Canceled ->
                    Decode.succeed Canceled x token
                | FlightStatusKind.Unknown ->
                    Decode.succeed Unknown x token
                | unknown ->
                    let msg = "Trying to decode flight status, but flight status " + unknown + " is not supported"
                    Decode.fail msg "" token
            )
        static member Encoder (x: FlightStatus) = Encode.string x.AsString
        static member FromJson json = Decode.fromString FlightStatus.Decoder json
        static member ToJson instance = FlightStatus.Encoder instance |> Encode.toString 4

and FlightStatusJsonConverter () =
    inherit Json.ValueTypeJsonConverter<FlightStatus> (FlightStatus.ToJson, FlightStatus.Decoder "")

[<Newtonsoft.Json.JsonConverter(typeof<FlightTimeJsonConverter>)>]
type FlightInfo =
    { ArrivingAirport: Airport.T
      DepartingAirport: Airport.T
      DepartureTime: FlightTime
      ArrivalTime: FlightTime
      Status: FlightStatus }
    with
        static member Decoder =
            Decode.map5
                (fun arrivingAirport departingAirport departureTime arrivalTime status ->
                    { ArrivingAirport = arrivingAirport
                      DepartingAirport = departingAirport
                      DepartureTime = departureTime
                      ArrivalTime = arrivalTime
                      Status = status }
                )
                (Decode.field "arrivingAirport" Airport.T.Decoder)
                (Decode.field "departingAirport" Airport.T.Decoder)
                (Decode.field "departureTime" FlightTime.Decoder)
                (Decode.field "arrivalTime" FlightTime.Decoder)
                (Decode.field "status" FlightStatus.Decoder)
        static member Encoder (instance: FlightInfo) =
            Encode.object
                [ "arrivingAirport", Airport.T.Encoder instance.ArrivingAirport
                  "departingAirport", Airport.T.Encoder instance.DepartingAirport
                  "departureTime", FlightTime.Encoder instance.DepartureTime
                  "arrivalTime", FlightTime.Encoder instance.ArrivalTime
                  "status", FlightStatus.Encoder instance.Status ]
        static member FromJson json = Decode.fromString FlightInfo.Decoder json
        static member ToJson instance = FlightInfo.Encoder instance |> Encode.toString 4

and FlightInfoJsonConverter () =
    inherit Json.ValueTypeJsonConverter<FlightInfo> (FlightInfo.ToJson, FlightInfo.Decoder "")

[<RequireQualifiedAccess>]
module FlightDepartureDate =
    open System.Globalization

    let private tryParseDepartureDate date =
        let result = DateTimeOffset.TryParseExact (date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None)
        match result with
        | (true, _) -> Ok ()
        | _ -> Error [Error.InvalidFlightDepartureDate]

    let private isValid value =
        Result.isOk (tryParseDepartureDate value)

    let private asString (year, month, day) = sprintf "%04i%02i%02i" year month day

    [<Newtonsoft.Json.JsonConverter(typeof<FlightDepartureDateJsonConverter>)>]
    type T = private FlightDepartureDate of string
        with
            static member CanCreate = isValid
            static member TryCreate (value: string) =
                if T.CanCreate value then
                    tryParseDepartureDate value
                    |> Result.map (fun _ -> FlightDepartureDate value)
                else Error [Error.InvalidFlightDepartureDate]
            static member TryCreate (year, month, day) =
                let value = asString (year, month, day)
                if T.CanCreate value
                then Ok <| FlightDepartureDate value
                else Error [Error.InvalidFlightDepartureDate]
            static member Create (value: string) =
                match T.TryCreate value with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Create (year, month, day) =
                match T.TryCreate (year, month, day) with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (FlightDepartureDate value) = value
            override x.ToString () =
                let (FlightDepartureDate value) = x
                value

            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (FlightDepartureDate s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and FlightDepartureDateJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate (value: string) = T.TryCreate value
    let create (value: string) = T.Create value
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module ServiceClassId =

    let private isValid value =
        not (isNull value)
        && Regex.IsMatch (value, "^[a-zA-Z][0-9a-zA-Z_.-]*[0-9a-zA-Z]$")

    [<Newtonsoft.Json.JsonConverter(typeof<ServiceClassIdJsonConverter>)>]
    type T = private ServiceClassId of string
        with
            static member CanCreate = isValid
            static member TryCreate (serviceClassId) =
                if T.CanCreate serviceClassId
                then Ok <| ServiceClassId serviceClassId
                else Error [Error.InvalidServiceClassId]
            static member Create (serviceClassId) =
                match T.TryCreate serviceClassId with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (ServiceClassId s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (ServiceClassId s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and ServiceClassIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module ServiceClassName =

    let private isValid = not << String.IsNullOrWhiteSpace

    [<Newtonsoft.Json.JsonConverter(typeof<ServiceClassNameJsonConverter>)>]
    type T = private ServiceClassName of string
        with
            static member CanCreate = isValid
            static member TryCreate (serviceClassName) =
                if T.CanCreate serviceClassName
                then Ok <| ServiceClassName serviceClassName
                else Error [Error.InvalidServiceClassName]
            static member Create (serviceClassName) =
                match T.TryCreate serviceClassName with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (ServiceClassName s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (ServiceClassName s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and ServiceClassNameJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module ServiceClassDescription =

    let private isValid = not << String.IsNullOrWhiteSpace

    [<Newtonsoft.Json.JsonConverter(typeof<ServiceClassDescriptionJsonConverter>)>]
    type T = private ServiceClassDescription of string
        with
            static member CanCreate = isValid
            static member TryCreate (serviceClassDescription) =
                if T.CanCreate serviceClassDescription
                then Ok <| ServiceClassDescription serviceClassDescription
                else Error [Error.InvalidServiceClassDescription]
            static member Create (serviceClassDescription) =
                match T.TryCreate serviceClassDescription with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (ServiceClassDescription s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (ServiceClassDescription s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and ServiceClassDescriptionJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module ServiceClassPaxCapacity =

    let private isValid value = value >= 0

    [<Newtonsoft.Json.JsonConverter(typeof<ServiceClassPaxCapacityJsonConverter>)>]
    type T = private ServiceClassPaxCapacity of int
        with
            static member CanCreate = isValid
            static member TryCreate (serviceClassPaxCapacity) =
                if T.CanCreate serviceClassPaxCapacity
                then Ok <| ServiceClassPaxCapacity serviceClassPaxCapacity
                else Error [Error.InvalidServiceClassPaxCapacity]
            static member Create (serviceClassDescription) =
                match T.TryCreate serviceClassDescription with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (ServiceClassPaxCapacity s) = s
            static member Decoder path token =
                Decode.int path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (ServiceClassPaxCapacity s) = Encode.int s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and ServiceClassPaxCapacityJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module ServiceClassLuggageCapacity =

    let private isValid value = value >= 0

    [<Newtonsoft.Json.JsonConverter(typeof<ServiceClassLuggageCapacityJsonConverter>)>]
    type T = private ServiceClassLuggageCapacity of int
        with
            static member CanCreate = isValid
            static member TryCreate (serviceClassLuggageCapacity) =
                if T.CanCreate serviceClassLuggageCapacity
                then Ok <| ServiceClassLuggageCapacity serviceClassLuggageCapacity
                else Error [Error.InvalidServiceClassLuggageCapacity]
            static member Create (serviceClassDescription) =
                match T.TryCreate serviceClassDescription with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (ServiceClassLuggageCapacity s) = s
            static member Decoder path token =
                Decode.int path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (ServiceClassLuggageCapacity s) = Encode.int s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and ServiceClassLuggageCapacityJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module ResourceId =

    let private asString = stringf "N"

    let private isValid value =
        Guid.TryParse value
        |> fun (result, _) -> result

    [<Newtonsoft.Json.JsonConverter(typeof<ResourceIdJsonConverter>)>]
    type T = private ResourceId of Guid
        with
            static member CanCreate = isValid
            static member TryCreate (id) =
                if T.CanCreate id
                then Ok (ResourceId (Guid.Parse id))
                else Error [Error.InvalidResourceId]
            static member Create (id) =
                match T.TryCreate id with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member NewId () =
                ResourceId (Guid.NewGuid ())
            static member Value (ResourceId s) = asString s
            override x.ToString () =
                let (ResourceId value) = x
                asString value
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (ResourceId s) =
                Encode.string (asString s)
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and ResourceIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f
    let newId = T.NewId
    let fromGuid = ResourceId
    let asGuid (ResourceId value) = value
    let empty = ResourceId Guid.Empty

[<RequireQualifiedAccess>]
module OfferId =

    let private asString = stringf "N"

    let private isValid value =
        Guid.TryParse value
        |> fun (result, _) -> result

    [<Newtonsoft.Json.JsonConverter(typeof<OfferIdJsonConverter>)>]
    type T = private OfferId of Guid
        with
            static member CanCreate = isValid
            static member TryCreate (id) =
                if T.CanCreate id
                then Ok (OfferId (Guid.Parse id))
                else Error [Error.InvalidOfferId]
            static member Create (id) =
                match T.TryCreate id with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member NewId () =
                OfferId (Guid.NewGuid ())
            static member Value (OfferId s) = asString s
            override x.ToString () =
                let (OfferId value) = x
                asString value
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (OfferId s) =
                Encode.string (asString s)
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and OfferIdJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f
    let newId = T.NewId
    let fromGuid = OfferId
    let asGuid (OfferId value) = value
    let empty = OfferId Guid.Empty

[<RequireQualifiedAccess>]
module BookingReference =

    let private isValid value =
        not (isNull value)
        && Regex.IsMatch (value, "^[0-9A-Z]{6,10}$")

    [<Newtonsoft.Json.JsonConverter(typeof<BookingReferenceJsonConverter>)>]
    type T = private BookingReference of string
        with
            static member CanCreate = isValid
            static member TryCreate (bookingReference) =
                if T.CanCreate bookingReference
                then Ok <| BookingReference bookingReference
                else Error [Error.InvalidBookingReference]
            static member Create (bookingReference) =
                match T.TryCreate bookingReference with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (BookingReference s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (BookingReference s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and BookingReferenceJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value

[<RequireQualifiedAccess>]
module CustomerBookingNote =

    let private isValid x = not (isNull x)

    [<Newtonsoft.Json.JsonConverter(typeof<CustomerBookingNoteJsonConverter>)>]
    type T = private CustomerBookingNote of string
        with
            static member CanCreate = isValid
            static member TryCreate (note) =
                if T.CanCreate note
                then Ok <| CustomerBookingNote note
                else Error [Error.InvalidCustomerBookingNote]
            static member Create (note) =
                match T.TryCreate note with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (CustomerBookingNote s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (CustomerBookingNote s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and CustomerBookingNoteJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value

[<RequireQualifiedAccess>]
module VehicleMake =

    let private isValid = not << String.IsNullOrWhiteSpace

    [<Newtonsoft.Json.JsonConverter(typeof<VehicleMakeJsonConverter>)>]
    type T = private VehicleMake of string
        with
            static member CanCreate = isValid
            static member TryCreate (vehicleMake) =
                if T.CanCreate vehicleMake
                then Ok <| VehicleMake vehicleMake
                else Error [Error.InvalidVehicleMake]
            static member Create (vehicleMake) =
                match T.TryCreate vehicleMake with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (VehicleMake s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (VehicleMake s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and VehicleMakeJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module VehicleModel =

    let private isValid = not << String.IsNullOrWhiteSpace

    [<Newtonsoft.Json.JsonConverter(typeof<VehicleModelJsonConverter>)>]
    type T = private VehicleModel of string
        with
            static member CanCreate = isValid
            static member TryCreate (vehicleModel) =
                if T.CanCreate vehicleModel
                then Ok <| VehicleModel vehicleModel
                else Error [Error.InvalidVehicleModel]
            static member Create (vehicleMake) =
                match T.TryCreate vehicleMake with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (VehicleModel s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (VehicleModel s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and VehicleModelJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module VehicleLicensePlate =

    let private isValid = not << String.IsNullOrWhiteSpace

    [<Newtonsoft.Json.JsonConverter(typeof<VehicleLicensePlateJsonConverter>)>]
    type T = private VehicleLicensePlate of string
        with
            static member CanCreate = isValid
            static member TryCreate (vehicleLicensePlate) =
                if T.CanCreate vehicleLicensePlate
                then Ok <| VehicleLicensePlate vehicleLicensePlate
                else Error [Error.InvalidVehicleLicensePlate]
            static member Create (vehicleMake) =
                match T.TryCreate vehicleMake with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (VehicleLicensePlate s) = s
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (VehicleLicensePlate s) = Encode.string s
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and VehicleLicensePlateJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let value = T.Value
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module VehicleColor =
    open System.Drawing

    let private tryCreateColor color =
        try
            Some (ColorTranslator.FromHtml (color))
        with
        | _ -> None

    [<CustomEquality; NoComparison>]
    [<Newtonsoft.Json.JsonConverter(typeof<VehicleColorJsonConverter>)>]
    type T = private VehicleColor of Color
        with
            static member CanCreate vehicleColor = Option.isSome (tryCreateColor vehicleColor)
            static member TryCreate (vehicleColor) =
                tryCreateColor vehicleColor
                |> Result.ofOption [Error.InvalidVehicleColor]
                |> Result.map VehicleColor
            static member Create (vehicleColor) =
                match T.TryCreate vehicleColor with
                | Ok s -> s
                | Error m ->
                    Error.toErrorDetails m.Head
                    |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
            static member Value (VehicleColor c) = c
            static member Empty = VehicleColor Color.Empty
            member x.AsHexString =
                T.Value x
                |> fun c ->
                    "#" + c.R.ToString("X2", null) + c.G.ToString("X2", null) + c.B.ToString("X2", null)
            override x.GetHashCode () = hash x
            override x.Equals (c) =
                match c with
                | :? T as c -> c.AsHexString = x.AsHexString
                | _ -> false
            static member Decoder path token =
                Decode.string path token
                |> Result.bind (fun x ->
                    T.TryCreate x
                    |> Result.mapError (DeserializationHelper.decoderError path)
                )
            static member Encoder (x: T) = Encode.string x.AsHexString
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 0

    and VehicleColorJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let empty = T.Empty
    let value = T.Value
    let asHexString (color: T) = color.AsHexString
    let apply f = T.Value >> f

[<RequireQualifiedAccess>]
module RoleKind =
    let [<Literal>] Operator = "OPERATOR"
    let [<Literal>] Administrator = "ADMINISTRATOR"

[<Newtonsoft.Json.JsonConverter(typeof<RoleJsonConverter>)>]
type Role =
    | Operator
    | Administrator
    with
        member x.AsString =
            match x with
            | Operator  -> RoleKind.Operator
            | Administrator -> RoleKind.Administrator
        static member FromString = function
            | RoleKind.Operator -> Operator
            | RoleKind.Administrator -> Administrator
            | unsupported -> failwithf "Role %s is not supported" unsupported
        static member Decoder path token =
            Decode.string path token
            |> Result.bind (fun x ->
                match x with
                | RoleKind.Operator ->
                    Decode.succeed Operator x token
                | RoleKind.Administrator ->
                    Decode.succeed Administrator x token
                | unknown ->
                    let msg = "Trying to decode role, but role type " + unknown + " is not supported"
                    Decode.fail msg path token
            )
        static member Encoder (x: Role) = Encode.string x.AsString
        static member FromJson json = Decode.fromString Role.Decoder json
        static member ToJson instance = Role.Encoder instance |> Encode.toString 4

and RoleJsonConverter () =
    inherit Json.ValueTypeJsonConverter<Role> (Role.ToJson, Role.Decoder "")
