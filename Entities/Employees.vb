
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Services.Common
Imports System.Linq
Imports System.Web

<DataServiceKey("ID")>
Public Class Employee
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
    Public Property Surname() As String
        Get
            Return m_Surname
        End Get
        Set(value As String)
            m_Surname = value
        End Set
    End Property
    Private m_Surname As String
    Public Property Password() As String
        Get
            Return m_Password
        End Get
        Set(value As String)
            m_Password = value
        End Set
    End Property
    Private m_Password As String
    Public Property PhotoUrl() As String
        Get
            Return m_PhotoUrl
        End Get
        Set(value As String)
            m_PhotoUrl = value
        End Set
    End Property
    Private m_PhotoUrl As String

    Public ReadOnly Property Orders() As ICollection(Of Order)
        Get
            Return SessionConnectionProvider.GetSessionData().Orders.Where(Function(item) item.EmployeeID = ID).ToList()
        End Get
    End Property

    Public Sub New()
        Me.ID = Guid.NewGuid()
    End Sub
End Class