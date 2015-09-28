Imports System.ComponentModel
Imports System
Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Security
Imports System.Security.Cryptography

<Description("Data security class through data encryption using the DES standard.")> _
Public Class DES_Codec
    Implements IComponent

#Region "Attributes"
    Private m_Key() As Byte = New Byte(7) {}
    Private m_IV() As Byte = New Byte(7) {}
    ' Key Size needs to be 64
    Private m_strKey As String = "^3(%hdGCBE3@945k"
    Private m_CodePrefix As String = "R]"
#End Region

#Region "Constructors"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DES_Codec Class Constructor
    ''' </summary>
    ''' <remarks>
    ''' No Parameters.  Uses default key.
    ''' </remarks>
    ''' <history>
    ''' 	[e35346]	9/8/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Sub New()
        Try
            InitKey(Me.m_strKey)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DES_Codec Class Constructor
    ''' </summary>
    ''' <param name="strKey"></param>
    ''' <remarks>
    ''' Input String - 16-byte key.  Must be 16 bytes or default is used
    ''' </remarks>
    ''' <history>
    ''' 	[e35346]	9/8/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Sub New(ByVal strKey As String)
        ' Key length of 64 bits is the only size supported by the DES encryption
        ' If not a valid size, Use the default key.
        If strKey.Length <> 16 Then
            Throw New System.Exception("Invalid Key Length.  Must be 16 chars.")
        Else
            Try
                InitKey(strKey)
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub

    ''' <summary>
    ''' DES_Codes Class Constructor
    ''' 
    ''' This version of the constructor takes a Key and a encoded preface value.
    '''
    ''' </summary>
    ''' <param name="strKey"></param>
    ''' <param name="strPrefix"></param>
    ''' <remarks></remarks>
    Sub New(ByVal strKey As String, ByVal strPrefix As String)

        If strKey.Length <> 16 Then
            Throw New System.Exception("Invalid Key Length.  Must be 16 chars.")
        Else
            Try
                InitKey(strKey)
                Me.m_CodePrefix = strPrefix
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub
#End Region

