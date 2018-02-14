Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("Product")>
Partial Public Class Products
    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    Public Property Id As Integer

    <Required>
    <StringLength(100)>
    Public Property Name As String

    Public Property Price As Double?

    Public Property CategoryId As Integer?

    Public Overridable Property Category As Category
End Class
