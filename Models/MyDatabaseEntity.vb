
Imports System.Collections.Generic


Public Class MyDatabaseEntity
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set
                m_Id = Value
            End Set
        End Property
        Private m_Id As Integer
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set
                m_Description = Value
            End Set
        End Property
        Private m_Description As String
        Public Property Children() As ICollection(Of MyDatabaseComplexType)
            Get
                Return m_Children
            End Get
            Set
                m_Children = Value
            End Set
        End Property
        Private m_Children As ICollection(Of MyDatabaseComplexType)
    End Class