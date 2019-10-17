namespace Core.ValueTypes.Tests

open Test.Helpers
open Xunit
open Swensen.Unquote

[<Trait (Trait.Category, Category.UnitTest)>]
module LocationTests =
    open FsCheck.Xunit
    open OpenLimo.Core.Serialization
    open OpenLimo.Core.ValueTypes
    open TestDataGen

    let serializeDeserialize<'a> (x: 'a) =
        let serialized = Json.serialize x
        Json.deserialize<'a> serialized

    [<Property(Arbitrary=[|typeof<LocationGen.Valid>|])>]
    let ``valid Locations`` (lat, lng) =
        let expected = Ok (Location.create (lat, lng))
        let result = Location.tryCreate (lat, lng)
        result =! expected

    [<Property(Arbitrary=[|typeof<LocationGen.Invalid>|])>]
    let ``invalid Locations`` (lat, lng, error) =
        let expected = Error error
        let result = Location.tryCreate (lat, lng)
        result =! expected

    [<Property(Arbitrary=[|typeof<LocationGen.Instances>|])>]
    let ``Location serialization roundtrip`` (value) =
        let result = serializeDeserialize<Location.T> value
        result =! value

    [<Property(Arbitrary=[|typeof<LocationGen.InvalidJsons>|])>]
    let ``Location deserializing invalid JSON`` (json) =
        Assert.Throws<Json.InvalidJsonValueException> (fun _ ->
            Json.deserialize<Location.T> json |> ignore
        ) |> ignore