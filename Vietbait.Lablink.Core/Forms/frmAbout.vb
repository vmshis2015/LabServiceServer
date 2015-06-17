Imports System.ComponentModel
Imports System.Resources

Public Class frmAbout
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
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lblAbout As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As ResourceManager = New ResourceManager (GetType (frmAbout))
        Me.LinkLabel1 = New LinkLabel
        Me.PictureBox1 = New PictureBox
        Me.lblAbout = New Label
        Me.Label1 = New Label
        Me.Label2 = New Label
        Me.Label3 = New Label
        Me.Label4 = New Label
        Me.SuspendLayout()
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Anchor = AnchorStyles.None
        Me.LinkLabel1.Font = New Font ("Tahoma", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.LinkLabel1.ForeColor = Color.Yellow
        Me.LinkLabel1.LinkColor = Color.Yellow
        Me.LinkLabel1.Location = New Point (225, 219)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New Size (207, 23)
        Me.LinkLabel1.TabIndex = 0
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Email: Mabujin2003@yahoo.com"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType (resources.GetObject ("PictureBox1.Image"), Image)
        Me.PictureBox1.Location = New Point (24, 21)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New Size (195, 216)
        Me.PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'lblAbout
        '
        Me.lblAbout.Anchor = AnchorStyles.None
        Me.lblAbout.Font = _
            New Font ("Microsoft Sans Serif", 9.0!, FontStyle.Bold, GraphicsUnit.Point, CType (163, Byte))
        Me.lblAbout.ForeColor = Color.Yellow
        Me.lblAbout.Location = New Point (225, 24)
        Me.lblAbout.Name = "lblAbout"
        Me.lblAbout.Size = New Size (210, 16)
        Me.lblAbout.TabIndex = 2
        Me.lblAbout.Text = "Tác giả : Đào Văn Cường          "
        '
        'Label1
        '
        Me.Label1.Anchor = AnchorStyles.None
        Me.Label1.Font = _
            New Font ("Microsoft Sans Serif", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (163, Byte))
        Me.Label1.ForeColor = Color.White
        Me.Label1.Location = New Point (225, 81)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size (208, 84)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Phần mềm đã được bảo hộ quyền tác giả. Việc sao chép dưới bất cứ hình thức hoặc b" & _
                         "ằng bất kỳ phương tiện nào cũng là bất hợp pháp và phải chịu mức truy tố cao nhấ" & _
                         "t trước pháp luật. "
        '
        'Label2
        '
        Me.Label2.Anchor = AnchorStyles.None
        Me.Label2.Font = New Font ("Microsoft Sans Serif", 9.0!, FontStyle.Bold, GraphicsUnit.Point, CType (163, Byte))
        Me.Label2.ForeColor = Color.White
        Me.Label2.Location = New Point (225, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New Size (208, 32)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Company: EVNIT 16 Le Dai Hanh - Hai Ba Trung - Ha Noi"
        '
        'Label3
        '
        Me.Label3.Anchor = AnchorStyles.None
        Me.Label3.Font = New Font ("Microsoft Sans Serif", 9.0!, FontStyle.Bold, GraphicsUnit.Point, CType (163, Byte))
        Me.Label3.ForeColor = Color.Yellow
        Me.Label3.Location = New Point (225, 171)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New Size (208, 18)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Mobile: 0904648006"
        '
        'Label4
        '
        Me.Label4.Anchor = AnchorStyles.None
        Me.Label4.Font = New Font ("Microsoft Sans Serif", 9.0!, FontStyle.Bold, GraphicsUnit.Point, CType (163, Byte))
        Me.Label4.ForeColor = Color.Yellow
        Me.Label4.Location = New Point (225, 192)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New Size (208, 24)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Phone Number: (08)046442376"
        '
        'frmAbout
        '
        Me.AutoScaleBaseSize = New Size (5, 13)
        Me.BackColor = Color.RoyalBlue
        Me.ClientSize = New Size (456, 267)
        Me.Controls.Add (Me.Label4)
        Me.Controls.Add (Me.Label3)
        Me.Controls.Add (Me.Label2)
        Me.Controls.Add (Me.lblAbout)
        Me.Controls.Add (Me.PictureBox1)
        Me.Controls.Add (Me.LinkLabel1)
        Me.Controls.Add (Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "frmAbout"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.ResumeLayout (False)

    End Sub

#End Region

    Private Sub frmAbout_Load (ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
    End Sub

    Private Sub PictureBox1_Click (ByVal sender As Object, ByVal e As EventArgs) Handles PictureBox1.Click
        Me.Close()
    End Sub

    Private Sub Label1_Click (ByVal sender As Object, ByVal e As EventArgs) Handles Label1.Click
        Me.Close()
    End Sub

    Private Sub Label2_Click (ByVal sender As Object, ByVal e As EventArgs) Handles Label2.Click
        Me.Close()
    End Sub

    Private Sub Label3_Click (ByVal sender As Object, ByVal e As EventArgs) Handles Label3.Click
        Me.Close()
    End Sub

    Private Sub lblAbout_Click (ByVal sender As Object, ByVal e As EventArgs) Handles lblAbout.Click
        Me.Close()
    End Sub

    Private Sub frmAbout_Click (ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Click
        Me.Close()
    End Sub

    Private Sub frmAbout_KeyDown (ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Label4_Click (ByVal sender As Object, ByVal e As EventArgs) Handles Label4.Click
        Me.Close()
    End Sub
End Class
