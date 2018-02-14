
Imports System.Linq
Imports System.Web.Http
Imports System.Web.OData

Public Class ProductsController
    Inherits ODataController
    ReadOnly _db As New EntitiesModel()

    <EnableQuery(PageSize:=20)>
    Public Function [Get]() As IHttpActionResult
        Return Ok(_db.Products.AsQueryable())
    End Function

    Public Function [Get](<FromODataUri> key As Integer) As IHttpActionResult
        Return Ok(_db.Products.SingleOrDefault(Function(t) t.Id = key))
    End Function

    Protected Overrides Sub Dispose(disposing As Boolean)
        _db.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class