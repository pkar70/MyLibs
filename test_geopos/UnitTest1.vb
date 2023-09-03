Imports NUnit.Framework

Namespace test_geopos

    Public Class Tests

        <SetUp>
        Public Sub Setup()
        End Sub

        <Test>
        Public Sub GeokrakowCenter()
            Dim oGeo As pkar.BasicGeopos = pkar.BasicGeopos.GetKrakowCenter

            Assert.AreEqual(50.06138, oGeo.Latitude, 0.01, "Bad latitude of Krak�w's center")
            Assert.AreEqual(19.93833, oGeo.Longitude, 0.01, "Bad longitude of Krak�w's center")
        End Sub

        <Test>
        Public Sub GeokrakowAsQTH()
            Dim oGeo As pkar.BasicGeopos = pkar.BasicGeopos.GetKrakowCenter

            Assert.AreEqual("JO90xb", oGeo.ToQTH(3), "Bad QTH for Krak�w")
        End Sub

        <Test>
        Public Sub GeokrakowAsDMS()
            Dim oGeo As pkar.BasicGeopos = pkar.BasicGeopos.GetKrakowCenter

            Assert.AreEqual("50 19", oGeo.StringDM("%ad %od"), "Bad DMS lat degrees for Krak�w")
            Assert.AreEqual("50 4", oGeo.StringDM("%ad %am", 0), "Bad DMS lat minutes for Krak�w")
            Assert.AreEqual("50 3 40", oGeo.StringDM("%ad %am %as", 0), "Bad DMS lat seconds for Krak�w")

            Assert.AreEqual("19 56", oGeo.StringDM("%od %om", 0), "Bad DMS lon minutes for Krak�w")
            Assert.AreEqual("19 56 17", oGeo.StringDM("%od %om %os", 0), "Bad DMS lon seconds for Krak�w")

        End Sub

        <Test>
        Public Sub GeokrakowFromDMS()
            Dim oGeo As pkar.BasicGeopos = pkar.BasicGeopos.FromQTH("JO90xb")
            Dim oKrak As pkar.BasicGeopos = pkar.BasicGeopos.GetKrakowCenter

            Assert.LessOrEqual(oGeo.DistanceKmTo(oKrak), 25, "Too big distance between Krak�w and QTH LOC JO90xb")

        End Sub

    End Class

End Namespace