#Region "Public Interface "

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' EncodeString(ByVal strData As String) As String
    ''' </summary>
    ''' <param name="strData">String to be encoded</param>
    ''' <returns>Encoded string</returns>
    ''' <remarks>
    ''' Using DES encryption, this function takes in a string and returns the encoded
    ''' results.
    ''' </remarks>
    ''' <history>
    '''     RSmith	9/28/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    <Description("my description")> _
    Public Function EncodeString(ByVal strData As String) As String
        Dim strResult As String
        '04/19/2005 -RD- commented out unused variable -- Dim strLocalData As String
        Dim strSize As String

        If Me.IsEncrypted(strData) = True Then
            Throw New System.Exception("Invalid Operation.  Attempt to Encode an Encoded Element.")
            strResult = ""
        Else
            If strData.Length <= 92160 Then

                Try
                    strSize = String.Format("{0,5:00000}", strData.Length)
                    strData = strSize + strData

                    Dim rbData As Byte() = New Byte(strData.Length) {}
                    Dim aEnc As ASCIIEncoding = New ASCIIEncoding
                    aEnc.GetBytes(strData, 0, strData.Length, rbData, 0)

                    Dim descsp As DESCryptoServiceProvider = New DESCryptoServiceProvider
                    Dim desEncrypt As ICryptoTransform = descsp.CreateEncryptor(Me.m_Key, Me.m_IV)

                    Dim mStream As MemoryStream = New MemoryStream(rbData)
                    Dim cs As CryptoStream = New CryptoStream(mStream, desEncrypt, CryptoStreamMode.Read)
                    Dim mOut As MemoryStream = New MemoryStream

                    Dim bytesRead As Integer
                    Dim output() As Byte = New Byte(1024) {}

                    Do
                        bytesRead = cs.Read(output, 0, 1024)
                        If bytesRead <> 0 Then
                            mOut.Write(output, 0, bytesRead)
                        End If
                    Loop While bytesRead > 0

                    If mOut.Length = 0 Then
                        strResult = ""
                    Else
                        ' Updated to prefix with the encode Prefix
                        strResult = Me.m_CodePrefix & Convert.ToBase64String(mOut.GetBuffer(), 0, CType(mOut.Length, Integer))
                    End If
                Catch ex As Exception
                    ' If an exception is found, throw it back to the calling
                    ' routine.
                    Throw ex
                    strResult = ""
                End Try

            Else
                Throw New System.Exception("Data String too large. Keep within 90Kb.")
                strResult = ""
            End If

        End If

        Return strResult

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DecodeString(ByVal strData As String) As String
    ''' </summary>
    ''' <param name="strData">String to be decoded</param>
    ''' <returns>A string containing the results of the decode operation.</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    '''     RSmith	9/28/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function DecodeString(ByVal strData As String) As String
        Dim strResult As String
        Dim iReturn As Integer = 0
        Dim descsp As DESCryptoServiceProvider = New DESCryptoServiceProvider
        Dim desDecrypt As ICryptoTransform = descsp.CreateDecryptor(Me.m_Key, Me.m_IV)
        Dim mOut As MemoryStream = New MemoryStream
        Dim cs As CryptoStream = New CryptoStream(mOut, desDecrypt, CryptoStreamMode.Write)
        Dim bPlain() As Byte = New Byte(strData.Length) {}

        If Me.IsEncrypted(strData) = True Then

            ' Remove the CodePrefix before decoding
            strData = strData.Substring(Me.m_CodePrefix.Length)
            Try
                bPlain = Convert.FromBase64CharArray(strData.ToCharArray(), 0, strData.Length)
            Catch ex As Exception
                Throw ex
                strResult = ""
                Return strResult
            End Try

            Dim lRead As Long = 0
            Dim lTotal As Long = strData.Length

            Try
                While lTotal >= lRead
                    cs.Write(bPlain, 0, CType(bPlain.Length, Integer))
                    lRead = lRead + mOut.Length
                End While

                Dim aEnc As ASCIIEncoding = New ASCIIEncoding
                strResult = aEnc.GetString(mOut.GetBuffer(), 0, CType(mOut.Length, Integer))

                Dim strLen As String = strResult.Substring(0, 5)
                Dim nLen As Integer = Convert.ToInt32(strLen)
                strResult = strResult.Substring(5, nLen)
                iReturn = CType(mOut.Length, Integer)
                Return strResult

            Catch ex As Exception
                Throw ex
                Return strResult
            End Try
        Else
            Throw New System.Exception("Invalid Operation.  Attempt to DeEncode a Non-Encoded Element.")
            strResult = ""
        End If

        DecodeString = strResult

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' EncodeXMLDocument(ByVal oXmlDoc As XmlDocument) As String
    ''' </summary>
    ''' <param name="oXmlDoc">XmlDocument</param>
    ''' <returns>Encoded XML Document in the form of a string</returns>
    ''' <remarks>
    ''' This version encodes everything in the XML document including tags
    ''' as well as tag values and tag atributes
    ''' </remarks>
    ''' <history>
    '''     RSmith	9/28/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function EncodeXMLDocument(ByVal oXmlDoc As XmlDocument) As String

        Dim strEncodedXMLString As String
        Dim sw As StringWriter = New StringWriter
        Dim xw As XmlTextWriter = New XmlTextWriter(sw)

        Try
            oXmlDoc.WriteTo(xw)
            strEncodedXMLString = Me.EncodeString(sw.ToString())

        Catch ex As Exception
            Throw ex

        Finally
            sw = Nothing
            xw = Nothing
        End Try

        Return strEncodedXMLString

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DecodeXMLDocument(ByVal strSrcString As String) As XmlDocument
    ''' </summary>
    ''' <param name="strSrcString">Encoded string</param>
    ''' <returns>XMLDocument</returns>
    ''' <remarks>
    ''' The encoded string will need to adhere to proper XML Structure
    ''' </remarks>
    ''' <history>
    '''     RSmith	9/28/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function DecodeXMLDocument(ByVal strSrcString As String) As XmlDocument

        Dim strResult As String
        Dim oXMLDoc As XmlDocument = New XmlDocument

        Try
            strResult = Me.DecodeString(strSrcString)
            oXMLDoc.Load(New StringReader(strResult))

        Catch ex As Exception
            Throw ex

        Finally
            '''    oXMLDoc = Nothing
        End Try

        Return oXMLDoc

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' EncodeFileStream(ByVal oFsIn As FileStream, ByRef oFsOut As FileStream)
    ''' </summary>
    ''' <param name="oFsIn">File Stream containing data to be encrypted</param>
    ''' <param name="oFsOut">File Stream in which the encrypted data will be placed</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    '''     RSmith	9/28/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub EncodeFileStream(ByVal oFsIn As FileStream, ByRef oFsOut As FileStream)
        Dim bSrc As Byte() = New Byte(oFsIn.Length) {}
        Dim strEncodedString As String
        Dim strOutput As String

        Try
            oFsIn.Read(bSrc, 0, oFsIn.Length)
            strEncodedString = System.Text.ASCIIEncoding.ASCII.GetString(bSrc)
            strOutput = Me.EncodeString(strEncodedString)
            Dim bOut() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(strOutput)

            oFsOut.Write(bOut, 0, bOut.Length)
            oFsOut.Flush()

        Catch ex As Exception
            Throw ex
        Finally
            bSrc = Nothing
        End Try

    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DecodeFileStream(ByRef oFsIn As FileStream, ByRef oFsOut As FileStream) As FileStream
    ''' </summary>
    ''' <param name="oFsIn">Input File Stream in encrypted format</param>
    ''' <param name="oFsOut">Output File Stream to which the decrypted results will be placed.</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    '''     RSmith	9/28/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub DecodeFileStream(ByRef oFsIn As FileStream, ByRef oFsOut As FileStream)
        Dim bSrc As Byte() = New Byte(oFsIn.Length - 1) {}
        Dim strDecodeString As String
        Dim strOutput As String

        Try
            oFsIn.Read(bSrc, 0, oFsIn.Length)
            strDecodeString = System.Text.ASCIIEncoding.ASCII.GetString(bSrc)
            strOutput = Me.DecodeString(strDecodeString)
            Dim bOut() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(strOutput)

            oFsOut.Write(bOut, 0, bOut.Length)

        Catch ex As Exception
            Throw ex
        Finally
            bSrc = Nothing
        End Try

    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' EncodeXML(ByVal oXmlDoc As XmlDocument) As XmlDocument
    ''' </summary>
    ''' <param name="oXmlDoc">Input XMLDocument Object containing data to be
    ''' encrypted.
    ''' </param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Encrypts on node values and attributes.  Keys remain untouched
    ''' </remarks>
    ''' <history>
    '''     RSmith	9/28/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Function EncodeXML(ByVal oXmlDoc As XmlDocument) As XmlDocument

        Try
            Me.UpdateXMLNode(oXmlDoc.DocumentElement, True)

        Catch ex As Exception
            Throw ex
        End Try

        Return oXmlDoc

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DecodeXML(ByVal oXmlDoc As XmlDocument) As XmlDocument
    ''' </summary>
    ''' <param name="oXmlDoc">Input XMLDocument Object containing data to be
    ''' encrypted.
    ''' </param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Decrypts node values and attributes..  Keys remain untouched
    ''' </remarks>
    ''' <history>
    '''     RSmith	9/28/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------

    Public Function DecodeXML(ByVal oXmlDoc As XmlDocument) As XmlDocument

        Try
            Me.UpdateXMLNode(oXmlDoc.DocumentElement, False)

        Catch ex As Exception
            Throw ex
        End Try

        Return oXmlDoc

    End Function

    ''' <summary>
    ''' IsEncrypted - Indicates whether a specified string is already encoded or not.
    '''
    ''' </summary>
    ''' <param name="strSrcString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsEncrypted(ByVal strSrcString As String) As Boolean
        If strSrcString.StartsWith(Me.m_CodePrefix) Then
            Return True
        Else
            Return False
        End If
    End Function

