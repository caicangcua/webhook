Imports System.Data.Entity.Infrastructure
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
'Imports System.Web.Http
Imports System.Web.OData
Imports System.Web.OData.Extensions
Imports System.Web.OData.Routing
Imports System.Data.Services
Imports System.IO
Imports System.Reflection
Imports System.Web.Http.Filters
Imports System.Web.Http.Controllers
Imports System.Web.Http
Imports Newtonsoft.Json

Public Class OrdersController
    Inherits ODataController


    '<EnableQuery>
    '<Action1DebugActionWebApiFilter>
    'Public Function [Get]() As HttpResponseMessage
    '    Dim _repo = New SK8Data()
    '    Dim _orders = _repo.Orders()

    '    Dim response = Request.CreateResponse(HttpStatusCode.OK)
    '    response.Content = _orders ' New StringContent(jObject__1.ToString(), Encoding.UTF8, "application/json")
    '    Return response
    'End Function

    <EnableQuery(MaxExpansionDepth:=10)>
    Public Function [Get]() As IHttpActionResult
        ' If you have any security filters you should apply them before returning then from this method.
        Dim _repo = New SK8Data()
        Dim _orders = _repo.Orders()
        Return Ok(_orders)
    End Function




    <EnableQuery(MaxExpansionDepth:=10)>
    Public Function [Get](<FromODataUri> key As Guid) As IHttpActionResult
        Dim _repo = New SK8Data()
        Dim _orders = _repo.Orders()
        Return Ok(_orders.SingleOrDefault(Function(t) t.ID = key))
    End Function

    '<CustomEnableQueryAttribute(MaxExpansionDepth:=30)>
    'Public Function [Get](queryOptions As ODataQueryOptions) As IQueryable(Of Order)
    '    Dim _repo = New SK8Data()
    '    Dim _orders = _repo.Orders()
    '    Return _orders
    '    'Dim dog = TryCast(queryOptions.ApplyTo(_orders), IQueryable(Of Order))
    '    'Return dog
    'End Function



    Protected Overrides Sub Dispose(disposing As Boolean)
        'If disposing AndAlso _db IsNot Nothing Then
        '    _db.Dispose()
        '    _db = Nothing
        'End If
        MyBase.Dispose(disposing)
    End Sub
End Class

Public Class Action1DebugActionWebApiFilter
    Inherits ActionFilterAttribute
    Public Overrides Sub OnActionExecuting(actionContext As HttpActionContext)
        Dim url = actionContext.Request.RequestUri.OriginalString

        Dim coll As Dictionary(Of String, String) = actionContext.Request.GetQueryNameValuePairs().ToDictionary(Function(kv) kv.Key, Function(kv) kv.Value, StringComparer.OrdinalIgnoreCase)
        Dim value As String
        'For Each key As String In coll.AllKeys
        '    value = coll(key)
        'Next

        coll.Remove("$callback")
        coll.Remove("$format")

        Dim result As New StringBuilder()
        For Each kvp In coll
            If result.Length > 0 Then result.Append("&"c)
            result.Append(kvp.Key).Append("="c).Append(kvp.Value)
        Next


        'Dim match As UriTemplateMatch = DirectCast(url, UriTemplateMatch)

        'Dim format As String = match.QueryParameters("$format")
        'If "json".Equals(format, StringComparison.InvariantCultureIgnoreCase) Then
        '    ' strip out $format from the query options to avoid an error
        '    ' due to use of a reserved option (starts with "$")
        '    match.QueryParameters.Remove("$format")

        '    ' replace the Accept header so that the Data Services runtime 
        '    ' assumes the client asked for a JSON representation
        '    httpmsg.Headers("Accept") = "application/json;odata=verbose, text/plain;q=0.5"
        '    httpmsg.Headers("Accept-Charset") = "utf-8"

        '    Dim callback As String = match.QueryParameters("$callback")
        '    If Not String.IsNullOrEmpty(callback) Then
        '        match.QueryParameters.Remove("$callback")
        '        Return callback
        '    End If
        'End If


        'change something in original url, 
        'for example change all A charaters to B charaters,
        'consider decoding url using WebUtility.UrlDecode() if necessary
        Dim newUrl = url.Replace(actionContext.Request.RequestUri.Query, "?" & result.ToString()) ' ModifyUrl(url)

        actionContext.Request.RequestUri = New Uri(newUrl)
        MyBase.OnActionExecuting(actionContext)
    End Sub


    Public Overrides Sub OnActionExecuted(actionExecutedContext As HttpActionExecutedContext)
        'actionExecutedContext.Response.Content.Headers.ContentType = New MediaTypeHeaderValue("text/javascript")
        ''actionExecutedContext.Response.Content.a.Request.Properties["AppId"].
        ''$callback=jQuery32101982390712754974_1511416557583

        ''actionExecutedContext.ActionContext.


        'Dim headers = actionExecutedContext.Response.Content.Headers.ToString()
        'Dim _response = actionExecutedContext.Response.Content.ReadAsStringAsync().Result

        'Dim response As New HttpResponseMessage(HttpStatusCode.OK)
        'response.Content = New StringContent(_response, Encoding.UTF8, "application/json")
        'response.Headers.Add("MyHeader", "MyHeaderValue")
        ''
        ''
        'actionExecutedContext.Response = response
        'MyBase.OnActionExecuted(actionExecutedContext)

    End Sub

