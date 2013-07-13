Public Class TestClass
    Property Ids() As List(Of Integer) =
        New List(Of Integer) From {
            1, 2, 3, 4
        }

    Public Sub New(Optional ByVal i As Integer? = 0)
        If i.HasValue Then
            Ids.Add(i)
        End If

        _Ids.Add(5)

        Dim test = {"a", 5, Nothing}
        Dim moreIds = {5, 6, 7}
        Ids.AddRange(moreIds)

        Dim o As IEnumerable(Of Object) = {""}
    End Sub
End Class
