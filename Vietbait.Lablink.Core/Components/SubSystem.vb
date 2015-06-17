Imports System.ComponentModel

Public Class SubSystem
    Inherits UserControl

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl overrides dispose to clean up the component list.
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
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents LinkLabel1 As LinkLabel

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PictureBox1 = New PictureBox
        Me.LinkLabel1 = New LinkLabel
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New Point (3, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New Size (51, 42)
        Me.PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Font = _
            New Font ("Microsoft Sans Serif", 8.25!, FontStyle.Bold, GraphicsUnit.Point, CType (0, Byte))
        Me.LinkLabel1.LinkColor = Color.Navy
        Me.LinkLabel1.Location = New Point (54, 3)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New Size (150, 63)
        Me.LinkLabel1.TabIndex = 1
        Me.LinkLabel1.TextAlign = ContentAlignment.MiddleCenter
        '
        'SubSystem
        '
        Me.Controls.Add (Me.LinkLabel1)
        Me.Controls.Add (Me.PictureBox1)
        Me.Name = "SubSystem"
        Me.Size = New Size (201, 69)
        Me.ResumeLayout (False)

    End Sub

#End Region

    Private Sub LinkLabel1_LinkClicked (ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) _
        Handles LinkLabel1.LinkClicked

    End Sub
End Class
