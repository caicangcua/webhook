
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Services.Common
Imports System.Linq
Imports System.Web

<DataServiceKey("ID")>
Public Class Brand
    <Key>
    Public Property ID() As Guid
        Get
            Return m_ID
        End Get
        Set(value As Guid)
            m_ID = value
        End Set
    End Property
    Private m_ID As Guid
    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(value As String)
            m_Name = value
        End Set
    End Property
    Private m_Name As String
    Public Property ImageUrl() As String
        Get
            Return m_ImageUrl
        End Get
        Set(value As String)
            m_ImageUrl = value
        End Set
    End Property
    Private m_ImageUrl As String

    Public ReadOnly Property Products() As ICollection(Of Product)
        Get
            Return SessionConnectionProvider.GetSessionData().Products.Where(Function(item) item.BrandID = ID).ToList()
        End Get
    End Property

    Public Sub New()
        Me.ID = Guid.NewGuid()
    End Sub
End Class