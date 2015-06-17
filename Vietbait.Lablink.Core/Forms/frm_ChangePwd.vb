Imports System.ComponentModel
Imports System.Resources

Public Class frm_ChangePwd
    Inherits Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        txtOldPwd.Focus()
        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents cmdClose As Button
    Friend WithEvents cmdLogin As Button
    Friend WithEvents txtConfirm As TextBox
    Friend WithEvents txtNewPwd As TextBox
    Friend WithEvents txtOldPwd As TextBox

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New Container
        Dim resources As ResourceManager = New ResourceManager (GetType (frm_ChangePwd))
        Me.GroupBox1 = New GroupBox
        Me.txtOldPwd = New TextBox
        Me.txtConfirm = New TextBox
        Me.Label4 = New Label
        Me.txtNewPwd = New TextBox
        Me.Label3 = New Label
        Me.Label2 = New Label
        Me.Label6 = New Label
        Me.Label5 = New Label
        Me.PictureBox1 = New PictureBox
        Me.Timer1 = New Timer (Me.components)
        Me.Label1 = New Label
        Me.cmdClose = New Button
        Me.cmdLogin = New Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add (Me.txtOldPwd)
        Me.GroupBox1.Controls.Add (Me.txtConfirm)
        Me.GroupBox1.Controls.Add (Me.Label4)
        Me.GroupBox1.Controls.Add (Me.txtNewPwd)
        Me.GroupBox1.Controls.Add (Me.Label3)
        Me.GroupBox1.Controls.Add (Me.Label2)
        Me.GroupBox1.Location = New Point (3, 57)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New Size (339, 114)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'txtOldPwd
        '
        Me.txtOldPwd.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtOldPwd.Location = New Point (120, 28)
        Me.txtOldPwd.MaxLength = 25
        Me.txtOldPwd.Name = "txtOldPwd"
        Me.txtOldPwd.PasswordChar = ChrW (42)
        Me.txtOldPwd.Size = New Size (184, 22)
        Me.txtOldPwd.TabIndex = 0
        Me.txtOldPwd.Text = ""
        '
        'txtConfirm
        '
        Me.txtConfirm.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtConfirm.Location = New Point (120, 81)
        Me.txtConfirm.MaxLength = 25
        Me.txtConfirm.Name = "txtConfirm"
        Me.txtConfirm.PasswordChar = ChrW (42)
        Me.txtConfirm.Size = New Size (184, 22)
        Me.txtConfirm.TabIndex = 2
        Me.txtConfirm.Text = ""
        '
        'Label4
        '
        Me.Label4.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label4.Location = New Point (9, 78)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New Size (104, 20)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "&Xác nhận lại"
        Me.Label4.TextAlign = ContentAlignment.MiddleRight
        '
        'txtNewPwd
        '
        Me.txtNewPwd.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtNewPwd.Location = New Point (120, 54)
        Me.txtNewPwd.MaxLength = 25
        Me.txtNewPwd.Name = "txtNewPwd"
        Me.txtNewPwd.PasswordChar = ChrW (42)
        Me.txtNewPwd.Size = New Size (184, 22)
        Me.txtNewPwd.TabIndex = 1
        Me.txtNewPwd.Text = ""
        '
        'Label3
        '
        Me.Label3.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label3.Location = New Point (8, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New Size (104, 20)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "&Mật khẩu mới"
        Me.Label3.TextAlign = ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label2.Location = New Point (8, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New Size (104, 20)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Mật khẩu &cũ"
        Me.Label2.TextAlign = ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.BackColor = SystemColors.ControlLightLight
        Me.Label6.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label6.Location = New Point (21, 30)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New Size (272, 16)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Thay đổi mật khẩu người dùng"
        '
        'Label5
        '
        Me.Label5.BackColor = SystemColors.ControlLightLight
        Me.Label5.Font = New Font ("Arial", 9.75!, FontStyle.Bold, GraphicsUnit.Point, CType (0, Byte))
        Me.Label5.Location = New Point (18, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New Size (184, 16)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Thay đổi mật khẩu"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = AnchorStyles.None
        Me.PictureBox1.BackColor = SystemColors.ControlLightLight
        Me.PictureBox1.Image = CType (resources.GetObject ("PictureBox1.Image"), Image)
        Me.PictureBox1.Location = New Point (291, - 9)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New Size (48, 66)
        Me.PictureBox1.TabIndex = 13
        Me.PictureBox1.TabStop = False
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        '
        'Label1
        '
        Me.Label1.BackColor = SystemColors.Window
        Me.Label1.ForeColor = Color.Black
        Me.Label1.Location = New Point (3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size (342, 57)
        Me.Label1.TabIndex = 14
        '
        'cmdClose
        '
        Me.cmdClose.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.cmdClose.Location = New Point (180, 186)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New Size (92, 28)
        Me.cmdClose.TabIndex = 5
        Me.cmdClose.Text = "&Kết thúc"
        '
        'cmdLogin
        '
        Me.cmdLogin.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.cmdLogin.Location = New Point (78, 186)
        Me.cmdLogin.Name = "cmdLogin"
        Me.cmdLogin.Size = New Size (92, 28)
        Me.cmdLogin.TabIndex = 4
        Me.cmdLogin.Text = "&Thay đổi"
        '
        'frm_ChangePwd
        '
        Me.AutoScaleBaseSize = New Size (5, 13)
        Me.ClientSize = New Size (343, 221)
        Me.Controls.Add (Me.GroupBox1)
        Me.Controls.Add (Me.cmdClose)
        Me.Controls.Add (Me.cmdLogin)
        Me.Controls.Add (Me.Label6)
        Me.Controls.Add (Me.Label5)
        Me.Controls.Add (Me.PictureBox1)
        Me.Controls.Add (Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType (resources.GetObject ("$this.Icon"), Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frm_ChangePwd"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Thay đổi mật khẩu người dùng"
        Me.GroupBox1.ResumeLayout (False)
        Me.ResumeLayout (False)

    End Sub

#End Region

    Private Sub cmdLogin_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdLogin.Click
        Dim sv_sPWD As String = String.Empty
        Dim sv_oEncrypt As New Encrypt.Encrypt (gv_sSymmetricAlgorithmName)
        Dim sv_oUser As New clsUser
        Try

            sv_sPWD = sv_oEncrypt.Mahoa (txtOldPwd.Text.Trim)
            If Not txtNewPwd.Text.Trim.Equals (txtConfirm.Text.Trim) Then
                MessageBox.Show ("Mật khẩu xác nhận phải giống mật khẩu mới!", gv_sAnnouncement, MessageBoxButtons.OK, _
                                 MessageBoxIcon.Information)
                txtConfirm.Focus()
                Return
            End If
            'Kiểm tra xem đã nhập mật khẩu cũ đúng hay chưa?
            If gv_bAdminLogin Then
                If Not sv_oUser.bLoginSuccessAdmin (gv_sUID, sv_sPWD) Then
                    MessageBox.Show ("Sai mật khẩu đăng nhập", gv_sAnnouncement, MessageBoxButtons.OK, _
                                     MessageBoxIcon.Information)
                    txtOldPwd.Focus()
                    Return
                End If
            Else
                If Not sv_oUser.bLoginSuccess (gv_sUID, sv_sPWD) Then
                    MessageBox.Show ("Sai mật khẩu đăng nhập", gv_sAnnouncement, MessageBoxButtons.OK, _
                                     MessageBoxIcon.Information)
                    txtOldPwd.Focus()
                    Return
                End If
            End If

            'Kiểm tra xem mật khẩu cũ và mật khẩu mới có giống nhau không
            If txtNewPwd.Text.Trim.Equals (txtOldPwd.Text.Trim) Then
                MessageBox.Show ("Đã thay đổi mật khẩu thành công!", gv_sAnnouncement, MessageBoxButtons.OK, _
                                 MessageBoxIcon.Information)
                Me.Close()
                Return
            End If
            If gv_bAdminLogin Then
                If sv_oUser.bChangePasswordForAdmin (gv_sUID, sv_oEncrypt.Mahoa (txtNewPwd.Text.Trim)) Then
                    MessageBox.Show ("Đã thay đổi mật khẩu thành công!", gv_sAnnouncement, MessageBoxButtons.OK, _
                                     MessageBoxIcon.Information)
                    Me.Close()
                    Return
                End If
            Else
                If sv_oUser.bChangePassword (gv_sUID, sv_oEncrypt.Mahoa (txtNewPwd.Text.Trim)) Then
                    MessageBox.Show ("Đã thay đổi mật khẩu thành công!", gv_sAnnouncement, MessageBoxButtons.OK, _
                                     MessageBoxIcon.Information)
                    Me.Close()
                    Return
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdClose_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub frm_ChangePwd_Load (ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        txtOldPwd.Focus()
    End Sub

    Private Sub frm_ChangePwd_KeyDown (ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        Try
            Select Case e.KeyCode
                Case Keys.Enter
                    ProcessTabKey (True)
                Case Keys.Escape
                    Me.Close()
            End Select
        Catch ex As Exception

        End Try
    End Sub
End Class
