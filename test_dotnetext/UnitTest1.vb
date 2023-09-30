Imports NUnit.Framework
Imports pkar.DotNetExtensions

Namespace test_dotnetext

    Public Class Tests

        <SetUp>
        Public Sub Setup()
        End Sub

        <Test>
        Public Sub TestKaznContains()
            Assert.IsTrue("kaźń i żółtość będą".Contains("kaźń"))
        End Sub

        <Test>
        Public Sub TestKaznEquals()
            Assert.IsTrue("kaźń i żóltość będą".EqualsCSAI("kazn i zoltosc beda"))
        End Sub

        <Test>
        Public Sub TestKaznEqualsCS()
            Assert.IsFalse("kaźń i żóltość będą".EqualsCSAI("kazn I zoltosc beda"))
        End Sub

        <Test>
        Public Sub TestKaznEquals12()
            Assert.IsFalse("kaźń i żóltość będą".Equals("kazn i zoltosc beda", StringComparison.InvariantCultureIgnoreCase))
        End Sub


        ' "ł" po DropAccents dalej jest ł!
        <Test>
        Public Sub TestKaznDrop()
            Assert.AreEqual("kaźń i żóltość będą".DropAccents, "kazn i zoltosc beda")
        End Sub


        <Test>
        Public Sub TestAe()
            Assert.AreEqual("æ".Normalize(Text.NormalizationForm.FormKD), "ae".Normalize(Text.NormalizationForm.FormKD))
        End Sub

        '<Test>
        'Public Sub TestKaznDropC()
        '    ' KC/C: kaźń i żółtość będą
        '    ' KD/D: kazn i zołtosc beda
        '    Assert.AreEqual(DropAccents("kaźń i żółtość będą"), "kazn i zoltosc beda")
        'End Sub

        'Public Function DropAccents(basestring As String) As String
        '    Dim FKD As String = basestring.Normalize(Text.NormalizationForm.FormKD)
        '    Dim sRet As String = ""

        '    For Each cTmp As Char In FKD
        '        If AscW(cTmp) >= &H2B0 AndAlso AscW(cTmp) < &H36F Then
        '            ' 02B0 - 02FF	Spacing Modifier Letters
        '            ' 0300 - 036F	Combining Diacritical Marks
        '        ElseIf AscW(cTmp) >= &H1AB0 AndAlso AscW(cTmp) < &H1B00 Then
        '            ' 1AB0 - 1AFF	Combining Diacritical Marks Extended
        '        ElseIf AscW(cTmp) >= &H1DC0 AndAlso AscW(cTmp) < &H1E00 Then
        '            ' 1DC0 - 1DFF	Combining Diacritical Marks Supplement
        '        ElseIf AscW(cTmp) >= &H20D0 AndAlso AscW(cTmp) < &H2100 Then
        '            ' 20D0 - 20FF	Combining Diacritical Marks for Symbols
        '        ElseIf AscW(cTmp) >= &HFE20 AndAlso AscW(cTmp) < &HFE30 Then
        '            ' FE20 - FE2F	Combining Half Marks
        '        Else
        '            sRet &= cTmp
        '        End If
        '    Next

        '    Return sRet
        'End Function
    End Class

End Namespace