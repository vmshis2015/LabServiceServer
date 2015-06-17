Imports System.Reflection
Imports System.IO

Public Class DVC_MenuItem
    Public Function GetMenuItem (ByVal pv_sRoleName As String, ByVal pv_iRole As Integer, _
                                 ByVal pv_sDLLName As String, ByVal pv_sFormName As String, _
                                 ByVal pv_sAssemblyName As String, ByVal pv_sParameterList As String, _
                                 ByVal pv_oImgList As ImageList, _
                                 ByVal pv_sIconPath As String, ByVal pv_intShorCutKey As Integer, _
                                 ByVal pv_intIgmIndex As Integer) As MenuInherits
        Dim fv_oMenuItem As New MenuInherits (pv_sRoleName, pv_oImgList, pv_intIgmIndex, pv_intShorCutKey)
        With fv_oMenuItem
            .mv_sParameterList = pv_sParameterList
            .mv_sRoleName = pv_sRoleName
            .mv_iID = pv_iRole
            .mv_sAssemblyName = pv_sAssemblyName
            .mv_sDLLName = pv_sDLLName
            .mv_sFormName = pv_sFormName
            .mv_intImgIndex = pv_intIgmIndex
            .mv_intShortCutKey = pv_intShorCutKey
            .mv_objImgList = pv_oImgList
        End With
        AddHandler fv_oMenuItem.Click, AddressOf _Click
        Return fv_oMenuItem
    End Function

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Thực hiện khi chọn một chức năng trên Menu
    'Đầu vào         :
    'Đầu ra           :
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Private Sub _Click (ByVal sender As Object, ByVal e As EventArgs)
        Try
            ms_InvokeForm (CType (sender, MenuInherits).mv_sAssemblyName, CType (sender, MenuInherits).mv_sDLLName, _
                           CType (sender, MenuInherits).mv_sFormName, CType (sender, MenuInherits).mv_sParameterList)
        Catch ex As Exception
            'Throw ex
            System.IO.File.WriteAllText("C:\Lablink-Menu-Error.txt" & Now.ToString, ex.ToString)
        End Try
    End Sub

    '----------------------------------------------------------------------------------------------------
    'Mục đích       :Thực hiện chức năng tương ứng với hàm được gọi trong Assembly
    'Đầu vào         :Tên Assembly,tên DLL,tên Form
    'Đầu ra           :Menu của phân hệ được xây dựng 
    'Người tạo      :CuongDV
    'Ngày tạo       :
    'Last Modified:
    '----------------------------------------------------------------------------------------------------
    Public Sub ms_InvokeForm(ByVal pv_sAssemblyName As String, _
                              ByVal pv_sDLLName As String, _
                              ByVal pv_sFormName As String, ByVal pv_sParameterList As String)
        Try
            Dim sv_oAss As Assembly
            Dim bExistForm As Boolean = False
            Dim sv_oAllForm As Type()
            'Contains All Forms from Assembly
            If pv_sDLLName.Trim.Equals(String.Empty) Then
                MessageBox.Show("Menu này chưa được gán chức năng", "Thông báo", MessageBoxButtons.OK, _
                                 MessageBoxIcon.Information)
                Return
            End If
            If Not File.Exists(Application.StartupPath & "\" & pv_sDLLName & ".DLL") Then
                MessageBox.Show("Không tồn tại phân hệ " & pv_sDLLName & ".DLL", gv_sAnnouncement, MessageBoxButtons.OK, _
                                 MessageBoxIcon.Information)
                Return
            End If

            'Load Assembly Information from AssemblyName
            sv_oAss = [Assembly].LoadFrom(Application.StartupPath & "\" & pv_sDLLName & ".DLL")
            'Get Forms from AssemblyInfor
            sv_oAllForm = sv_oAss.GetTypes
            gv_CurrAssembly = sv_oAss
            If Not gv_objArrDLL.Contains(pv_sDLLName) Then
                gv_objArrDLL.Add(pv_sDLLName)
                ProcessInterfacesAndMethods(sv_oAss)
            End If
            LoadParamsValues(sv_oAss)
            '----------------------------------------------------------------------------------
            Dim sv_iflags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.Static Or _
                                            BindingFlags.Instance Or BindingFlags.DeclaredOnly
            Dim sv_oType As Type
            For Each sv_oType In sv_oAllForm
                'If sv_oType.Equals(gv_oMainForm.GetType) Then
                Try
                    If sv_oType.Name.Trim.ToUpper.Trim.Equals(pv_sFormName.Trim.ToUpper) Then
                        Dim instance As Object
                        If pv_sParameterList.Trim.Equals(String.Empty) Then
                            instance = Activator.CreateInstance(sv_oType)
                        Else
                            instance = Activator.CreateInstance(sv_oType, New Object() {pv_sParameterList})
                        End If
                        'Console.WriteLine(instance.GetType.BaseType.ToString.ToUpper)

                        bExistForm = True
                        If instance.GetType.BaseType.ToString.ToUpper.Equals("DevComponents.DotNetBar.Office2007Form".ToUpper) Then
                            CType(instance, DevComponents.DotNetBar.Office2007Form).ShowDialog()
                        Else
                            CType(instance, System.Windows.Forms.Form).ShowDialog()
                        End If
                        ''''Dim sv_oCalledForm As Form
                        ''''If instance.GetType.BaseType Then

                        ''''End If
                        '''If pv_sParameterList.Trim.Equals(String.Empty) Then
                        '''    sv_oCalledForm = CType(instance, Form)
                        '''Else
                        '''    sv_oCalledForm = CType(Activator.CreateInstance(sv_oType, New Object() {pv_sParameterList}), Form)
                        '''End If
                        'bExistForm = True
                        'sv_oCalledForm.ShowDialog()
                        Exit For
                    End If
                Catch ex As Exception
                    System.IO.File.WriteAllText("C:\Lablink-Menu-Error.txt" & Now.ToString, ex.ToString)
                End Try

                'End If
            Next
            If Not bExistForm Then
                MessageBox.Show("Không tồn tại chức năng " & pv_sFormName & " trong phân hệ " & pv_sDLLName & ".DLL", _
                                 gv_sAnnouncement, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
        Catch ex As Exception
            MessageBox.Show("Lỗi khởi động chức năng!" & vbCrLf & ex.Message, "Thông báo", MessageBoxButtons.OK, _
                             MessageBoxIcon.Information)
            System.IO.File.WriteAllText("C:\Lablink-Menu-Error.txt" & Now.ToString, ex.ToString)
        End Try
    End Sub

    'Private Function _Split(ByVal pv_sParam As String) As ArrayList
    '    Try
    '        Dim sReValue As New ArrayList
    '        Dim splitstr() As String
    '        splitstr = pv_sParam.Split(",")
    '        For i As Integer = 0 To splitstr.GetLength(0) - 1
    '            sReValue.Add(splitstr(i))
    '        Next
    '        Return sReValue
    '    Catch ex As Exception

    '    End Try
    'End Function
End Class