
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Services.Common
Imports System.Linq
Imports System.Web

<DataServiceKey("ID")>
Public Class OrderItem
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

    Public Property OrderID() As Guid
        Get
            Return m_OrderID
        End Get
        Set(value As Guid)
            m_OrderID = value
        End Set
    End Property
    Private m_OrderID As Guid
    Public Property ProductID() As Guid
        Get
            Return m_ProductID
        End Get
        Set(value As Guid)
            m_ProductID = value
        End Set
    End Property
    Private m_ProductID As Guid

    Private _state As Byte
    Public Property State() As Byte
        Get
            Return _state
        End Get
        Set(value As Byte)
            If value <> _state Then
                If value = 1 Then
                    Product.UpdateProductQuantity()
                End If
                _state = value
                Order.UpdateOrderState()
            End If
        End Set
    End Property

    Public Property Order() As Order
        Get
            Return SessionConnectionProvider.GetSessionData().Orders.Where(Function(order__1) order__1.ID = OrderID).FirstOrDefault()
        End Get

        Set(value As Order)
            Me.OrderID = value.ID
        End Set
    End Property

    Public Property Product() As Product
        Get
            Return SessionConnectionProvider.GetSessionData().Products.Where(Function(product__1) product__1.ID = ProductID).FirstOrDefault()
        End Get

        Set(value As Product)
            Me.ProductID = value.ID
        End Set
    End Property

    Public Sub New()
        Me.New(0)
    End Sub

    Public Sub New(state As Byte)
        Me.ID = Guid.NewGuid()
        _state = state
    End Sub
End Class