#Region "BUG"

    'Public Overrides Sub OnActionExecuting(actionContext As HttpActionContext)
    '    ' pre-processing
    '    Debug.WriteLine("ACTION 1 DEBUG pre-processing logging")

    '    Dim Request As System.Net.Http.HttpRequestMessage = actionContext.Request
    '    If Request.Properties.ContainsKey("UriTemplateMatchResults") Then
    '        Dim httpmsg As HttpRequestMessageProperty = DirectCast(Request.Properties(HttpRequestMessageProperty.Name), HttpRequestMessageProperty)
    '        Dim match As UriTemplateMatch = DirectCast(Request.Properties("UriTemplateMatchResults"), UriTemplateMatch)

    '        Dim format As String = match.QueryParameters("$format")
    '        If "json".Equals(format, StringComparison.InvariantCultureIgnoreCase) Then
    '            ' strip out $format from the query options to avoid an error
    '            ' due to use of a reserved option (starts with "$")
    '            match.QueryParameters.Remove("$format")

    '            ' replace the Accept header so that the Data Services runtime 
    '            ' assumes the client asked for a JSON representation
    '            httpmsg.Headers("Accept") = "application/json;odata=verbose, text/plain;q=0.5"
    '            httpmsg.Headers("Accept-Charset") = "utf-8"

    '            Dim callback As String = match.QueryParameters("$callback")
    '            If Not String.IsNullOrEmpty(callback) Then
    '                match.QueryParameters.Remove("$callback")
    '            End If
    '        End If
    '    End If
    'End Sub

    'Public Overrides Sub OnActionExecuted(actionExecutedContext As HttpActionExecutedContext)
    '    Dim objectContent = TryCast(actionExecutedContext.Response.Content, ObjectContent)
    '    If objectContent IsNot Nothing Then
    '        Dim type = objectContent.ObjectType
    '        'type of the returned object
    '        'holding the returned value
    '        Dim value = objectContent.Value

    '        Dim json = New JsonMediaTypeFormatter()
    '        Dim Str As String = Serialize(json, value)

    '        'Dim bodyIsText As Boolean
    '        'Dim contentType As String = value.Headers.ContentType.ToString
    '        'If contentType IsNot Nothing Then
    '        '    ' Check the response type and change it to text/javascript if we know how.
    '        '    If contentType.StartsWith("text/plain", StringComparison.InvariantCultureIgnoreCase) Then
    '        '        bodyIsText = True
    '        '        contentType = "text/javascript;charset=utf-8"
    '        '    ElseIf contentType.StartsWith("application/json", StringComparison.InvariantCultureIgnoreCase) Then
    '        '        contentType = contentType.Replace("application/json", "text/javascript")
    '        '    End If
    '        'End If

    '        'Dim Dog = New MediaTypeHeaderValue(contentType)
    '        'value.Headers.ContentType = Dog

    '        '    Dim Dog = New HttpResponseMessage() With {
    '        '.Content = New StringContent(JArray.FromObject(TestOrder.CreateOrders()).ToString(), Encoding.UTF8, "application/json")

    '        'Dim line As String = Nothing
    '        'Using reader As StreamReader = File.OpenText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\dog.txt"))
    '        '    line = reader.ReadToEnd
    '        'End Using
    '        'Dim fuck As String = (Convert.ToString(Me.Callback & Convert.ToString("(")) & line) + ")"
    '        'Using streamWriter As New StreamWriter(writeStream, Me.SupportedEncodings.First())
    '        '    streamWriter.Write(content)
    '        'End Using

    '    End If

    '    Debug.WriteLine("ACTION 1 DEBUG  OnActionExecuted Response " + actionExecutedContext.Response.StatusCode.ToString())
    'End Sub

    'Private Function Serialize(Of T)(formatter As MediaTypeFormatter, value As T) As String
    '    ' Create a dummy HTTP Content.
    '    Dim stream As Stream = New MemoryStream()
    '    Dim content = New StreamContent(stream)
    '    '''' Serialize the object.
    '    formatter.WriteToStreamAsync(GetType(T), value, stream, content, Nothing).Wait()
    '    ' Read the serialized string.
    '    stream.Position = 0
    '    Return content.ReadAsStringAsync().Result
    'End Function

    'Private Function Deserialize(Of T As Class)(formatter As MediaTypeFormatter, str As String) As T
    '    ' Write the serialized string to a memory stream.
    '    Dim stream As Stream = New MemoryStream()
    '    Dim writer As New StreamWriter(stream)
    '    writer.Write(str)
    '    writer.Flush()
    '    stream.Position = 0
    '    ' Deserialize to an object of type T
    '    Return TryCast(formatter.ReadFromStreamAsync(GetType(T), stream, Nothing, Nothing).Result, T)

    'End Function
#End Region
End Class

Public Class CustomEnableQueryAttribute
    Inherits EnableQueryAttribute
    Public Overrides Sub OnActionExecuting(actionContext As HttpActionContext)
        Dim url = actionContext.Request.RequestUri.OriginalString

        Dim coll As Dictionary(Of String, String) = actionContext.Request.GetQueryNameValuePairs().ToDictionary(Function(kv) kv.Key, Function(kv) kv.Value, StringComparer.OrdinalIgnoreCase)
        Dim value As String
        'For Each key As String In coll.AllKeys
        '    value = coll(key)
        'Next

        coll.Remove("$callback")
        coll.Remove("$format")

        Dim result As New StringBuilder()
        For Each kvp In coll
            If result.Length > 0 Then result.Append("&"c)
            result.Append(kvp.Key).Append("="c).Append(kvp.Value)
        Next


        'Dim match As UriTemplateMatch = DirectCast(url, UriTemplateMatch)

        'Dim format As String = match.QueryParameters("$format")
        'If "json".Equals(format, StringComparison.InvariantCultureIgnoreCase) Then
        '    ' strip out $format from the query options to avoid an error
        '    ' due to use of a reserved option (starts with "$")
        '    match.QueryParameters.Remove("$format")

        '    ' replace the Accept header so that the Data Services runtime 
        '    ' assumes the client asked for a JSON representation
        '    httpmsg.Headers("Accept") = "application/json;odata=verbose, text/plain;q=0.5"
        '    httpmsg.Headers("Accept-Charset") = "utf-8"

        '    Dim callback As String = match.QueryParameters("$callback")
        '    If Not String.IsNullOrEmpty(callback) Then
        '        match.QueryParameters.Remove("$callback")
        '        Return callback
        '    End If
        'End If


        'change something in original url, 
        'for example change all A charaters to B charaters,
        'consider decoding url using WebUtility.UrlDecode() if necessary
        Dim newUrl = url.Replace(actionContext.Request.RequestUri.Query, "?" & result.ToString()) ' ModifyUrl(url)

        actionContext.Request.RequestUri = New Uri(newUrl)
        MyBase.OnActionExecuting(actionContext)
    End Sub

    'Public Overrides Function ApplyQuery(queryable As IQueryable, queryOptions As ODataQueryOptions) As IQueryable
    '    Dim result As IQueryable = Nothing

    '    ' get the original request before the alterations
    '    Dim originalRequest As HttpRequestMessage = queryOptions.Request

    '    ' get the original URL before the alterations
    '    Dim url As String = originalRequest.RequestUri.AbsoluteUri

    '    ' rebuild the URL if it contains a specific filter for "ID = 0" to select all records
    '    If queryOptions.Filter IsNot Nothing AndAlso url.Contains("$filter=ID%20eq%200") Then
    '        ' apply the new filter
    '        url = url.Replace("$filter=ID%20eq%200", "$filter=ID%20ne%200")

    '        ' build a new request for the filter
    '        Dim req As New HttpRequestMessage(HttpMethod.[Get], url)

    '        ' reset the query options with the new request
    '        queryOptions = New ODataQueryOptions(queryOptions.Context, req)
    '    End If

    '    ' set a top filter if one was not supplied
    '    If queryOptions.Top Is Nothing Then
    '        ' apply the query options with the new top filter
    '        result = queryOptions.ApplyTo(queryable, New ODataQuerySettings() With {
    '            .PageSize = 100
    '        })
    '    Else
    '        ' apply any pending information that was not previously applied
    '        result = queryOptions.ApplyTo(queryable)
    '    End If

    '    ' add the NextLink if one exists
    '    If queryOptions.Request.ODataProperties().NextLink IsNot Nothing Then
    '        originalRequest.ODataProperties().NextLink = queryOptions.Request.ODataProperties().NextLink
    '    End If
    '    ' add the TotalCount if one exists
    '    If queryOptions.Request.ODataProperties().TotalCount IsNot Nothing Then
    '        originalRequest.ODataProperties().TotalCount = queryOptions.Request.ODataProperties().TotalCount
    '    End If

    '    ' return all results
    '    Return result
    'End Function

End Class
