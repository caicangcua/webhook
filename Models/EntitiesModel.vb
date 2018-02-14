Imports System
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Linq

Partial Public Class EntitiesModel
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=EntitiesModel")
    End Sub

    Public Overridable Property Category As DbSet(Of Category)
    Public Overridable Property Products As DbSet(Of Products)

    Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
    End Sub
End Class
