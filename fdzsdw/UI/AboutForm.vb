Public Class AboutForm


    Private Sub AboutForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LabInfo.Text = $"服毒自杀的我
fdzsdw.exe 版本：v{My.Application.Info.Version.ToString(2)}
作者：戈登走過去
更多信息请点击右侧链接。
以及请翻阅本软件目录下自带的【说明.txt】"
        AddURLButton("游戏源码", "https://github.com/gordonwalkedby/fdzsdw")
        AddURLButton("我的博客", "https://wby2001.blogspot.com/")
        AddURLButton("游戏目录", GetCurrentProgramFile.DirectoryName)
        AddURLButton("打赏戈登", "https://github.com/gordonwalkedby/MyContent/blob/master/money.md")
    End Sub

    Private Sub AddURLButton(text As String, url As String)
        Dim b As New Button
        Pn.Controls.Add(b)
        With b
            .Dock = DockStyle.Top
            .Text = text
            .Height = 33
            .BringToFront()
            AddHandler .Click, Sub()
                                   OpenBrower(url)
                               End Sub
        End With
    End Sub

End Class