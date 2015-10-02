'*************************************************************************************
'
' RMSSeesion - 	RMS System Session Class
' Author -	Randy Smith
' Description -	Used to retain RMS Session state across page boundaries.
' Date -	September 30, 2015
' Examples
'               // Creates new session object and loads it from the stored session
'               Dim objSession As clsRMSSession = Session("RMS")
'
'               // Store values into session object inbetween page jumps
'               objSession.Authenticated(True)
'               objSession.LoggedInUser = "somebody@anywhere.net"
'
'               // Stores Session before redirection
'               Session("RMS") = objSession
'
'*************************************************************************************

Public Class clsRMSSession

#Region "  Private Members..."
    Private m_Authenticated As Boolean
    Private m_LoggedInUser As String
    Private m_RMSFunction As String
#End Region

#Region "  Public Properties..."
    Public Property Authenticated() As Boolean
        Get
            Return Me.m_Authenticated
        End Get
        Set (ByVal value As Boolean)
            Me.m_Authenticated = value
        End Set
    End Property

    Public Property LoggedInUser() As String
        Get
            Return Me.m_LoggedInUser 
        End Get
        Set (ByVal value As String)
            Me.m_LoggedInUser = value
        End Set
    End Property

    Public Property RMSFunction() As String
        Get
            Return Me.m_RMSFunction
        End Get
        Set(ByVal value As String)
            Me.m_RMSFunction = value
        End Set
    End Property

#End Region

#Region "   Constructors..."
    Public Sub New()
        Me.LoggedInUser = ""
        Me.Authenticated = False
        Me.RMSFunction = "NONE"
    End Sub
#End Region

End Class



