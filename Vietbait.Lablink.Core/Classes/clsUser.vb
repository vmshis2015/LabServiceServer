
'-------------------------------------------------------------------------------------------------------
'Mục đích: Lớp User nhằm xử lý tất cả các nghiệp vụ liên quan đến người dùng
'Người tạo: CuongDV
'Ngày tạo :09/03/2005
'-------------------------------------------------------------------------------------------------------
Imports System.Data.SqlClient

Public Class clsUser
    Dim mv_sSql As String

    Public Sub New()
        Try
            If Not gv_ConnectSuccess Then
                gv_ConnectSuccess = KhoiTaoKetNoi()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function InsertUser (ByVal pv_sUID As String, ByVal pv_sFullName As String, _
                                ByVal pv_sDepart As String, ByVal pv_sDesc As String) As Boolean
        Dim sv_oCmd As SqlCommand
        mv_sSql = "INSERT INTO TBL_USERS(PK_sUID,sPWD,sFullName,sDepart,sDesc) VALUES(N'" & _
                  pv_sUID & "','',N'" & pv_sFullName & "',N'" & pv_sDepart & "',N'" & pv_sDesc & "')"
        Try
            sv_oCmd = New SqlCommand (mv_sSql, gv_oSqlCnn)
            sv_oCmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function UpdateUser (ByVal pv_sUID As String, ByVal pv_sFullName As String, _
                                ByVal pv_sDepart As String, ByVal pv_sDesc As String) As Boolean
        Dim sv_oCmd As SqlCommand
        mv_sSql = "UPDATE TBL_USERS SET sFullName=N'" & pv_sFullName & "',sDepart=N'" & pv_sDepart & _
                  "',sDesc=N'" & pv_sDesc & "' WHERE upper(PK_sUID)=N'" & pv_sUID & "'"
        Try
            sv_oCmd = New SqlCommand (mv_sSql, gv_oSqlCnn)
            sv_oCmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function bDeleteUser (ByVal pv_sUID As String) As Boolean
        Dim sv_oCmd As SqlCommand
        mv_sSql = "DELETE FROM  TBL_USERS  WHERE upper(PK_sUID)=N'" & pv_sUID & "'"
        Try
            sv_oCmd = New SqlCommand (mv_sSql, gv_oSqlCnn)
            sv_oCmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function dsGetUserInfor (ByVal pv_sUID As String) As DataSet
        Dim sv_DS As New DataSet
        Dim sv_DA As SqlDataAdapter
        mv_sSql = "SELECT * FROM  TBL_USERS  WHERE upper(PK_sUID)=N'" & pv_sUID & "'"
        Try
            sv_DA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
            sv_DA.Fill (sv_DS, "TBL_USERS")
            Return sv_DS
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function bChangePassword (ByVal pv_sUID As String, ByVal pv_sNewPWD As String) As Boolean
        Dim sv_oCmd As SqlCommand
        mv_sSql = "UPDATE TBL_USERS SET sPWD=N'" & pv_sNewPWD & "' WHERE upper(PK_sUID)=N'" & pv_sUID & "'"
        Try
            sv_oCmd = New SqlCommand (mv_sSql, gv_oSqlCnn)
            sv_oCmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function bLoginSuccess (ByVal pv_sUID As String, ByVal pv_sPWD As String) As Boolean
        Dim sv_oDS As New DataSet
        Dim sv_oDA As SqlDataAdapter
        If bIsExistedAmin (pv_sUID) Then
            gv_bAdminLogin = True
            Return bLoginSuccessAdmin (pv_sUID, pv_sPWD)

        Else
            gv_bAdminLogin = False
            mv_sSql = "SELECT * FROM TBL_USERS  WHERE upper(PK_sUID)=N'" & pv_sUID & "' AND sPWD=N'" & pv_sPWD & _
                      "' AND FP_sBranchID=N'" & gv_sBranchID & "'"
            Try
                sv_oDA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
                sv_oDA.Fill (sv_oDS, "TBL_USERS")
                If sv_oDS.Tables (0).Rows.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End If
    End Function

    Public Function dsGetAllUser() As DataSet
        Dim sv_oDS As New DataSet
        Dim sv_oDA As SqlDataAdapter
        Try
            mv_sSql = "SELECT * from TBL_USERS order by PK_sUID ASC"
            sv_oDA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
            sv_oDA.Fill (sv_oDS, "TBL_USERS")
            Return sv_oDS
        Catch ex As Exception
        End Try
    End Function

    Public Function bIsExisted (ByVal pv_sUID As String) As Boolean
        Dim sv_oDS As New DataSet
        Dim sv_oDA As SqlDataAdapter
        Try
            If bIsExistedAmin (pv_sUID) Then
                Return True
            End If
            mv_sSql = "SELECT * from TBL_USERS WHERE upper(PK_sUID) =N'" & pv_sUID & "' AND FP_sBranchID=N'" & _
                      gv_sBranchID & "'"
            sv_oDA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
            sv_oDA.Fill (sv_oDS, "TBL_USERS")
            If sv_oDS.Tables (0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function bIsAdmin (ByVal pv_sUID As String) As Boolean
        Dim sv_oDS As New DataSet
        Dim sv_oDA As SqlDataAdapter
        Try
            mv_sSql = "SELECT * from TBL_ADMINISTRATOR WHERE upper(PK_sAdminID) =N'" & pv_sUID & "' AND FP_sBranchID=N'" & _
                      gv_sBranchID & "'"
            sv_oDA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
            sv_oDA.Fill (sv_oDS, "TBL_ADMINISTRATOR")
            If sv_oDS.Tables (0).Rows.Count > 0 Then
                Return True
            Else
                sv_oDS.Tables.Clear()
                mv_sSql = "SELECT * from TBL_USERS WHERE upper(PK_sUID) =N'" & pv_sUID & "' AND FP_sBranchID=N'" & _
                          gv_sBranchID & "' AND iSecurityLevel=1"
                sv_oDA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
                sv_oDA.Fill (sv_oDS, "TBL_USERS")
                If sv_oDS.Tables (0).Rows.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function bIsExistedAmin (ByVal pv_sUID As String) As Boolean
        Dim sv_oDS As New DataSet
        Dim sv_oDA As SqlDataAdapter
        Try
            mv_sSql = "SELECT * from tbl_Administrator WHERE upper(PK_sAdminID) =N'" & pv_sUID & "'" & _
                      " AND FP_sBranchID=N'" & gv_sBranchID & "'"
            sv_oDA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
            sv_oDA.Fill (sv_oDS, "tbl_Administrator")
            If sv_oDS.Tables (0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    '------------------------------------------------------------------------------------------------------------
    'Mục đích        : Thay đổi mật khẩu quản trị
    'Đầu vào          :Mã người dùng, mật khẩu mới
    'Đầu ra            :Thành công=True. Ngược lại=False
    'Người tạo       :CuongDV
    'Ngày tạo         :09/03/2005
    'Nhật kí sửa đổi:
    '------------------------------------------------------------------------------------------------------------
    Public Function bChangePasswordForAdmin (ByVal pv_sUID As String, ByVal pv_sNewPWD As String) As Boolean
        Dim sv_oCmd As SqlCommand
        Dim sv_oEncrypt As New Encrypt.Encrypt (gv_sSymmetricAlgorithmName)
        mv_sSql = "UPDATE TBL_ADMINISTRATOR SET sPWD=N'" & pv_sNewPWD & "' WHERE PK_sAdminID=N'" & pv_sUID & "'" & _
                  " AND FP_sBranchID=N'" & gv_sBranchID & "'"
        Try
            sv_oCmd = New SqlCommand (mv_sSql, gv_oSqlCnn)
            sv_oCmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    '------------------------------------------------------------------------------------------------------------
    'Mục đích        : Kiểm tra sự tồn tại của một Account Admin trong CSDL
    'Đầu vào          :UserName của Admin, Password
    'Đầu ra            :Thành công=True. Ngược lại=False
    'Người tạo       :CuongDV
    'Ngày tạo         :09/03/2005
    'Nhật kí sửa đổi:
    '------------------------------------------------------------------------------------------------------------
    Public Function bLoginSuccessAdmin (ByVal pv_sUID As String, ByVal pv_sPWD As String) As Boolean
        Dim sv_oDS As New DataSet
        Dim sv_oDA As SqlDataAdapter
        mv_sSql = "SELECT * FROM tbl_Administrator  WHERE PK_sAdminID=N'" & pv_sUID & "' AND sPWD=N'" & pv_sPWD & "'" & _
                  " AND FP_sBranchID=N'" & gv_sBranchID & "'"
        Try
            sv_oDA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
            sv_oDA.Fill (sv_oDS, "tbl_Administrator")
            If sv_oDS.Tables (0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
