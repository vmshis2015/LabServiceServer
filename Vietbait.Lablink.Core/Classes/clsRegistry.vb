Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports Microsoft.Win32
Imports System
Imports System.Security

Namespace clsRegistry
    Public Class clsRegistry
        ' Methods
        Public Function GetReg(ByVal iType As Short, ByVal pSubKeyl1 As String, ByVal pSubKeyl2 As String, ByVal pValueKey As String) As String
            Dim str As String
            Dim name As String = ("SOFTWARE\" & pSubKeyl1 & "\" & pSubKeyl2)
            Try 
                Dim key As RegistryKey
                Select Case iType
                    Case 0
                        key = Registry.ClassesRoot.OpenSubKey(name, False)
                        Exit Select
                    Case 1
                        key = Registry.CurrentUser.OpenSubKey(name, False)
                        Exit Select
                    Case 2
                        key = Registry.LocalMachine.OpenSubKey(name, False)
                        Exit Select
                    Case 3
                        key = Registry.Users.OpenSubKey(name, False)
                        Exit Select
                    Case 4
                        key = Registry.CurrentConfig.OpenSubKey(name, False)
                        Exit Select
                    Case 5
                        key = Registry.DynData.OpenSubKey(name, False)
                        Exit Select
                End Select
                Dim str3 As String = StringType.FromObject(key.GetValue(pValueKey))
                key.Close
                str = str3
            Catch exception1 As SecurityException
                ProjectData.SetProjectError(exception1)
                Dim exception As SecurityException = exception1
                Interaction.MsgBox(exception.Message, MsgBoxStyle.OkOnly, Nothing)
                str = ""
                ProjectData.ClearProjectError
            Catch exception4 As UnauthorizedAccessException
                ProjectData.SetProjectError(exception4)
                Dim exception2 As UnauthorizedAccessException = exception4
                Interaction.MsgBox(exception2.Message, MsgBoxStyle.OkOnly, Nothing)
                str = ""
                ProjectData.ClearProjectError
            Catch exception5 As Exception
                ProjectData.SetProjectError(exception5)
                Dim exception3 As Exception = exception5
                str = ""
                ProjectData.ClearProjectError
            End Try
            Return str
        End Function

        Public Function SaveReg(ByVal iType As Short, ByVal pSubKeyl1 As String, ByVal pSubKeyl2 As String, ByVal pValueKey As String, ByVal pSubValue As String) As Boolean
            Dim flag As Boolean
            Try 
                Dim key As RegistryKey
                Select Case iType
                    Case 0
                        key = Registry.ClassesRoot.OpenSubKey("SOFTWARE", True)
                        Exit Select
                    Case 1
                        key = Registry.CurrentUser.OpenSubKey("SOFTWARE", True)
                        Exit Select
                    Case 2
                        key = Registry.LocalMachine.OpenSubKey("SOFTWARE", True)
                        Exit Select
                    Case 3
                        key = Registry.Users.OpenSubKey("SOFTWARE", True)
                        Exit Select
                    Case 4
                        key = Registry.CurrentConfig.OpenSubKey("SOFTWARE", True)
                        Exit Select
                    Case 5
                        key = Registry.DynData.OpenSubKey("SOFTWARE", True)
                        Exit Select
                End Select
                Dim key2 As RegistryKey = key.CreateSubKey(pSubKeyl1)
                Dim key3 As RegistryKey = key2.CreateSubKey(pSubKeyl2)
                key3.SetValue(pValueKey, pSubValue)
                key3.Close
                key2.Close
                key.Close
                flag = True
            Catch exception1 As SecurityException
                ProjectData.SetProjectError(exception1)
                Dim exception As SecurityException = exception1
                Interaction.MsgBox(exception.Message, MsgBoxStyle.OkOnly, Nothing)
                flag = False
                ProjectData.ClearProjectError
            Catch exception4 As UnauthorizedAccessException
                ProjectData.SetProjectError(exception4)
                Dim exception2 As UnauthorizedAccessException = exception4
                Interaction.MsgBox(exception2.Message, MsgBoxStyle.OkOnly, Nothing)
                flag = False
                ProjectData.ClearProjectError
            Catch exception5 As Exception
                ProjectData.SetProjectError(exception5)
                Dim exception3 As Exception = exception5
                flag = False
                ProjectData.ClearProjectError
            End Try
            Return flag
        End Function


        ' Fields
        Private Const iHKEY_CLASSES_ROOT As Integer = 0
        Private Const iHKEY_CURRENT_CONFIG As Integer = 4
        Private Const iHKEY_CURRENT_USER As Integer = 1
        Private Const iHKEY_DYN_DATA As Integer = 5
        Private Const iHKEY_LOCAL_MACHINE As Integer = 2
        Private Const iHKEY_USERS As Integer = 3
    End Class
End Namespace

