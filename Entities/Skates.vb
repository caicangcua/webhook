
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Services.Common
Imports System.Linq
Imports System.Web

<DataServiceKey("ID")>
Public Class Skate
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
    Public Property ImageUrl() As String
        Get
            Return m_ImageUrl
        End Get
        Set(value As String)
            m_ImageUrl = value
        End Set
    End Property
    Private m_ImageUrl As String

    Public Property DeckID() As Guid
        Get
            Return m_DeckID
        End Get
        Set(value As Guid)
            m_DeckID = value
        End Set
    End Property
    Private m_DeckID As Guid
    Public Property TruckID() As Guid
        Get
            Return m_TruckID
        End Get
        Set(value As Guid)
            m_TruckID = value
        End Set
    End Property
    Private m_TruckID As Guid
    Public Property WheelID() As Guid
        Get
            Return m_WheelID
        End Get
        Set(value As Guid)
            m_WheelID = value
        End Set
    End Property
    Private m_WheelID As Guid

    Public Property Deck() As Product
        Get
            Return SessionConnectionProvider.GetSessionData().Products.Where(Function(product) product.ID = DeckID).FirstOrDefault()
        End Get

        Set(value As Product)
            Me.DeckID = value.ID
        End Set
    End Property

    Public Property Truck() As Product
        Get
            Return SessionConnectionProvider.GetSessionData().Products.Where(Function(product) product.ID = TruckID).FirstOrDefault()
        End Get

        Set(value As Product)
            Me.TruckID = value.ID
        End Set
    End Property

    Public Property Wheel() As Product
        Get
            Return SessionConnectionProvider.GetSessionData().Products.Where(Function(product) product.ID = WheelID).FirstOrDefault()
        End Get

        Set(value As Product)
            Me.WheelID = value.ID
        End Set
    End Property

    Public Sub New()
        Me.ID = Guid.NewGuid()
    End Sub
End Class