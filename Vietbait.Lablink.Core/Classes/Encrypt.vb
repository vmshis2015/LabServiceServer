Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Windows.Forms

Namespace Encrypt
    Public Class Encrypt
        ' Methods
        Public Sub New(ByVal strCryptoName As String)
            Me.crpSym = SymmetricAlgorithm.Create(strCryptoName)
            Me.ReDimByteArrays
        End Sub

        Protected Friend Function DecryptString(ByVal strCipherText As String, ByVal strEncPass As String) As String
            If (StringType.StrCmp(Me.strPassword, "", False) <> 0) Then
                Me.OpenSaltIVFileAndSetKeyIV
            End If
            Dim encoding2 As New UnicodeEncoding
            Dim encoding As New ASCIIEncoding
            Dim buffer As Byte() = Convert.FromBase64String(strCipherText)
            Dim stream3 As New MemoryStream
            Dim stream2 As New MemoryStream(buffer)
            Dim stream As New CryptoStream(stream2, Me.crpSym.CreateDecryptor, CryptoStreamMode.Read)
            Dim writer As New StreamWriter(stream3)
            Dim reader As New StreamReader(stream)
            Try 
                writer.Write(reader.ReadToEnd)
            Catch exception1 As CryptographicException
                ProjectData.SetProjectError(exception1)
                Dim exception As CryptographicException = exception1
                Throw New CryptographicException
                ProjectData.ClearProjectError
            Finally
                writer.Close
                stream.Close
            End Try
            Return encoding2.GetString(stream3.ToArray)
        End Function

        Protected Friend Function EncryptString(ByVal strPlainText As String, ByVal strEncPass As String) As String
            If (StringType.StrCmp(Me.strPassword, "", False) <> 0) Then
                Me.OpenSaltIVFileAndSetKeyIV
            End If
            Dim encoding2 As New UnicodeEncoding
            Dim encoding As New ASCIIEncoding
            Dim bytes As Byte() = encoding2.GetBytes(strPlainText)
            Dim stream2 As New MemoryStream
            Dim stream As New CryptoStream(stream2, Me.crpSym.CreateEncryptor, CryptoStreamMode.Write)
            stream.Write(bytes, 0, bytes.Length)
            stream.FlushFinalBlock
            stream.Close
            Return Convert.ToBase64String(stream2.ToArray)
        End Function

        Public Function GiaiMa(ByVal chuoi As String) As String
            Dim str As String
            Try 
                Dim encrypt As New Encrypt("Rijndael")
                Dim encrypt2 As Encrypt = encrypt
                encrypt2.SaltIVFile = (Environment.CurrentDirectory & "\data.dat")
                encrypt2.Password = Me.sPwd
                encrypt2 = Nothing
                str = encrypt.DecryptString(chuoi, Me.Password).Replace("'", "0")
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                MessageBox.Show(exception.Message)
                ProjectData.ClearProjectError
            End Try
            Return str
        End Function

        Public Function Mahoa(ByVal chuoi As String) As String
            Dim str As String
            Try 
                Dim encrypt As New Encrypt("Rijndael")
                Dim encrypt2 As Encrypt = encrypt
                encrypt2.SaltIVFile = (Environment.CurrentDirectory & "\data.dat")
                encrypt2.Password = Me.sPwd
                encrypt2 = Nothing
                str = encrypt.EncryptString(chuoi, Me.Password).Replace("'", "0")
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                MessageBox.Show(exception.Message)
                ProjectData.ClearProjectError
            End Try
            Return str
        End Function

        Private Sub OpenSaltIVFileAndSetKeyIV()
            Me.ReDimByteArrays
            Dim stream As New FileStream(Me.strSaltIVFile, FileMode.Open, FileAccess.Read)
            stream.Read(Me.abytSalt, 0, Me.abytSalt.Length)
            stream.Read(Me.abytIV, 0, Me.abytIV.Length)
            stream.Close
            Me.abytKey = New PasswordDeriveBytes(Me.strPassword, Me.abytSalt).GetBytes(Me.abytKey.Length)
            Me.crpSym.Key = Me.abytKey
            Me.crpSym.IV = Me.abytIV
        End Sub

        Private Sub ReDimByteArrays()
            If (Me.crpSym.GetType Is GetType(RijndaelManaged)) Then
                Me.abytKey = New Byte(&H20  - 1) {}
                Me.abytSalt = New Byte(&H10  - 1) {}
                Me.abytIV = New Byte(&H10  - 1) {}
            Else
                Me.abytKey = New Byte(&H18  - 1) {}
                Me.abytSalt = New Byte(12  - 1) {}
                Me.abytIV = New Byte(8  - 1) {}
            End If
        End Sub


        ' Properties
        Public Property Password As String
            Get
                Return Me.strPassword
            End Get
            Set(ByVal Value As String)
                Me.strPassword = Value
            End Set
        End Property

        Public Property SaltIVFile As String
            Get
                Return Me.strSaltIVFile
            End Get
            Set(ByVal Value As String)
                If Not File.Exists(Value) Then
                    Throw New FileNotFoundException("Không tìm thấy file data.dat")
                End If
                Me.strSaltIVFile = Value
            End Set
        End Property


        ' Fields
        Private abytIV As Byte()
        Private abytKey As Byte()
        Private abytSalt As Byte()
        Private crpSym As SymmetricAlgorithm
        Public sPwd As String = "DVC@COMPANY"
        Private strPassword As String = ""
        Private strSaltIVFile As String = ""
        Private strSourceFile As String = ""
    End Class
End Namespace

