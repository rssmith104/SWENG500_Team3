Imports System
Imports System.ComponentModel
Imports System.Collections
Imports System.Diagnostics
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports Microsoft.Win32

''' -----------------------------------------------------------------------------
''' Class	 : clsData_DB
''' -----------------------------------------------------------------------------
''' <summary>
''' </summary>
''' <remarks>Class Used by the RMS application for interfacing
''' with the Recipe database
''' </remarks>
''' <history>
''' </history>
''' -----------------------------------------------------------------------------
Public Class clsData_DB
    Implements IComponent

#Region "Attributes"

    ' connection to data source
    Private SQLcon As SqlConnection
    Private SQLTrans As SqlTransaction
    Private SQLTransCmd As SqlCommand
    Private m_strConnection As String

#End Region

#Region "Constructors"

    Public Sub New(ByVal strCon As String)
        m_strConnection = strCon
    End Sub 'New

#End Region

#Region "Public Interfaces"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' BeginTrans() - Open a SQL Connection under transaction control.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	RSmith	9/15/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub BeginTrans()
        If Not (SQLcon Is Nothing) Then
            If Not (SQLTrans Is Nothing) Then
                Dim sqlEx As New Exception("Already in a transaction")
                sqlEx.Source = "BeginTrans()"
                Throw sqlEx
            End If
        End If
        Open()
        SQLTrans = SQLcon.BeginTransaction()
    End Sub 'BeginTrans

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' RollBackTrans() - Perform Rollback based on transaction control.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	RSmith	9/15/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub RollBackTrans()
        If Not (SQLTrans Is Nothing) Then
            SQLTrans.Rollback()
            SQLTrans = Nothing
        End If
    End Sub 'RollBackTrans

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' CommitTrans() - Commit database updates under transactional control
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	RSmith	9/15/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub CommitTrans()
        If Not (SQLTrans Is Nothing) Then
            SQLTrans.Commit()
            SQLTrans = Nothing
        End If
    End Sub 'CommitTrans

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' StartTransProc(ByVal procName As String) As Integer
    ''' </summary>
    ''' <param name="procName">Stored Procedure Name As String.</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	RSmith	9/15/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overloads Function StartTransProc(ByVal procName As String) As Integer
        Return StartTransProc(procName, Nothing)
    End Function    'StartTransProc

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' StartTransProc(ByVal procName As String, ByVal prams() As SqlParameter) As Integer
    ''' </summary>
    ''' <param name="procName"></param>
    ''' <param name="prams"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	RSmith	9/15/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overloads Function StartTransProc(ByVal procName As String, ByVal prams() As SqlParameter) As Integer
        If SQLTrans Is Nothing Then
            Dim sqlEx As New Exception("BeginTrans() must be called first.")
            sqlEx.Source = "int StartTransProc(string procName, SqlParameter[] prams)"
            Throw sqlEx
        End If

        SQLTransCmd = CreateCommand(procName, prams)
        SQLTransCmd.Transaction = SQLTrans

        'Execute Query
        SQLTransCmd.ExecuteNonQuery()
        Return CInt(SQLTransCmd.Parameters("ReturnValue").Value)
    End Function    'StartTransProc

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' StartTransProc(ByVal ci As commandInfo, ByRef dt As DataTable)
    ''' </summary>
    ''' <param name="ci"></param>
    ''' <param name="dt"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	RSmith	9/15/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overloads Sub StartTransProc(ByVal ci As commandInfo, ByRef dt As DataTable)
        If SQLTrans Is Nothing Then
            Dim sqlEx As New Exception("BeginTrans() must be called first.")
            sqlEx.Source = "int StartTransProc(string procName, SqlParameter[] prams, DataTable dt, commandInfo ci)"
            Throw sqlEx
        End If

        Dim SQLDA As New SqlDataAdapter
        Open()

        Dim intCount As Integer = ci.Count
        Dim intLoop As Integer
        For intLoop = 0 To intCount - 1
            Dim pnc As procNcommandType = ci(intLoop)
            Select Case pnc.commandType
                Case procNcommandType.eCommandType.InsertCommand
                    SQLDA.InsertCommand = CreateCommand(pnc.procName, pnc.SQLPrams)
                    SQLDA.InsertCommand.Transaction = SQLTrans
                    Exit For
                Case procNcommandType.eCommandType.UpdateCommand
                    SQLDA.UpdateCommand = CreateCommand(pnc.procName, pnc.SQLPrams)
                    SQLDA.UpdateCommand.Transaction = SQLTrans
                    Exit For
                Case procNcommandType.eCommandType.DeleteCommand
                    SQLDA.DeleteCommand = CreateCommand(pnc.procName, pnc.SQLPrams)
                    SQLDA.DeleteCommand.Transaction = SQLTrans
                    Exit For
            End Select
        Next intLoop

        'update Datatable
        SQLDA.Update(dt)
        'identCol = null;//set back to null after update
        SQLDA.Dispose()
    End Sub 'StartTransProc

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' RunProc(ByVal procName As String) As Integer
    ''' </summary>
    ''' <param name="procName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	RSmith	9/15/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overloads Sub RunProc(ByVal procName As String)
        Dim dataTable = New DataTable("tempName")

        RunProc(procName, dataTable)
    End Sub    'RunProc

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' RunProc(ByVal procName As String, ByVal prams() As SqlParameter) As Integer
    ''' </summary>
    ''' <param name="procName"></param>
    ''' <param name="prams"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	RSmith	9/15/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overloads Function RunProc(ByVal procName As String, ByVal prams() As SqlParameter) As Integer
        Dim SQLcmd As SqlCommand = CreateCommand(procName, prams)

        'check to see if currently in transaction
        If Not (SQLTrans Is Nothing) Then
            SQLcmd.Transaction = SQLTrans
        End If

        SQLcmd.ExecuteNonQuery()
        Return CInt(SQLcmd.Parameters("ReturnValue").Value)
    End Function    'RunProc

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' RunProc(ByVal procName As String, ByRef dataSet As DataSet, ByVal tableName As String)
    ''' </summary>
    ''' <param name="procName"></param>
    ''' <param name="dataSet"></param>
    ''' <param name="tableName"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	RSmith	9/15/2015	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overloads Sub RunProc(ByVal procName As String, ByRef dataSet As DataSet, ByVal tableName As String)
        Dim SQLCMD As SqlCommand = CreateCommand(procName, Nothing)
        Dim SQLDA As New SqlDataAdapter(SQLCMD)
        If dataSet Is Nothing Then     'check for null object
            dataSet = New DataSet
        End If
        SQLDA.Fill(dataSet, tableName)
        SQLDA.Dispose()
    End Sub 'RunProc

    Public Overloads Sub RunProc(ByVal procName As String, ByRef dataSet As DataSet, ByVal startRecord As Integer, ByVal maxRecords As Integer, ByVal tableName As String)
        Dim SQLCMD As SqlCommand = CreateCommand(procName, Nothing)
        Dim SQLDA As New SqlDataAdapter(SQLCMD)
        If dataSet Is Nothing Then     'check for null object
            dataSet = New DataSet
        End If
        SQLDA.Fill(dataSet, startRecord, maxRecords, tableName)
        SQLDA.Dispose()
    End Sub 'RunProc

    Public Overloads Sub RunProc(ByVal procName As String, ByRef dataTable As DataTable, ByVal tableName As String)
        Dim SQLCMD As SqlCommand = CreateCommand(procName, Nothing)
        Dim SQLDA As New SqlDataAdapter(SQLCMD)
        If dataTable Is Nothing Then       'check for null object
            dataTable = New DataTable(tableName)
        End If
        SQLDA.Fill(dataTable)
        SQLDA.Dispose()
    End Sub 'RunProc

    Public Overloads Sub RunProc(ByVal procName As String, ByRef dataTable As DataTable)
        'Make sure it's not null
        If dataTable Is Nothing Then
            dataTable = New DataTable("tempName")
        End If

        RunProc(procName, dataTable, dataTable.TableName)
    End Sub 'RunProc

    Public Overloads Sub RunProc(ByVal procName As String, ByRef dataReader As SqlDataReader, ByVal cmdBehavior As CommandBehavior)
        RunProc(procName, Nothing, dataReader, cmdBehavior)
    End Sub 'RunProc

    Public Overloads Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataReader As SqlDataReader, ByVal cmdBehavior As CommandBehavior)
        Dim SQLcmd As SqlCommand = CreateCommand(procName, prams)

        'check to see if currently in transaction
        If Not (SQLTrans Is Nothing) Then
            SQLcmd.Transaction = SQLTrans
        End If

        dataReader = SQLcmd.ExecuteReader(cmdBehavior)
    End Sub 'RunProc

    Public Overloads Sub RunProc(ByVal procName As String, ByRef dataReader As SqlDataReader)
        RunProc(procName, dataReader, CommandBehavior.CloseConnection)
    End Sub 'RunProc

    Public Overloads Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataReader As SqlDataReader)
        RunProc(procName, prams, dataReader, CommandBehavior.CloseConnection)
    End Sub 'RunProc

    Public Overloads Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataSet As DataSet, ByVal tableName As String)
        Dim SQLCMD As SqlCommand = CreateCommand(procName, prams)
        Dim SQLDA As New SqlDataAdapter(SQLCMD)
        If dataSet Is Nothing Then     'check for null object
            dataSet = New DataSet
        End If
        SQLDA.Fill(dataSet, tableName)
        SQLDA.Dispose()
    End Sub 'RunProc

    Public Overloads Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataTable As DataTable, ByVal tableName As String)
        Dim SQLCMD As SqlCommand = CreateCommand(procName, prams)
        Dim SQLDA As New SqlDataAdapter(SQLCMD)
        If dataTable Is Nothing Then       'check for null object
            dataTable = New DataTable(tableName)
        End If
        SQLDA.Fill(dataTable)
        SQLDA.Dispose()
    End Sub 'RunProc

    Public Overloads Sub RunProc(ByVal procName As String, ByVal prams() As SqlParameter, ByRef dataTable As DataTable)
        'Make sure it's not null
        If dataTable Is Nothing Then
            dataTable = New DataTable("tempName")
        End If
        RunProc(procName, prams, dataTable, dataTable.TableName)
    End Sub 'RunProc

    Public Overloads Function RunStoredProc(ByVal procName As String, ByVal prams() As SqlParameter) As SqlDataReader
        Dim objSQLDataReader As SqlDataReader

        objSQLDataReader = RunStoredProc(procName, prams, CommandBehavior.CloseConnection)

        Return objSQLDataReader
    End Function 'RunStoredProc


    Public Overloads Function RunStoredProc(ByVal procName As String, ByVal prams() As SqlParameter, ByVal cmdBehavior As CommandBehavior) As SqlDataReader
        Dim SQLcmd As SqlCommand = CreateCommand(procName, prams)
        Dim DataReader As SqlDataReader

        'check to see if currently in transaction
        If Not (SQLTrans Is Nothing) Then
            SQLcmd.Transaction = SQLTrans
        End If

        DataReader = SQLcmd.ExecuteReader(cmdBehavior)

        Return DataReader
    End Function 'RunProc

    Private Function CreateCommand(ByVal procName As String, ByVal prams() As SqlParameter) As SqlCommand
        ' make sure connection is open
        Me.Open()

        Dim SQLcmd As New SqlCommand(procName, SQLcon)

        SQLcmd.CommandType = CommandType.StoredProcedure
        SQLcmd.CommandTimeout = 600                         ' Timeout = 10 minutes

        ' add proc parameters
        If Not (prams Is Nothing) Then
            Dim parameter As SqlParameter
            For Each parameter In prams
                SQLcmd.Parameters.Add(parameter)
            Next parameter
        End If

        'See if returnvalue parameter has already set
        If SQLcmd.Parameters.Contains("ReturnValue") = False Then
            ' add a return param to the parameter list
            SQLcmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, False, 0, 0, String.Empty, DataRowVersion.Default, Nothing))
        End If

        Return SQLcmd
    End Function    'CreateCommand

    Private Sub Open()

        If SQLcon Is Nothing Then
            SQLcon = New SqlConnection(Me.m_strConnection)
            SQLcon.Open()
        Else
            If Not SQLcon.State = ConnectionState.Open Then
                SQLcon.Open()
            End If
        End If     'open the connection
    End Sub 'Open

    Public Sub Close()
        If Not (SQLcon Is Nothing) Then
            If SQLcon.State <> ConnectionState.Closed Then
                SQLcon.Close()
                SQLcon.Dispose()
                SQLcon = Nothing
            End If
        End If
    End Sub 'Close

    '/ <summary>
    '/ Make input param.
    '/ </summary>
    '/ <param name="ParamName">Name of param.</param>
    '/ <param name="DbType">Param type.</param>
    '/ <param name="Size">Param size.</param>
    '/ <param name="Value">Param value.</param>
    '/ <returns>New parameter.</returns>
    Public Function MakeInParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Integer, ByVal Value As Object) As SqlParameter
        Return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value)
    End Function    'MakeInParam

    '/ <summary>
    '/ Make input param.
    '/ </summary>
    '/ <param name="ParamName">Name of param.</param>
    '/ <param name="DbType">Param type.</param>
    '/ <param name="Size">Param size.</param>
    '/ <returns>New parameter.</returns>
    Public Shared Function MakeOutParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Integer) As SqlParameter
        Return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, Nothing)
    End Function    'MakeOutParam

    '/ <summary>
    '/ Make input param. for a DataTable update with a DataAdapter
    '/ </summary>
    '/ <param name="ParamName">Name of param.</param>
    '/ <param name="DbType">Param type.</param>
    '/ <param name="Size">Param size.</param>
    '/ <param name="srcCol">Source Column</param>
    '/ <returns>New parameter.</returns>
    Public Shared Function MakeInDTParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Integer, ByVal srcCol As String) As SqlParameter
        Return New SqlParameter(ParamName, DbType, Size, srcCol)
    End Function    'MakeInDTParam

    '/ <summary>
    '/ Make Output param. for a DataTable update with a DataAdapter
    '/ </summary>
    '/ <param name="ParamName">Name of param.</param>
    '/ <param name="DbType">Param type.</param>
    '/ <param name="Size">Param size.</param>
    '/ <param name="srcCol">Source Column</param>
    '/ <returns>New parameter.</returns>
    Public Shared Function MakeOutDTParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Integer, ByVal scrCol As String) As SqlParameter
        Dim SQLpram As New SqlParameter(ParamName, DbType, Size, scrCol)
        SQLpram.Direction = ParameterDirection.Output
        Return SQLpram
    End Function    'MakeOutDTParam

    '/ <summary>
    '/ Make stored procedure param.
    '/ </summary>
    '/ <param name="ParamName">Name of param.</param>
    '/ <param name="DbType">Param type.</param>
    '/ <param name="Size">Param size.</param>
    '/ <param name="Direction">Parm direction.</param>
    '/ <param name="Value">Param value.</param>
    '/ <returns>New parameter.</returns>
    Public Shared Function MakeParam(ByVal ParamName As String, ByVal DbType As SqlDbType, ByVal Size As Int32, ByVal Direction As ParameterDirection, ByVal Value As Object) As SqlParameter
        Dim param As SqlParameter

        If Size > 0 Then
            param = New SqlParameter(ParamName, DbType, Size)
        Else
            param = New SqlParameter(ParamName, DbType)
        End If
        param.Direction = Direction
        If Not (Direction = ParameterDirection.Output And Value Is Nothing) Then
            param.Value = Value
        End If
        Return param
    End Function    'MakeParam

    Public Overloads Function RunCommand(ByVal strSQLCmd As String) As SqlDataReader
        Dim objOleDBCmd As SqlCommand = CreateSQLCommand(strSQLCmd, Nothing)
        Dim objSQLDataReader As SqlDataReader

        objSQLDataReader = objOleDBCmd.ExecuteReader(CommandBehavior.CloseConnection)
        Return objSQLDataReader

    End Function 'RunProc

    Private Function CreateSQLCommand(ByVal strSQL As String, ByVal prams() As SqlParameter) As SqlCommand
        ' make sure connection is open
        Me.Open()

        Dim SQLcmd As New SqlCommand(strSQL, SQLcon)

        SQLcmd.CommandType = CommandType.Text
        SQLcmd.CommandTimeout = 600                         ' Timeout = 10 minutes

        ' add proc parameters
        If Not (prams Is Nothing) Then
            Dim parameter As SqlParameter
            For Each parameter In prams
                SQLcmd.Parameters.Add(parameter)
            Next parameter
        End If

        'See if returnvalue parameter has already set
        If SQLcmd.Parameters.Contains("ReturnValue") = False Then
            ' add a return param to the parameter list
            SQLcmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, False, 0, 0, String.Empty, DataRowVersion.Default, Nothing))
        End If

        Return SQLcmd
    End Function    'CreateCommand


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

            If Not (SQLcon Is Nothing) Then
                Close()
            End If

            If Not (SQLTrans Is Nothing) Then
                SQLTrans.Dispose()
                SQLTrans = Nothing
            End If

            If Not (SQLTransCmd Is Nothing) Then
                SQLTransCmd.Dispose()
                SQLTransCmd = Nothing
            End If
        End If

        '** Free unmanaged resources
        '** Set large variables to null

    End Sub

    Protected Overrides Sub Finalize()

        Dispose(False)
        MyBase.Finalize()

    End Sub
