Imports System.Net
Imports System.Web.Http
Imports System.Web.OData

Public Class MyODataModelController
    Inherits ODataController
    Private ReadOnly Property MyODataModelService() As IMyODataModelService

    ' Delete this constructor and user dependency injection instead
    ' For simplicity, i omitted any DI container, use the one you like :)
    Public Sub New()
        Me.New(New MyODataModelService(New MyDatabaseEntityRepository(), New MyODataModelMapper()))
    End Sub

    Public Sub New(myODataModelService__1 As IMyODataModelService)
        If myODataModelService__1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(myODataModelService__1))
        End If
        MyODataModelService = myODataModelService__1
    End Sub

    <EnableQuery>
    Public Function [Get]() As IHttpActionResult
        Return Ok(MyODataModelService.All())
    End Function

    <EnableQuery>
    Public Function [Get](<FromODataUri> key As Integer) As IHttpActionResult
        Dim record = MyODataModelService.Find(key)
        If record Is Nothing Then
            Return NotFound()
        End If
        Return Ok(record)
    End Function
End Class