namespace OpenLimo.Core.ValueTypes

[<RequireQualifiedAccess>]
module Location =
    open OpenLimo.Core.Library
    open OpenLimo.Core.Library.Result
    open OpenLimo.Core.Library.Measures
    open OpenLimo.Core.Serialization

    [<RequireQualifiedAccess>]
    module Latitude =

        let private isValid lat =
            lat > -90.m<deg> && lat < 90.m<deg>

        [<Newtonsoft.Json.JsonConverter(typeof<LatitudeJsonConverter>)>]
        type T = private Latitude of decimal<deg>
            with
                static member CanCreate = isValid
                static member TryCreate (lat) =
                    if T.CanCreate lat
                    then Ok (Latitude lat)
                    else Error [Error.InvalidLatitude]
                static member Create (lat) =
                    match T.TryCreate lat with
                    | Ok s -> s
                    | Error m ->
                        Error.toErrorDetails m.Head
                        |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
                static member Value (Latitude s) = s
                static member Decoder path token =
                    Decode.decimal path token
                    |> Result.bind (fun x ->
                        T.TryCreate (x * 1m<deg>)
                        |> Result.mapError (DeserializationHelper.decoderError path)
                    )
                static member Encoder (Latitude s) = Encode.decimal (decimal s)
                static member FromJson json = Decode.fromString T.Decoder json
                static member ToJson instance = T.Encoder instance |> Encode.toString 0

        and LatitudeJsonConverter () =
            inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

        let canCreate = T.CanCreate
        let tryCreate = T.TryCreate
        let create = T.Create
        let value = T.Value
        let apply f = T.Value >> f

    [<RequireQualifiedAccess>]
    module Longitude =

        let private isValid lng =
            lng > -180m<deg> && lng < 180m<deg>

        [<Newtonsoft.Json.JsonConverter(typeof<LongitudeJsonConverter>)>]
        type T = private Longitude of decimal<deg>
            with
                static member CanCreate = isValid
                static member TryCreate (lat) =
                    if T.CanCreate lat
                    then Ok (Longitude lat)
                    else Error [Error.InvalidLongitude]
                static member Create (lat) =
                    match T.TryCreate lat with
                    | Ok s -> s
                    | Error m ->
                        Error.toErrorDetails m.Head
                        |> fun e -> invalidArg (e.TargetAsString "") e.Message.AsString
                static member Value (Longitude s) = s
                static member Decoder path token =
                    Decode.decimal path token
                    |> Result.bind (fun x ->
                        T.TryCreate (x * 1m<deg>)
                        |> Result.mapError (DeserializationHelper.decoderError path)
                    )
                static member Encoder (Longitude s) = Encode.decimal (decimal s)
                static member FromJson json = Decode.fromString T.Decoder json
                static member ToJson instance = T.Encoder instance |> Encode.toString 4

        and LongitudeJsonConverter () =
            inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

        let canCreate = T.CanCreate
        let tryCreate = T.TryCreate
        let create = T.Create
        let value = T.Value
        let apply f = T.Value >> f

    type Info =
        { Latitude: Latitude.T
          Longitude: Longitude.T }

    let isValid (lat, lng) =
        Latitude.canCreate lat && Longitude.canCreate lng

    [<Newtonsoft.Json.JsonConverter(typeof<LocationJsonConverter>)>]
    type T = private Location of Info
        with
            static member CanCreate = isValid
            static member TryCreate (lat, lng) =
                lift2
                    (fun lat lng ->
                        Location
                            { Latitude = lat
                              Longitude = lng })
                    (Latitude.tryCreate lat)
                    (Longitude.tryCreate lng)
            static member Create (lat, lng) =
                { Latitude = Latitude.create lat
                  Longitude = Longitude.create lng }
                |> Location
            override x.ToString () =
                let (Location info) = x
                sprintf "%f,%f"
                    (Latitude.apply decimal info.Latitude)
                    (Longitude.apply decimal info.Longitude)
            static member Decoder =
                Decode.map2
                    (fun latitude longitude ->
                        Location
                            { Latitude = latitude
                              Longitude = longitude }
                    )
                    (Decode.field "latitude" Latitude.T.Decoder)
                    (Decode.field "longitude" Longitude.T.Decoder)
            static member Encoder (Location info) =
                Encode.object
                    [ "latitude", Latitude.T.Encoder info.Latitude
                      "longitude", Longitude.T.Encoder info.Longitude ]
            static member FromJson json = Decode.fromString T.Decoder json
            static member ToJson instance = T.Encoder instance |> Encode.toString 4

    and LocationJsonConverter () =
        inherit Json.ValueTypeJsonConverter<T> (T.ToJson, T.Decoder "")

    let canCreate = T.CanCreate
    let tryCreate = T.TryCreate
    let create = T.Create
    let latitude (Location x) =  x.Latitude
    let longitude (Location x) =  x.Longitude

    let private haversineDistance<[<Measure>] 'u> (R: float<'u>) (p1: T) (p2: T) =
        let sq x = x * x
        // take the sin of the half and square the result
        let sinSqHf (a: decimal<rad>) = (float >> System.Math.Sin >> sq) (a / 2m<rad>)
        let cos (a: decimal<deg>) = (float >> System.Math.Cos) (degToRad a / 1m<rad>)

        let p1Lat = (latitude >> Latitude.value) p1
        let p1Long = (longitude >> Longitude.value) p1
        let p2Lat = (latitude >> Latitude.value) p2
        let p2Long = (longitude >> Longitude.value) p2

        let dLat = p2Lat - p1Lat |> degToRad
        let dLon = p2Long - p1Long |> degToRad

        let a = sinSqHf dLat + cos p1Lat * cos p2Lat * sinSqHf dLon
        let c = 2.0 * System.Math.Atan2 (System.Math.Sqrt (a), System.Math.Sqrt (1.0 - a))

        R * c

    let aerialDistance = haversineDistance 6371000.<meter>
