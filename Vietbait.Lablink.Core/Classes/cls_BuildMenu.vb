'Docking Manager*********
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Menus
Imports Crownwood.Magic.Docking
Imports System.Data.SqlClient
Imports System.IO

'General Libararies

Public Class cls_BuildMenu
    Dim mv_sSql As String

    Dim gv_oDSRFUForToolBarButton As New DataSet
    ' Dataset chứa danh sách các Button của mỗi phân hệ
    Dim mv_oMenu As New MenuControl
    Public mv_oTbrButton() As ToolBarButton
    Dim mv_oSubSystemMenu As New MenuCommand ("Danh sách các phân hệ")
    Dim mv_oMenuSubSystem As MenuInherits
    Dim mv_oHTB As New Hashtable
    'Bảng băm chứa ID của mỗi phân hệ người dùng
    Dim mv_oHtbVNName As New Hashtable
    ' Bảng băm chứa danh sách tên =Text của mỗi phân hệ(Tiếng Việt)
    Dim mv_oHtbEngName As New Hashtable
    ' Bảng băm chứa danh sách tên =Text của mỗi phân hệ(Tiếng Anh)
    Dim mv_oHtbDllName As New Hashtable
    ' Bảng băm chứa danh sách tên DLL mỗi phân hệ để lấy về thông tin phiên bản
    Dim mv_oHtbImgPath As New Hashtable
    ' Bảng băm chứa danh sách đường dẫn ảnh của mỗi phân hệ
    Public mv_sSubSystemName As String = ""
    ' Biến chứa tên phân hệ
    Public mv_bInitialize As Boolean = False
    ' Biến xác định xem là nạp CT lần đầu cho mỗi User hay là nạp lại?
    Public mv_ContextMenu As New ContextMenu
    ' Chứa danh sách menu các phân hệ để tiện lợi cho người dùng khi chuyển phân hệ làm việc
    Public mv_iCurrentSubSystem As Integer = - 100
    'Lưu trữ mã phân hệ hiện thời để không phải Load lại Menu khi chọn phân hệ làm việc mới= phân hệ đang sử dụng
    '------------------------------------------------------------------
    Public mv_oHtbEngMenu As New Hashtable
    Public mv_oHtbVnMenu As New Hashtable
    Public mv_objImageList As New ImageList
    Public mv_dsIconPath As New DataSet
    Public mv_dsIconPathForToolBarButton As New DataSet
    'Data Set chứa đường dẫn tới ảnh của các Button
    Public mv_objImageListForToolBarButton As New ImageList
    Public mv_objImgForToolBarButton As New ArrayList
    Public mv_objImg As New ArrayList
    Public mv_intSubSystemcount As Integer = 0
    Public mv_drSubSystem() As DataRow
    Public mv_iIDfromTab As Integer
    'Lấy về mã phân hệ từ Tab phân hệ
    Protected Friend _IconForSubSystem As New Hashtable

    Public Sub New (ByVal FrmMain As Form)
        Try
            If Not gv_ConnectSuccess Then
                gv_ConnectSuccess = KhoiTaoKetNoi()
            End If
            gv_oDSRFU.Reset()
            gv_dsParam = dsGetAllParams()
            'Bắt đầu xây dựng Menu cho Người dùng
            'Lấy về danh sách các MenuIcon
            mv_dsIconPath = ms_dsIconList()
            mv_dsIconPathForToolBarButton = ms_dsIconList ("TBL_TOOLBARBUTTON")
            'Tạo danh sách ảnh
            CreateImageList()
            '1: Lấy về quyền của User
            ms_GetAllRoleOfUser (gv_sUID)
            mv_oSubSystemMenu.Tag = "-111"
            'Xây menu các phân hệ
            BuildMenuSubSystem()
            'Xây ngẫu nhiên phân hệ đầu tiên trong danh mục các phân hệ
            mv_bInitialize = True
            If mv_intSubSystemcount > 1 Then
                If gv_sSubSystemDisplay.ToUpper.Equals ("DOCKING") Then
                    If gv_intPicOrder = 0 Then
                        _Click (CType (SubSystemPanel.pnlBackGround.Controls (2), PicAboveText)._Link, New EventArgs)
                    Else
                        _Click (CType (SubSystemPanel.pnlBackGround.Controls (2), PicBeforeText)._Link, New EventArgs)
                    End If
                Else
                    _Click (mv_oSubSystemMenu.MenuCommands (0), New EventArgs)
                End If
            Else
                _Click (mv_oSubSystemMenu.MenuCommands (0), New EventArgs)
            End If
            mv_bInitialize = False
            RemoveHandler gv_oMainForm.tbr.ButtonClick, AddressOf _ClickTbr
            AddHandler gv_oMainForm.tbr.ButtonClick, AddressOf _ClickTbr
        Catch ex As Exception

        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Tạo các DefaultMenu như đăng nhập, đăng xuất, đổi mật khẩu, thông tin phiên bản, thoát khỏi hệ thống
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :05/03/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Function GetDefaultMenu() As MenuCommand
        Try
            Dim mnuItem As New MenuCommand ("1.Hệ thống")
            mnuItem.MenuCommands.AddRange ( _
                                           New MenuCommand() _
                                              {New MenuCommand ("Đổi mật khẩu", New EventHandler (AddressOf ChangePass)), _
                                               New MenuCommand ("Thông tin phiên bản", _
                                                                New EventHandler (AddressOf VersionInfor)), _
                                               New MenuCommand ("-"), _
                                               New MenuCommand ("Đăng xuất", New EventHandler (AddressOf LogOut)), _
                                               New MenuCommand ("Đăng nhập", New EventHandler (AddressOf LogIn)), _
                                               New MenuCommand ("-"), _
                                               New MenuCommand ("Thoát", New EventHandler (AddressOf _ClickExit))})
            mnuItem.MenuCommands (4).Enabled = False
            Return mnuItem
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Tạo các HelpMenu (Hướng dẫn trợ giúp, thông tin tác giả, thông tin bản quyền...)
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :05/03/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Function GetHelpMenu() As MenuCommand
        Try
            Dim mnuItem As New MenuCommand ("1.Trợ giúp")
            mnuItem.MenuCommands.AddRange ( _
                                           New MenuCommand() _
                                              {New MenuCommand ("Hướng dẫn sử dụng", New EventHandler (AddressOf Help)), _
                                               New MenuCommand ("About DynamicMenu", New EventHandler (AddressOf About))})
            Return mnuItem
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Gọi Form thay đổi PassWord
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :05/03/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub ChangePass (ByVal sender As Object, ByVal e As EventArgs)
        Dim sv_oChangePwd As New frm_ChangePwd
        sv_oChangePwd.ShowDialog()
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Gọi sự kiện Log out khỏi hệ thống
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :05/03/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub LogOut (ByVal sender As Object, ByVal e As EventArgs)
        Dim i As Integer = 0
        Dim s As String
        Try
            For Each mnuItem As MenuCommand In getMenuControl.MenuCommands
                i += 1
                'Kiểm tra xem có phải là Menu hệ thống ko?
                'Nếu là Docking thì Menu hệ thống ở vị trí số 1. Ngược lại nó ở vị trí số 2 đứng sau Menu phân hệ
                If i <> IIf (gv_sSubSystemDisplay.ToUpper.Equals ("DOCKING"), 1, IIf (mv_oHTB.Count > 1, 2, 1)) Then
                    For Each mnuItem1 As MenuCommand In mnuItem.MenuCommands
                        If Not mnuItem1.Tag Is Nothing Then
                            s = mnuItem1.Tag.ToString.Substring (mnuItem1.Tag.ToString.IndexOf ("$") + 1)
                            If s <> "0" Then
                                mnuItem1.Enabled = False
                            End If
                        End If
                    Next
                Else 'Nếu là Menu hệ thống thì thao tác trực tiếp
                    mnuItem.MenuCommands (0).Enabled = False
                    mnuItem.MenuCommands (3).Enabled = False
                    mnuItem.MenuCommands (4).Enabled = True
                    mnuItem.MenuCommands (6).Enabled = False
                End If
            Next
            EnableSubSystem (False)
            Dim k As Integer = 0
            k = gv_oMainForm.tbr.Buttons.Count - 6
            If k > 0 Then
                Try
                    For i = 0 To k
                        gv_oMainForm.tbr.Buttons (5 + i).Enabled = False
                    Next
                Catch ex As IndexOutOfRangeException

                End Try
            End If

            mv_bCanClose = False
        Catch ex As Exception

        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Gọi sự kiện log in vào hệ thống
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :05/03/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub LogIn (ByVal sender As Object, ByVal e As EventArgs)
        Dim i As Integer = 0
        Dim _frmLogin As New frmLogin
        Dim s As String
        Try
            _frmLogin.mv_bCallFromLogin = True
            _frmLogin.ShowDialog()
            If _frmLogin.mv_bLoginSuccess Then
                For Each mnuItem As MenuCommand In getMenuControl.MenuCommands
                    i += 1
                    'Kiểm tra xem có phải là Menu hệ thống ko?
                    'Nếu là Docking thì Menu hệ thống ở vị trí số 1. Ngược lại nó ở vị trí số 2 đứng sau Menu phân hệ
                    If i <> IIf (gv_sSubSystemDisplay.ToUpper.Equals ("DOCKING"), 1, IIf (mv_oHTB.Count > 1, 2, 1)) Then
                        For Each mnuItem1 As MenuCommand In mnuItem.MenuCommands
                            If Not mnuItem1.Tag Is Nothing Then
                                s = mnuItem1.Tag.ToString.Substring (mnuItem1.Tag.ToString.IndexOf ("$") + 1)
                                If s <> "0" Then
                                    mnuItem1.Enabled = True
                                End If
                            End If
                        Next
                    Else
                        mnuItem.MenuCommands (0).Enabled = True
                        mnuItem.MenuCommands (3).Enabled = True
                        mnuItem.MenuCommands (4).Enabled = False
                        mnuItem.MenuCommands (6).Enabled = True
                    End If
                Next
                mv_bCanClose = True
                EnableSubSystem()
                Dim k As Integer = 0
                k = gv_oMainForm.tbr.Buttons.Count - 6
                If k > 0 Then
                    Try
                        For i = 0 To k
                            If gv_oMainForm.tbr.Buttons (5 + i).Tag = "1" Then
                                gv_oMainForm.tbr.Buttons (5 + i).Enabled = True
                            Else
                            End If
                        Next
                    Catch ex As IndexOutOfRangeException

                    End Try
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub VersionInfor (ByVal sender As Object, ByVal e As EventArgs)
        MessageBox.Show ("Chức năng này sẽ được tích hợp trong phiên bản kế tiếp", gv_sAnnouncement, _
                         MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Hiện Form Help
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :05/03/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub Help (ByVal sender As Object, ByVal e As EventArgs)
        Dim sv_oFLashForm As New frmSplash
        sv_oFLashForm._bStartApp = False
        sv_oFLashForm.ShowDialog()
        'MessageBox.Show("Chức năng này sẽ được tích hợp trong phiên bản kế tiếp", gv_sAnnouncement, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Hiện Form About
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :05/03/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub About (ByVal sender As Object, ByVal e As EventArgs)
        Try
            'Dim sv_oForm As New frmAbout
            'sv_oForm.ShowDialog()
        Catch ex As Exception

        End Try

    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Lấy về danh sách quyền của User
    'Đầu vào         :Tên đăng nhập
    'Đầu ra           :Danh sách các quyền
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub ms_GetAllRoleOfUser (ByVal pv_sUID As String)
        Dim sv_oDA As SqlDataAdapter
        Dim sv_oUser As New clsUser
        Try
            If sv_oUser.bIsAdmin (pv_sUID) Then
                mv_sSql = _
                    "SELECT distinct R.iRole AS iRoleID,R.iParentRole AS iParentRoleID,R.iOrder,R.intShortCutKey,R.sIconPath,R.sRoleName,R.sEngRoleName,F.sDLLName,F.sFormName,F.sAssemblyName,sParameterList,ISNull(F.bEnable,1) bEnable, R.sImgPath " & _
                    " FROM TBL_ROLES R " & _
                    ",TBL_FUNCTIONS F  " & _
                    " WHERE  R.FK_iFunctionID *=F.PK_iID " & _
                    " AND R.FP_sBranchID=N'" & gv_sBranchID & "'" & _
                    " AND F.FP_sBranchID=N'" & gv_sBranchID & "' ORDER BY iOrder,iParentRoleID,iRoleID"
                sv_oDA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
                sv_oDA.Fill (gv_oDSRFU, "TBL_ROLES")
            Else
                mv_sSql = _
                    "SELECT RFU.iRoleID,RFU.iParentRoleID,R.iOrder,R.intShortCutKey,R.sIconPath,R.sRoleName,R.sEngRoleName,F.sDLLName,F.sFormName,F.sAssemblyName,sParameterList,ISNull(F.bEnable,1) bEnable, R.sImgPath " & _
                    " FROM TBL_ROLESFORUSERS RFU INNER JOIN TBL_ROLES R " & _
                    " ON R.iRole=RFU.iRoleID LEFT JOIN TBL_FUNCTIONS F ON R.FK_iFunctionID=F.PK_iID " & _
                    " WHERE sUID=N'" & pv_sUID & "' ORDER BY iOrder,iParentRoleID,iRoleID"
                sv_oDA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
                sv_oDA.Fill (gv_oDSRFU, "TBL_ROLES")
            End If
        Catch ex As Exception

        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Lấy về danh sách quyền của User
    'Đầu vào         :Tên đăng nhập
    'Đầu ra           :Danh sách các quyền
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub ms_GetAllToolBarButton (ByVal pv_intSubSysID As Integer)
        Dim sv_oDA As SqlDataAdapter
        Dim sv_oUser As New clsUser
        Try
            gv_oDSRFUForToolBarButton.Clear()
            mv_sSql = _
                "SELECT RFU.intOrder,RFU.intDisplayText, RFU.intStyle,RFU.sIconPath,RFU.FP_intRoleID,RFU.intRolePerformed,RFU.sText,RFU.sTTT,R.iOrder,R.intShortCutKey,R.sRoleName,R.sEngRoleName,F.sDLLName,F.sFormName,F.sAssemblyName,sParameterList,ISNull(F.bEnable,1) bEnable, R.sImgPath " & _
                " FROM TBL_ToolBarButton RFU LEFT JOIN TBL_ROLES R " & _
                " ON R.iRole=RFU.intRolePerformed LEFT JOIN TBL_FUNCTIONS F ON R.FK_iFunctionID=F.PK_iID " & _
                " WHERE RFU.FP_intRoleID=" & pv_intSubSysID & " ORDER BY RFU.intOrder ASC"
            sv_oDA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
            sv_oDA.Fill (gv_oDSRFUForToolBarButton, "TBL_ROLES")
        Catch ex As Exception

        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Lấy về danh sách các Icon 
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Function ms_dsIconList (Optional ByVal pv_sTableName As String = "TBL_ROLES") As DataSet
        Dim sv_oDA As SqlDataAdapter
        Dim fv_ds As New DataSet
        Try
            mv_sSql = "SELECT distinct sIconPath  FROM " & pv_sTableName & _
                      " WHERE sIconPath is not NUll AND  sIconPath<>N'UNKNOWN'"
            sv_oDA = New SqlDataAdapter (mv_sSql, gv_oSqlCnn)
            sv_oDA.Fill (fv_ds, "TBL_ROLES")
            Return fv_ds
        Catch ex As Exception

        End Try
    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Trả về Menu của User đăng nhập
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Public Function GetMainMenu() As MenuControl
        Return mv_oMenu
    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Xây dựng ToolBarButton 
    'Đầu vào         :Mã phân hệ
    'Đầu ra           :Menu của phân hệ được xây dựng 
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Public Sub BuildToolBarButtons (ByVal pv_iSubSystemID As Integer)
        Dim sv_oDVCTbrBtn As New DVC_ToolBarButton
        Dim sv_oDR As DataRow()
        Dim k As Integer
        Try
            k = gv_oMainForm.tbr.Buttons.Count - 6
            If k > 0 Then
                Try
                    For i As Integer = 0 To k
                        gv_oMainForm.tbr.Buttons.RemoveAt (5)
                    Next
                Catch ex As IndexOutOfRangeException

                End Try
            End If
            sv_oDVCTbrBtn.mv_DS = gv_oDSRFU
            For i As Integer = 0 To gv_oDSRFUForToolBarButton.Tables (0).Rows.Count - 1
                Dim sv_otbrBtn As ToolBarBtn
                'Lấy về ImageIndex của Menu
                sv_otbrBtn = _
                    sv_oDVCTbrBtn.GetToolBarButton(gv_oDSRFUForToolBarButton.Tables(0).Rows(i)("sText"), _
                                                    gv_oDSRFUForToolBarButton.Tables(0).Rows(i)("sTTT"), _
                                                    IsNull_VN(gv_oDSRFUForToolBarButton.Tables(0).Rows(i)("intRolePerformed")), _
                                                    IsNull_VN(gv_oDSRFUForToolBarButton.Tables(0).Rows(i)("sDLLName")), _
                                                    IsNull_VN(gv_oDSRFUForToolBarButton.Tables(0).Rows(i)("sFormName")), _
                                                    IsNull_VN(gv_oDSRFUForToolBarButton.Tables(0).Rows(i)("sAssemblyName")), _
                                                    IsNull_VN(gv_oDSRFUForToolBarButton.Tables(0).Rows(i)("sParameterList")), _
                                                    mv_objImageListForToolBarButton, _
                                                    gv_oDSRFUForToolBarButton.Tables(0).Rows(i)("sIconPath"), _
                                                    intGetImgIndexForToolBarButton( _
                                                                                    gv_oDSRFUForToolBarButton.Tables(0) _
                                                                                       .Rows(i)("sIconPath")), _
                                                    gv_oDSRFUForToolBarButton.Tables(0).Rows(i)("intStyle"), _
                                                    gv_oMainForm.tbr, _
                                                    IIf( _
                                                         gv_oDSRFUForToolBarButton.Tables(0).Rows(i)("intDisplayText") = _
                                                         0, False, True))

                'Kiểm tra xem Menu có được kích hoạt hay không
                If Not CBool (gv_oDSRFUForToolBarButton.Tables (0).Rows (i) ("bEnable")) Then
                    sv_otbrBtn.Enabled = False
                Else
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Xây dựng cây Menu của từng phân hệ
    'Đầu vào         :Mã phân hệ
    'Đầu ra           :Menu của phân hệ được xây dựng 
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Public Sub BuildMenuItems (ByVal pv_iSubSystemID As Integer)
        Dim sv_oMenu As New DVC_MenuItem
        Dim sv_oDR As DataRow()
        Try
            'get all ChildRoles of this Role
            sv_oDR = gv_oDSRFU.Tables (0).Select ("iParentRoleID=" & pv_iSubSystemID)
            For i As Integer = 0 To sv_oDR.Length - 1
                Dim sv_oMenuItem As MenuInherits
                'Lấy về ImageIndex của Menu
                If gv_sLanguageDisplay.ToUpper = "VN" Then
                    sv_oMenuItem = sv_oMenu.GetMenuItem (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("iRoleID"), _
                                                         IsNull_VN (sv_oDR (i) ("sDLLName")), _
                                                         IsNull_VN (sv_oDR (i) ("sFormName")), _
                                                         IsNull_VN (sv_oDR (i) ("sAssemblyName")), _
                                                         IsNull_VN (sv_oDR (i) ("sParameterList")), mv_objImageList, _
                                                         sv_oDR (i) ("sIconPath"), _
                                                         ShortCutZero (sv_oDR (i) ("intShortCutKey")), _
                                                         intGetImgIndex (sv_oDR (i) ("sIconPath")))
                Else
                    sv_oMenuItem = _
                        sv_oMenu.GetMenuItem (IsNull_VNEN (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("sEngRoleName")), _
                                              sv_oDR (i) ("iRoleID"), _
                                              IsNull_VN (sv_oDR (i) ("sDLLName")), IsNull_VN (sv_oDR (i) ("sFormName")), _
                                              IsNull_VN (sv_oDR (i) ("sAssemblyName")), _
                                              IsNull_VN (sv_oDR (i) ("sParameterList")), mv_objImageList, _
                                              sv_oDR (i) ("sIconPath"), ShortCutZero (sv_oDR (i) ("intShortCutKey")), _
                                              intGetImgIndex (sv_oDR (i) ("sIconPath")))
                End If
                'Kiểm tra xem Menu có được kích hoạt hay không
                If Not CBool (sv_oDR (i) ("bEnable")) Then
                    sv_oMenuItem.Enabled = False
                Else
                End If
                'Gán Tag cho MenuItem
                sv_oMenuItem.Tag = sv_oDR (i) ("sRoleName").ToString & "@" & _
                                   IsNull_VNEN (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("sEngRoleName")) & "$1"
                gv_intMenuItemCount += 1
                AddMenuItem (sv_oMenuItem, sv_oDR (i) ("iRoleID"))
                mv_oMenu.MenuCommands.Add (sv_oMenuItem)

            Next
            'Sau khi xây xong cây menu chức năng của mỗi phân hệ. Ta add thêm Menu Help vào sau cùng
            'mv_oMenu.MenuCommands.Add(GetHelpMenu)

        Catch ex As Exception

        End Try
    End Sub

    'Thủ tục đệ quy tạo cây menu
    Private Sub AddMenuItem (ByVal pv_oParentMenuItem As MenuInherits, ByVal pv_oRoleID As Integer)
        Dim sv_oDR As DataRow()
        Dim sv_oMenu As New DVC_MenuItem
        Try
            'get all ChildRoles of this Role
            sv_oDR = gv_oDSRFU.Tables (0).Select ("iParentRoleID=" & pv_oRoleID)
            For i As Integer = 0 To sv_oDR.Length - 1
                Dim sv_oMenuItem As MenuInherits
                If gv_sLanguageDisplay.ToUpper = "VN" Then
                    sv_oMenuItem = sv_oMenu.GetMenuItem (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("iRoleID"), _
                                                         IsNull_VN (sv_oDR (i) ("sDLLName")), _
                                                         IsNull_VN (sv_oDR (i) ("sFormName")), _
                                                         IsNull_VN (sv_oDR (i) ("sAssemblyName")), _
                                                         IsNull_VN (sv_oDR (i) ("sParameterList")), mv_objImageList, _
                                                         sv_oDR (i) ("sIconPath"), _
                                                         ShortCutZero (sv_oDR (i) ("intShortCutKey")), _
                                                         intGetImgIndex (sv_oDR (i) ("sIconPath")))
                Else
                    sv_oMenuItem = _
                        sv_oMenu.GetMenuItem (IsNull_VNEN (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("sEngRoleName")), _
                                              sv_oDR (i) ("iRoleID"), _
                                              IsNull_VN (sv_oDR (i) ("sDLLName")), IsNull_VN (sv_oDR (i) ("sFormName")), _
                                              IsNull_VN (sv_oDR (i) ("sAssemblyName")), _
                                              IsNull_VN (sv_oDR (i) ("sParameterList")), mv_objImageList, _
                                              sv_oDR (i) ("sIconPath"), ShortCutZero (sv_oDR (i) ("intShortCutKey")), _
                                              intGetImgIndex (sv_oDR (i) ("sIconPath")))
                End If
                'Kiểm tra xem Menu có được kích hoạt hay không
                If Not CBool (sv_oDR (i) ("bEnable")) Then
                    sv_oMenuItem.Enabled = False
                    sv_oMenuItem.Tag = sv_oDR (i) ("sRoleName") & "@" & _
                                       IsNull_VNEN (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("sEngRoleName")) & "$0"
                Else
                    sv_oMenuItem.Tag = sv_oDR (i) ("sRoleName") & "@" & _
                                       IsNull_VNEN (sv_oDR (i) ("sRoleName"), sv_oDR (i) ("sEngRoleName")) & "$1"
                End If
                gv_intMenuItemCount += 1
                pv_oParentMenuItem.MenuCommands.Add (sv_oMenuItem)
                'Gọi đệ quy đối với chức năng vừa được xét đến
                AddMenuItem (sv_oMenuItem, sv_oDR (i) ("iRoleID"))
            Next
        Catch ex As Exception
        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Xây dựng cây Menu danh sách phân hệ
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub BuildMenuSubSystem()
        Dim sv_oDR As DataRow()
        Dim i As Integer = 0
        Dim sv_oMenuSubSystem As MenuCommand
        Dim sv_oCtxtMnuSubSystem As MenuItem
        Try
            sv_oDR = gv_oDSRFU.Tables (0).Select ("iParentRoleID=-2")
            mv_drSubSystem = gv_oDSRFU.Tables (0).Select ("iParentRoleID=-2")
            mv_intSubSystemcount = sv_oDR.Length
            gv_intSubSystemCount = mv_intSubSystemcount
            _IconForSubSystem.Clear()
            For i = 0 To sv_oDR.Length - 1
                'Kiểm tra xem ngôn ngữ hiển thị là Tiếng Anh hay tiếng Việt
                If gv_sLanguageDisplay.ToUpper = "VN" Then
                    sv_oMenuSubSystem = _
                        New MenuCommand ("&" & (i + 1).ToString & ". " & sv_oDR (i).Item ("sRoleName").ToString, _
                                         mv_objImageList, intGetImgIndex (sv_oDR (i) ("sIconPath")), _
                                         New EventHandler (AddressOf _Click))
                    sv_oCtxtMnuSubSystem = _
                        New MenuItem ("&" & (i + 1).ToString & ". " & sv_oDR (i).Item ("sRoleName").ToString, _
                                      New EventHandler (AddressOf _Click))
                    sv_oCtxtMnuSubSystem.MergeOrder = sv_oDR (i) ("iRoleID")
                Else
                    sv_oMenuSubSystem = _
                        New MenuCommand ( _
                                         "&" & (i + 1).ToString & ". " & _
                                         IsNull_VNEN (sv_oDR (i).Item ("sRoleName"), sv_oDR (i).Item ("sEngRoleName")), _
                                         mv_objImageList, intGetImgIndex (sv_oDR (i) ("sIconPath")), _
                                         New EventHandler (AddressOf _Click))
                    sv_oCtxtMnuSubSystem = _
                        New MenuItem ( _
                                      "&" & (i + 1).ToString & ". " & _
                                      IsNull_VNEN (sv_oDR (i).Item ("sRoleName"), sv_oDR (i).Item ("sEngRoleName")), _
                                      New EventHandler (AddressOf _Click))
                    sv_oCtxtMnuSubSystem.MergeOrder = sv_oDR (i) ("iRoleID")
                End If
                'Đưa các Icon vào Danh sách
                _IconForSubSystem.Add (CInt (sv_oDR (i) ("iRoleID")), sv_oDR (i) ("sIconPath"))
                'Bảng chứa các giá trị RoleID của các Role phân hệ
                mv_oHTB.Add (i + 1, sv_oDR (i) ("iRoleID"))
                'Bảng chứa ngôn ngữ hiển thị
                mv_oHtbVNName.Add (i + 1, sv_oDR (i) ("sRoleName"))
                mv_oHtbEngName.Add (i + 1, sv_oDR (i) ("sEngRoleName"))
                'Bảng chứa tên DLL
                mv_oHtbDllName.Add (i + 1, sv_oDR (i) ("sDLLName"))
                'Bảng chứa tên đường dẫn ảnh của mỗi phân hệ
                mv_oHtbImgPath.Add (i + 1, sv_oDR (i) ("sImgPath"))
                mv_oSubSystemMenu.MenuCommands.Add (sv_oMenuSubSystem)
                mv_ContextMenu.MenuItems.Add (sv_oCtxtMnuSubSystem)
                'Gán tag cho MenuItem="TiếngViệt!Tiếng Anh!0"
                sv_oMenuSubSystem.Tag = "&" & (i + 1).ToString & ". " & sv_oDR (i).Item ("sRoleName").ToString & "@" & _
                                        "&" & (i + 1).ToString & ". " & _
                                        IsNull_VNEN (sv_oDR (i).Item ("sRoleName"), sv_oDR (i).Item ("sEngRoleName")) & _
                                        "$1"
                'sv_oCtxtMnuSubSystem.tag = "&" & (i + 1).ToString & ". " & sv_oDR(i).Item("sRoleName").ToString & "@" & "&" & (i + 1).ToString & ". " & IsNull_VNEN(sv_oDR(i).Item("sRoleName"), sv_oDR(i).Item("sEngRoleName")) & "$1"
                gv_intMenuItemCount += 1
            Next
            'Tạo menu Thoát để gắn vào sau cùng của menu Danh sách các phân hệ
            Dim sv_oMnuExit As New MenuCommand ("&" & (mv_oHTB.Count + 1).ToString & ". Thoát", AddressOf _ClickExit)
            sv_oMnuExit.Tag = "Thoát@Exit$1"
            Dim sv_oCtxtMnuExit As New MenuItem ("&" & (mv_oHTB.Count + 1).ToString & ". Thoát", AddressOf _ClickExit)
            mv_oSubSystemMenu.MenuCommands.Add (sv_oMnuExit)
            mv_ContextMenu.MenuItems.Add (sv_oCtxtMnuExit)
            If mv_oHTB.Count > 1 Then
                If gv_sSubSystemDisplay.ToUpper.Equals ("DOCKING") Then
                    CreateDockingManager()
                Else
                    mv_oMenu.MenuCommands.Add (mv_oSubSystemMenu)
                End If
            End If
            'Sau khi thêm menu danh sách các phân hệ. Ta add thêm menu hệ thống vào mỗi phân hệ được chọn
            mv_oMenu.MenuCommands.Add (GetDefaultMenu)
        Catch ex As Exception

        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Xử lý sự kiện khi người dùng chọn một phân hệ làm việc trên Menu
    'Đầu vào         :
    'Đầu ra           :Menu của phân hệ được xây dựng 
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Public Sub _Click (ByVal sender As Object, ByVal e As EventArgs)
        Dim sv_iSubSystemID As Integer
        Try
            'Kiểm tra xem có phải là nạp CT lần đầu không. Nếu lần đầu thì xây menu cho phân hệ đầu tiên
            If mv_bInitialize Then
                'Xóa menu của phân hệ trước đó để xây menu của phân hệ được chọn
                If mv_oMenu.MenuCommands.Count > 1 Then
                    For i As Integer = 2 To mv_oMenu.MenuCommands.Count - 1
                        'Chú ý: Luôn xóa ở vị trí số 1
                        mv_oMenu.MenuCommands.Remove (mv_oMenu.MenuCommands (1))
                    Next
                End If
                ms_GetAllToolBarButton (mv_oHTB (1))
                BuildToolBarButtons (mv_oHTB (1))
                BuildMenuItems (mv_oHTB (1))
                LoadBackGroundImage (mv_oHtbImgPath (1))
                gv_oMainForm.Text = _
                    IIf (gv_sLanguageDisplay.ToUpper.Equals ("VN"), mv_oHtbVNName (1), mv_oHtbEngName (1))
                '& " - " & getVersionInfor(mv_oHtbDllName(sv_iSubSystemID))
                mv_iCurrentSubSystem = 1

                'Gắn dấu check cho MenuItem được chọn
                If gv_intPicOrder = 0 Then
                    Dim iu As New PicAboveText
                    Dim mn As New MenuItem
                    Dim lblCurrent As PicAboveText
                    If sender.GetType.Equals (iu._Link.GetType) Then
                        Dim _l As LinkLabel
                        CheckChange (mn, sCurrLinkText)
                        If sender.GetType.Equals (iu.GetType) Then
                            lblCurrent = DirectCast (sender, PicAboveText)
                        Else
                            lblCurrent = DirectCast (sender.parent, PicAboveText)
                        End If
                        If Not lblCurrent Is Nothing Then
                            ChangeColorForElectedItem (lblCurrent, True)
                        End If

                        Return
                    End If
                Else
                    Dim iu As New PicBeforeText
                    Dim mn As New MenuItem
                    Dim lblCurrent As PicBeforeText
                    If sender.GetType.Equals (iu._Link.GetType) Then
                        Dim _l As LinkLabel
                        CheckChange (mn, sCurrLinkText)
                        If sender.GetType.Equals (iu.GetType) Then
                            lblCurrent = DirectCast (sender, PicBeforeText)
                        Else
                            lblCurrent = DirectCast (sender.parent, PicBeforeText)
                        End If
                        If Not lblCurrent Is Nothing Then
                            ChangeColorForElectedItemPBT (lblCurrent, True)
                        End If

                        Return
                    End If
                End If

                CheckChange (CType (sender, MenuCommand))
            Else 'Còn nếu không phải là nạp lần đầu thì xây menu cho phân hệ được chọn
                'Lấy về mã phân hệ được chọn
                If TypeOf (sender) Is MenuCommand Then
                    sv_iSubSystemID = _
                        CInt ( _
                            CType (sender, MenuCommand).Text.Substring ( _
                                                                        CType (sender, MenuCommand).Text.IndexOf ("&") + _
                                                                        1, _
                                                                        CType (sender, MenuCommand).Text.IndexOf (".") - _
                                                                        1))
                ElseIf TypeOf (sender) Is MenuItem Then
                    sv_iSubSystemID = _
                        CInt ( _
                            CType (sender, MenuItem).Text.Substring (CType (sender, MenuItem).Text.IndexOf ("&") + 1, _
                                                                     CType (sender, MenuItem).Text.IndexOf (".") - 1))
                Else 'Chọn từ Tab phân hệ
                    sv_iSubSystemID = mv_iIDfromTab
                End If
                '-------------------------------------------------------------------------
                'Kiểm tra nếu chọn phân hệ làm việc là phân hệ hiện thời thì không cần Load lại Menu
                If sv_iSubSystemID = mv_iCurrentSubSystem Then Return
                '-------------------------------------------------------------------------
                'Xóa menu của phân hệ trước đó để xây menu của phân hệ được chọn
                If mv_oMenu.MenuCommands.Count > 1 Then
                    For i As Integer = 1 To mv_oMenu.MenuCommands.Count - 1
                        'Chú ý: Luôn xóa ở vị trí số 1
                        mv_oMenu.MenuCommands.Remove (mv_oMenu.MenuCommands (1))
                    Next
                End If
                '-------------------------------------------------------------------------
                If gv_sSubSystemDisplay.ToUpper.Equals ("DOCKING") Then
                Else
                    mv_oMenu.MenuCommands.Add (GetDefaultMenu)
                End If
                '-------------------------------------------------------------------------
                'Lấy danh sách nút trên Toolbar của phân hệ
                ms_GetAllToolBarButton (mv_oHTB (sv_iSubSystemID))
                '-------------------------------------------------------------------------
                'Xóa các nút trên Toolbar của phân hệ cũ
                If mv_dsIconPathForToolBarButton.Tables.Count > 0 Then
                    mv_dsIconPathForToolBarButton.Tables (0).Clear()
                End If
                mv_objImgForToolBarButton.Clear()
                '-------------------------------------------------------------------------
                'Xóa ảnh cũ
                Try
                    Dim k As Integer
                    k = gv_oMainForm.tbr.ImageList.Images.Count - 15
                    For i As Integer = 0 To k
                        gv_oMainForm.tbr.ImageList.Images.RemoveAt (14)
                    Next
                Catch ex As Exception

                End Try
                '-------------------------------------------------------------------------
                'Xây ToolBar mới
                mv_dsIconPathForToolBarButton = ms_dsIconList ("TBL_TOOLBARBUTTON")
                For i As Integer = 0 To mv_dsIconPathForToolBarButton.Tables (0).Rows.Count - 1
                    If File.Exists (mv_dsIconPathForToolBarButton.Tables (0).Rows (i) ("sIconPath")) Then
                        Try
                            gv_oMainForm.tbr.ImageList.Images.Add ( _
                                                                   Image.FromFile ( _
                                                                                   mv_dsIconPathForToolBarButton.Tables ( _
                                                                                                                         0) _
                                                                                      .Rows (i) ("sIconPath")))
                        Catch ex As Exception
                        End Try
                        mv_objImgForToolBarButton.Add (mv_dsIconPathForToolBarButton.Tables (0).Rows (i) ("sIconPath"))
                    End If
                Next
                BuildToolBarButtons (mv_oHTB (sv_iSubSystemID))
                '-------------------------------------------------------------------------
                'Xây Menu mới cho phân hệ
                BuildMenuItems (mv_oHTB (sv_iSubSystemID))
                'Nạp lại ảnh nền cho ứng dụng
                LoadBackGroundImage (mv_oHtbImgPath (sv_iSubSystemID))
                'Nạp lại CaptionText cho ứng dụng
                gv_oMainForm.Text = _
                    IIf (gv_sLanguageDisplay.ToUpper.Equals ("VN"), mv_oHtbVNName (sv_iSubSystemID), _
                         mv_oHtbEngName (sv_iSubSystemID))
                '& " - " & getVersionInfor(mv_oHtbDllName(sv_iSubSystemID))
                mv_iCurrentSubSystem = sv_iSubSystemID
                '-------------------------------------------------------------------------
                If gv_intPicOrder = 0 Then
                    Dim iu As New PicAboveText
                    Dim mn As New MenuItem
                    If _
                        sender.GetType.Equals (iu.GetType) Or sender.GetType.Equals (iu._Link.GetType) Or _
                        sender.GetType.Equals (iu.Picture.GetType) Then
                        Dim _l As LinkLabel
                        CheckChange (mn, sCurrLinkText)
                        Return
                    End If
                Else
                    Dim iu As New PicBeforeText
                    Dim mn As New MenuItem
                    If _
                        sender.GetType.Equals (iu.GetType) Or sender.GetType.Equals (iu._Link.GetType) Or _
                        sender.GetType.Equals (iu.Picture.GetType) Then
                        Dim _l As LinkLabel
                        CheckChange (mn, sCurrLinkText)
                        Return
                    End If
                End If
                Dim mnuCmd As New MenuCommand
                If sender.GetType.Equals (mnuCmd.GetType) Then
                    CheckChange (CType (sender, MenuCommand))
                End If

            End If
            Dim mnu As New MenuItem
            If sender.GetType.Equals (mnu.GetType) Then
                CheckChange (CType (sender, MenuItem))
            End If
        Catch ex As Exception
            CheckChange (CType (sender, MenuItem))
        End Try
    End Sub

    Private Sub _ClickExit (ByVal sender As Object, ByVal e As EventArgs)
        Try
            Application.Exit()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadBackGroundImage (ByVal pv_iImgPath As String)
        Try
            If Not IsDBNull (pv_iImgPath) Then
                If File.Exists (pv_iImgPath) Then
                    gv_oMainForm.PictureBox1.Image = Image.FromFile (pv_iImgPath)
                Else
                    gv_oMainForm.PictureBox1.Image = Image.FromFile (Application.StartupPath & "\Images\Default.JPG")
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Function getVersionInfor (ByVal DLLName As Object) As String
        Try
            Dim fileInfor As FileVersionInfo
            If IsDBNull (DLLName) Then
                Return "Version 0.0"
            End If
            fileInfor.GetVersionInfo (Application.StartupPath & "\" & DLLName)
            Return "Version " & fileInfor.ProductVersion
        Catch ex As Exception
            Return "Version 0.0"
        End Try
    End Function

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

    Private Sub ClearAllMenu()
        Try
            For Each mnuItem As MenuCommand In mv_oMenu.MenuCommands
                mv_oMenu.MenuCommands.Remove (mnuItem)
            Next
        Catch ex As Exception

        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Tạo ImageList để gán cho MenuCommand
    'Đầu vào         :None
    'Đầu ra           :Image List chứa tất cả các Icon của hệ thống Menu của ứng dụng
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub CreateImageList()
        Try
            For i As Integer = 0 To mv_dsIconPath.Tables (0).Rows.Count - 1
                If File.Exists (mv_dsIconPath.Tables (0).Rows (i) ("sIconPath")) Then
                    mv_objImageList.Images.Add (Image.FromFile (mv_dsIconPath.Tables (0).Rows (i) ("sIconPath")))
                    mv_objImg.Add (mv_dsIconPath.Tables (0).Rows (i) ("sIconPath"))
                End If
            Next
            For i As Integer = 0 To mv_dsIconPathForToolBarButton.Tables (0).Rows.Count - 1
                If File.Exists (mv_dsIconPathForToolBarButton.Tables (0).Rows (i) ("sIconPath")) Then
                    gv_oMainForm.tbr.ImageList.Images.Add ( _
                                                           Image.FromFile ( _
                                                                           mv_dsIconPathForToolBarButton.Tables (0).Rows ( _
                                                                                                                          i) ( _
                                                                                                                              "sIconPath")))
                    mv_objImgForToolBarButton.Add (mv_dsIconPathForToolBarButton.Tables (0).Rows (i) ("sIconPath"))
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Xử lý giá trị ShortCut trước khi đưa vào Menu
    'Đầu vào         :Mã ShortCut
    'Đầu ra           :Nếu chưa được gán trả về -1(None).
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Function ShortCutZero (ByVal pv_intValue As Integer) As Integer
        If pv_intValue = - 1 Then
            Return 0
        Else
            Return pv_intValue
        End If
    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Lấy về ImageIndex Icon của Menu trong Image List
    'Đầu vào         :Tên file Icon
    'Đầu ra           :Index tìm thấy
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Function intGetImgIndex (ByVal pv_sIconPath As String)
        Try
            For i As Integer = 0 To mv_objImg.Count - 1
                If mv_objImg (i) = pv_sIconPath Then
                    Return i
                End If
            Next
            Return - 1
        Catch ex As Exception
            Return - 1
        End Try
    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Lấy về ImageIndex Icon của Menu trong Image List
    'Đầu vào         :Tên file Icon
    'Đầu ra           :Index tìm thấy
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Function intGetImgIndexForToolBarButton (ByVal pv_sIconPath As String)
        Try
            For i As Integer = 0 To mv_objImgForToolBarButton.Count - 1
                If mv_objImgForToolBarButton (i) = pv_sIconPath Then
                    Return i + 14
                End If
            Next
            Return - 1
        Catch ex As Exception
            Return - 1
        End Try
    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Xử lý sự kiện khi người dùng chọn một phân hệ làm việc trên Menu
    'Đầu vào         :Phân hệ làm việc
    'Đầu ra           :Hủy chọn phân hệ trước đó và đánh dấu phân hệ được chọn
    'Người tạo      :CuongDV
    'Ngày tạo       :20/03/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub CheckChange (ByVal pv_oMnuItem As MenuCommand)
        Try
            For Each mnu As MenuCommand In mv_oSubSystemMenu.MenuCommands
                If mnu.Text = pv_oMnuItem.Text Then
                    mnu.Checked = True
                Else
                    mnu.Checked = False
                End If
            Next
            For Each mnu As MenuItem In mv_ContextMenu.MenuItems
                If mnu.Text = pv_oMnuItem.Text Then
                    mnu.Checked = True
                Else
                    mnu.Checked = False
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckChange (ByVal pv_oMnuItem As MenuItem, Optional ByVal s As String = "-10001000")
        Try
            'Chọn Menu tương ứng trong Menu danh sách phân hệ
            For Each mnu As MenuCommand In mv_oSubSystemMenu.MenuCommands
                If s <> "-10001000" Then
                    If _
                        mnu.Text.Substring (mnu.Text.lastIndexOf (".") + 1).Trim = _
                        IIf (s <> "-10001000", s, pv_oMnuItem.Text) Then
                        mnu.Checked = True
                    Else
                        mnu.Checked = False
                    End If
                Else
                    If mnu.Text = pv_oMnuItem.Text Then
                        mnu.Checked = True
                    Else
                        mnu.Checked = False
                    End If
                End If
            Next
            'Chọn Menu tương ứng phía ContextMenu
            For Each mnu As MenuItem In mv_ContextMenu.MenuItems
                If s <> "-10001000" Then
                    'If mnu.Text.Substring(mnu.Text.lastIndexOf(".") + 1).Trim = IIf(s <> "-10001000", s, pv_oMnuItem.Text) Then
                    If mnu.MergeOrder = mv_oHTB (mv_iCurrentSubSystem) Then
                        mnu.Checked = True
                    Else
                        mnu.Checked = False
                    End If
                Else
                    'If mnu.Text = pv_oMnuItem.Text Then
                    If mnu.MergeOrder = mv_oHTB (mv_iCurrentSubSystem) Then
                        mnu.Checked = True
                    Else
                        mnu.Checked = False
                    End If
                End If
            Next

            'Chọn Phân hệ tương ứng phía Docking Manager
            If gv_intPicOrder = 0 Then
                Dim iu As New PicAboveText
                For Each ctr As Control In SubSystemPanel.pnlBackGround.Controls
                    If ctr.GetType.Equals (iu.GetType) Then
                        'If CType(ctr, PicAboveText)._Link.Text <> IIf(s <> "-10001000", s, pv_oMnuItem.Text).ToString.Substring(IIf(s <> "-10001000", s, pv_oMnuItem.Text).ToString.LastIndexOf(".") + 1).Trim Then
                        If CType (ctr, PicAboveText)._LabelID <> mv_iCurrentSubSystem Then
                            CType (ctr, PicAboveText)._Link.LinkColor = Color.Black
                            CType (ctr, PicAboveText)._Link.Font = New Font ("Arial", 9.0F, FontStyle.Regular)
                            ChangeColorForElectedItem (DirectCast (ctr, PicAboveText), False)
                        Else
                            CType (ctr, PicAboveText)._Link.LinkColor = Color.DarkBlue
                            CType (ctr, PicAboveText)._Link.Font = New Font ("Arial", 10.0F, FontStyle.Bold)
                            ChangeColorForElectedItem (DirectCast (ctr, PicAboveText), True)
                        End If
                    End If
                Next
            Else
                Dim iu As New PicBeforeText
                For Each ctr As Control In SubSystemPanel.pnlBackGround.Controls
                    If ctr.GetType.Equals (iu.GetType) Then
                        'If CType(ctr, PicBeforeText)._Link.Text <> IIf(s <> "-10001000", s, pv_oMnuItem.Text).ToString.Substring(IIf(s <> "-10001000", s, pv_oMnuItem.Text).ToString.LastIndexOf(".") + 1).Trim Then
                        If CType (ctr, PicBeforeText)._LabelID <> mv_iCurrentSubSystem Then
                            CType (ctr, PicBeforeText)._Link.LinkColor = Color.Black
                            CType (ctr, PicBeforeText)._Link.Font = New Font ("Arial", 9.0F, FontStyle.Regular)
                            ChangeColorForElectedItemPBT (DirectCast (ctr, PicBeforeText), False)
                        Else
                            CType (ctr, PicBeforeText)._Link.LinkColor = Color.DarkBlue
                            CType (ctr, PicBeforeText)._Link.Font = New Font ("Arial", 10.0F, FontStyle.Bold)
                            ChangeColorForElectedItemPBT (DirectCast (ctr, PicBeforeText), True)
                        End If
                    End If
                Next
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Function getMenuControl() As MenuControl
        Dim m As New MenuControl
        Try
            For Each ctr As Control In gv_oMainForm.Controls
                If ctr.GetType.Equals (m.GetType) Then
                    Return CType (ctr, MenuControl)
                End If
            Next
        Catch ex As Exception

        End Try
    End Function

    Public Sub RebuildContextMenu()
        For Each mnuItem As MenuItem In mv_ContextMenu.MenuItems
            If gv_sLanguageDisplay.ToUpper = "VN" Then
                mnuItem.Text = _
                    mv_oSubSystemMenu.MenuCommands (mnuItem.Index).Tag.ToString.Substring (0, _
                                                                                           Len ( _
                                                                                                mv_oSubSystemMenu. _
                                                                                                   MenuCommands ( _
                                                                                                                 mnuItem _
                                                                                                                    . _
                                                                                                                    Index) _
                                                                                                   .Tag.ToString. _
                                                                                                   Substring (0, _
                                                                                                              mv_oSubSystemMenu _
                                                                                                                 . _
                                                                                                                 MenuCommands ( _
                                                                                                                               mnuItem _
                                                                                                                                  . _
                                                                                                                                  Index) _
                                                                                                                 .Tag. _
                                                                                                                 ToString _
                                                                                                                 . _
                                                                                                                 IndexOf ( _
                                                                                                                          "@"))))
            Else
                mnuItem.Text = _
                    mv_oSubSystemMenu.MenuCommands (mnuItem.Index).Tag.ToString.Substring ( _
                                                                                           mv_oSubSystemMenu. _
                                                                                               MenuCommands ( _
                                                                                                             mnuItem. _
                                                                                                                Index). _
                                                                                               Tag.ToString.IndexOf ("@") + _
                                                                                           1, _
                                                                                           Len ( _
                                                                                                mv_oSubSystemMenu. _
                                                                                                   MenuCommands ( _
                                                                                                                 mnuItem _
                                                                                                                    . _
                                                                                                                    Index) _
                                                                                                   .Tag.ToString. _
                                                                                                   Substring ( _
                                                                                                              mv_oSubSystemMenu _
                                                                                                                  . _
                                                                                                                  MenuCommands ( _
                                                                                                                                mnuItem _
                                                                                                                                   . _
                                                                                                                                   Index) _
                                                                                                                  .Tag. _
                                                                                                                  ToString _
                                                                                                                  . _
                                                                                                                  IndexOf ( _
                                                                                                                           "@") + _
                                                                                                              1)) - 2)
            End If
            'mnuItem.Text = mv_oSubSystemMenu.MenuCommands(mnuItem.Index).Text
        Next
    End Sub

#Region "DockingManager"

    Protected Friend _manager As DockingManager
    Protected Friend SubSystemContent, Branchcontent As Content
    Private mv_DR() As DataRow
    Dim sCurrLinkText As String = String.Empty
    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Tạo Docking Manager chứa các phân hệ làm việc trong trường hợp kiểu hiển thị phân hệ là Docking
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :02/04/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub CreateDockingManager()
        If Not _manager Is Nothing Then
            _manager.HideAllContents()
            _manager = Nothing
        End If
        ' Create the object that manages the docking state
        _manager = New DockingManager(CType(gv_oMainForm, FrmMain), VisualStyle.IDE)
        _manager.OuterControl = CType(gv_oMainForm, FrmMain).tbr
        _manager.OuterControl = CType(gv_oMainForm, FrmMain).StatusBar1
        _manager.CaptionFont = New Font("Arial", 9.0F)
        SubSystemPanel = New IFModulePanel
        SubSystemContent = _manager.Contents.Add(SubSystemPanel, "Phân hệ làm việc", CType(gv_oMainForm, FrmMain).imgAdminnistration, 4)
        SubSystemContent.CaptionBar = True
        SubSystemContent.CloseButton = False
        SubSystemContent.AutoHideSize = New Size (200, 400)
        SubSystemContent.DisplaySize = New Size (200, 400)
        DisplaySubSystem()
        'End If
        If mv_intSubSystemcount > 1 Then _manager.AddContentWithState (SubSystemContent, State.DockLeft)
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Lấy về Icon cho phân hệ dựa vào đường dẫn sẵn có
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :02/04/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Function GetIconForSubSystem (ByVal pv_intSubSystemID As Integer) As Image
        Try
            Dim img As Image
            If File.Exists (_IconForSubSystem (pv_intSubSystemID)) Then
                img = Image.FromFile (_IconForSubSystem (pv_intSubSystemID))
                Return img
            Else
                img = Image.FromFile (Application.StartupPath & "\Default.ico")
                Return img
            End If

        Catch ex As Exception
            Return Image.FromFile (Application.StartupPath & "\Default.ico")
        End Try

    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Tạo và hiển thị các phân hệ trong Docking Manager
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :02/04/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Protected Friend Sub DisplaySubSystem()

        Dim CurRow As DataRow
        Dim i As Integer
        Dim MaxHeight As Integer = 0
        Dim TooMany As Boolean = False

        For i = 0 To mv_intSubSystemcount - 1
            CurRow = mv_drSubSystem (i)
            With CurRow
                If Not IsDBNull (.Item ("iRoleID")) Then
                    'Nut goc
                    If gv_intPicOrder = 0 Then
                        Dim NewLabel As PicAboveText
                        Dim _tag As String
                        _tag = .Item ("sRoleName") & "@" & IsNull_VNEN (.Item ("sRoleName"), .Item ("sEngRoleName")) & _
                               "$" & .Item ("iRoleID")
                        '"$1"
                        If gv_sLanguageDisplay.ToUpper.Equals ("VN") Then
                            NewLabel = _
                                _PicAboveText (GetIconForSubSystem (.Item ("iRoleID")), .Item ("sRoleName"), _tag)
                        Else
                            NewLabel = _
                                _PicAboveText (GetIconForSubSystem (.Item ("iRoleID")), _
                                               IsNull_VNEN (.Item ("sRoleName"), .Item ("sEngRoleName")), _tag)
                        End If

                        MaxHeight = NewLabel.Height*i
                        '+ IIf(i = 0, 5, 0)
                        If i = 0 Then
                            NewLabel.Top = 0
                        Else
                            NewLabel.Top = MaxHeight
                        End If

                        NewLabel._LabelID = i + 1
                        If i = 0 Then
                            NewLabel._Link.LinkColor = Color.DarkBlue
                            NewLabel._Link.Font = New Font ("Arial", 9.0F, FontStyle.Bold)
                        Else
                            NewLabel._Link.LinkColor = Color.Black
                            NewLabel._Link.Font = New Font ("Arial", 9.0F, FontStyle.Regular)
                        End If
                        SubSystemPanel.pnlBackGround.Controls.Add (NewLabel)
                        NewLabel.Anchor = _
                            CType ((AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top), AnchorStyles)
                        'If MaxHeight + NewLabel.Height > SubSystemPanel.pnlBackGround.Height Then
                        'TooMany = True
                        'End If
                    Else
                        Dim NewLabel As PicBeforeText
                        Dim _tag As String
                        _tag = .Item ("sRoleName") & "@" & IsNull_VNEN (.Item ("sRoleName"), .Item ("sEngRoleName")) & _
                               "$" & .Item ("iRoleID")
                        '"$1"
                        If gv_sLanguageDisplay.ToUpper.Equals ("VN") Then
                            NewLabel = _
                                _PicBeforeText (GetIconForSubSystem (.Item ("iRoleID")), .Item ("sRoleName"), _tag)
                        Else
                            NewLabel = _
                                _PicBeforeText (GetIconForSubSystem (.Item ("iRoleID")), _
                                                IsNull_VNEN (.Item ("sRoleName"), .Item ("sEngRoleName")), _tag)
                        End If

                        MaxHeight = NewLabel.Height*i
                        '+ IIf(i = 0, 5, 0)
                        If i = 0 Then
                            NewLabel.Top = 0
                        Else
                            NewLabel.Top = MaxHeight
                        End If

                        NewLabel._LabelID = i + 1
                        If i = 0 Then
                            NewLabel._Link.LinkColor = Color.DarkBlue
                            NewLabel._Link.Font = New Font ("Arial", 9.0F, FontStyle.Bold)
                        Else
                            NewLabel._Link.LinkColor = Color.Black
                            NewLabel._Link.Font = New Font ("Arial", 9.0F, FontStyle.Regular)
                        End If
                        SubSystemPanel.pnlBackGround.Controls.Add (NewLabel)
                        NewLabel.Anchor = _
                            CType ((AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top), AnchorStyles)
                        'If MaxHeight + NewLabel.Height > SubSystemPanel.pnlBackGround.Height Then
                        '    TooMany = True
                        'End If
                    End If
                End If
            End With
        Next
        'If TooMany Then
        '    SubSystemPanel.ptbDown.Visible = True
        '    SubSystemPanel.ptbUp.Image = SubSystemPanel.imlUpDown.Images.Item(7)
        '    AllowUp = False
        '    AllowDown = True
        'Else
        '    SubSystemPanel.ptbDown.Visible = False
        'End If
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Tạo các phân hệ hiển thị Docking Manager
    'Đầu vào         :Icon phân hệ, tên phân hệ
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :02/04/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Function _PicAboveText (ByVal ImageOfLabel As Image, ByVal Label As String, ByVal _tag As String) _
        As PicAboveText

        Dim NewLabel As New PicAboveText

        If Not ImageOfLabel Is Nothing Then
            NewLabel.Picture.Image = ImageOfLabel
            NewLabel._Link.Text = Label
            NewLabel.Tag = _tag
            NewLabel._Link.Tag = _tag
        Else
            NewLabel._Link.Text = Label
            NewLabel.Tag = _tag
            NewLabel._Link.Tag = _tag
        End If
        AddHandler NewLabel.Click, AddressOf LabelClick
        AddHandler NewLabel.Picture.Click, AddressOf LabelClick
        AddHandler NewLabel._Link.Click, AddressOf LabelClick
        Return NewLabel

    End Function

    Private Function _PicBeforeText (ByVal ImageOfLabel As Image, ByVal Label As String, ByVal _tag As String) _
        As PicBeforeText

        Dim NewLabel As New PicBeforeText

        If Not ImageOfLabel Is Nothing Then
            NewLabel.Picture.Image = ImageOfLabel
            NewLabel._Link.Text = Label
            NewLabel.Tag = _tag
            NewLabel._Link.Tag = _tag
        Else
            NewLabel._Link.Text = Label
            NewLabel._Link.Tag = _tag
        End If
        AddHandler NewLabel.Click, AddressOf LabelClick
        AddHandler NewLabel.Picture.Click, AddressOf LabelClick
        AddHandler NewLabel._Link.Click, AddressOf LabelClick
        Return NewLabel

    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Thay đổi màu nền của phân hệ làm việc hiện thời để dễ nhận biết Phân hệ đang làm việc
    'Đầu vào         :Phân hệ làm việc, Trạng thái được chọn hoặc không
    'Đầu ra           :Chuyển đổi ngôn ngữ VN-->Eng hoặc ngược lại
    'Người tạo      :CuongDV
    'Ngày tạo       :20/04/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub ChangeColorForElectedItem (ByVal pv_objSubSystem As PicAboveText, _
                                           Optional ByVal pv_bSelected As Boolean = False)
        Try
            If pv_bSelected Then
                pv_objSubSystem.BackColor = Color.FromArgb (192, 192, 255)
                For Each ctr As Control In pv_objSubSystem.Controls
                    ctr.BackColor = Color.FromArgb (192, 192, 255)
                Next
            Else
                pv_objSubSystem.BackColor = Color.White
                For Each ctr As Control In pv_objSubSystem.Controls
                    ctr.BackColor = Color.White
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ChangeColorForElectedItemPBT (ByVal pv_objSubSystem As PicBeforeText, _
                                              Optional ByVal pv_bSelected As Boolean = False)
        Try
            If pv_bSelected Then
                pv_objSubSystem.BackColor = Color.FromArgb (192, 192, 255)
                For Each ctr As Control In pv_objSubSystem.Controls
                    ctr.BackColor = Color.FromArgb (192, 192, 255)
                Next
            Else
                pv_objSubSystem.BackColor = Color.White
                For Each ctr As Control In pv_objSubSystem.Controls
                    ctr.BackColor = Color.White
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Sự kiện Click chọn phân hệ làm việc mới
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :02/04/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub LabelClick (ByVal sender As Object, ByVal e As EventArgs)
        If gv_intPicOrder = 0 Then
            Dim lblCurrent As PicAboveText
            Dim lbl As New PicAboveText
            Try
                If sender.GetType.Equals (lbl.GetType) Then
                    lblCurrent = DirectCast (sender, PicAboveText)
                Else
                    lblCurrent = DirectCast (sender.parent, PicAboveText)
                End If
                If Not lblCurrent Is Nothing Then

                    For Each c As Control In SubSystemPanel.pnlBackGround.Controls
                        Try
                            If c.Name.ToUpper = "PicAboveText".ToUpper Then
                                Dim l As PicAboveText = DirectCast (c, PicAboveText)
                                If Not l Is Nothing Then
                                    l._Link.LinkColor = Color.Black
                                    l._Link.Font = New Font ("Arial", 9.0F, FontStyle.Regular)
                                    ChangeColorForElectedItem (c)
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    Next
                    lblCurrent._Link.LinkColor = Color.DarkBlue
                    lblCurrent._Link.Font = New Font ("Arial", 10.0F, FontStyle.Bold)
                    mv_iIDfromTab = lblCurrent._LabelID
                    sCurrLinkText = lblCurrent._Link.Text
                    ChangeColorForElectedItem (lblCurrent, True)
                    _Click (sender, e)

                End If
            Catch ex As Exception

            End Try
        Else
            Dim lblCurrent As PicBeforeText
            Dim lbl As New PicBeforeText
            Try
                If sender.GetType.Equals (lbl.GetType) Then
                    lblCurrent = DirectCast (sender, PicBeforeText)
                Else
                    lblCurrent = DirectCast (sender.parent, PicBeforeText)
                End If
                If Not lblCurrent Is Nothing Then

                    For Each c As Control In SubSystemPanel.pnlBackGround.Controls
                        Try
                            If c.Name.ToUpper = "PicBeforeText".ToUpper Then
                                Dim l As PicBeforeText = DirectCast (c, PicBeforeText)
                                If Not l Is Nothing Then
                                    l._Link.LinkColor = Color.Black
                                    l._Link.Font = New Font ("Arial", 9.0F, FontStyle.Regular)
                                    ChangeColorForElectedItemPBT (c)
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    Next
                    lblCurrent._Link.LinkColor = Color.DarkBlue
                    lblCurrent._Link.Font = New Font ("Arial", 10.0F, FontStyle.Bold)
                    mv_iIDfromTab = lblCurrent._LabelID
                    sCurrLinkText = lblCurrent._Link.Text
                    ChangeColorForElectedItemPBT (lblCurrent, True)
                    _Click (sender, e)

                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Thay đổi ngôn ngữ làm việc cho menu phân hệ khi kiểu hiển thị phân hệ là Docking
    'Đầu vào         :
    'Đầu ra           :Chuyển đổi ngôn ngữ VN-->Eng hoặc ngược lại
    'Người tạo      :CuongDV
    'Ngày tạo       :02/04/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Public Sub ChangeSubSystemLanguage()
        Dim iu As New PicAboveText
        Dim splt1(), splt2() As String
        'Thay đổi ngôn ngữ phía Docking phân hệ làm việc
        If gv_sSubSystemDisplay.ToUpper.Equals ("DOCKING") Then
            For Each ctr As Control In SubSystemPanel.pnlBackGround.Controls
                If ctr.GetType.Equals (iu.GetType) Then
                    splt1 = CType (ctr, PicAboveText)._Link.Tag.ToString.Split ("@")
                    splt2 = splt1 (1).Split ("$")
                    If gv_sLanguageDisplay.ToUpper = "VN" Then
                        CType (ctr, PicAboveText)._Link.Text = splt1 (0)
                        'CType(ctr, PicAboveText)._Link.Tag.ToString.Substring(0, Len(CType(ctr, PicAboveText)._Link.Tag.ToString.Substring(0, CType(ctr, PicAboveText)._Link.Tag.ToString.IndexOf("@"))))
                    Else
                        CType (ctr, PicAboveText)._Link.Text = splt2 (0)
                        ' CType(ctr, PicAboveText)._Link.Tag.ToString.Substring(CType(ctr, PicAboveText)._Link.Tag.ToString.IndexOf("@") + 1, Len(CType(ctr, PicAboveText)._Link.Tag.ToString.Substring(CType(ctr, PicAboveText)._Link.Tag.ToString.IndexOf("@") + 1)) - 2)
                    End If
                End If
            Next
        End If
    End Sub

    Public Sub ChangeSubSystemLanguagePBT()
        Dim iu As New PicBeforeText
        Dim splt1(), splt2() As String
        'Thay đổi ngôn ngữ phía Docking phân hệ làm việc
        If gv_sSubSystemDisplay.ToUpper.Equals ("DOCKING") Then
            For Each ctr As Control In SubSystemPanel.pnlBackGround.Controls
                If ctr.GetType.Equals (iu.GetType) Then
                    splt1 = CType (ctr, PicBeforeText)._Link.Tag.ToString.Split ("@")
                    splt2 = splt1 (1).Split ("$")
                    If gv_sLanguageDisplay.ToUpper = "VN" Then
                        CType (ctr, PicBeforeText)._Link.Text = splt1 (0)
                        ' CType(ctr, PicBeforeText)._Link.Tag.ToString.Substring(0, Len(CType(ctr, PicBeforeText)._Link.Tag.ToString.Substring(0, CType(ctr, PicBeforeText)._Link.Tag.ToString.IndexOf("@"))))
                    Else
                        CType (ctr, PicBeforeText)._Link.Text = splt2 (0)
                        ' CType(ctr, PicBeforeText)._Link.Tag.ToString.Substring(CType(ctr, PicBeforeText)._Link.Tag.ToString.IndexOf("@") + 1, Len(CType(ctr, PicBeforeText)._Link.Tag.ToString.Substring(CType(ctr, PicBeforeText)._Link.Tag.ToString.IndexOf("@") + 1)) - 2)
                    End If
                End If
            Next
        End If
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Enable hoặc Disable menu phân hệ làm việc khi thực hiện Logout hoặc Login
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :20/04/2006
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub EnableSubSystem (Optional ByVal pv_bValue As Boolean = True)
        Dim iu As New PicAboveText
        'Thay đổi ngôn ngữ phía Docking phân hệ làm việc
        If gv_sSubSystemDisplay.ToUpper.Equals ("DOCKING") Then
            For Each ctr As Control In SubSystemPanel.pnlBackGround.Controls
                If ctr.GetType.Equals (iu.GetType) Then
                    ctr.Enabled = pv_bValue
                End If
            Next
        Else

        End If
        For Each mnuItem As MenuItem In mv_ContextMenu.MenuItems
            mnuItem.Enabled = pv_bValue
        Next
    End Sub

    Private Sub EnableSubSystemPBT (Optional ByVal pv_bValue As Boolean = True)
        Dim iu As New PicBeforeText
        'Thay đổi ngôn ngữ phía Docking phân hệ làm việc
        If gv_sSubSystemDisplay.ToUpper.Equals ("DOCKING") Then
            For Each ctr As Control In SubSystemPanel.pnlBackGround.Controls
                If ctr.GetType.Equals (iu.GetType) Then
                    ctr.Enabled = pv_bValue
                End If
            Next
        Else

        End If
        For Each mnuItem As MenuItem In mv_ContextMenu.MenuItems
            mnuItem.Enabled = pv_bValue
        Next
    End Sub

#End Region
End Class
