
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Services.Common
Imports System.Linq
Imports System.Web

<Flags()>
Enum NotificationType As Byte
    PackageShip = 8
    PackageArrive = 4
    WeekDeals = 2
    PartnerDeals = 1
End Enum

Public Class AddressInfo
    Public Property Country() As String
        Get
            Return m_Country
        End Get
        Set(value As String)
            m_Country = value
        End Set
    End Property
    Private m_Country As String
    Public Property State() As String
        Get
            Return m_State
        End Get
        Set(value As String)
            m_State = value
        End Set
    End Property
    Private m_State As String
    Public Property City() As String
        Get
            Return m_City
        End Get
        Set(value As String)
            m_City = value
        End Set
    End Property
    Private m_City As String
    Public Property Postcode() As Integer
        Get
            Return m_Postcode
        End Get
        Set(value As Integer)
            m_Postcode = value
        End Set
    End Property
    Private m_Postcode As Integer
    Public Property Address() As String
        Get
            Return m_Address
        End Get
        Set(value As String)
            m_Address = value
        End Set
    End Property
    Private m_Address As String
    Public Property Phone() As String
        Get
            Return m_Phone
        End Get
        Set(value As String)
            m_Phone = value
        End Set
    End Property
    Private m_Phone As String
End Class

<DataServiceKey("ID")>
Public Class Customer
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
    Public Property Email() As String
        Get
            Return m_Email
        End Get
        Set(value As String)
            m_Email = value
        End Set
    End Property
    Private m_Email As String
    Public Property Password() As String
        Get
            Return m_Password
        End Get
        Set(value As String)
            m_Password = value
        End Set
    End Property
    Private m_Password As String
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
    Public Property PhotoUrl() As String
        Get
            Return m_PhotoUrl
        End Get
        Set(value As String)
            m_PhotoUrl = value
        End Set
    End Property
    Private m_PhotoUrl As String
    Public Property BillingAddress() As AddressInfo
        Get
            Return m_BillingAddress
        End Get
        Set(value As AddressInfo)
            m_BillingAddress = value
        End Set
    End Property
    Private m_BillingAddress As AddressInfo
    Public Property ShippingAddress() As AddressInfo
        Get
            Return m_ShippingAddress
        End Get
        Set(value As AddressInfo)
            m_ShippingAddress = value
        End Set
    End Property
    Private m_ShippingAddress As AddressInfo

    Private m_notifications As NotificationType

    Public Property Notifications() As Byte
        Get
            Return CByte(m_notifications)
        End Get
        Set(value As Byte)
            If value <> CByte(m_notifications) Then
                m_notifications = CType(value, NotificationType)
            End If
        End Set
    End Property

    Public ReadOnly Property Orders() As ICollection(Of Order)
        Get
            Return SessionConnectionProvider.GetSessionData().Orders.Where(Function(item) item.CustomerID = ID).ToList()
        End Get
    End Property

    Public Sub New()
        Me.ID = Guid.NewGuid()
    End Sub
End Class