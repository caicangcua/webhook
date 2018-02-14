
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data.Services
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Web
Imports System.Xml.Linq

Public Class SK8Data
    Implements IUpdatable
    Public Sub New()
        Fill()
    End Sub

    Private m_brands As New List(Of Brand)()
    Private m_customers As New List(Of Customer)()
    Private m_employees As New List(Of Employee)()
    Private m_orderItems As New List(Of OrderItem)()
    Private m_orders As New List(Of Order)()
    Private m_products As New List(Of Product)()
    Private m_productTypes As New List(Of ProductType)()
    Private m_skates As New List(Of Skate)()

    Public ReadOnly Property Brands() As IQueryable(Of Brand)
        Get
            Return m_brands.AsQueryable()
        End Get
    End Property
    Public ReadOnly Property Customers() As IQueryable(Of Customer)
        Get
            Return m_customers.AsQueryable()
        End Get
    End Property
    Public ReadOnly Property Employees() As IQueryable(Of Employee)
        Get
            Return m_employees.AsQueryable()
        End Get
    End Property
    Public ReadOnly Property OrderItems() As IQueryable(Of OrderItem)
        Get
            Return m_orderItems.AsQueryable()
        End Get
    End Property
    Public ReadOnly Property Orders() As IQueryable(Of Order)
        Get
            Return m_orders.AsQueryable()
        End Get
    End Property
    Public ReadOnly Property Products() As IQueryable(Of Product)
        Get
            Return m_products.AsQueryable()
        End Get
    End Property
    Public ReadOnly Property ProductTypes() As IQueryable(Of ProductType)
        Get
            Return m_productTypes.AsQueryable()
        End Get
    End Property
    Public ReadOnly Property Skates() As IQueryable(Of Skate)
        Get
            Return m_skates.AsQueryable()
        End Get
    End Property

