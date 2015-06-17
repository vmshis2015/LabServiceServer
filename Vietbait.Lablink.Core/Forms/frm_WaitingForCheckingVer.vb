Imports System.Threading
Imports System.ComponentModel

Public Class frm_WaitingForCheckingVer
    Inherits Form
    Public thre As Thread

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
    Friend WithEvents Label1 As Label
    Friend WithEvents lblStatus As Label

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New Label
        Me.lblStatus = New Label
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Dock = DockStyle.Top
        Me.Label1.Font = New Font ("Microsoft Sans Serif", 9.75!, FontStyle.Bold, GraphicsUnit.Point, CType (0, Byte))
        Me.Label1.Location = New Point (0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size (237, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Đang kiểm tra phiên bản>>"
        Me.Label1.TextAlign = ContentAlignment.MiddleCenter
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType ((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                       Or AnchorStyles.Left) _
                                      Or AnchorStyles.Right), AnchorStyles)
        Me.lblStatus.Font = _
            New Font ("Microsoft Sans Serif", 9.75!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me.lblStatus.Location = New Point (3, 27)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New Size (231, 18)
        Me.lblStatus.TabIndex = 1
        Me.lblStatus.Text = ".."
        Me.lblStatus.TextAlign = ContentAlignment.MiddleCenter
        '
        'frm_WaitingForCheckingVer
        '
        Me.AutoScaleBaseSize = New Size (5, 13)
        Me.ClientSize = New Size (237, 48)
        Me.Controls.Add (Me.lblStatus)
        Me.Controls.Add (Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frm_WaitingForCheckingVer"
        Me.ShowInTaskbar = False
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.ResumeLayout (False)

    End Sub

#End Region

    Private Sub frm_WaitingForCheckingVer_Load (ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        thre = New Thread (AddressOf CheckingVer)
        thre.Start()
    End Sub

    Private Sub CheckingVer()
        Try
            If Not gv_bHasCheckedVer Then
                For i As Integer = 0 To 100
                    If lblStatus.Text.Length < 100 Then
                        lblStatus.Text &= "."
                        thre.Sleep (50)
                    End If
                Next
            Else
                thre.Abort()
                Me.Close()
            End If
        Catch ex As Exception

        End Try
        'thre.Abort()
    End Sub

    Private Sub Label1_Click (ByVal sender As Object, ByVal e As EventArgs) Handles Label1.Click
        gv_bHasCheckedVer = True
    End Sub
End Class
