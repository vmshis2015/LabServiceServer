Imports Crownwood.Magic.Menus

Public Class MenuInherits
    Inherits MenuCommand
    Protected Friend mv_sDLLName As String
    Protected Friend mv_sFormName As String
    Protected Friend mv_sAssemblyName As String
    Protected Friend mv_sRoleName As String
    Protected Friend mv_iID As Integer
    Protected Friend mv_intShortCutKey As Integer
    Protected Friend mv_sInconPath As String
    Protected Friend mv_objImgList As ImageList
    Protected Friend mv_intImgIndex As Integer
    Protected Friend mv_sParameterList As String

    Sub New (ByVal pv_sRoleName As String, ByVal pv_oImgList As ImageList, ByVal pv_intImgIndex As Integer, _
             ByVal pv_intShortCutKey As Integer)
        MyBase.New (pv_sRoleName, pv_oImgList, pv_intImgIndex, pv_intShortCutKey)
    End Sub
End Class