#Region "IUpdatable implementation"

    Private Function IUpdatable_CreateResource(containerName As String, fullTypeName As String) As Object Implements IUpdatable.CreateResource
        Dim entityType = Type.[GetType](fullTypeName, True)
        Dim resource = Activator.CreateInstance(entityType)

        If Not String.IsNullOrEmpty(containerName) Then
            AddEntityToContainer(resource)
        End If

        Return resource
    End Function

    Private Function IUpdatable_GetResource(query As IQueryable, fullTypeName As String) As Object Implements IUpdatable.GetResource
        Dim resource As Object = Nothing

        For Each item As Object In query
            If resource IsNot Nothing Then
                Throw New DataServiceException("The query must return a single resource")
            End If
            resource = item
        Next

        If resource Is Nothing Then
            Throw New DataServiceException(404, "Resource not found")
        End If

        If fullTypeName IsNot Nothing AndAlso resource.[GetType]().FullName <> fullTypeName Then
            Throw New Exception("Unexpected type for resource")
        End If

        Return resource
    End Function

    Private Function IUpdatable_ResetResource(resource As Object) As Object Implements IUpdatable.ResetResource
        Return resource
    End Function

    Private Sub IUpdatable_SetValue(targetResource As Object, propertyName As String, propertyValue As Object) Implements IUpdatable.SetValue
        Dim propertyInfo = targetResource.[GetType]().GetProperty(propertyName)
        propertyInfo.SetValue(targetResource, propertyValue, Nothing)
    End Sub

    Private Function IUpdatable_GetValue(targetResource As Object, propertyName As String) As Object Implements IUpdatable.GetValue
        Dim propertyInfo = targetResource.[GetType]().GetProperty(propertyName)
        Return propertyInfo.GetValue(targetResource, Nothing)
    End Function

    Private Sub IUpdatable_SetReference(targetResource As Object, propertyName As String, propertyValue As Object) Implements IUpdatable.SetReference
        DirectCast(Me, IUpdatable).SetValue(targetResource, propertyName, propertyValue)
    End Sub

    Private Sub IUpdatable_AddReferenceToCollection(targetResource As Object, propertyName As String, resourceToBeAdded As Object) Implements IUpdatable.AddReferenceToCollection
        Dim propertyInfo = targetResource.[GetType]().GetProperty(propertyName)
        If propertyInfo Is Nothing Then
            Throw New Exception("Can't find property")
        End If

        Dim collection = DirectCast(propertyInfo.GetValue(targetResource, Nothing), IList)
        collection.Add(resourceToBeAdded)
    End Sub

    Private Sub IUpdatable_RemoveReferenceFromCollection(targetResource As Object, propertyName As String, resourceToBeRemoved As Object) Implements IUpdatable.RemoveReferenceFromCollection
        Dim propertyInfo = targetResource.[GetType]().GetProperty(propertyName)
        If propertyInfo Is Nothing Then
            Throw New Exception("Can't find property")
        End If
        Dim collection = DirectCast(propertyInfo.GetValue(targetResource, Nothing), IList)
        collection.Remove(resourceToBeRemoved)
    End Sub

    Private Sub IUpdatable_DeleteResource(targetResource As Object) Implements IUpdatable.DeleteResource
        RemoveEntityFromContainer(targetResource)
    End Sub

    Private Sub IUpdatable_SaveChanges() Implements IUpdatable.SaveChanges
    End Sub

    Private Function IUpdatable_ResolveResource(resource As Object) As Object Implements IUpdatable.ResolveResource
        Return resource
    End Function

    Private Sub IUpdatable_ClearChanges() Implements IUpdatable.ClearChanges
    End Sub

    Public Sub Add(entity As Object)
        AddEntityToContainer(entity)
    End Sub

    Public Sub Remove(entity As Object)
        RemoveEntityFromContainer(entity)
    End Sub

    Friend Sub AddEntityToContainer(entity As Object)
        Dim containerField = FindContainerField(entity.[GetType]())
        Dim method = containerField.FieldType.GetMethod("Add")
        method.Invoke(containerField.GetValue(Me), New Object() {entity})
    End Sub

    Friend Sub RemoveEntityFromContainer(entity As Object)
        Dim containerField = FindContainerField(entity.[GetType]())
        Dim method = containerField.FieldType.GetMethod("Remove")
        method.Invoke(containerField.GetValue(Me), New Object() {entity})
    End Sub

    Private Function FindContainerField(entityType As Type) As FieldInfo
        Dim containerName = "m_" & entityType.Name(0).ToString().ToLower() + entityType.Name.Substring(1) & "s"
        Return Me.[GetType]().GetField(containerName, BindingFlags.Instance Or BindingFlags.NonPublic)
    End Function

#End Region

