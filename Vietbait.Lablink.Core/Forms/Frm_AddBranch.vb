Imports System.ComponentModel
Imports System.Resources
Imports System.Data.SqlClient

Public Class Frm_AddBranch
    Inherits Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

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
    Friend WithEvents cmdSave As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtBankCode As TextBox
    Friend WithEvents txtPosition As TextBox
    Friend WithEvents txtProxyNumber As TextBox
    Friend WithEvents txtRepresentative As TextBox
    Friend WithEvents txtAccountCode As TextBox
    Friend WithEvents txtTaxCode As TextBox
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents txtFax As TextBox
    Friend WithEvents txtPhone As TextBox
    Friend WithEvents txtAddress As TextBox
    Friend WithEvents txtUnitName As TextBox
    Friend WithEvents txtUnitCode As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtHotPhone As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents txtSupportPhone As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents cmdClose As Button

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As ResourceManager = New ResourceManager (GetType (Frm_AddBranch))
        Me.cmdSave = New Button
        Me.GroupBox1 = New GroupBox
        Me.txtSupportPhone = New TextBox
        Me.Label15 = New Label
        Me.txtHotPhone = New TextBox
        Me.Label14 = New Label
        Me.txtBankCode = New TextBox
        Me.txtPosition = New TextBox
        Me.txtProxyNumber = New TextBox
        Me.txtRepresentative = New TextBox
        Me.txtAccountCode = New TextBox
        Me.txtTaxCode = New TextBox
        Me.txtEmail = New TextBox
        Me.txtFax = New TextBox
        Me.txtPhone = New TextBox
        Me.txtAddress = New TextBox
        Me.txtUnitName = New TextBox
        Me.txtUnitCode = New TextBox
        Me.Label12 = New Label
        Me.Label11 = New Label
        Me.Label10 = New Label
        Me.Label9 = New Label
        Me.Label8 = New Label
        Me.Label7 = New Label
        Me.Label6 = New Label
        Me.Label5 = New Label
        Me.Label4 = New Label
        Me.Label3 = New Label
        Me.Label2 = New Label
        Me.Label1 = New Label
        Me.cmdClose = New Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSave
        '
        Me.cmdSave.FlatStyle = FlatStyle.Flat
        Me.cmdSave.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.cmdSave.Image = CType (resources.GetObject ("cmdSave.Image"), Image)
        Me.cmdSave.ImageAlign = ContentAlignment.MiddleLeft
        Me.cmdSave.Location = New Point (273, 282)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New Size (81, 24)
        Me.cmdSave.TabIndex = 14
        Me.cmdSave.Text = "Ghi"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add (Me.txtSupportPhone)
        Me.GroupBox1.Controls.Add (Me.Label15)
        Me.GroupBox1.Controls.Add (Me.txtHotPhone)
        Me.GroupBox1.Controls.Add (Me.Label14)
        Me.GroupBox1.Controls.Add (Me.txtBankCode)
        Me.GroupBox1.Controls.Add (Me.txtPosition)
        Me.GroupBox1.Controls.Add (Me.txtProxyNumber)
        Me.GroupBox1.Controls.Add (Me.txtRepresentative)
        Me.GroupBox1.Controls.Add (Me.txtAccountCode)
        Me.GroupBox1.Controls.Add (Me.txtTaxCode)
        Me.GroupBox1.Controls.Add (Me.txtEmail)
        Me.GroupBox1.Controls.Add (Me.txtFax)
        Me.GroupBox1.Controls.Add (Me.txtPhone)
        Me.GroupBox1.Controls.Add (Me.txtAddress)
        Me.GroupBox1.Controls.Add (Me.txtUnitName)
        Me.GroupBox1.Controls.Add (Me.txtUnitCode)
        Me.GroupBox1.Controls.Add (Me.Label12)
        Me.GroupBox1.Controls.Add (Me.Label11)
        Me.GroupBox1.Controls.Add (Me.Label10)
        Me.GroupBox1.Controls.Add (Me.Label9)
        Me.GroupBox1.Controls.Add (Me.Label8)
        Me.GroupBox1.Controls.Add (Me.Label7)
        Me.GroupBox1.Controls.Add (Me.Label6)
        Me.GroupBox1.Controls.Add (Me.Label5)
        Me.GroupBox1.Controls.Add (Me.Label4)
        Me.GroupBox1.Controls.Add (Me.Label3)
        Me.GroupBox1.Controls.Add (Me.Label2)
        Me.GroupBox1.Controls.Add (Me.Label1)
        Me.GroupBox1.Location = New Point (3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New Size (447, 270)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Thông tin đơn vị"
        '
        'txtSupportPhone
        '
        Me.txtSupportPhone.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtSupportPhone.Location = New Point (335, 93)
        Me.txtSupportPhone.MaxLength = 20
        Me.txtSupportPhone.Name = "txtSupportPhone"
        Me.txtSupportPhone.TabIndex = 4
        Me.txtSupportPhone.Text = ""
        '
        'Label15
        '
        Me.Label15.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label15.Location = New Point (231, 96)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New Size (99, 18)
        Me.Label15.TabIndex = 98
        Me.Label15.Text = "Điện thoại hỗ trợ"
        '
        'txtHotPhone
        '
        Me.txtHotPhone.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtHotPhone.Location = New Point (126, 117)
        Me.txtHotPhone.MaxLength = 20
        Me.txtHotPhone.Name = "txtHotPhone"
        Me.txtHotPhone.TabIndex = 5
        Me.txtHotPhone.Text = ""
        '
        'Label14
        '
        Me.Label14.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label14.Location = New Point (12, 120)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New Size (108, 18)
        Me.Label14.TabIndex = 96
        Me.Label14.Text = "Điện thoại nóng"
        '
        'txtBankCode
        '
        Me.txtBankCode.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtBankCode.Location = New Point (335, 237)
        Me.txtBankCode.MaxLength = 7
        Me.txtBankCode.Name = "txtBankCode"
        Me.txtBankCode.TabIndex = 13
        Me.txtBankCode.Text = ""
        '
        'txtPosition
        '
        Me.txtPosition.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtPosition.Location = New Point (126, 213)
        Me.txtPosition.MaxLength = 50
        Me.txtPosition.Name = "txtPosition"
        Me.txtPosition.Size = New Size (309, 21)
        Me.txtPosition.TabIndex = 11
        Me.txtPosition.Text = ""
        '
        'txtProxyNumber
        '
        Me.txtProxyNumber.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtProxyNumber.Location = New Point (126, 237)
        Me.txtProxyNumber.MaxLength = 20
        Me.txtProxyNumber.Name = "txtProxyNumber"
        Me.txtProxyNumber.TabIndex = 12
        Me.txtProxyNumber.Text = ""
        '
        'txtRepresentative
        '
        Me.txtRepresentative.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtRepresentative.Location = New Point (126, 189)
        Me.txtRepresentative.MaxLength = 50
        Me.txtRepresentative.Name = "txtRepresentative"
        Me.txtRepresentative.Size = New Size (309, 21)
        Me.txtRepresentative.TabIndex = 10
        Me.txtRepresentative.Text = ""
        '
        'txtAccountCode
        '
        Me.txtAccountCode.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtAccountCode.Location = New Point (335, 165)
        Me.txtAccountCode.MaxLength = 15
        Me.txtAccountCode.Name = "txtAccountCode"
        Me.txtAccountCode.TabIndex = 9
        Me.txtAccountCode.Text = ""
        '
        'txtTaxCode
        '
        Me.txtTaxCode.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtTaxCode.Location = New Point (126, 165)
        Me.txtTaxCode.MaxLength = 17
        Me.txtTaxCode.Name = "txtTaxCode"
        Me.txtTaxCode.TabIndex = 8
        Me.txtTaxCode.Text = ""
        '
        'txtEmail
        '
        Me.txtEmail.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtEmail.Location = New Point (126, 141)
        Me.txtEmail.MaxLength = 30
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New Size (309, 21)
        Me.txtEmail.TabIndex = 7
        Me.txtEmail.Text = ""
        '
        'txtFax
        '
        Me.txtFax.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtFax.Location = New Point (335, 117)
        Me.txtFax.MaxLength = 13
        Me.txtFax.Name = "txtFax"
        Me.txtFax.TabIndex = 6
        Me.txtFax.Text = ""
        '
        'txtPhone
        '
        Me.txtPhone.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtPhone.Location = New Point (126, 93)
        Me.txtPhone.MaxLength = 20
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.TabIndex = 3
        Me.txtPhone.Text = ""
        '
        'txtAddress
        '
        Me.txtAddress.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtAddress.Location = New Point (126, 69)
        Me.txtAddress.MaxLength = 100
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.Size = New Size (309, 21)
        Me.txtAddress.TabIndex = 2
        Me.txtAddress.Text = ""
        '
        'txtUnitName
        '
        Me.txtUnitName.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.txtUnitName.Location = New Point (126, 45)
        Me.txtUnitName.MaxLength = 100
        Me.txtUnitName.Name = "txtUnitName"
        Me.txtUnitName.Size = New Size (309, 21)
        Me.txtUnitName.TabIndex = 1
        Me.txtUnitName.Text = ""
        '
        'txtUnitCode
        '
        Me.txtUnitCode.CharacterCasing = CharacterCasing.Upper
        Me.txtUnitCode.Font = New Font ("Arial", 9.0!, FontStyle.Bold, GraphicsUnit.Point, CType (0, Byte))
        Me.txtUnitCode.Location = New Point (126, 20)
        Me.txtUnitCode.MaxLength = 10
        Me.txtUnitCode.Name = "txtUnitCode"
        Me.txtUnitCode.TabIndex = 0
        Me.txtUnitCode.Text = ""
        '
        'Label12
        '
        Me.Label12.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label12.Location = New Point (231, 240)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New Size (102, 18)
        Me.Label12.TabIndex = 92
        Me.Label12.Text = "Mã số ngân hàng"
        '
        'Label11
        '
        Me.Label11.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label11.Location = New Point (12, 216)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New Size (111, 18)
        Me.Label11.TabIndex = 91
        Me.Label11.Text = "Chức danh"
        '
        'Label10
        '
        Me.Label10.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label10.Location = New Point (12, 240)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New Size (111, 18)
        Me.Label10.TabIndex = 90
        Me.Label10.Text = "Số giấy ủy quyền"
        '
        'Label9
        '
        Me.Label9.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label9.Location = New Point (12, 192)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New Size (111, 18)
        Me.Label9.TabIndex = 89
        Me.Label9.Text = "Tên người đại diện"
        '
        'Label8
        '
        Me.Label8.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label8.Location = New Point (231, 168)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New Size (99, 18)
        Me.Label8.TabIndex = 88
        Me.Label8.Text = "Mã số tài khoản"
        '
        'Label7
        '
        Me.Label7.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label7.Location = New Point (12, 168)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New Size (111, 18)
        Me.Label7.TabIndex = 87
        Me.Label7.Text = "Mã số thuế"
        '
        'Label6
        '
        Me.Label6.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label6.Location = New Point (12, 144)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New Size (111, 18)
        Me.Label6.TabIndex = 86
        Me.Label6.Text = "Email"
        '
        'Label5
        '
        Me.Label5.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label5.Location = New Point (231, 120)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New Size (81, 18)
        Me.Label5.TabIndex = 85
        Me.Label5.Text = "Số Fax"
        '
        'Label4
        '
        Me.Label4.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label4.Location = New Point (12, 96)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New Size (111, 18)
        Me.Label4.TabIndex = 84
        Me.Label4.Text = "Điện thoại đơn vị"
        '
        'Label3
        '
        Me.Label3.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label3.Location = New Point (12, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New Size (111, 18)
        Me.Label3.TabIndex = 83
        Me.Label3.Text = "Địa chỉ"
        '
        'Label2
        '
        Me.Label2.Font = New Font ("Arial", 9.0!, FontStyle.Bold, GraphicsUnit.Point, CType (0, Byte))
        Me.Label2.ForeColor = Color.Maroon
        Me.Label2.Location = New Point (12, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New Size (111, 18)
        Me.Label2.TabIndex = 82
        Me.Label2.Text = "Tên chi nhánh"
        '
        'Label1
        '
        Me.Label1.Font = New Font ("Arial", 9.0!, FontStyle.Bold, GraphicsUnit.Point, CType (0, Byte))
        Me.Label1.ForeColor = Color.Maroon
        Me.Label1.Location = New Point (12, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size (111, 18)
        Me.Label1.TabIndex = 81
        Me.Label1.Text = "Mã chi nhánh"
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType ((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.FlatStyle = FlatStyle.Flat
        Me.cmdClose.Image = CType (resources.GetObject ("cmdClose.Image"), Image)
        Me.cmdClose.ImageAlign = ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New Point (363, 282)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New Size (81, 25)
        Me.cmdClose.TabIndex = 15
        Me.cmdClose.Text = "Th&oát"
        '
        'Frm_AddBranch
        '
        Me.AutoScaleBaseSize = New Size (5, 13)
        Me.ClientSize = New Size (456, 314)
        Me.Controls.Add (Me.cmdClose)
        Me.Controls.Add (Me.GroupBox1)
        Me.Controls.Add (Me.cmdSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType (resources.GetObject ("$this.Icon"), Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Frm_AddBranch"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Đơn vị làm việc"
        Me.GroupBox1.ResumeLayout (False)
        Me.ResumeLayout (False)

    End Sub

#End Region

    Private Sub cmdSave_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        If txtUnitCode.Text.Trim.Equals (String.Empty) Then
            MessageBox.Show ("Bạn phải nhập mã chi nhánh", gv_sAnnouncement, MessageBoxButtons.OK, _
                             MessageBoxIcon.Information)
            txtUnitCode.Focus()
            Return
        End If
        If txtUnitName.Text.Trim.Equals (String.Empty) Then
            MessageBox.Show ("Bạn phải nhập tên chi nhánh", gv_sAnnouncement, MessageBoxButtons.OK, _
                             MessageBoxIcon.Information)
            txtUnitName.Focus()
            Return
        End If
        Dim sv_ssql As String
        Dim sv_oCmd As New SqlCommand
        sv_ssql = _
            "INSERT INTO TBL_MANAGEMENTUNIT(PK_sBranchID,sName,sAddress,sPhone,sFAX,sEMAIL,sTaxCode,sAccountID,sRepresentative,sPosition,sProxyNumber,sBankCode) "
        sv_ssql &= " VALUES(N'" & txtUnitCode.Text.Trim & "',N'" & txtUnitName.Text.Trim & "',"
        sv_ssql &= "N'" & txtAddress.Text.Trim & "',N'" & txtPhone.Text.Trim & "',"
        sv_ssql &= "N'" & txtFax.Text.Trim & "',N'" & txtEmail.Text.Trim & "',"
        sv_ssql &= "N'" & txtTaxCode.Text.Trim & "',N'" & txtAccountCode.Text.Trim & "',"
        sv_ssql &= "N'" & txtRepresentative.Text.Trim & "',N'" & txtPosition.Text.Trim & "',"
        sv_ssql &= "N'" & txtProxyNumber.Text.Trim & "',N'" & txtBankCode.Text.Trim & "')"
        Try
            With sv_oCmd
                .Connection = gv_oSqlCnn
                .CommandType = CommandType.Text
                .CommandText = sv_ssql
                .ExecuteNonQuery()
            End With
            gv_sBranchID = txtUnitCode.Text.Trim
            gv_sBranchName = txtUnitName.Text.Trim
            MessageBox.Show ( _
                             "Đã thêm đơn vị làm việc thành công. Bạn có thể phải Config lại file cấu hình cho lần chạy kế tiếp!", _
                             gv_sAnnouncement, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Frm_AddBranch_Load (ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Frm_AddBranch_KeyDown (ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then Me.ProcessTabKey (True)
    End Sub

    Private Sub cmdClose_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub
End Class
