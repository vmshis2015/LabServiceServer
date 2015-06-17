Imports System.Data.SqlClient
Imports System.Reflection
Imports System.IO

Public Class DVC_ToolBarButton
    Public mv_DS As New DataSet

    Public Function GetToolBarButton (ByVal pv_sText As String, ByVal pv_sToolTipText As String, _
                                      ByVal pv_iRole As Integer, _
                                      ByVal pv_sDLLName As String, ByVal pv_sFormName As String, _
                                      ByVal pv_sAssemblyName As String, ByVal pv_sParameterList As String, _
                                      ByVal pv_oImgList As ImageList, _
                                      ByVal pv_sIconPath As String, ByVal pv_intIgmIndex As Integer, _
                                      ByVal pv_intStyle As Integer, _
                                      ByVal pv_tbr As ToolBar, Optional ByVal pv_bDisplayText As Boolean = False) _
        As ToolBarBtn
        Dim fv_bCtxMenu As Boolean = False
        Dim fv_objCtxMenu As ContextMenu
        fv_objCtxMenu = objGetPopupMenu (pv_iRole)
        If fv_objCtxMenu Is Nothing Then
            fv_bCtxMenu = False
        Else
            fv_bCtxMenu = True
        End If
        Dim _
            fv_oToolBarButton As _
                New ToolBarBtn (pv_sText, pv_sToolTipText, pv_intIgmIndex, pv_bDisplayText, _
                                IIf (pv_intStyle = 0, False, True), fv_bCtxMenu, fv_objCtxMenu)
        With fv_oToolBarButton
            .bDisplayText = pv_bDisplayText
            .IntRolePerformed = pv_iRole
            .mv_sParameterList = pv_sParameterList
            .mv_sText = pv_sText
            .mv_iID = pv_iRole
            .mv_sAssemblyName = pv_sAssemblyName
            .mv_sDLLName = pv_sDLLName
            .mv_sFormName = pv_sFormName
            .mv_intImgIndex = pv_intIgmIndex
        End With
        If gv_oDSRFU.Tables (0).Select ("iRoleID=" & pv_iRole).GetLength (0) > 0 Then
            fv_oToolBarButton.Tag = "1"
        Else
            fv_oToolBarButton.Enabled = False
            fv_oToolBarButton.Tag = "-1"
        End If
        gv_oMainForm.tbr.Buttons.Add (fv_oToolBarButton)
        Return fv_oToolBarButton
    End Function

    Private Function objGetPopupMenu (ByVal pv_intRolePerformed As Integer) As ContextMenu
        Try
            If bRoleHasChildren (pv_intRolePerformed) Then
                Return CtxGetMenu (pv_intRolePerformed)
            Else
                Return Nothing
            End If
        Catch ex As Exception

        End Try
    End Function

    Private Function bRoleHasChildren (ByVal pv_intRolePerformed As Integer) As Boolean
        Dim sv_Ds As New DataSet
        Dim sv_DA As SqlDataAdapter
        Dim sv_sSql As String
        Try
            sv_sSql = "SELECT * FROM TBL_ROLES WHERE iParentRole=" & pv_intRolePerformed & " AND FP_sBranchID=N'" & _
                      gv_sBranchID & "'"
            sv_DA = New SqlDataAdapter (sv_sSql, gv_oSqlCnn)
            sv_DA.Fill (sv_Ds, "TBL_ROLES")
            If sv_Ds.Tables (0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Thực hiện khi chọn một chức năng trên Menu
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub _Click (ByVal sender As Object, ByVal e As ToolBarButtonClickEventArgs)
        Try
            If e.Button.Tag = "-1" Or e.Button.Tag = "-2" Or e.Button.Tag = "-3" Or e.Button.Tag = "-4" Then
            Else
                ms_InvokeForm (CType (e.Button, ToolBarBtn).mv_sAssemblyName, CType (e.Button, ToolBarBtn).mv_sDLLName, _
                               CType (e.Button, ToolBarBtn).mv_sFormName, CType (e.Button, ToolBarBtn).mv_sParameterList)
            End If
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
            If pv_sAssemblyName = "-1" Then
                Return
            End If
            If Not File.Exists (Application.StartupPath & "\" & pv_sDLLName & ".DLL") Then
                MessageBox.Show ("Không tồn tại phân hệ " & pv_sDLLName & ".DLL", gv_sAnnouncement, MessageBoxButtons.OK, _
                                 MessageBoxIcon.Information)
                Return
            End If

            'Load Assembly Information from AssemblyName
            sv_oAss = [Assembly].LoadFrom (pv_sDLLName & ".DLL")
            'Get Forms from AssemblyInfor
            sv_oAllForm = sv_oAss.GetTypes
            gv_CurrAssembly = sv_oAss
            If Not gv_objArrDLL.Contains (pv_sDLLName) Then
                gv_objArrDLL.Add (pv_sDLLName)
                LoadParamsValues (sv_oAss)
            End If
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

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Xây dựng cây Menu của từng phân hệ
    'Đầu vào         :Mã phân hệ
    'Đầu ra           :Menu của phân hệ được xây dựng 
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Function CtxGetMenu (ByVal pv_intRolePerformed As Integer) As ContextMenu
        Dim fv_oCtxMenu As New ContextMenu
        Dim fv_oMnu As New DVC_MenuItemForToolBar
        Dim sv_oDR As DataRow()
        Try
            'get all ChildRoles of this Role
            sv_oDR = mv_DS.Tables (0).Select ("iParentRoleID=" & pv_intRolePerformed)
            For i As Integer = 0 To sv_oDR.Length - 1
                Dim sv_oMenuItem As _MenuItem
                'Lấy về ImageIndex của Menu
                If gv_sLanguageDisplay.ToUpper = "VN" Then
                    sv_oMenuItem = fv_oMnu.GetMenuItem (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("iRoleID"), _
                                                        IsNull_VN (sv_oDR (i) ("sDLLName")), _
                                                        IsNull_VN (sv_oDR (i) ("sFormName")), _
                                                        IsNull_VN (sv_oDR (i) ("sAssemblyName")), _
                                                        IsNull_VN (sv_oDR (i) ("sParameterList")), _
                                                        sv_oDR (i) ("sIconPath"), _
                                                        ShortCutZero (sv_oDR (i) ("intShortCutKey")))
                Else
                    sv_oMenuItem = _
                        fv_oMnu.GetMenuItem (IsNull_VNEN (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("sEngRoleName")), _
                                             sv_oDR (i) ("iRoleID"), _
                                             IsNull_VN (sv_oDR (i) ("sDLLName")), IsNull_VN (sv_oDR (i) ("sFormName")), _
                                             IsNull_VN (sv_oDR (i) ("sAssemblyName")), _
                                             IsNull_VN (sv_oDR (i) ("sParameterList")), sv_oDR (i) ("sIconPath"), _
                                             ShortCutZero (sv_oDR (i) ("intShortCutKey")))
                End If
                'Kiểm tra xem Menu có được kích hoạt hay không
                If Not CBool (sv_oDR (i) ("bEnable")) Then
                    sv_oMenuItem.Enabled = False
                Else
                End If
                If gv_oDSRFU.Tables (0).Select ("iRoleID=" & pv_intRolePerformed).GetLength (0) > 0 Then
                Else
                    sv_oMenuItem.Enabled = False
                End If
                sv_oMenuItem.Tag = sv_oDR (i) ("sRoleName").ToString & "@" & _
                                   IsNull_VNEN (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("sEngRoleName")) & "$1"
                AddMenuItem (sv_oMenuItem, sv_oDR (i) ("iRoleID"))
                'Dim ii As MenuInherits
                fv_oCtxMenu.MenuItems.Add (sv_oMenuItem)
            Next
            Return fv_oCtxMenu
        Catch ex As Exception

        End Try
    End Function

    'Thủ tục đệ quy tạo cây menu
    Private Sub AddMenuItem (ByVal pv_oParentMenuItem As MenuItem, ByVal pv_oRoleID As Integer)
        Dim sv_oDR As DataRow()
        Dim fv_oMnu As New DVC_MenuItemForToolBar
        Try
            'get all ChildRoles of this Role
            sv_oDR = mv_DS.Tables (0).Select ("iParentRoleID=" & pv_oRoleID)
            For i As Integer = 0 To sv_oDR.Length - 1
                Dim sv_oMenuItem As _MenuItem
                If gv_sLanguageDisplay.ToUpper = "VN" Then
                    sv_oMenuItem = fv_oMnu.GetMenuItem (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("iRoleID"), _
                                                        IsNull_VN (sv_oDR (i) ("sDLLName")), _
                                                        IsNull_VN (sv_oDR (i) ("sFormName")), _
                                                        IsNull_VN (sv_oDR (i) ("sAssemblyName")), _
                                                        IsNull_VN (sv_oDR (i) ("sParameterList")), _
                                                        sv_oDR (i) ("sIconPath"), _
                                                        ShortCutZero (sv_oDR (i) ("intShortCutKey")))
                Else
                    sv_oMenuItem = _
                        fv_oMnu.GetMenuItem (IsNull_VNEN (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("sEngRoleName")), _
                                             sv_oDR (i) ("iRoleID"), _
                                             IsNull_VN (sv_oDR (i) ("sDLLName")), IsNull_VN (sv_oDR (i) ("sFormName")), _
                                             IsNull_VN (sv_oDR (i) ("sAssemblyName")), _
                                             IsNull_VN (sv_oDR (i) ("sParameterList")), sv_oDR (i) ("sIconPath"), _
                                             ShortCutZero (sv_oDR (i) ("intShortCutKey")))
                End If
                'Kiểm tra xem Menu có được kích hoạt hay không
                If Not CBool (sv_oDR (i) ("bEnable")) Then
                    sv_oMenuItem.Enabled = False
                Else
                End If
                pv_oParentMenuItem.MenuItems.Add (sv_oMenuItem)
                'Gọi đệ quy đối với chức năng vừa được xét đến
                AddMenuItem (sv_oMenuItem, sv_oDR (i) ("iRoleID"))
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Function IsNull_VN (ByVal pv_obj As Object) As String
        Return IIf (IsDBNull (pv_obj), " ", pv_obj.ToString)
    End Function

    Private Function IsNull_VNEN (ByVal pv_obj As Object, ByVal pv_obj1 As Object) As String
        If pv_obj1.ToString.Trim.Equals (String.Empty) Then
            Return pv_obj.ToString
        Else
            Return pv_obj1.ToString
        End If
    End Function

    Private Function ShortCutZero (ByVal pv_intValue As Integer) As Integer
        If pv_intValue = - 1 Then
            Return 0
        Else
            Return pv_intValue
        End If
    End Function
End Class
