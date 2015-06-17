Imports System.ComponentModel
Imports Crownwood.Magic.Menus
Imports System.Threading
Imports DevComponents.DotNetBar

Public Class FrmMain
    Inherits DevComponents.DotNetBar.Office2007Form


#Region "Load Background Image"
    Private Sub LoadDefaultBackGround()
        Try
            If IO.File.Exists(Application.StartupPath & "\Images\Default.jpg") Then
                Me.PictureBox1.Image = Image.FromFile(Application.StartupPath & "\Images\Default.jpg")
                Return
            ElseIf IO.File.Exists(Application.StartupPath & "\Images\Default.Gif") Then
                Me.PictureBox1.Image = Image.FromFile(Application.StartupPath & "\Images\Default.Gif")
                Return
            ElseIf IO.File.Exists(Application.StartupPath & "\Images\Default.Bmp") Then
                Me.PictureBox1.Image = Image.FromFile(Application.StartupPath & "\Images\Default.Bmp")
                Return
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

    Protected Friend mnuMain As New MenuControl
    Dim mv_omenu As cls_BuildMenu
    Private mv_arrCtr As New ArrayList

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        'Load hình nền mặc định
        LoadDefaultBackGround()
        TryToSetStyle(Vietbait.Lablink.Utilities.LablinkBusinessConfig.GetStyle())
        If LoginSystem(sender, e) Then

        Else
            Application.Exit()
        End If
    End Sub

    Private Function LoginSystem(ByVal sender As Object, ByVal e As EventArgs) As Boolean
        Dim clsUtil As New clsUtility
        If gv_oSqlCnn Is Nothing Then
            Return False
        End If
        If Not clsUtil.CheckTable("TBL_MANAGEMENTUNIT") Then
            Dim sv_oForm As New Frm_AddBranch
            sv_oForm.ShowDialog()
        End If
        If Not clsUtil.CheckTable("TBL_MANAGEMENTUNIT", "PK_sBranchID=N'" & gv_sBranchID & "'") Then
            MessageBox.Show("Không tồn tại chi nhánh làm việc có mã: " & gv_sBranchID & _
                             " trong CSDL. Đề nghị bạn chỉnh lại thông tin file cấu hình cho đúng", gv_sAnnouncement, _
                             MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If
        GetBranchInfo()
        Dim clsReg As New clsRegistry.clsRegistry
        gv_bAnnouceWhenHavingLastestVersion = _
            IIf(clsReg.GetReg(2, "DVC_COMPANY", "SYSMAN_APP", "AnnouceWhenHavingLastestVersion") = "0", False, True)
        Dim sv_oLoginForm As New frmLogin
        sv_oLoginForm.ShowDialog()
        Try
            'Kiểm tra nếu đăng nhập thành công
            If gv_bLoginSuccess Then
                If gv_bAnnouceWhenHavingLastestVersion Then
                    Dim threCheckUpdate As Thread
                    threCheckUpdate = New Thread(AddressOf CheckLastestVersion)
                    threCheckUpdate.Start()
                End If
                Initialize(sender, e)
                Application.DoEvents()
            Else
                Me.Close()
            End If
            tbr.Buttons(1).Text = IIf(gv_sLanguageDisplay.ToUpper.Equals("VN"), "EN", "VN")
            tbr.Buttons(1).ToolTipText = _
                IIf(gv_sLanguageDisplay.ToUpper.Equals("VN"), "Chuyển sang giao diện Tiếng Anh", _
                     "Chuyển sang giao diện Tiếng Việt")
            'Me.Opacity = 1
            If gv_intWidth * gv_intSubSystemCount > SubSystemPanel.pnlBackGround.Height Then
                SubSystemPanel.ptbDown.Visible = True
                SubSystemPanel.ptbUp.Image = SubSystemPanel.imlUpDown.Images.Item(7)
                AllowUp = False
                AllowDown = True
            Else
                SubSystemPanel.ptbDown.Visible = False
            End If
            Return True
            'Me.Cursor = Cursors.Arrow
        Catch ex As Exception
            Me.Cursor = Cursors.Arrow
            'Me.Opacity = 1
            'mv_oFLashForm.thre.Abort()
            'mv_oFLashForm.Close()
            Return False
        End Try
    End Function

    Private Sub Initialize(ByVal sender As Object, ByVal e As EventArgs)
        mv_omenu = New cls_BuildMenu(Me)
        mnuMain = mv_omenu.GetMainMenu
        Me.ContextMenu = mv_omenu.mv_ContextMenu
        'mv_intSubSystemCount = mv_omenu.mv_intSubSystemcount
        'mv_DR = mv_omenu.mv_drSubSystem
        Me.Controls.Add(mnuMain)
        'Me.tbr.Buttons.AddRange(mv_omenu.mv_oTbrButton)
        'CreateDockingManager()
        LoadMainInfo()
    End Sub

    Private Sub LoadMainInfo()
        Me.StatusBar1.Panels(0).Text = gv_sBranchName
        Me.StatusBar1.Panels(1).Text = "   Người dùng :" & gv_sUID & "   "
        Me.StatusBar1.Panels(2).Text = Now.ToShortDateString
    End Sub

    Private Sub tbr_ButtonClick(ByVal sender As Object, ByVal e As ToolBarButtonClickEventArgs) Handles tbr.ButtonClick
        Try
            Select Case e.Button.Tag
                Case "-1"
                    Dim sv_oFrmLogon As New frmLogin
                    sv_oFrmLogon.ShowDialog()
                    'Nếu người dùng nhấn nút Close
                    If gv_bReturnHome = True Then
                    Else
                        mv_arrCtr.Clear()
                        'Xóa tất cả các Control ngoại trừ Toolbar và StatusBar trên Form Main
                        For Each ctr As Control In Me.Controls
                            If _
                                ctr.Name.Equals(Me.tbr.Name) Or ctr.Name.Equals(Me.StatusBar1.Name) Or _
                                ctr.Name.Equals(Me.PictureBox1.Name) Then
                            Else
                                mv_arrCtr.Add(ctr)
                            End If
                        Next
                        For i As Integer = 0 To mv_arrCtr.Count - 1
                            Me.Controls.Remove(mv_arrCtr(i))
                        Next
                        '************************************************
                        '************************************************
                        RemoveHandler tbr.ButtonClick, AddressOf _ClickTbr
                        Initialize(sender, e)
                        'Thủ thuật để ko bị lỗi MainMenu ko nhận Focus khi có 1 phân hệ kiểu Docking
                        Me.WindowState = FormWindowState.Minimized
                        Me.WindowState = FormWindowState.Maximized
                    End If
                Case "-2"
                    If tbr.Buttons(1).Text = "VN" Then
                        tbr.Buttons(1).Text = "EN"
                        gv_sLanguageDisplay = "VN"
                    Else
                        tbr.Buttons(1).Text = "VN"
                        gv_sLanguageDisplay = "EN"
                    End If

                    ChangeLanguage()

                    tbr.Buttons(1).ToolTipText = _
                        IIf(gv_sLanguageDisplay.ToUpper.Equals("VN"), "Chuyển sang giao diện Tiếng Anh", _
                             "Chuyển sang giao diện Tiếng Việt")
                Case "-3"
                    Application.Exit()
                Case "-4"
                    Dim frm As New FRM_SetParamSystem
                    frm.ShowDialog()
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RecursiveChangeLanguageTbr(ByVal pv_oMnuItem As MenuItem)
        Try
            For Each mnuItem As _MenuItem In pv_oMnuItem.MenuItems
                If Not mnuItem.Tag Is Nothing Then
                    If gv_sLanguageDisplay.ToUpper = "VN" Then
                        mnuItem.Text = mnuItem.Tag.ToString.Substring(0, Len(mnuItem.Tag.ToString.Substring(0, mnuItem.Tag.ToString.IndexOf("@"))))
                    Else
                        mnuItem.Text = mnuItem.Tag.ToString.Substring(mnuItem.Tag.ToString.IndexOf("@") + 1, Len(mnuItem.Tag.ToString.Substring(mnuItem.Tag.ToString.IndexOf("@") + 1)) - 2)
                    End If
                    RecursiveChangeLanguageTbr(mnuItem)
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Sub RecursiveChangeLanguage(ByVal pv_oMnuItem As MenuCommand)
        Try
            For Each mnuItem As MenuCommand In pv_oMnuItem.MenuCommands
                If Not mnuItem.Tag Is Nothing Then
                    If gv_sLanguageDisplay.ToUpper = "VN" Then
                        mnuItem.Text = _
                            mnuItem.Tag.ToString.Substring(0, Len(mnuItem.Tag.ToString.Substring(0, mnuItem.Tag.ToString.IndexOf("@"))))
                    Else
                        mnuItem.Text = mnuItem.Tag.ToString.Substring(mnuItem.Tag.ToString.IndexOf("@") + 1, Len(mnuItem.Tag.ToString.Substring(mnuItem.Tag.ToString.IndexOf("@") + 1)) - 2)
                    End If
                    RecursiveChangeLanguage(mnuItem)
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ChangeLanguage()
        Try
            For Each mnuItem As MenuCommand In getMenuControl.MenuCommands
                If Not mnuItem.Tag Is Nothing And mnuItem.Tag <> "-111" Then
                    If gv_sLanguageDisplay.ToUpper = "VN" Then
                        mnuItem.Text = _
                            mnuItem.Tag.ToString.Substring(0, _
                                                            Len( _
                                                                 mnuItem.Tag.ToString.Substring(0, _
                                                                                                 mnuItem.Tag.ToString. _
                                                                                                    IndexOf("@"))))
                    Else
                        mnuItem.Text = _
                            mnuItem.Tag.ToString.Substring(mnuItem.Tag.ToString.IndexOf("@") + 1, _
                                                            Len( _
                                                                 mnuItem.Tag.ToString.Substring( _
                                                                                                 mnuItem.Tag.ToString. _
                                                                                                     IndexOf("@") + 1)) - _
                                                            2)
                    End If
                    RecursiveChangeLanguage(mnuItem)
                Else
                    RecursiveChangeLanguage(mnuItem)
                End If
            Next
            '**********************************************
            'For Each btn As ToolBarButton In Me.tbr.Buttons
            '    If btn.Tag = "-1" Or btn.Tag = "-2" Or btn.Tag = "-3" Or btn.Tag = "-4" Or btn.Tag = "-5" Then
            '    Else 'Xet cac Button duoc xay dung
            '        Dim btn1 As ToolBarBtn = CType(btn, ToolBarBtn)
            '        If btn1.bDisplayText Then
            '            btn1.Text = sGetTextDisplay(btn1.IntRolePerformed)
            '        End If
            '        If btn.ToolTipText.Trim = "" Then
            '        Else
            '            btn.ToolTipText = sGetTextDisplay(btn1.IntRolePerformed)
            '        End If
            '    End If
            'Next
            '**********************************************
            '**********************************************
            For Each btn As ToolBarButton In Me.tbr.Buttons
                If Not btn.DropDownMenu Is Nothing Then
                    For Each mnuItem As _MenuItem In CType(btn, ToolBarBtn).DropDownMenu.MenuItems
                        If Not mnuItem.Tag Is Nothing And mnuItem.Tag <> "-111" Then
                            If gv_sLanguageDisplay.ToUpper = "VN" Then
                                mnuItem.Text = _
                                    mnuItem.Tag.ToString.Substring(0, _
                                                                    Len( _
                                                                         mnuItem.Tag.ToString.Substring(0, _
                                                                                                         mnuItem.Tag. _
                                                                                                            ToString. _
                                                                                                            IndexOf("@"))))
                            Else
                                mnuItem.Text = _
                                    mnuItem.Tag.ToString.Substring(mnuItem.Tag.ToString.IndexOf("@") + 1, _
                                                                    Len( _
                                                                         mnuItem.Tag.ToString.Substring( _
                                                                                                         mnuItem.Tag. _
                                                                                                             ToString. _
                                                                                                             IndexOf( _
                                                                                                                      "@") + _
                                                                                                         1)) - 2)
                            End If
                            RecursiveChangeLanguageTbr(mnuItem)
                        Else
                            RecursiveChangeLanguageTbr(mnuItem)
                        End If
                    Next
                End If
            Next
            '**********************************************
            'If getMenuControl.MenuCommands(0).Tag = "-111" Then
            '    For Each mnuItem As MenuItem In Me.ContextMenu.MenuItems
            '        mnuItem.Text = getMenuControl.MenuCommands(0).MenuCommands(mnuItem.Index).Text
            '    Next
            'End If
            mv_omenu.RebuildContextMenu()
            If gv_intPicOrder = 0 Then
                mv_omenu.ChangeSubSystemLanguage()
            Else
                mv_omenu.ChangeSubSystemLanguagePBT()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function getMenuControl() As MenuControl
        Dim m As New MenuControl
        Try
            For Each ctr As Control In Me.Controls
                If ctr.GetType.Equals(m.GetType) Then
                    Return CType(ctr, MenuControl)
                End If
            Next
        Catch ex As Exception

        End Try
    End Function

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As CancelEventArgs) Handles MyBase.Closing
        e.Cancel = Not mv_bCanClose
    End Sub

    Private Sub frmMain_SizeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.SizeChanged
        Try
            If gv_intWidth * gv_intSubSystemCount > SubSystemPanel.pnlBackGround.Height Then
                SubSystemPanel.ptbDown.Visible = True
                SubSystemPanel.ptbUp.Image = SubSystemPanel.imlUpDown.Images.Item(7)
                If SubSystemPanel.pnlBackGround.Controls(2).Top < 0 Then
                    AllowUp = True
                    SubSystemPanel.ptbUp.Visible = True
                Else
                    SubSystemPanel.ptbUp.Visible = False
                    AllowUp = False
                End If
                AllowDown = True
            Else
                If SubSystemPanel.pnlBackGround.Controls(2).Top < 0 Then
                    AllowUp = True
                    SubSystemPanel.ptbUp.Visible = True
                Else
                    SubSystemPanel.ptbUp.Visible = False
                    AllowUp = False
                End If
                SubSystemPanel.ptbDown.Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub StatusBar1_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBar1.DoubleClick
        If Not gv_bIsChecking Then
            gv_bCallCheckUpdate = True
            Dim threCheckUpdate As Thread
            threCheckUpdate = New Thread(AddressOf CheckLastestVersion)
            threCheckUpdate.Start()
        End If

    End Sub

    Private Sub TryToSetStyle(ByVal style As String)
        Dim myStyle = DirectCast([Enum].Parse(GetType(eStyle), style), eStyle)
        Dim newStyleManager = New StyleManager(components)
        newStyleManager.ManagerStyle = myStyle
    End Sub

    Public Delegate Sub myDelegate()

End Class