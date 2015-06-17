Imports System.ComponentModel
Imports System.Resources

Public Class IFModulePanel
    Inherits Form
    Protected _LabelImage As Image

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
    Public WithEvents pnlBackGround As Panel
    Friend WithEvents ptbUp As PictureBox
    Friend WithEvents ptbDown As PictureBox
    Friend WithEvents imlUpDown As ImageList

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New Container
        Dim resources As ResourceManager = New ResourceManager (GetType (IFModulePanel))
        Me.pnlBackGround = New Panel
        Me.ptbUp = New PictureBox
        Me.ptbDown = New PictureBox
        Me.imlUpDown = New ImageList (Me.components)
        Me.pnlBackGround.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlBackGround
        '
        Me.pnlBackGround.BackColor = SystemColors.ControlLightLight
        Me.pnlBackGround.BorderStyle = BorderStyle.Fixed3D
        Me.pnlBackGround.Controls.Add (Me.ptbUp)
        Me.pnlBackGround.Controls.Add (Me.ptbDown)
        Me.pnlBackGround.Dock = DockStyle.Fill
        Me.pnlBackGround.Location = New Point (0, 0)
        Me.pnlBackGround.Name = "pnlBackGround"
        Me.pnlBackGround.Size = New Size (193, 384)
        Me.pnlBackGround.TabIndex = 0
        '
        'ptbUp
        '
        Me.ptbUp.Anchor = CType ((AnchorStyles.Top Or AnchorStyles.Right), AnchorStyles)
        Me.ptbUp.Image = CType (resources.GetObject ("ptbUp.Image"), Image)
        Me.ptbUp.Location = New Point (165, 0)
        Me.ptbUp.Name = "ptbUp"
        Me.ptbUp.Size = New Size (25, 24)
        Me.ptbUp.TabIndex = 1
        Me.ptbUp.TabStop = False
        Me.ptbUp.Visible = False
        '
        'ptbDown
        '
        Me.ptbDown.Anchor = CType ((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
        Me.ptbDown.Image = CType (resources.GetObject ("ptbDown.Image"), Image)
        Me.ptbDown.Location = New Point (165, 356)
        Me.ptbDown.Name = "ptbDown"
        Me.ptbDown.Size = New Size (25, 25)
        Me.ptbDown.TabIndex = 1
        Me.ptbDown.TabStop = False
        '
        'imlUpDown
        '
        Me.imlUpDown.ColorDepth = ColorDepth.Depth32Bit
        Me.imlUpDown.ImageSize = New Size (24, 24)
        Me.imlUpDown.ImageStream = CType (resources.GetObject ("imlUpDown.ImageStream"), ImageListStreamer)
        Me.imlUpDown.TransparentColor = Color.Transparent
        '
        'IFModulePanel
        '
        Me.AutoScaleBaseSize = New Size (5, 13)
        Me.BackColor = SystemColors.Control
        Me.ClientSize = New Size (193, 384)
        Me.ControlBox = False
        Me.Controls.Add (Me.pnlBackGround)
        Me.Name = "IFModulePanel"
        Me.pnlBackGround.ResumeLayout (False)
        Me.ResumeLayout (False)

    End Sub

#End Region

    Private Sub ptbUp_MouseDown (ByVal sender As Object, ByVal e As MouseEventArgs) Handles ptbUp.MouseDown
        If AllowUp Then
            ptbUp.Image = imlUpDown.Images.Item (6)
        End If
    End Sub

    Private Sub ptbUp_MouseEnter (ByVal sender As Object, ByVal e As EventArgs) Handles ptbUp.MouseEnter
        If AllowUp Then
            ptbUp.Image = imlUpDown.Images.Item (4)
        End If
    End Sub

    Private Sub ptbUp_MouseLeave (ByVal sender As Object, ByVal e As EventArgs) Handles ptbUp.MouseLeave
        If AllowUp Then
            ptbUp.Image = imlUpDown.Images.Item (5)
        End If
    End Sub

    Private Sub ptbDown_MouseDown (ByVal sender As Object, ByVal e As MouseEventArgs) Handles ptbDown.MouseDown
        If AllowDown Then
            ptbDown.Image = imlUpDown.Images.Item (2)
        End If
    End Sub

    Private Sub ptbDown_MouseEnter (ByVal sender As Object, ByVal e As EventArgs) Handles ptbDown.MouseEnter
        If AllowDown Then
            ptbDown.Image = imlUpDown.Images.Item (0)
        End If
    End Sub

    Private Sub ptbDown_MouseLeave (ByVal sender As Object, ByVal e As EventArgs) Handles ptbDown.MouseLeave
        If AllowDown Then
            ptbDown.Image = imlUpDown.Images.Item (1)
        End If
    End Sub

    Private Sub ptbUp_MouseUp (ByVal sender As Object, ByVal e As MouseEventArgs) Handles ptbUp.MouseUp
        If AllowUp Then
            ptbUp.Image = imlUpDown.Images.Item (4)
        End If
    End Sub

    Private Sub ptbDown_MouseUp (ByVal sender As Object, ByVal e As MouseEventArgs) Handles ptbDown.MouseUp
        If AllowDown Then
            ptbDown.Image = imlUpDown.Images.Item (0)
        End If
    End Sub

    Private Sub ptbUp_Click (ByVal sender As Object, ByVal e As EventArgs) Handles ptbUp.Click
        If gv_intSubSystemCount > 0 Then
            Dim i As Integer
            For i = gv_intSubSystemCount - 1 To 0 Step - 1
                If AllowUp Then
                    pnlBackGround.Controls.Item (i + 2).Top = pnlBackGround.Controls.Item (i + 2).Top + _
                                                              pnlBackGround.Controls.Item (i + 2).Height
                    '+10
                    If gv_intWidth*gv_intSubSystemCount > SubSystemPanel.pnlBackGround.Height Then
                        AllowDown = True
                        ptbDown.Visible = True
                        ptbDown.Image = imlUpDown.Images.Item (2)
                    Else
                        ptbDown.Visible = False
                        AllowDown = False
                    End If
                    If pnlBackGround.Controls.Item (2).Top = pnlBackGround.Top Then
                        AllowUp = False
                        ptbUp.Visible = False
                        If gv_intWidth*gv_intSubSystemCount > SubSystemPanel.pnlBackGround.Height Then
                            ptbDown.Visible = True
                            AllowDown = True
                        Else
                            ptbDown.Visible = False
                        End If
                        Exit Sub
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub ptbDown_Click (ByVal sender As Object, ByVal e As EventArgs) Handles ptbDown.Click
        If gv_intSubSystemCount > 0 Then
            Dim i As Integer
            For i = 0 To gv_intSubSystemCount - 1
                If AllowDown Then
                    pnlBackGround.Controls.Item (i + 2).Top = pnlBackGround.Controls.Item (i + 2).Top - _
                                                              pnlBackGround.Controls.Item (i + 2).Height
                    '- 10
                    AllowUp = True
                    ptbUp.Visible = True
                    ptbUp.Image = imlUpDown.Images.Item (5)
                    If _
                        pnlBackGround.Controls.Item (gv_intSubSystemCount - 1 + 2).Top + _
                        pnlBackGround.Controls.Item (gv_intSubSystemCount - 1 + 2).Height < pnlBackGround.Height Then
                        AllowDown = False
                        ptbDown.Visible = False
                        'ptbDown.Image = imlUpDown.Images.Item(3)
                        Exit Sub
                    End If
                End If
            Next
            If gv_intWidth*gv_intSubSystemCount > SubSystemPanel.pnlBackGround.Height Then
                SubSystemPanel.ptbDown.Visible = True
                SubSystemPanel.ptbUp.Image = SubSystemPanel.imlUpDown.Images.Item (7)
                If SubSystemPanel.pnlBackGround.Controls (2).Top < 0 Then
                    AllowUp = True
                    SubSystemPanel.ptbUp.Visible = True
                Else
                    SubSystemPanel.ptbUp.Visible = False
                    AllowUp = False
                End If
                AllowDown = True
            Else
                If SubSystemPanel.pnlBackGround.Controls (2).Top < 0 Then
                    AllowUp = True
                    SubSystemPanel.ptbUp.Visible = True
                Else
                    SubSystemPanel.ptbUp.Visible = False
                    AllowUp = False
                End If
                SubSystemPanel.ptbDown.Visible = False
            End If
        End If

    End Sub

    Private Sub pnlBackGround_Paint (ByVal sender As Object, ByVal e As PaintEventArgs) Handles pnlBackGround.Paint

    End Sub

    Private Sub pnlBackGround_MouseHover (ByVal sender As Object, ByVal e As EventArgs) Handles pnlBackGround.MouseHover
        Dim t As New ToolTip
        t.SetToolTip (sender, sender.height)
    End Sub
End Class
