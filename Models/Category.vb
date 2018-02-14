Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("Category")>
Partial Public Class Category
    Public Sub New()
        Products = New HashSet(Of Products)()
    End Sub

    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    Public Property Id As Integer

    <Required>
    <StringLength(100)>
    Public Property Name As String

    <StringLength(255)>
    Public Property Description As String

    Public Overridable Property Products As ICollection(Of Products)
End Class
