Imports System.Reflection
Imports System.IO

Public Class DVC_MenuItemForToolBar
    Public Function GetMenuItem (ByVal pv_sRoleName As String, ByVal pv_iRole As Integer, _
                                 ByVal pv_sDLLName As String, ByVal pv_sFormName As String, _
                                 ByVal pv_sAssemblyName As String, ByVal pv_sParameterList As String, _
                                 ByVal pv_sIconPath As String, ByVal pv_intShorCutKey As Integer) As _MenuItem
        Dim fv_oMenuItem As New _MenuItem (pv_sRoleName, pv_intShorCutKey)
        With fv_oMenuItem
            .mv_sParameterList = pv_sParameterList
            .mv_sRoleName = pv_sRoleName
            .mv_iID = pv_iRole
            .mv_sAssemblyName = pv_sAssemblyName
            .mv_sDLLName = pv_sDLLName
            .mv_sFormName = pv_sFormName
            .mv_intShortCutKey = pv_intShorCutKey
        End With
        AddHandler fv_oMenuItem.Click, AddressOf _Click
        Return fv_oMenuItem
    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Thực hiện khi chọn một chức năng trên Menu
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub _Click (ByVal sender As Object, ByVal e As EventArgs)
        Try
            ms_InvokeForm (CType (sender, _MenuItem).mv_sAssemblyName, CType (sender, _MenuItem).mv_sDLLName, _
                           CType (sender, _MenuItem).mv_sFormName, CType (sender, _MenuItem).mv_sParameterList)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Thực hiện chức năng tương ứng với hàm được gọi trong Assembly
    'Đầu vào         :Tên Assembly,tên DLL,tên Form
    'Đầu ra           :Menu của phân hệ được xây dựng 
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Public Sub ms_InvokeForm (ByVal pv_sAssemblyName As String, _
                              ByVal pv_sDLLName As String, _
                              ByVal pv_sFormName As String, ByVal pv_sParameterList As String)
        Try
            Dim sv_oAss As Assembly
            Dim bExistForm As Boolean = False
            Dim sv_oAllForm As Type()
            'Contains All Forms from Assembly
            If pv_sDLLName.Trim.Equals (String.Empty) Then
                MessageBox.Show ("Menu này chưa được gán chức năng", "Thông báo", MessageBoxButtons.OK, _
                                 MessageBoxIcon.Information)
                Return
            End If
            If Not File.Exists (Application.StartupPath & "\" & pv_sDLLName & ".DLL") Then
                MessageBox.Show ("Không tồn tại phân hệ " & pv_sDLLName & ".DLL", gv_sAnnouncement, MessageBoxButtons.OK, _
                                 MessageBoxIcon.Information)
                Return
            End If

            'Load Assembly Information from AssemblyName
            sv_oAss = [Assembly].LoadFrom (Application.StartupPath & "\" & pv_sDLLName & ".DLL")
            'Get Forms from AssemblyInfor
            sv_oAllForm = sv_oAss.GetTypes
            gv_CurrAssembly = sv_oAss
            If Not gv_objArrDLL.Contains (pv_sDLLName) Then
                gv_objArrDLL.Add (pv_sDLLName)
            End If
            LoadParamsValues (sv_oAss)
            '----------------------------------------------------------------------------------
            Dim sv_iflags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.Static Or _
                                            BindingFlags.Instance Or BindingFlags.DeclaredOnly
            Dim sv_oType As Type
            For Each sv_oType In sv_oAllForm
                Try
                    If sv_oType.Name.Trim.ToUpper.Trim.Equals (pv_sFormName.Trim.ToUpper) Then
                        Dim sv_oCalledForm As Form
                        If pv_sParameterList.Trim.Equals (String.Empty) Then
                            sv_oCalledForm = CType (Activator.CreateInstance (sv_oType), Form)
                        Else
                            sv_oCalledForm = _
                                CType (Activator.CreateInstance (sv_oType, New Object() {pv_sParameterList}), Form)
                        End If
                        bExistForm = True
                        sv_oCalledForm.ShowDialog()
                        Exit For
                    End If
                Catch ex As Exception
                End Try

            Next
            If Not bExistForm Then
                MessageBox.Show ("Không tồn tại chức năng " & pv_sFormName & " trong phân hệ " & pv_sDLLName & ".DLL", _
                                 gv_sAnnouncement, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
        Catch ex As Exception
            MessageBox.Show ("Lỗi khởi động chức năng!" & vbCrLf & ex.Message, "Thông báo", MessageBoxButtons.OK, _
                             MessageBoxIcon.Information)
        End Try
    End Sub
End Class