#End Region

End Class 'clsData_DB

Public Class procNcommandType

#Region "Private Members - procNcommandType"
    Private _procName As String = ""
    Private _cType As eCommandType
    Private _prams As SqlParameter() = Nothing
#End Region

#Region "Public Members - procNcommandType"
    Public Enum eCommandType
        InsertCommand = 0
        UpdateCommand = 1
        DeleteCommand = 2
    End Enum 'eCommandType

    Public Property procName() As String
        Get
            Return _procName
        End Get
        Set(ByVal Value As String)
            If Value Is Nothing Then 'check for value
                Dim ex As New Exception("Must provide a valid string")
                ex.Source = "procNcommandType.procName"
                Throw ex
            Else
                _procName = Value
            End If
        End Set
    End Property

    Public Property commandType() As eCommandType
        Get
            Return _cType
        End Get

        Set(ByVal Value As eCommandType)
            If Value.ToString().Length = 0 Then 'check for value
                Dim ex As New Exception("Must provide a valid CommandType")
                ex.Source = "procNcommandType.CommandType"
                Throw ex
            Else
                _cType = Value
            End If
        End Set
    End Property

    Public Property SQLPrams() As SqlParameter()
        Get
            Return _prams
        End Get
        Set(ByVal Value As SqlParameter())
            If Value.Length = 0 Then 'check for value
                Dim ex As New Exception("Must provide a valid SqlParameter[]")
                ex.Source = "procNcommandType.SqlParameter"
                Throw ex
            Else
                _prams = Value
            End If
        End Set
    End Property
