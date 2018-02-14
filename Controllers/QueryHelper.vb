Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Newtonsoft.Json.Linq
Imports System.Linq.Dynamic


Public Module QueryHelper

        <System.Runtime.CompilerServices.Extension>
        Public Function PageByOptions(ByVal query As IEnumerable(Of Category), ByVal options As Dictionary(Of String, Object)) As IEnumerable(Of Category)
            If options.ContainsKey("skip") Then
                Dim skip = Convert.ToInt32(options("skip"))
                Dim take = Convert.ToInt32(options("take"))
                query = query.Skip(skip).Take(take)
            End If
            Return query
        End Function

        <System.Runtime.CompilerServices.Extension>
        Public Function SortByOptions(ByVal query As IEnumerable(Of Category), ByVal options As Dictionary(Of String, Object)) As IEnumerable(Of Category)
            If options.ContainsKey("sortOptions") AndAlso options("sortOptions") IsNot Nothing Then
                Dim sortOptions = JObject.Parse(JArray.FromObject(options("sortOptions"))(0).ToString())
                Dim columnName = CStr(sortOptions.SelectToken("selector"))
                Dim descending = CBool(sortOptions.SelectToken("desc"))

                If descending Then
                    columnName &= " DESC"
                End If
                query = query.OrderBy(columnName)
            End If
            Return query
        End Function


        <System.Runtime.CompilerServices.Extension>
        Public Function FilterByOptions(ByVal query As IEnumerable(Of Category), ByVal options As Dictionary(Of String, Object)) As IEnumerable(Of Category)
            If options.ContainsKey("filterOptions") AndAlso options("filterOptions") IsNot Nothing Then
                Dim filterTree = JArray.FromObject(options("filterOptions"))
                Return ReadExpression(query, filterTree)
            End If
            Return query
        End Function

        Public Function ReadExpression(ByVal source As IEnumerable(Of Category), ByVal array As JArray) As IEnumerable(Of Category)
            If array(0).Type = JTokenType.String Then
                Return FilterQuery(source, array(0).ToString(), array(1).ToString(), array(2).ToString())
            Else
                For i As Integer = 0 To array.Count - 1
                    If array(i).ToString().Equals("and") Then
                        Continue For
                    End If
                    source = ReadExpression(source, CType(array(i), JArray))
                Next i
                Return source
            End If
        End Function

        Public Function FilterQuery(ByVal source As IEnumerable(Of Category), ByVal ColumnName As String, ByVal Clause As String, ByVal Value As String) As IEnumerable(Of Category)
            Select Case Clause
                Case "="
                    Value = If(System.Text.RegularExpressions.Regex.IsMatch(Value, "^\d+$"), Value, String.Format("""{0}""", Value))
                    source = source.Where(String.Format("{0} == {1}", ColumnName, Value))
                Case "contains"
                    source = source.Where(ColumnName & ".Contains(@0)", Value)
                Case "<>"
                    source = source.Where(String.Format("!{0}.StartsWith(""{1}"")", ColumnName, Value))
                Case Else
            End Select
            Return source
        End Function
    End Module
