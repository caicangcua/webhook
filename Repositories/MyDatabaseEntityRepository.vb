
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports FirstWebAPI

Public Class MyDatabaseEntityRepository
    Implements IMyDatabaseEntityRepository

#Region "Some dummy data"

    Private Shared InternalDataStore As List(Of MyDatabaseEntity)
    Shared Sub New()
        InternalDataStore = New List(Of MyDatabaseEntity)()
        For i As Integer = 0 To 9
            Dim id = i + 1
            InternalDataStore.Add(CreateMyDatabaseEntity(id))
        Next
    End Sub

    Private Shared Function CreateMyDatabaseEntity(id As Integer) As MyDatabaseEntity
        Dim model = New MyDatabaseEntity() With {
                .Id = id,
                .Description = String.Format("MyODataModel {0}", id),
                .Children = New List(Of MyDatabaseComplexType)()
        }
        FillComplexTypeCollection(model.Children, 2)
        Return model
    End Function

    Private Shared Sub FillComplexTypeCollection(parentCollection As ICollection(Of MyDatabaseComplexType), level As Integer)
        If level < 5 Then
            For i As Integer = 0 To (level Mod 2)
                Dim complexType = New MyDatabaseComplexType() With {
                        .Description = "MyDatabaseComplexType level={level} | i = {i}",
                        .Children = New List(Of MyDatabaseComplexType)()
                }
                FillComplexTypeCollection(complexType.Children, level + 1)
                parentCollection.Add(complexType)
            Next
        End If
    End Sub

#End Region

    Public Function All() As IEnumerable(Of MyDatabaseEntity) Implements IMyDatabaseEntityRepository.All
        Return InternalDataStore
    End Function

    Public Function Find(id As Integer) As MyDatabaseEntity Implements IMyDatabaseEntityRepository.Find
        Return InternalDataStore.FirstOrDefault(Function(x) x.Id = id)
    End Function

End Class

Public Interface IMyDatabaseEntityRepository
    Function All() As IEnumerable(Of MyDatabaseEntity)
    Function Find(id As Integer) As MyDatabaseEntity
End Interface