#Region "Sample data"

    Private Sub Fill()
        Dim path__1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\SampleData.xml")
        'var currentUrl = HttpContext.Current.Request.Url.ToString();
        'var servicePath = currentUrl.Substring(0, currentUrl.IndexOf("/api.svc"));
        Dim xml As XDocument = XDocument.Load(path__1)

        '#Region "Employees"
        m_employees = (From employee In xml.Root.Element("Employees").Elements("Employee") Select New Employee() With { _
          .ID = Guid.Parse(employee.Attribute("ID").Value), _
          .Name = employee.Attribute("Name").Value, _
          .Surname = employee.Attribute("Surname").Value, _
          .PhotoUrl = employee.Attribute("PhotoUrl").Value, _
          .Password = employee.Attribute("Password").Value _
        }).ToList()
        '#End Region

        '#Region "Customers"
        m_customers = (From customer In xml.Root.Element("Customers").Elements("Customer") Let billingAddress = customer.Element("BillingAddress") Let shippingAddress = customer.Element("ShippingAddress") Select New Customer() With { _
          .ID = Guid.Parse(customer.Attribute("ID").Value), _
          .Email = customer.Attribute("Email").Value, _
          .Password = customer.Attribute("Password").Value, _
          .Name = customer.Attribute("Name").Value, _
          .Surname = customer.Attribute("Surname").Value, _
          .PhotoUrl = customer.Attribute("PhotoUrl").Value, _
          .BillingAddress = New AddressInfo() With { _
           .Country = billingAddress.Attribute("Country").Value, _
           .State = billingAddress.Attribute("State").Value, _
           .City = billingAddress.Attribute("City").Value, _
           .Postcode = Integer.Parse(billingAddress.Attribute("PostCode").Value), _
           .Address = billingAddress.Attribute("Address").Value, _
           .Phone = billingAddress.Attribute("Phone").Value _
         }, _
          .ShippingAddress = New AddressInfo() With { _
           .Country = shippingAddress.Attribute("Country").Value, _
           .State = shippingAddress.Attribute("State").Value, _
           .City = shippingAddress.Attribute("City").Value, _
           .Postcode = Integer.Parse(shippingAddress.Attribute("PostCode").Value), _
           .Address = shippingAddress.Attribute("Address").Value, _
           .Phone = shippingAddress.Attribute("Phone").Value _
         }, _
          .Notifications = Byte.Parse(customer.Attribute("Notifications").Value) _
        }).ToList()
        '#End Region

        '#Region "Brands"
        m_brands = (From brand In xml.Root.Element("Brands").Elements("Brand") Select New Brand() With { _
          .ID = Guid.Parse(brand.Attribute("ID").Value), _
          .Name = brand.Attribute("Name").Value, _
          .ImageUrl = brand.Attribute("ImageUrl").Value _
        }).ToList()
        '#End Region

        '#Region "ProductTypes"
        m_productTypes = (From productType In xml.Root.Element("ProductTypes").Elements("ProductType") Select New ProductType() With { _
          .ID = Guid.Parse(productType.Attribute("ID").Value), _
          .Name = productType.Attribute("Name").Value _
        }).ToList()
        '#End Region

        '#Region "Products"
        m_products = (From product In xml.Root.Element("Products").Elements("Product") Select New Product() With { _
          .ID = Guid.Parse(product.Attribute("ID").Value), _
          .Name = product.Attribute("Name").Value, _
          .ImageUrl = product.Attribute("ImageUrl").Value, _
          .Price = Decimal.Parse(product.Attribute("Price").Value), _
          .Quantity = Integer.Parse(product.Attribute("Quantity").Value), _
          .TypeID = Guid.Parse(product.Attribute("TypeID").Value), _
          .BrandID = Guid.Parse(product.Attribute("BrandID").Value) _
        }).ToList()
        '#End Region

        '#Region "Orders"
        m_orders = (From order In xml.Root.Element("Orders").Elements("Order") Select New Order() With { _
          .ID = Guid.Parse(order.Attribute("ID").Value), _
          .OrderDate = DateTime.Parse(order.Attribute("OrderDate").Value), _
          .State = Byte.Parse(order.Attribute("State").Value), _
          .CustomerID = Guid.Parse(order.Attribute("CustomerID").Value), _
          .EmployeeID = Guid.Parse(order.Attribute("EmployeeID").Value) _
        }).ToList()
        '#End Region

        '#Region "OrderItems" 
        m_orderItems = (From item In xml.Root.Element("OrderItems").Elements("OrderItem") Let state = Byte.Parse(item.Attribute("State").Value) Select New OrderItem(state) With { _
          .ID = Guid.Parse(item.Attribute("ID").Value), _
          .OrderID = Guid.Parse(item.Attribute("OrderID").Value), _
          .ProductID = Guid.Parse(item.Attribute("ProductID").Value) _
        }).ToList()
        '#End Region

        '#Region "Skates"
        m_skates = (From skate In xml.Root.Element("Skates").Elements("Skate") Select New Skate() With { _
          .ID = Guid.Parse(skate.Attribute("ID").Value), _
          .ImageUrl = skate.Attribute("ImageUrl").Value, _
          .DeckID = Guid.Parse(skate.Attribute("DeckID").Value), _
          .TruckID = Guid.Parse(skate.Attribute("TruckID").Value), _
          .WheelID = Guid.Parse(skate.Attribute("WheelID").Value) _
        }).ToList()
        '#End Region
    End Sub

#End Region
End Class