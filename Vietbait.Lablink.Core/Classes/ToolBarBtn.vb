Public Class ToolBarBtn
    Inherits ToolBarButton
    Protected Friend mv_sDLLName As String
    Protected Friend mv_sFormName As String
    Protected Friend mv_sAssemblyName As String
    Protected Friend mv_sText As String
    Protected Friend mv_sToolTipText As String
    Protected Friend mv_iID As Integer
    Protected Friend mv_sInconPath As String
    Protected Friend mv_intImgIndex As Integer
    Protected Friend mv_sParameterList As String
    Protected Friend IntRolePerformed As Integer = - 1
    Protected Friend bDisplayText As Boolean = False
    Protected Friend bDisplayTTT As Boolean = False

    Sub New (ByVal pv_sText As String, ByVal pv_sToolTipText As String, ByVal pv_intImgIndex As Integer, _
             Optional ByVal pv_bDisplayText As Boolean = False, Optional ByVal pv_bSeperator As Boolean = False, _
             Optional ByVal pv_bPopUp As Boolean = False, Optional ByVal pv_objCtxMenu As ContextMenu = Nothing)
        MyBase.New (IIf (pv_bDisplayText = True, pv_sText, ""))
        If pv_bSeperator Then
            Me.Style = ToolBarButtonStyle.Separator
        Else
            If pv_bPopUp Then
                Me.Style = ToolBarButtonStyle.DropDownButton
                If pv_objCtxMenu Is Nothing Then
                Else
                    Me.DropDownMenu = pv_objCtxMenu
                End If
            End If
            Me.ImageIndex = pv_intImgIndex
            Me.ToolTipText = pv_sToolTipText
        End If
    End Sub
End Class
