
Imports System.Collections.Generic

Public Class MyComplexType1
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set
                m_Description = Value
            End Set
        End Property
        Private m_Description As String
        Public Property Children() As ICollection(Of MyComplexType2)
            Get
                Return m_Children
            End Get
            Set
                m_Children = Value
            End Set
        End Property
        Private m_Children As ICollection(Of MyComplexType2)
    End Class

    Public Class MyComplexType2
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set
                m_Description = Value
            End Set
        End Property
        Private m_Description As String
        Public Property Children() As ICollection(Of MyComplexType1)
            Get
                Return m_Children
            End Get
            Set
                m_Children = Value
            End Set
        End Property
        Private m_Children As ICollection(Of MyComplexType1)
    End Class