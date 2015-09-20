''' <summary>
''' clsWebConfig - Class to interface with the applications's web config.
''' </summary>
''' <remarks></remarks>
Public Class clsWebConfig

    ' Class Constructor
    Public Sub New()

    End Sub

    'Public Interface
    Public Function GetWebConfig(ByVal strKey As String) As String
        Dim strValue As String
        Dim objConfig As System.Configuration.Configuration

        Try
            Dim strVirtualPath As String = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath
            objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(strVirtualPath)

            Dim objSetting As System.Configuration.KeyValueConfigurationElement
            objSetting = objConfig.AppSettings.Settings(strKey)

            strValue = objSetting.Value.ToString

        Catch ex As Exception
            strValue = ex.Message
        End Try

        Return strValue

    End Function

End Class
