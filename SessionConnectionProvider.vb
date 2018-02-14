
'Imports System.Collections.Generic
'Imports System.Configuration
'Imports System.Data
'Imports System.Data.EntityClient
'Imports System.Data.Metadata.Edm
'Imports System.Data.SqlClient
'Imports System.Linq
'Imports System.Reflection
'Imports System.Web


'Public NotInheritable Class SessionConnectionProvider
'    Private Sub New()
'    End Sub
'    Public Shared Function GetSessionData() As SK8Data
'        Dim data = TryCast(HttpContext.Current.Session("Data"), SK8Data)
'        If data Is Nothing Then
'            data = New SK8Data()
'            HttpContext.Current.Session("Data") = data
'        End If
'        Return data
'    End Function
'End Class

Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Reflection
Imports System.Web
Imports System.Collections.Concurrent
Public NotInheritable Class SessionConnectionProvider
    Private Sub New()
    End Sub
    Public Shared Function GetSessionData() As SK8Data
        Dim data As SK8Data = CacheDXSK8.RetrieveAndUpdateOrAdd("Data")
        Return data
    End Function
End Class
Public Class CacheDXSK8
    '
    Private Shared WebFarmID As New ConcurrentDictionary(Of String, SK8Data) '(System.Environment.ProcessorCount, 10)
    Public Shared Sub AddOrUpdateWithoutRetrieving(ByVal searchKey As String, ByVal newVAL As SK8Data)
        WebFarmID.AddOrUpdate(searchKey, newVAL, Function(key, existingVal)
                                                     existingVal = newVAL
                                                     Return existingVal
                                                 End Function)
    End Sub

    Public Shared Function RetrieveAndUpdateOrAdd(ByVal Key As String) As SK8Data
        '
        Dim retrievedValue As SK8Data = Nothing
        '
        If (WebFarmID.TryGetValue(Key, retrievedValue)) Then
            Return retrievedValue
        End If
        '
        Return RetrieveValueOrAdd(Key)
        '
    End Function

    Private Shared Function RetrieveValueOrAdd(ByVal Key As String) As SK8Data
        '
        Dim retrievedValue As SK8Data = Nothing
        '
        Try
            retrievedValue = WebFarmID.GetOrAdd(Key, GetConfigOpt())
        Catch e As ArgumentException
        End Try
        '
        Return retrievedValue
        '
    End Function

    Private Shared Function GetConfigOpt() As SK8Data
        Return New SK8Data()
    End Function

End Class