#End Region

#Region "Private Interface"

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' InitKey(ByVal strKey As String) As Boolean
    ''' </summary>
    ''' <param name="strKey">Encryption Key</param>
    ''' <returns>Boolean outlining the status of the operation</returns>
    ''' <remarks>
    ''' Must be a 16-byte key.
    ''' </remarks>
    ''' <history>
    '''     RSmith	9/28/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Function InitKey(ByVal strKey As String) As Boolean

        Try
            Dim bp() As Byte = New Byte(strKey.Length) {}
            Dim aEnc As ASCIIEncoding = New ASCIIEncoding
            aEnc.GetBytes(strKey, 0, strKey.Length, bp, 0)

            Dim sha As SHA1CryptoServiceProvider = New SHA1CryptoServiceProvider
            Dim bpHash() As Byte = sha.ComputeHash(bp)

            Dim i As Integer
            For i = 0 To 7
                Me.m_Key(i) = bpHash(i)
            Next

            For i = 8 To 15
                Me.m_IV(i - 8) = bpHash(i)
            Next

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' UpdateXMLNode(ByVal node As XmlNode, ByVal bEncode As Boolean)
    ''' </summary>
    ''' <param name="node">Tree Root to be parsed for values</param>
    ''' <param name="bEncode">Indicates Encode or Decode operation</param>
    ''' <remarks>
    ''' Recursive Alorithm
    ''' </remarks>
    ''' <history>
    '''     RSmith	9/28/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub UpdateXMLNode(ByVal node As XmlNode, ByVal bEncode As Boolean)

        Try
            '            If node.Value <> Nothing Then
            If bEncode Then
                'Encode the value for the node
                If node.Value <> Nothing Then
                    node.Value = Me.EncodeString(node.Value.ToString())
                End If

                'We also need to hit all attributes for this node
                Dim objNodeAttributes As XmlAttributeCollection = node.Attributes
                Dim objNodeAtt As XmlAttribute

                If objNodeAttributes Is Nothing = False Then
                    For Each objNodeAtt In objNodeAttributes
                        objNodeAtt.Value = Me.EncodeString(objNodeAtt.Value.ToString())
                    Next
                End If
            Else
                'Decode the value for the node
                If node.Value <> Nothing Then
                    node.Value = Me.DecodeString(node.Value.ToString())
                End If

                'We also need to hit all attributes for this node
                Dim objNodeAttributes As XmlAttributeCollection = node.Attributes
                Dim objNodeAtt As XmlAttribute

                If objNodeAttributes Is Nothing = False Then
                    For Each objNodeAtt In objNodeAttributes
                        objNodeAtt.Value = Me.DecodeString(objNodeAtt.Value.ToString())
                    Next
                End If
            End If
            '            End If

            node = node.FirstChild
            While Not node Is Nothing
                UpdateXMLNode(node, bEncode)
                node = node.NextSibling
            End While

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "Destructors"


    Public Event Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Implements System.ComponentModel.IComponent.Disposed

    Public Property Site() As System.ComponentModel.ISite Implements System.ComponentModel.IComponent.Site
        Get

        End Get
        Set(ByVal Value As System.ComponentModel.ISite)

        End Set
    End Property

    Public Overloads Sub Dispose() Implements System.ComponentModel.IComponent.Dispose

        Dispose(True)
        GC.SuppressFinalize(Me)

    End Sub

    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
            '** Free managed resources
            On Error Resume Next
            Me.m_Key = Nothing
            Me.m_IV = Nothing
        End If

        '** Free unmanaged resources
        '** Set large variables to null

    End Sub

    Protected Overrides Sub Finalize()

        Dispose(False)
        MyBase.Finalize()

    End Sub


#End Region
End Class