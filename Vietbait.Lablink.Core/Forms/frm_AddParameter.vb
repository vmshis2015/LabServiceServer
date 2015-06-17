Imports System.ComponentModel

Public Class frm_AddParameter
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
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents ComboBox1 As ComboBox

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupBox1 = New GroupBox
        Me.Button1 = New Button
        Me.Button2 = New Button
        Me.Label1 = New Label
        Me.Label2 = New Label
        Me.Label3 = New Label
        Me.Label4 = New Label
        Me.TextBox1 = New TextBox
        Me.TextBox2 = New TextBox
        Me.TextBox3 = New TextBox
        Me.ComboBox1 = New ComboBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add (Me.ComboBox1)
        Me.GroupBox1.Controls.Add (Me.TextBox3)
        Me.GroupBox1.Controls.Add (Me.TextBox2)
        Me.GroupBox1.Controls.Add (Me.TextBox1)
        Me.GroupBox1.Controls.Add (Me.Label4)
        Me.GroupBox1.Controls.Add (Me.Label3)
        Me.GroupBox1.Controls.Add (Me.Label2)
        Me.GroupBox1.Controls.Add (Me.Label1)
        Me.GroupBox1.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.GroupBox1.Location = New Point (3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New Size (384, 192)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Thông tin tham số"
        '
        'Button1
        '
        Me.Button1.Location = New Point (225, 204)
        Me.Button1.Name = "Button1"
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Ghi"
        '
        'Button2
        '
        Me.Button2.Location = New Point (309, 204)
        Me.Button2.Name = "Button2"
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "&Thoát(Esc)"
        '
        'Label1
        '
        Me.Label1.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label1.Location = New Point (12, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tên tham số"
        '
        'Label2
        '
        Me.Label2.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label2.Location = New Point (12, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Giá trị tham số"
        '
        'Label3
        '
        Me.Label3.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label3.Location = New Point (12, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Kiểu dữ liệu"
        '
        'Label4
        '
        Me.Label4.Font = New Font ("Arial", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.Label4.Location = New Point (12, 99)
        Me.Label4.Name = "Label4"
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Diễn giải"
        '
        'TextBox1
        '
        Me.TextBox1.Font = New Font ("Arial", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.TextBox1.Location = New Point (117, 27)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New Size (261, 20)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = ""
        '
        'TextBox2
        '
        Me.TextBox2.Font = New Font ("Arial", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.TextBox2.Location = New Point (117, 51)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New Size (159, 20)
        Me.TextBox2.TabIndex = 1
        Me.TextBox2.Text = ""
        '
        'TextBox3
        '
        Me.TextBox3.Font = New Font ("Arial", 8.25!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.TextBox3.Location = New Point (117, 102)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ScrollBars = ScrollBars.Vertical
        Me.TextBox3.Size = New Size (261, 81)
        Me.TextBox3.TabIndex = 3
        Me.TextBox3.Text = ""
        '
        'ComboBox1
        '
        Me.ComboBox1.Items.AddRange ( _
                                     New Object() _
                                        {"bigint", "binary", "bit", "char", "datetime", "decimal", "float", "int", _
                                         "money", "numeric", "nvarchar", "real", "smallint"})
        Me.ComboBox1.Location = New Point (117, 75)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New Size (159, 24)
        Me.ComboBox1.TabIndex = 2
        '
        'frm_AddParameter
        '
        Me.AutoScaleBaseSize = New Size (5, 13)
        Me.ClientSize = New Size (390, 236)
        Me.Controls.Add (Me.Button2)
        Me.Controls.Add (Me.Button1)
        Me.Controls.Add (Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frm_AddParameter"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Tham số hệ thống"
        Me.GroupBox1.ResumeLayout (False)
        Me.ResumeLayout (False)

    End Sub

#End Region
End Class
