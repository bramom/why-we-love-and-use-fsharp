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
                |> Gen.map Locatzion.create
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