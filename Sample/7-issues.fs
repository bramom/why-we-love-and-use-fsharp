    let private findWithinRange (fromDate : DateTime) (toDate : DateTime)  =
        let statusAndRangeQuery (q : QueryContainerDescriptor<Ride>) =
            (q.Bool(fun b ->
                b.MustNot(
                    fun (q: QueryContainerDescriptor<Ride>) ->
                        q.Exists(fun m ->
                            m.Field(fun r -> r.PickupDateTimeUtc :> obj) :> _)
                ) :> _)
             ||| (q.DateRange(fun d ->
                        d.GreaterThanOrEquals(DateMath.FromString(fromDate.ToString("yyyy-MM-ddTHH:mm:ss")))
                            .Field(fun r -> r.PickupDateTimeUtc :> obj) :> _)
                  &&& q.DateRange(fun d ->
                        d.LessThanOrEquals(DateMath.FromString(toDate.ToString("yyyy-MM-ddTHH:mm:ss")))
                            .Field(fun r -> r.PickupDateTimeUtc :> obj) :> _)))
            &&& (q.Match(fun t ->
                    t.Query(RideStatus.Active.ToString())
                        .Field(fun r -> r.Status :> obj)
                        .MinimumShouldMatch(MinimumShouldMatch.Percentage(100.0)) :> _)
                 ||| q.Match(fun t ->
                        t.Query(RideStatus.Assigned.ToString())
                            .Field(fun r -> r.Status :> obj)
                            .MinimumShouldMatch(MinimumShouldMatch.Percentage(100.0)) :> _)
                 ||| q.Match(fun t ->
                        t.Query(RideStatus.Confirmed.ToString())
                            .Field(fun r -> r.Status :> obj)
                            .MinimumShouldMatch(MinimumShouldMatch.Percentage(100.0)) :> _)
                 ||| q.Match(fun t ->
                        t.Query(RideStatus.FarmedOut.ToString())
                            .Field(fun r -> r.Status :> obj)
                            .MinimumShouldMatch(MinimumShouldMatch.Percentage(100.0)) :> _)
                 ||| q.Match(fun t ->
                        t.Query(RideStatus.Preassigned.ToString())
                            .Field(fun r -> r.Status :> obj)
                            .MinimumShouldMatch(MinimumShouldMatch.Percentage(100.0)) :> _)
                 ||| q.Match(fun t ->
                        t.Query(RideStatus.Upcoming.ToString())
                            .Field(fun r -> r.Status :> obj)
                            .MinimumShouldMatch(MinimumShouldMatch.Percentage(100.0)) :> _))

        let searchReq (s : SearchDescriptor<Ride>) =
            s.Query(fun q -> statusAndRangeQuery q).Take(10000) :> ISearchRequest

        createClient () |> searchEs searchReq