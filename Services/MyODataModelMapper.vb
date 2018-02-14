Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports FirstWebAPI

Public Class MyODataModelMapper
    Implements IMyODataModelMapper

    Public Function Map(entity As MyDatabaseEntity) As MyODataModel Implements IMyODataModelMapper.Map
        Return New MyODataModel() With {
                .Id = entity.Id,
                .Description = entity.Description,
                .Children = MapChildren(entity)
            }
    End Function

    Private Function MapChildren(entity As MyDatabaseEntity) As ICollection(Of MyComplexType1)
        Return entity.Children.[Select](Function(dbComplexType) New MyComplexType1() With {
                .Description = dbComplexType.Description,
                .Children = MapChildren2(dbComplexType)
            }).ToList()
    End Function

    Private Function MapChildren1(parent As MyDatabaseComplexType) As ICollection(Of MyComplexType1)
        Return parent.Children.[Select](Function(dbComplexType) New MyComplexType1() With {
                .Description = dbComplexType.Description,
                .Children = MapChildren2(dbComplexType)
            }).ToList()
    End Function

    Private Function MapChildren2(parent As MyDatabaseComplexType) As ICollection(Of MyComplexType2)
        Return parent.Children.[Select](Function(dbComplexType) New MyComplexType2() With {
                .Description = dbComplexType.Description,
                .Children = MapChildren1(dbComplexType)
            }).ToList()
    End Function
End Class

Public Interface IMyODataModelMapper
    Function Map(entity As MyDatabaseEntity) As MyODataModel
End Interface