Imports System.ComponentModel
Imports System.Resources

Public Class FRM_SetParamSystem
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
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents cmdRefresh As Button
    Friend WithEvents cmdSetagain As Button
    Friend WithEvents cmdClose As Button
    Friend WithEvents cmdHelp As Button
    Friend WithEvents grdList2 As DataGrid
    Friend WithEvents dtbParam2 As DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumn5 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn6 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn7 As DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumn8 As DataGridTextBoxColumn

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As ResourceManager = New ResourceManager (GetType (FRM_SetParamSystem))
        Me.cmdRefresh = New Button
        Me.cmdSetagain = New Button
        Me.TabControl1 = New TabControl
        Me.TabPage2 = New TabPage
        Me.grdList2 = New DataGrid
        Me.dtbParam2 = New DataGridTableStyle
        Me.DataGridTextBoxColumn5 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn6 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn7 = New DataGridTextBoxColumn
        Me.DataGridTextBoxColumn8 = New DataGridTextBoxColumn
        Me.GroupBox1 = New GroupBox
        Me.cmdClose = New Button
        Me.cmdHelp = New Button
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType (Me.grdList2, ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.cmdRefresh.Location = New Point (168, 378)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New Size (81, 27)
        Me.cmdRefresh.TabIndex = 2
        Me.cmdRefresh.Text = "Refresh"
        '
        'cmdSetagain
        '
        Me.cmdSetagain.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.cmdSetagain.Location = New Point (252, 378)
        Me.cmdSetagain.Name = "cmdSetagain"
        Me.cmdSetagain.Size = New Size (81, 27)
        Me.cmdSetagain.TabIndex = 3
        Me.cmdSetagain.Text = "Thiết lập lại"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add (Me.TabPage2)
        Me.TabControl1.Dock = DockStyle.Top
        Me.TabControl1.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.TabControl1.Location = New Point (0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New Size (547, 363)
        Me.TabControl1.TabIndex = 5
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add (Me.grdList2)
        Me.TabPage2.Location = New Point (4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New Size (539, 335)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Thông tin tham số hệ thống mới nhất từ CSDL"
        '
        'grdList2
        '
        Me.grdList2.BackgroundColor = Color.White
        Me.grdList2.CaptionVisible = False
        Me.grdList2.DataMember = ""
        Me.grdList2.Dock = DockStyle.Fill
        Me.grdList2.HeaderForeColor = SystemColors.ControlText
        Me.grdList2.Location = New Point (0, 0)
        Me.grdList2.Name = "grdList2"
        Me.grdList2.RowHeaderWidth = 5
        Me.grdList2.Size = New Size (539, 335)
        Me.grdList2.TabIndex = 0
        Me.grdList2.TableStyles.AddRange (New DataGridTableStyle() {Me.dtbParam2})
        '
        'dtbParam2
        '
        Me.dtbParam2.DataGrid = Me.grdList2
        Me.dtbParam2.GridColumnStyles.AddRange ( _
                                                New DataGridColumnStyle() _
                                                   {Me.DataGridTextBoxColumn5, Me.DataGridTextBoxColumn6, _
                                                    Me.DataGridTextBoxColumn7, Me.DataGridTextBoxColumn8})
        Me.dtbParam2.HeaderForeColor = SystemColors.ControlText
        Me.dtbParam2.MappingName = ""
        Me.dtbParam2.RowHeaderWidth = 5
        Me.dtbParam2.SelectionBackColor = Color.MediumSlateBlue
        '
        'DataGridTextBoxColumn5
        '
        Me.DataGridTextBoxColumn5.Format = ""
        Me.DataGridTextBoxColumn5.FormatInfo = Nothing
        Me.DataGridTextBoxColumn5.MappingName = ""
        Me.DataGridTextBoxColumn5.NullText = ""
        Me.DataGridTextBoxColumn5.Width = 0
        '
        'DataGridTextBoxColumn6
        '
        Me.DataGridTextBoxColumn6.Format = ""
        Me.DataGridTextBoxColumn6.FormatInfo = Nothing
        Me.DataGridTextBoxColumn6.HeaderText = "Tên tham số"
        Me.DataGridTextBoxColumn6.MappingName = "sName"
        Me.DataGridTextBoxColumn6.NullText = ""
        Me.DataGridTextBoxColumn6.Width = 150
        '
        'DataGridTextBoxColumn7
        '
        Me.DataGridTextBoxColumn7.Format = ""
        Me.DataGridTextBoxColumn7.FormatInfo = Nothing
        Me.DataGridTextBoxColumn7.HeaderText = "Giá trị "
        Me.DataGridTextBoxColumn7.MappingName = "sValue"
        Me.DataGridTextBoxColumn7.NullText = ""
        Me.DataGridTextBoxColumn7.Width = 150
        '
        'DataGridTextBoxColumn8
        '
        Me.DataGridTextBoxColumn8.Format = ""
        Me.DataGridTextBoxColumn8.FormatInfo = Nothing
        Me.DataGridTextBoxColumn8.HeaderText = "Kiểu dữ liệu"
        Me.DataGridTextBoxColumn8.MappingName = "sDataType"
        Me.DataGridTextBoxColumn8.NullText = ""
        Me.DataGridTextBoxColumn8.Width = 250
        '
        'GroupBox1
        '
        Me.GroupBox1.Location = New Point (3, 369)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New Size (540, 3)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        '
        'cmdClose
        '
        Me.cmdClose.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.cmdClose.Location = New Point (453, 378)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New Size (81, 27)
        Me.cmdClose.TabIndex = 7
        Me.cmdClose.Text = "&Thoát"
        '
        'cmdHelp
        '
        Me.cmdHelp.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.cmdHelp.Location = New Point (336, 378)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New Size (81, 27)
        Me.cmdHelp.TabIndex = 8
        Me.cmdHelp.Text = "&Help"
        '
        'FRM_SetParamSystem
        '
        Me.AutoScaleBaseSize = New Size (5, 13)
        Me.ClientSize = New Size (547, 411)
        Me.Controls.Add (Me.cmdHelp)
        Me.Controls.Add (Me.cmdClose)
        Me.Controls.Add (Me.GroupBox1)
        Me.Controls.Add (Me.TabControl1)
        Me.Controls.Add (Me.cmdSetagain)
        Me.Controls.Add (Me.cmdRefresh)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType (resources.GetObject ("$this.Icon"), Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FRM_SetParamSystem"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Thiết lập giá trị tham số hệ thống"
        Me.TabControl1.ResumeLayout (False)
        Me.TabPage2.ResumeLayout (False)
        CType (Me.grdList2, ISupportInitialize).EndInit()
        Me.ResumeLayout (False)

    End Sub

#End Region

    Private Sub FRM_SetParamSystem_Load (ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        GetParamAgain()
        If gv_CurrAssembly Is Nothing Then
            cmdSetagain.Enabled = False
        Else
            cmdSetagain.Enabled = True
        End If
    End Sub

    Private Sub cmdClose_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub cmdSetagain_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdSetagain.Click
        LoadParamsValuesAgain (gv_CurrAssembly)
    End Sub

    Private Sub cmdRefresh_Click (ByVal sender As Object, ByVal e As EventArgs) Handles cmdRefresh.Click
        GetParamAgain()
    End Sub

    Private Sub GetParamAgain()
        Try
            gv_dsParam = dsGetAllParams()
            With grdList2
                .TableStyles (0).MappingName = gv_dsParam.Tables (0).TableName
                .DataSource = gv_dsParam.Tables (0).DefaultView
            End With
            gv_dsParam.Tables (0).DefaultView.AllowDelete = False
            gv_dsParam.Tables (0).DefaultView.AllowEdit = False
            gv_dsParam.Tables (0).DefaultView.AllowNew = False
        Catch ex As Exception

        End Try
    End Sub


    Private Sub grdList2_CurrentCellChanged (ByVal sender As Object, ByVal e As EventArgs) _
        Handles grdList2.CurrentCellChanged
        Try
            grdList2.Select (grdList2.CurrentRowIndex)
            grdList2.CurrentCell = New DataGridCell (grdList2.CurrentRowIndex, 0)
        Catch ex As Exception

        End Try
    End Sub
End Class
