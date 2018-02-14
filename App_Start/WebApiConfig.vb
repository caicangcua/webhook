Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Net.Http
Imports System.Net.Http.Extensions.Compression.Core.Compressors
Imports System.Net.Http.Formatting
Imports System.Net.Http.Headers
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web.Http
Imports System.Web.OData
Imports System.Web.OData.Builder
Imports System.Web.OData.Extensions
Imports System.Web.OData.Formatter
Imports Microsoft.AspNet.WebApi.Extensions.Compression.Server
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)
        ' Web API configuration and services
        'config.MessageHandlers.Add(New CustomHeaderHandler())
        ' Web API routes
        config.MapHttpAttributeRoutes()




        '' OData configs
        'Dim builder = New ODataConventionModelBuilder()






        config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always
        config.MapODataServiceRoute(routeName:="OData", routePrefix:="api", model:=GetModel)

        config.Routes.MapHttpRoute(name:="DefaultApi", routeTemplate:="api/{controller}/{id}", defaults:=New With {
            Key .id = RouteParameter.[Optional]
        })

        'config.Formatters.Clear()
        'config.Formatters.AddRange(ODataMediaTypeFormatters.Create())
        'config.Formatters.Add(New RawJsonMediaTypeFormatter())


        ' now the default setting for WebAPI OData is:
        '            client can’t apply $count, $orderby, $select, $top, $expand, $filter in the query, query
        '            like localhost\odata\Customers?$orderby=Name will failed as BadRequest,
        '            because all properties are not sort-able by default, this is a breaking change in 6.0.0
        '            So, we now need to enable OData Model Bound Attributes
        '            

        config.Count().Filter().OrderBy().Expand().[Select]().MaxTop(Nothing)
        ''''config.Formatters.Clear()
        ''''config.Formatters.Add(New JsonMediaTypeFormatter())

        ''''' Ignore JSON reference loops
        ''''config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore

        config.MessageHandlers.Insert(0, New ServerCompressionHandler(New GZipCompressor(), New DeflateCompressor()))


    End Sub


    Public Function GetModel() As Microsoft.OData.Edm.IEdmModel
        'Dim builder As ODataModelBuilder = New ODataConventionModelBuilder()

        ''builder.EntitySet(Of Products)("Products")
        ''builder.EntitySet(Of Category)("Category")
        'builder.EntitySet(Of Order)("Orders")



        Dim builder = New ODataConventionModelBuilder() With {.[Namespace] = "Default"}
        builder.EntitySet(Of Order)("Orders")
        builder.EntitySet(Of OrderItem)("OrderItems")
        builder.EntitySet(Of Product)("Product")
        'builder.EntitySet(Of MyType)("Types")


        Return builder.GetEdmModel()
    End Function

    Public Function GetControllerNameOf(Of TController As ODataController)() As String
        Return GetType(TController).Name.Replace("Controller", "")
    End Function


End Module

Public Class CustomHeaderHandler
    Inherits DelegatingHandler

    Protected Overrides Function SendAsync(request As HttpRequestMessage, cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
        Dim queryStrings = request.RequestUri.ParseQueryString()
        Dim format As String = queryStrings("$format")

        Select Case format
            Case Nothing
                Exit Select
            Case "xml"
                request.Headers.Accept.Clear()
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml"))
                Exit Select
            Case "json"
                request.Headers.Accept.Clear()
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"))
                Exit Select
            Case "atom"
                request.Headers.Accept.Clear()
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/atom+xml"))
                Exit Select
            Case Else
                request.Headers.Accept.Clear()
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(format))
                Exit Select
        End Select

        Return MyBase.SendAsync(request, cancellationToken)
    End Function

End Class



Public Class RawJsonMediaTypeFormatter
    Inherits MediaTypeFormatter
    Private Shared ReadOnly _customMediaType As MediaTypeHeaderValue = MediaTypeHeaderValue.Parse("application/prs.adrianm+json")

    Public Sub New()
        MyBase.New()
        Me.Intialize()
    End Sub

    Protected Sub New(formatter As MediaTypeFormatter)
        MyBase.New(formatter)
        Me.Intialize()
    End Sub

    Protected Sub Intialize()
        Me.SupportedMediaTypes.Add(_customMediaType)
    End Sub

    Public Overrides Function GetPerRequestFormatterInstance(type As Type, request As HttpRequestMessage, mediaType As MediaTypeHeaderValue) As MediaTypeFormatter
        If type = GetType(JToken) AndAlso mediaType.MediaType = _customMediaType.MediaType Then
            Return Me
        End If

        Return MyBase.GetPerRequestFormatterInstance(type, request, mediaType)
    End Function

    Public Overrides Function CanReadType(type As Type) As Boolean
        Return type = GetType(JToken)
    End Function

    Public Overrides Function CanWriteType(type As Type) As Boolean
        Return False
    End Function

    Public Overrides Function ReadFromStreamAsync(type As Type, readStream As Stream, content As HttpContent, formatterLogger As IFormatterLogger) As Task(Of Object)
        Return Me.ReadFromStreamAsync(type, readStream, content, formatterLogger, Nothing)
    End Function

    Public Overrides Function ReadFromStreamAsync(type As Type, readStream As Stream, content As HttpContent, formatterLogger As IFormatterLogger, cancellationToken As CancellationToken) As Task(Of Object)
        Dim result As Object

        Using reader = New JsonTextReader(New StreamReader(readStream))
            result = JToken.ReadFrom(reader)
        End Using

        Return Task.FromResult(result)
    End Function
End Class