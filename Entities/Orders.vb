
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Services.Common
Imports System.Linq
Imports System.Web

Enum OrderState As Byte
    Draft = 0
    [New] = 1
    Inprogress = 2
    Completed = 3
    OutOfStock = 4
End Enum

<DataServiceKey("ID")>
Public Class Order
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
    Public Property OrderDate() As DateTime
        Get
            Return m_OrderDate
        End Get
        Set(value As DateTime)
            m_OrderDate = value
        End Set
    End Property
    Private m_OrderDate As DateTime
    Public Property State() As Byte
        Get
            Return m_State
        End Get
        Set(value As Byte)
            m_State = value
        End Set
    End Property
    Private m_State As Byte

    Public Property CustomerID() As Guid
        Get
            Return m_CustomerID
        End Get
        Set(value As Guid)
            m_CustomerID = value
        End Set
    End Property
    Private m_CustomerID As Guid
    Public Property EmployeeID() As Guid
        Get
            Return m_EmployeeID
        End Get
        Set(value As Guid)
            m_EmployeeID = value
        End Set
    End Property
    Private m_EmployeeID As Guid

    Public Property Customer() As Customer
        Get
            Return SessionConnectionProvider.GetSessionData().Customers.Where(Function(customer__1) customer__1.ID = CustomerID).FirstOrDefault()
        End Get

        Set(value As Customer)
            Me.CustomerID = value.ID
        End Set
    End Property

    Public Property Employee() As Employee
        Get
            Return SessionConnectionProvider.GetSessionData().Employees.Where(Function(employee__1) employee__1.ID = EmployeeID).FirstOrDefault()
        End Get

        Set(value As Employee)
            Me.EmployeeID = value.ID
        End Set
    End Property

    Public ReadOnly Property OrderItems() As ICollection(Of OrderItem)
        Get
            Return SessionConnectionProvider.GetSessionData().OrderItems.Where(Function(item) item.OrderID = ID).ToList()
        End Get
    End Property

    Public Sub New()
        Me.ID = Guid.NewGuid()
    End Sub

    Public Sub UpdateOrderState()
        State = CByte(OrderState.Inprogress)
    End Sub
End Class