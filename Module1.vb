Imports System.Net
Imports System.IO
Imports Newtonsoft.Json
'Imports System.Xml.Linq

Module Module1

    Sub Main()
        Dim jsonURL As String = "http://mysafeinfo.com/api/data?list=englishmonarchs&format=json"
        Dim reader As StreamReader
        Dim errorMsg As String = Nothing

        Try
            Dim request As HttpWebRequest = CType(WebRequest.Create(jsonURL), HttpWebRequest)
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            reader = New StreamReader(response.GetResponseStream())
            Dim jsonStr As String = reader.ReadToEnd()

            Console.WriteLine(jsonStr)

            printJSON(jsonStr)

        Catch ex As WebException
            errorMsg = "Download failed. The response from the server was: " +
                   CType(ex.Response, HttpWebResponse).StatusDescription
            Console.WriteLine("Error: " + errorMsg)
        End Try
    End Sub

    Private Sub printJSON(jsonStr As String)
        ' Deserialize our JSON string, then filter and print it

        ' Json.NET deserializer gives you a list of .NET objects
        Dim monarchs As List(Of JSON_data) = JsonConvert.DeserializeObject(Of List(Of JSON_data))(jsonStr)

        Dim i As Int16 = 0
        ' Iterate over the objects and print each name property
        For Each monarch_1 As JSON_data In monarchs
            i += 1
            Console.WriteLine(i & ": " & monarch_1.Name)
        Next

        ' Set up a LINQ statement to filter the monarchs list
        Dim monarchList = From monarch In monarchs Where monarch.Name.Contains("Ed") Select monarch

        ' Print the results of our LINQ query
        For Each monarch_2 In monarchList
            Console.WriteLine("King or Queen: " + monarch_2.Name + ", Years: " + monarch_2.Reign)
        Next
    End Sub

End Module

Public Class JSON_data
    Public ID As Int32
    Public Name As String
    Public Country As String
    Public House As String
    Public Reign As String
End Class
