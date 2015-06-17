Imports System.ComponentModel

Public Class PicBeforeText
    Inherits UserControl
    Public _LabelID As Integer

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
    Friend WithEvents Picture As PictureBox
    Friend WithEvents _Link As LinkLabel

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Picture = New PictureBox
        Me._Link = New LinkLabel
        Me.SuspendLayout()
        '
        'Picture
        '
        Me.Picture.AccessibleName = "PicBeforeText"
        Me.Picture.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane
        Me.Picture.Anchor = AnchorStyles.Left
        Me.Picture.Cursor = Cursors.Hand
        Me.Picture.Location = New Point (12, 9)
        Me.Picture.Name = "Picture"
        Me.Picture.Size = New Size (30, 24)
        Me.Picture.SizeMode = PictureBoxSizeMode.StretchImage
        Me.Picture.TabIndex = 0
        Me.Picture.TabStop = False
        '
        '_Link
        '
        Me._Link.AccessibleName = "PicBeforeText"
        Me._Link.ActiveLinkColor = Color.MidnightBlue
        Me._Link.Anchor = CType ((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                                   Or AnchorStyles.Left) _
                                  Or AnchorStyles.Right), AnchorStyles)
        Me._Link.BackColor = SystemColors.ControlLightLight
        Me._Link.Cursor = Cursors.Hand
        Me._Link.Font = New Font ("Arial", 9.0!, FontStyle.Regular, GraphicsUnit.Point, CType (0, Byte))
        Me._Link.LinkBehavior = LinkBehavior.HoverUnderline
        Me._Link.LinkColor = Color.Black
        Me._Link.Location = New Point (42, 0)
        Me._Link.Name = "_Link"
        Me._Link.Size = New Size (159, 45)
        Me._Link.TabIndex = 1
        Me._Link.TabStop = True
        Me._Link.Text = "Tên của phân hệ"
        Me._Link.TextAlign = ContentAlignment.MiddleLeft
        Me._Link.VisitedLinkColor = Color.MidnightBlue
        '
        'PicBeforeText
        '
        Me.AccessibleName = "PicBeforeText"
        Me.BackColor = SystemColors.ControlLightLight
        Me.Controls.Add (Me._Link)
        Me.Controls.Add (Me.Picture)
        Me.Cursor = Cursors.Hand
        Me.Name = "PicBeforeText"
        Me.Size = New Size (210, 45)
        Me.ResumeLayout (False)

    End Sub

#End Region

    Private Sub _Link_LinkClicked (ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) _
        Handles _Link.LinkClicked

    End Sub
End Class
