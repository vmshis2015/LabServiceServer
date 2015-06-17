Imports System.Threading
Imports System.ComponentModel
Imports System.Resources
Imports System.IO

Public Class frm_DownLoadVersion
    Inherits Form
    Dim t As Thread

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
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents tbl_Version As DataGridTableStyle
    Friend WithEvents DataGridBoolColumn1 As DataGridBoolColumn
    Friend WithEvents DataGridTextBoxColumn1 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn2 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn3 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn4 As DataGridTextBoxColumn
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents grdList As DataGrid
    Friend WithEvents cmdUpdate As Button
    Friend WithEvents cmdClose As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents chkCheckUpdate As CheckBox
    Friend WithEvents chkSaveOldVersion As CheckBox
    Friend WithEvents cmdDefault As Button

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As ResourceManager = New ResourceManager (GetType (frm_DownLoadVersion))
        Me.cmdUpdate = New Button
        Me.TabControl1 = New TabControl
        Me.TabPage1 = New TabPage
        Me.PictureBox1 = New PictureBox
        Me.Label1 = New Label
        Me.grdList = New DataGrid
        Me.tbl_Version = New DataGridTableStyle
        Me.DataGridBoolColumn1 = New DataGridBoolColumn
        Me.DataGridTextBoxColumn1 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn2 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn3 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn4 = New DataGridTextBoxColumn
        Me.TabPage2 = New TabPage
        Me.cmdDefault = New Button
        Me.chkSaveOldVersion = New CheckBox
        Me.GroupBox2 = New GroupBox
        Me.Label2 = New Label
        Me.chkCheckUpdate = New CheckBox
        Me.GroupBox1 = New GroupBox
        Me.cmdClose = New Button
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType (Me.grdList, ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdUpdate
        '
        Me.cmdUpdate.Anchor = CType ((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
        Me.cmdUpdate.FlatStyle = FlatStyle.Flat
        Me.cmdUpdate.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.cmdUpdate.Image = CType (resources.GetObject ("cmdUpdate.Image"), Image)
        Me.cmdUpdate.ImageAlign = ContentAlignment.MiddleLeft
        Me.cmdUpdate.Location = New Point (291, 332)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.Size = New Size (108, 27)
        Me.cmdUpdate.TabIndex = 2
        Me.cmdUpdate.Text = "Cập nhật"
        Me.cmdUpdate.TextAlign = ContentAlignment.MiddleRight
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add (Me.TabPage1)
        Me.TabControl1.Controls.Add (Me.TabPage2)
        Me.TabControl1.Location = New Point (3, 3)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New Size (516, 312)
        Me.TabControl1.TabIndex = 3
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add (Me.PictureBox1)
        Me.TabPage1.Controls.Add (Me.Label1)
        Me.TabPage1.Controls.Add (Me.grdList)
        Me.TabPage1.Location = New Point (4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New Size (508, 286)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Cập nhật phiên bản"
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = BorderStyle.FixedSingle
        Me.PictureBox1.Dock = DockStyle.Left
        Me.PictureBox1.Image = CType (resources.GetObject ("PictureBox1.Image"), Image)
        Me.PictureBox1.Location = New Point (0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New Size (201, 286)
        Me.PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 10
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.BackColor = Color.LightGreen
        Me.Label1.BorderStyle = BorderStyle.Fixed3D
        Me.Label1.Font = New Font ("Arial", 9.75!, FontStyle.Bold, GraphicsUnit.Point, CType (0, Byte))
        Me.Label1.ForeColor = Color.FromArgb (CType (0, Byte), CType (0, Byte), CType (64, Byte))
        Me.Label1.Location = New Point (204, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size (303, 27)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Danh mục các phiên bản sẽ được cập nhật"
        Me.Label1.TextAlign = ContentAlignment.MiddleCenter
        '
        'grdList
        '
        Me.grdList.BackgroundColor = Color.White
        Me.grdList.CaptionVisible = False
        Me.grdList.DataMember = ""
        Me.grdList.GridLineColor = Color.White
        Me.grdList.HeaderFont = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.grdList.HeaderForeColor = SystemColors.ControlText
        Me.grdList.Location = New Point (204, 27)
        Me.grdList.Name = "grdList"
        Me.grdList.RowHeaderWidth = 0
        Me.grdList.Size = New Size (303, 258)
        Me.grdList.TabIndex = 4
        Me.grdList.TableStyles.AddRange (New DataGridTableStyle() {Me.tbl_Version})
        '
        'tbl_Version
        '
        Me.tbl_Version.DataGrid = Me.grdList
        Me.tbl_Version.GridColumnStyles.AddRange ( _
                                                  New DataGridColumnStyle() _
                                                     {Me.DataGridBoolColumn1, Me.DataGridTextBoxColumn1, _
                                                      Me.DataGridTextBoxColumn2, Me.DataGridTextBoxColumn3, _
                                                      Me.DataGridTextBoxColumn4})
        Me.tbl_Version.GridLineColor = Color.White
        Me.tbl_Version.HeaderFont = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.tbl_Version.HeaderForeColor = SystemColors.ControlText
        Me.tbl_Version.MappingName = ""
        Me.tbl_Version.RowHeadersVisible = False
        Me.tbl_Version.RowHeaderWidth = 0
        Me.tbl_Version.SelectionBackColor = Color.MediumSlateBlue
        '
        'DataGridBoolColumn1
        '
        Me.DataGridBoolColumn1.AllowNull = False
        Me.DataGridBoolColumn1.FalseValue = "F"
        Me.DataGridBoolColumn1.MappingName = "CHON"
        Me.DataGridBoolColumn1.NullText = ""
        Me.DataGridBoolColumn1.NullValue = CType (resources.GetObject ("DataGridBoolColumn1.NullValue"), Object)
        Me.DataGridBoolColumn1.TrueValue = "T"
        Me.DataGridBoolColumn1.Width = 20
        '
        'DataGridTextBoxColumn1
        '
        Me.DataGridTextBoxColumn1.Format = ""
        Me.DataGridTextBoxColumn1.FormatInfo = Nothing
        Me.DataGridTextBoxColumn1.HeaderText = "Tên File"
        Me.DataGridTextBoxColumn1.MappingName = "sFileName"
        Me.DataGridTextBoxColumn1.NullText = ""
        Me.DataGridTextBoxColumn1.Width = 101
        '
        'DataGridTextBoxColumn2
        '
        Me.DataGridTextBoxColumn2.Alignment = HorizontalAlignment.Center
        Me.DataGridTextBoxColumn2.Format = ""
        Me.DataGridTextBoxColumn2.FormatInfo = Nothing
        Me.DataGridTextBoxColumn2.HeaderText = "Trạng thái Download"
        Me.DataGridTextBoxColumn2.MappingName = "sStatus"
        Me.DataGridTextBoxColumn2.Width = 220
        '
        'DataGridTextBoxColumn3
        '
        Me.DataGridTextBoxColumn3.Format = ""
        Me.DataGridTextBoxColumn3.FormatInfo = Nothing
        Me.DataGridTextBoxColumn3.HeaderText = "Dung lượng"
        Me.DataGridTextBoxColumn3.MappingName = "dblCapacity"
        Me.DataGridTextBoxColumn3.Width = 0
        '
        'DataGridTextBoxColumn4
        '
        Me.DataGridTextBoxColumn4.Format = ""
        Me.DataGridTextBoxColumn4.FormatInfo = Nothing
        Me.DataGridTextBoxColumn4.HeaderText = "Số hiệu phiên bản"
        Me.DataGridTextBoxColumn4.MappingName = "sVersion"
        Me.DataGridTextBoxColumn4.NullText = ""
        Me.DataGridTextBoxColumn4.Width = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add (Me.cmdDefault)
        Me.TabPage2.Controls.Add (Me.chkSaveOldVersion)
        Me.TabPage2.Controls.Add (Me.GroupBox2)
        Me.TabPage2.Controls.Add (Me.Label2)
        Me.TabPage2.Controls.Add (Me.chkCheckUpdate)
        Me.TabPage2.Location = New Point (4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New Size (508, 286)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Tùy chọn cập nhật"
        '
        'cmdDefault
        '
        Me.cmdDefault.FlatStyle = FlatStyle.Flat
        Me.cmdDefault.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.cmdDefault.ImageAlign = ContentAlignment.MiddleLeft
        Me.cmdDefault.Location = New Point (57, 138)
        Me.cmdDefault.Name = "cmdDefault"
        Me.cmdDefault.Size = New Size (108, 27)
        Me.cmdDefault.TabIndex = 5
        Me.cmdDefault.Text = "Restore Default"
        Me.cmdDefault.TextAlign = ContentAlignment.MiddleRight
        '
        'chkSaveOldVersion
        '
        Me.chkSaveOldVersion.Checked = True
        Me.chkSaveOldVersion.CheckState = CheckState.Checked
        Me.chkSaveOldVersion.Enabled = False
        Me.chkSaveOldVersion.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.chkSaveOldVersion.Location = New Point (45, 102)
        Me.chkSaveOldVersion.Name = "chkSaveOldVersion"
        Me.chkSaveOldVersion.Size = New Size (351, 24)
        Me.chkSaveOldVersion.TabIndex = 4
        Me.chkSaveOldVersion.Text = "Luôn lưu phiên bản mới nhất ra thư mục OldVersion"
        '
        'GroupBox2
        '
        Me.GroupBox2.Location = New Point (96, 36)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New Size (345, 3)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        '
        'Label2
        '
        Me.Label2.Font = New Font ("Arial", 9.75!, FontStyle.Bold, GraphicsUnit.Point, CType (0, Byte))
        Me.Label2.Location = New Point (45, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New Size (72, 23)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Startup"
        '
        'chkCheckUpdate
        '
        Me.chkCheckUpdate.Checked = True
        Me.chkCheckUpdate.CheckState = CheckState.Checked
        Me.chkCheckUpdate.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.chkCheckUpdate.Location = New Point (45, 66)
        Me.chkCheckUpdate.Name = "chkCheckUpdate"
        Me.chkCheckUpdate.Size = New Size (351, 24)
        Me.chkCheckUpdate.TabIndex = 0
        Me.chkCheckUpdate.Text = "Thông báo cho người dùng mỗi khi có phiên bản mới nhất"
        '
        'GroupBox1
        '
        Me.GroupBox1.Location = New Point (6, 324)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New Size (510, 3)
        Me.GroupBox1.TabIndex = 11
        Me.GroupBox1.TabStop = False
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType ((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
        Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdClose.FlatStyle = FlatStyle.Flat
        Me.cmdClose.Image = CType (resources.GetObject ("cmdClose.Image"), Image)
        Me.cmdClose.ImageAlign = ContentAlignment.MiddleLeft
        Me.cmdClose.Location = New Point (413, 332)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New Size (96, 27)
        Me.cmdClose.TabIndex = 5
        Me.cmdClose.Text = "Th&oát"
        '
        'frm_DownLoadVersion
        '
        Me.AcceptButton = Me.cmdUpdate
        Me.AutoScaleBaseSize = New Size (5, 13)
        Me.ClientSize = New Size (522, 365)
        Me.Controls.Add (Me.cmdClose)
        Me.Controls.Add (Me.GroupBox1)
        Me.Controls.Add (Me.TabControl1)
        Me.Controls.Add (Me.cmdUpdate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType (resources.GetObject ("$this.Icon"), Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frm_DownLoadVersion"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Cập nhật phiên bản mới nhất về máy Client"
        Me.TopMost = True
        Me.TabControl1.ResumeLayout (False)
        Me.TabPage1.ResumeLayout (False)
        CType (Me.grdList, ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout (False)
        Me.ResumeLayout (False)

    End Sub

#End Region

    Private Sub frm_DownLoadVersion_Load (ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        chkCheckUpdate.Checked = gv_bAnnouceWhenHavingLastestVersion
        gv_bIsChecking = True
        LoadData()
    End Sub

    Private Sub LoadData()
        If gv_DSVersionFiles.Tables.Count > 0 Then
            If gv_DSVersionFiles.Tables (0).Rows.Count > 0 Then
                With grdList
                    .TableStyles (0).MappingName = gv_DSVersionFiles.Tables (0).TableName
                    .DataSource = gv_DSVersionFiles.Tables (0).DefaultView
                End With
                gv_DSVersionFiles.Tables (0).DefaultView.AllowDelete = False
                gv_DSVersionFiles.Tables (0).DefaultView.AllowNew = False
            End If
        End If
    End Sub


    Private Sub grdList_CurrentCellChanged (ByVal sender As Object, ByVal e As EventArgs) _
        Handles grdList.CurrentCellChanged
        If grdList.VisibleRowCount > 0 Then
            grdList.CurrentCell = New DataGridCell (grdList.CurrentRowIndex, 0)
            grdList.Select (grdList.CurrentRowIndex)
        End If
    End Sub

    Private Sub cmdUpdate_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
        TabControl1.SelectedIndex = 0
        If cmdUpdate.Text.ToUpper = "CẬP NHẬT" Then
            cmdUpdate.Text = "Hủy cập nhật"
            t = New Thread (AddressOf DownLoadVersion)
            t.Start()
        Else
            If Not t Is Nothing Then
                If t.ThreadState = ThreadState.Running Then
                    t.Suspend()
                    If _
                        MessageBox.Show ("Bạn có thực sự muốn hủy cập nhật không?", gv_sAnnouncement, _
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        t.Resume()
                        t.Abort()
                        cmdUpdate.Visible = False
                    Else
                        t.Resume()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub DownLoadVersion()
        Try
            '-----------------------------------------------------------------------------
            'Kiểm tra sự tồn tại của ứng dụng Winrar. Nếu ko có thì Copy về thư mục cài ứng dụng để chạy
            If Not File.Exists (Application.StartupPath & "\WINRAR\WINRAR.EXE") Then
                Dim sv_oDlg As New OpenFileDialog
                sv_oDlg.Title = "Chọn đến thư mục chứa ứng dụng Winrar"
                sv_oDlg.Filter = "Winrar|Winrar.exe"
                If sv_oDlg.ShowDialog = DialogResult.OK Then
                    If Not Directory.Exists (Application.StartupPath & "\WINRAR") Then
                        Directory.CreateDirectory (Application.StartupPath & "\WINRAR")
                    End If
                    File.Copy (sv_oDlg.FileName, Application.StartupPath & "\WINRAR\WINRAR.EXE", True)
                    MessageBox.Show ("Hãy nhấn lại nút Download để thực hiện cập nhật phiên bản", gv_sAnnouncement, _
                                     MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If
            End If
            '-----------------------------------------------------------------------------
            Dim i As Integer
            For Each dr As DataRow In gv_DSVersionFiles.Tables (0).Rows
                grdList.UnSelect (0)
                If dr ("CHON") = "T" Then
                    grdList.CurrentCell = New DataGridCell (i, 0)
                    grdList.Select (i)
                    dr ("sStatus") = "Đang download file..."
                    SaveOldDLL (dr ("sFileName"))
                    Dim objData() As Byte = CType (dr ("objData"), Byte())
                    Dim ms As New MemoryStream (objData)
                    Dim _
                        fs As _
                            New FileStream ( _
                                            Application.StartupPath & "\" & _
                                            IIf (dr ("intRar") = 0, dr ("sFileName"), dr ("sRarFileName")), _
                                            FileMode.Create, FileAccess.Write)
                    ms.WriteTo (fs)
                    ms.Flush()
                    fs.Close()
                    If dr ("intRar") = 1 Then
                        Dim _
                            pStartupPath As String = Application.StartupPath & "\" & _
                                                     IIf (dr ("intRar") = 0, dr ("sFileName"), dr ("sRarFileName"))
                        Dim info As New ProcessStartInfo
                        info.FileName = Application.StartupPath & "\WinRAR\WinRAR.exe"
                        info.Arguments = "e -pSYSMAN -o+ " & Chr (34) & pStartupPath & Chr (34) & " " & Chr (34) & _
                                         Application.StartupPath & Chr (34)
                        info.WindowStyle = ProcessWindowStyle.Hidden
                        Dim pro As Process = Process.Start (info)
                        pro.WaitForExit()
                        DeleteFile (pStartupPath)
                    End If
                    dr ("sStatus") = "Đã download thành công"
                End If
                If i > 0 Then
                    grdList.UnSelect (i)
                End If
                i += 1
            Next
            cmdUpdate.Visible = False
            cmdClose.Text = "Kết thúc"
            cmdClose.Focus()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub SaveOldDLL (ByVal pv_sFileName As String)
        Try
            If Not Directory.Exists (Application.StartupPath & "\OldVersion") Then
                Directory.CreateDirectory (Application.StartupPath & "\OldVersion")
            End If
            If File.Exists (Application.StartupPath & "\" & pv_sFileName) Then
                File.Copy (Application.StartupPath & "\" & pv_sFileName, _
                           Application.StartupPath & "\OldVersion\" & pv_sFileName, True)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DeleteFile (ByVal pv_sFilePath As String)
        Try
            If File.Exists (pv_sFilePath) Then
                File.Delete (pv_sFilePath)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DownLoadFile (ByVal pv_arrData() As Byte)
        'Kiểm tra sự tồn tại của ứng dụng Winrar. Nếu ko có thì Copy về thư mục cài ứng dụng để chạy
        If Not File.Exists (Application.StartupPath & "\WINRAR\WINRAR.EXE") Then
            Dim sv_oDlg As New OpenFileDialog
            sv_oDlg.Title = "Chọn đến thư mục chứa ứng dụng Winrar"
            sv_oDlg.Filter = "Winrar|Winrar.exe"
            If sv_oDlg.ShowDialog = DialogResult.OK Then
                If Not Directory.Exists (Application.StartupPath & "\WINRAR") Then
                    Directory.CreateDirectory (Application.StartupPath & "\WINRAR")
                End If
                File.Copy (sv_oDlg.FileName, Application.StartupPath & "\WINRAR\WINRAR.EXE", True)
                MessageBox.Show ("Hãy nhấn lại nút Download để thực hiện cập nhật lại phiên bản", gv_sAnnouncement, _
                                 MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
        End If
        '---------------------------------------------------------------------------------

    End Sub

    Private Sub cmdClose_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdClose.Click
        Dim clsReg As New clsRegistry.clsRegistry
        gv_bAnnouceWhenHavingLastestVersion = chkCheckUpdate.Checked
        clsReg.SaveReg (2, "DVC_COMPANY", "SYSMAN_APP", "AnnouceWhenHavingLastestVersion", _
                        IIf (gv_bAnnouceWhenHavingLastestVersion = True, "1", "0"))
        gv_bIsChecking = False
        Try
            If t.ThreadState = ThreadState.Running Then
                t.Abort()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frm_DownLoadVersion_KeyDown (ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then Me.Close()
    End Sub

    Private Sub cmdDefault_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdDefault.Click
        chkCheckUpdate.Checked = True
    End Sub
End Class
