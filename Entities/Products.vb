
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Services.Common
Imports System.Linq
Imports System.Web

<DataServiceKey("ID")>
Public Class Product
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
    Public Property Price() As Decimal
        Get
            Return m_Price
        End Get
        Set(value As Decimal)
            m_Price = value
        End Set
    End Property
    Private m_Price As Decimal
    Public Property Quantity() As Integer
        Get
            Return m_Quantity
        End Get
        Set(value As Integer)
            m_Quantity = value
        End Set
    End Property
    Private m_Quantity As Integer
    Public Property ImageUrl() As String
        Get
            Return m_ImageUrl
        End Get
        Set(value As String)
            m_ImageUrl = value
        End Set
    End Property
    Private m_ImageUrl As String

    Public Property TypeID() As Guid
        Get
            Return m_TypeID
        End Get
        Set(value As Guid)
            m_TypeID = value
        End Set
    End Property
    Private m_TypeID As Guid
    Public Property BrandID() As Guid
        Get
            Return m_BrandID
        End Get
        Set(value As Guid)
            m_BrandID = value
        End Set
    End Property
    Private m_BrandID As Guid

    Public Property [Type]() As ProductType
        Get
            Return SessionConnectionProvider.GetSessionData().ProductTypes.Where(Function(type__1) type__1.ID = TypeID).FirstOrDefault()
        End Get

        Set(value As ProductType)
            Me.TypeID = value.ID
        End Set
    End Property

    Public Property Brand() As Brand
        Get
            Return SessionConnectionProvider.GetSessionData().Brands.Where(Function(brand__1) brand__1.ID = BrandID).FirstOrDefault()
        End Get

        Set(value As Brand)
            Me.BrandID = value.ID
        End Set
    End Property

    Public ReadOnly Property OrderItems() As ICollection(Of OrderItem)
        Get
            Return SessionConnectionProvider.GetSessionData().OrderItems.Where(Function(item) item.ProductID = ID).ToList()
        End Get
    End Property

    Public Sub New()
        Me.ID = Guid.NewGuid()
    End Sub

    Public Sub UpdateProductQuantity()
        If Quantity = 0 Then
            Throw New Exception(Name & Convert.ToString(" is out of stock"))
        End If

        Quantity -= 1
        If Quantity = 0 Then

            Dim orders = (From item In OrderItems
                          Let order = item.Order
                          Where item.State <> 1 AndAlso order.State <> CByte(OrderState.Completed)
                          Select order).Distinct()

            For Each order In orders
                order.State = CByte(OrderState.OutOfStock)
            Next
        End If
    End Sub
End Class