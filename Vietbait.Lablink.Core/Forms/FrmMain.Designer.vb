<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
    Inherits DevComponents.DotNetBar.Office2007Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.tbr = New System.Windows.Forms.ToolBar
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton2 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton4 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton3 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton5 = New System.Windows.Forms.ToolBarButton
        Me.imgAdminnistration = New System.Windows.Forms.ImageList(Me.components)
        Me.StatusBarPanel1 = New System.Windows.Forms.StatusBarPanel
        Me.StatusBarPanel2 = New System.Windows.Forms.StatusBarPanel
        Me.StatusBarPanel3 = New System.Windows.Forms.StatusBarPanel
        Me.StatusBarPanel4 = New System.Windows.Forms.StatusBarPanel
        Me.StatusBar1 = New System.Windows.Forms.StatusBar
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        CType(Me.StatusBarPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanel2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanel3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.StatusBarPanel4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbr
        '
        Me.tbr.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.ToolBarButton1, Me.ToolBarButton2, Me.ToolBarButton4, Me.ToolBarButton3, Me.ToolBarButton5})
        Me.tbr.DropDownArrows = True
        Me.tbr.ImageList = Me.imgAdminnistration
        Me.tbr.Location = New System.Drawing.Point(0, 0)
        Me.tbr.Name = "tbr"
        Me.tbr.ShowToolTips = True
        Me.tbr.Size = New System.Drawing.Size(792, 28)
        Me.tbr.TabIndex = 7
        Me.tbr.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.ImageIndex = 0
        Me.ToolBarButton1.Name = "ToolBarButton1"
        Me.ToolBarButton1.Tag = "-1"
        Me.ToolBarButton1.ToolTipText = "Đăng nhập"
        '
        'ToolBarButton2
        '
        Me.ToolBarButton2.ImageIndex = 5
        Me.ToolBarButton2.Name = "ToolBarButton2"
        Me.ToolBarButton2.Tag = "-2"
        Me.ToolBarButton2.ToolTipText = "Thay đổi ngôn ngữ hiển thị"
        '
        'ToolBarButton4
        '
        Me.ToolBarButton4.ImageIndex = 13
        Me.ToolBarButton4.Name = "ToolBarButton4"
        Me.ToolBarButton4.Tag = "-3"
        Me.ToolBarButton4.ToolTipText = "Thoát"
        '
        'ToolBarButton3
        '
        Me.ToolBarButton3.ImageIndex = 8
        Me.ToolBarButton3.Name = "ToolBarButton3"
        Me.ToolBarButton3.Tag = "-4"
        Me.ToolBarButton3.ToolTipText = "Thiết lập lại tham số hệ thống"
        '
        'ToolBarButton5
        '
        Me.ToolBarButton5.ImageIndex = 7
        Me.ToolBarButton5.Name = "ToolBarButton5"
        Me.ToolBarButton5.Tag = "-5"
        '
        'imgAdminnistration
        '
        Me.imgAdminnistration.ImageStream = CType(resources.GetObject("imgAdminnistration.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgAdminnistration.TransparentColor = System.Drawing.Color.Transparent
        Me.imgAdminnistration.Images.SetKeyName(0, "")
        Me.imgAdminnistration.Images.SetKeyName(1, "")
        Me.imgAdminnistration.Images.SetKeyName(2, "")
        Me.imgAdminnistration.Images.SetKeyName(3, "")
        Me.imgAdminnistration.Images.SetKeyName(4, "")
        Me.imgAdminnistration.Images.SetKeyName(5, "")
        Me.imgAdminnistration.Images.SetKeyName(6, "")
        Me.imgAdminnistration.Images.SetKeyName(7, "")
        Me.imgAdminnistration.Images.SetKeyName(8, "")
        Me.imgAdminnistration.Images.SetKeyName(9, "")
        Me.imgAdminnistration.Images.SetKeyName(10, "")
        Me.imgAdminnistration.Images.SetKeyName(11, "")
        Me.imgAdminnistration.Images.SetKeyName(12, "")
        Me.imgAdminnistration.Images.SetKeyName(13, "")
        '
        'StatusBarPanel1
        '
        Me.StatusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.StatusBarPanel1.Icon = CType(resources.GetObject("StatusBarPanel1.Icon"), System.Drawing.Icon)
        Me.StatusBarPanel1.Name = "StatusBarPanel1"
        Me.StatusBarPanel1.Text = "Chi nhánh"
        Me.StatusBarPanel1.Width = 87
        '
        'StatusBarPanel2
        '
        Me.StatusBarPanel2.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.StatusBarPanel2.Icon = CType(resources.GetObject("StatusBarPanel2.Icon"), System.Drawing.Icon)
        Me.StatusBarPanel2.Name = "StatusBarPanel2"
        Me.StatusBarPanel2.Text = "Người dùng"
        Me.StatusBarPanel2.Width = 578
        '
        'StatusBarPanel3
        '
        Me.StatusBarPanel3.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents
        Me.StatusBarPanel3.Name = "StatusBarPanel3"
        Me.StatusBarPanel3.Width = 10
        '
        'StatusBarPanel4
        '
        Me.StatusBarPanel4.Alignment = System.Windows.Forms.HorizontalAlignment.Center
        Me.StatusBarPanel4.Icon = CType(resources.GetObject("StatusBarPanel4.Icon"), System.Drawing.Icon)
        Me.StatusBarPanel4.Name = "StatusBarPanel4"
        Me.StatusBarPanel4.Text = "Phiên bản mới"
        Me.StatusBarPanel4.ToolTipText = "Click vào đây để quét phiên bản mới nhất"
        '
        'StatusBar1
        '
        Me.StatusBar1.Location = New System.Drawing.Point(0, 556)
        Me.StatusBar1.Name = "StatusBar1"
        Me.StatusBar1.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.StatusBarPanel1, Me.StatusBarPanel2, Me.StatusBarPanel3, Me.StatusBarPanel4})
        Me.StatusBar1.ShowPanels = True
        Me.StatusBar1.Size = New System.Drawing.Size(792, 22)
        Me.StatusBar1.TabIndex = 6
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Image = Global.LabLink.My.Resources.Resources.LablinkWallpaper
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(792, 578)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 8
        Me.PictureBox1.TabStop = False
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(792, 578)
        Me.Controls.Add(Me.tbr)
        Me.Controls.Add(Me.StatusBar1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmMain"
        Me.Text = "HỆ THÔNG THÔNG TIN KHOA XÉT NGHIỆM - LABLink"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.StatusBarPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanel2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanel3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.StatusBarPanel4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbr As System.Windows.Forms.ToolBar
    Friend WithEvents ToolBarButton1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton4 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton3 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton5 As System.Windows.Forms.ToolBarButton
    Friend WithEvents imgAdminnistration As System.Windows.Forms.ImageList
    Friend WithEvents StatusBarPanel1 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatusBarPanel2 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatusBarPanel3 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatusBarPanel4 As System.Windows.Forms.StatusBarPanel
    Friend WithEvents StatusBar1 As System.Windows.Forms.StatusBar
    Public WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
