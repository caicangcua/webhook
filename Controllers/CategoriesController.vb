Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports Newtonsoft.Json

Namespace MyRestService.Controllers
    Public Class CategoriesController
        Inherits ApiController

        ReadOnly _db As New EntitiesModel()
        'Public Function [Get]() As IEnumerable(Of Category)



        '    Dim options = Request.GetQueryNameValuePairs().ToDictionary(Function(x) x.Key, Function(x) JsonConvert.DeserializeObject(x.Value)) 'parsed options

        '    'see the QueryHelper class for the implementation
        '    Dim query = _db.Category.AsEnumerable().FilterByOptions(options).SortByOptions(options).PageByOptions(options) 'paging - sorting - filtering
        '    Return query
        'End Function


        Public Function [Get]() As HttpResponseMessage
            Dim response = Request.CreateResponse(HttpStatusCode.OK)

            Dim _repo = New SK8Data()
            Dim _orders = _repo.Orders()
            Dim Json As String = JsonConvert.SerializeObject(_orders.ToArray())

            'Dim line As String = Nothing
            'Using reader As StreamReader = File.OpenText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\dog.txt"))
            '    line = reader.ReadToEnd
            'End Using

            response.Content = New StringContent(Json, Encoding.UTF8, "application/json")
            Return response

        End Function
        Public Function [Get](ByVal id As Integer) As Category
            Return _db.Category.Find(id)
        End Function
        Public Function Post(ByVal cat As Category) As Integer
            _db.Category.Add(cat)
            Return _db.SaveChanges()
        End Function
        Public Function Put(ByVal cat As Category) As Integer
            Dim categ As Category = _db.Category.Find(cat.Id)
            categ.Name = cat.Name
            Return _db.SaveChanges()
        End Function
        Public Function Delete(ByVal id As Integer) As Integer
            Dim cat As Category = _db.Category.Find(id)
            _db.Category.Remove(cat)
            Return _db.SaveChanges()
        End Function
    End Class
End Namespace
