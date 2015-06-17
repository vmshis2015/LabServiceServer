Imports System.ComponentModel

Public Class frmLogin
    Inherits Form
    Public mv_bCallFromLogin As Boolean = False
    Public mv_bLoginSuccess As Boolean = False

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        gv_bIncreateOrDecrete = True
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose (ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose (disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Label1 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents cmdLogin As Button
    Friend WithEvents cmdClose As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtPWD As TextBox
    Friend WithEvents txtUID As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents PictureBox1 As PictureBox

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As ComponentResourceManager = New ComponentResourceManager (GetType (frmLogin))
        Me.Label1 = New Label
        Me.Label5 = New Label
        Me.Label6 = New Label
        Me.cmdLogin = New Button
        Me.cmdClose = New Button
        Me.GroupBox1 = New GroupBox
        Me.txtPWD = New TextBox
        Me.txtUID = New TextBox
        Me.Label3 = New Label
        Me.Label2 = New Label
        Me.PictureBox1 = New PictureBox
        Me.GroupBox1.SuspendLayout()
        CType (Me.PictureBox1, ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = SystemColors.Window
        Me.Label1.ForeColor = Color.Black
        Me.Label1.Location = New Point (0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size (364, 68)
        Me.Label1.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.BackColor = SystemColors.ControlLightLight
        Me.Label5.Font = New Font ("Arial", 9.75!, FontStyle.Bold, GraphicsUnit.Point, CType (0, Byte))
        Me.Label5.Location = New Point (12, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New Size (184, 16)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Đăng nhập hệ thống"
        '
        'Label6
        '
        Me.Label6.BackColor = SystemColors.ControlLightLight
        Me.Label6.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label6.Location = New Point (16, 28)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New Size (272, 16)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "Chứng thực quyền sử dụng của người dùng"
        '
        'cmdLogin
        '
        Me.cmdLogin.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.cmdLogin.Location = New Point (76, 180)
        Me.cmdLogin.Name = "cmdLogin"
        Me.cmdLogin.Size = New Size (92, 28)
        Me.cmdLogin.TabIndex = 2
        Me.cmdLogin.Text = "&Chấp nhận"
        '
        'cmdClose
        '
        Me.cmdClose.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.cmdClose.Location = New Point (176, 180)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New Size (92, 28)
        Me.cmdClose.TabIndex = 3
        Me.cmdClose.Text = "&Kết thúc"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add (Me.txtPWD)
        Me.GroupBox1.Controls.Add (Me.txtUID)
        Me.GroupBox1.Controls.Add (Me.Label3)
        Me.GroupBox1.Controls.Add (Me.Label2)
        Me.GroupBox1.Location = New Point (0, 68)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New Size (336, 100)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'txtPWD
        '
        Me.txtPWD.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtPWD.Location = New Point (120, 60)
        Me.txtPWD.MaxLength = 25
        Me.txtPWD.Name = "txtPWD"
        Me.txtPWD.PasswordChar = ChrW (42)
        Me.txtPWD.Size = New Size (184, 22)
        Me.txtPWD.TabIndex = 1
        '
        'txtUID
        '
        Me.txtUID.CharacterCasing = CharacterCasing.Upper
        Me.txtUID.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtUID.Location = New Point (120, 28)
        Me.txtUID.MaxLength = 25
        Me.txtUID.Name = "txtUID"
        Me.txtUID.Size = New Size (184, 22)
        Me.txtUID.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label3.Location = New Point (8, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New Size (104, 20)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "&Mật khẩu"
        Me.Label3.TextAlign = ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label2.Location = New Point (8, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New Size (104, 20)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "&Tên đăng nhập"
        Me.Label2.TextAlign = ContentAlignment.MiddleRight
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType ((AnchorStyles.Top Or AnchorStyles.Right), AnchorStyles)
        Me.PictureBox1.BackColor = SystemColors.ControlLightLight
        Me.PictureBox1.Image = CType (resources.GetObject ("PictureBox1.Image"), Image)
        Me.PictureBox1.Location = New Point (284, - 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New Size (48, 68)
        Me.PictureBox1.TabIndex = 7
        Me.PictureBox1.TabStop = False
        '
        'frmLogin
        '
        Me.AutoScaleBaseSize = New Size (5, 13)
        Me.BackColor = SystemColors.Control
        Me.ClientSize = New Size (338, 223)
        Me.Controls.Add (Me.GroupBox1)
        Me.Controls.Add (Me.cmdClose)
        Me.Controls.Add (Me.cmdLogin)
        Me.Controls.Add (Me.Label6)
        Me.Controls.Add (Me.Label5)
        Me.Controls.Add (Me.PictureBox1)
        Me.Controls.Add (Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType (resources.GetObject ("$this.Icon"), Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmLogin"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Đăng nhập hệ thống"
        Me.GroupBox1.ResumeLayout (False)
        Me.GroupBox1.PerformLayout()
        CType (Me.PictureBox1, ISupportInitialize).EndInit()
        Me.ResumeLayout (False)

    End Sub

#End Region

    Private Function mf_bCheckData() As Boolean
        Try
            If txtUID.Text.Trim.Equals (String.Empty) Then
                MessageBox.Show ("Bạn phải nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, _
                                 MessageBoxIcon.Information)
                txtUID.Focus()
                Return False
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub frmLogin_Load (ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Dim sv_sUID, sv_sPwd As String
        Dim _clsRegistry As New clsRegistry.clsRegistry
        Try
            If mv_bCallFromLogin Then
                txtUID.Text = ""
                txtPWD.Text = ""
                txtUID.Focus()
            Else
                sv_sUID = _clsRegistry.GetReg (2, "DVC_COMPANY", "APP_DVC", "APP_UID")
                sv_sPwd = _clsRegistry.GetReg (2, "DVC_COMPANY", "APP_DVC", "APP_PWD")
                txtUID.Text = sv_sUID.Trim
                'txtPWD.Text = sv_sPwd.Trim
                txtUID.Focus()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmLogin_KeyPress (ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles MyBase.KeyPress
        Try
            If Asc (e.KeyChar) = Keys.Enter Then Me.ProcessTabKey (True)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub cmdLogin_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdLogin.Click
        Dim sv_sUID As String = String.Empty
        Dim sv_sPWD As String = String.Empty
        Dim sv_oEncrypt As New Encrypt.Encrypt (gv_sSymmetricAlgorithmName)
        Dim _clsRegistry As New clsRegistry.clsRegistry
        Try
            Me.Cursor = Cursors.WaitCursor
            If mf_bCheckData() Then
                If mv_bCallFromLogin Then
                    If Not txtUID.Text.Trim.Equals (gv_sUID) Then
                        MessageBox.Show ("Sai tên đăng nhập. Đề nghị nhập lại", gv_sAnnouncement, MessageBoxButtons.OK, _
                                         MessageBoxIcon.Information)
                        txtUID.Focus()
                        Me.Cursor = Cursors.Default
                        Return
                    End If
                    If Not sv_oEncrypt.Mahoa (txtPWD.Text.Trim).Equals (gv_sPWD) Then
                        MessageBox.Show ("Sai mật khẩu đăng nhập", gv_sAnnouncement, MessageBoxButtons.OK, _
                                         MessageBoxIcon.Information)
                        txtPWD.Focus()
                        Me.Cursor = Cursors.Default
                        Return
                    End If
                    mv_bLoginSuccess = True
                    Me.Close()
                    Return
                Else
                    If gv_sUID.Trim.Equals (txtUID.Text.Trim) And gv_sPWD.Equals (sv_oEncrypt.Mahoa (txtPWD.Text.Trim)) _
                        Then
                        gv_bReturnHome = True
                        gv_bLoginSuccess = False
                        Me.Cursor = Cursors.Default
                        Me.Close()
                        Return
                    End If
                    If bLoginSuccess() Then
                        gv_sUID = txtUID.Text.Trim
                        gv_sPWD = sv_oEncrypt.Mahoa (txtPWD.Text.Trim)
                        _clsRegistry.SaveReg (2, "DVC_COMPANY", "APP_DVC", "APP_UID", txtUID.Text.Trim)
                        _clsRegistry.SaveReg (2, "DVC_COMPANY", "APP_DVC", "APP_PWD", txtPWD.Text.Trim)
                        gv_bLoginSuccess = True
                        gv_bReturnHome = False
                        Me.Cursor = Cursors.Default
                        Me.Close()
                    Else
                        gv_bLoginSuccess = False
                    End If
                End If
            End If
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Function bLoginSuccess() As Boolean
        Dim sv_oUser As New clsUser
        Dim sv_oEncrypt As New Encrypt.Encrypt (gv_sSymmetricAlgorithmName)
        If Not gv_ConnectSuccess Then
            Return False
        End If
        If Not sv_oUser.bIsExisted (txtUID.Text.Trim) Then
            MessageBox.Show ("Không tồn tại người dùng có tên đăng nhập là " & txtUID.Text.Trim & ". Đề nghị nhập lại", _
                             gv_sAnnouncement, MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtUID.Focus()
            Return False
        End If
        If Not sv_oUser.bLoginSuccess (txtUID.Text.Trim, sv_oEncrypt.Mahoa (txtPWD.Text.Trim)) Then
            MessageBox.Show ("Sai mật khẩu đăng nhập", gv_sAnnouncement, MessageBoxButtons.OK, _
                             MessageBoxIcon.Information)
            txtPWD.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub cmdClose_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdClose.Click
        Try
            'gv_bIncreateOrDecrete = False
            'If Timer1.Enabled = False Then
            '    Timer1.Enabled = True
            'End If
            'If Me.Opacity = 0 Then
            '    Me.Close()
            'End If
            mv_bLoginSuccess = False
            gv_bLoginSuccess = False
            gv_bReturnHome = True
            Me.Close()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Label3_Click (ByVal sender As Object, ByVal e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub txtUID_TextChanged (ByVal sender As Object, ByVal e As EventArgs) Handles txtUID.TextChanged

    End Sub

    Private Sub txtPWD_TextChanged (ByVal sender As Object, ByVal e As EventArgs) Handles txtPWD.TextChanged

    End Sub

    Private Sub GroupBox1_Enter (ByVal sender As Object, ByVal e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Label6_Click (ByVal sender As Object, ByVal e As EventArgs) Handles Label6.Click

    End Sub

    Private Sub Label5_Click (ByVal sender As Object, ByVal e As EventArgs) Handles Label5.Click

    End Sub

    Private Sub Label1_Click (ByVal sender As Object, ByVal e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub PictureBox1_Click (ByVal sender As Object, ByVal e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub Timer1_Tick (ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub Label2_Click (ByVal sender As Object, ByVal e As EventArgs) Handles Label2.Click

    End Sub
End Class
