Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports FirstWebAPI

Public Class MyODataModelService
    Implements IMyODataModelService

    Private ReadOnly Property MyDatabaseEntityRepository() As IMyDatabaseEntityRepository
    Private ReadOnly Property MyODataModelMapper() As IMyODataModelMapper

    Public Sub New(myDatabaseEntityRepository__1 As IMyDatabaseEntityRepository, myODataModelMapper__2 As IMyODataModelMapper)
        If myDatabaseEntityRepository__1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(myDatabaseEntityRepository__1))
        End If
        If myODataModelMapper__2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(myODataModelMapper__2))
        End If

        MyDatabaseEntityRepository = myDatabaseEntityRepository__1
        MyODataModelMapper = myODataModelMapper__2
    End Sub

    Public Function All() As IEnumerable(Of MyODataModel) Implements IMyODataModelService.All
        Return MyDatabaseEntityRepository.All().[Select](Function(dbModel) MyODataModelMapper.Map(dbModel))
    End Function

    Public Function Find(id As Integer) As MyODataModel Implements IMyODataModelService.Find
        Dim entity = MyDatabaseEntityRepository.All().FirstOrDefault(Function(x) x.Id = id)
        Return MyODataModelMapper.Map(entity)
    End Function

End Class

Public Interface IMyODataModelService
    Function All() As IEnumerable(Of MyODataModel)
    Function Find(id As Integer) As MyODataModel
End Interface