#End Region

#Region "Constructor - procNcommandType"
    Public Sub New(ByVal procName As String, ByVal ct As CommandType, ByVal prams() As SqlParameter)
        Me.procName = procName
        Me.commandType = ct
        Me.SQLPrams = prams
    End Sub 'New
#End Region

End Class 'procNcommandType

Public Class commandInfo

#Region "Private Members - commandInfo"
    Private procNcommandTypes As New ArrayList
#End Region

#Region "Public Members - commandInfo"
    Default Public ReadOnly Property Item(ByVal index As Integer) As procNcommandType
        Get
            Return CType(procNcommandTypes(index), procNcommandType)
        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            Return procNcommandTypes.Count
        End Get
    End Property

#End Region

#Region "Constructor - commandInfo"
    Public Sub New()
    End Sub 'New 
#End Region

#Region "Public Interfaces - commandInfo"
    Public Sub Clear()
        procNcommandTypes.Clear()
    End Sub 'Clear

    Public Sub Add(ByVal pNc As procNcommandType)
        Dim intCount As Integer = procNcommandTypes.Count
        Dim blnFound As Boolean = False 'default to false
        Dim intLoop As Integer
        For intLoop = 0 To intCount - 1
            If CType(procNcommandTypes(intLoop), procNcommandType).commandType = pNc.commandType Then
                blnFound = True 'commandType exists
            End If
        Next intLoop

        If blnFound Then
            Dim ex As New Exception("The CommandType can only be defined once.")
            ex.Source = "commandInfo.Add(procNcommandType pNc)"
            Throw ex
        Else
            procNcommandTypes.Add(pNc)
        End If
    End Sub 'Add

    Public Sub Remove(ByVal pNc As procNcommandType)
        procNcommandTypes.Remove(pNc)
    End Sub 'Remove
#End Region

End Class 'commandInfo
