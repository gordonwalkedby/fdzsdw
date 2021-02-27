Public Module Helpers

    ''' <summary>
    ''' 把字符串的两头加上引号，输入的字符串里面原有的引号不会做任何处理
    ''' </summary>
    ''' <returns></returns>
    Public Function QuoteStr(s As String) As String
        Dim qt = """"
        If s Is Nothing OrElse s.Length < 1 Then
            Return qt + qt
        End If
        Return qt + s + qt
    End Function

    ''' <summary>
    ''' 获取当前程序的文件信息
    ''' </summary>
    Public Function GetCurrentProgramFile() As FileInfo
        Static f As New FileInfo(Path.Combine(My.Application.Info.DirectoryPath, My.Application.Info.AssemblyName + ".exe"))
        f.Refresh()
        Return f
    End Function

    ''' <summary>
    ''' 重启本进程，使用cmd关闭我，再重启一个我，参数和工作文件夹都是照旧的
    ''' </summary>
    Public Sub RestartProgram(Optional admin As Boolean = False)
        Dim info As New ProcessStartInfo With {
            .FileName = "cmd.exe",
            .Arguments = $"/c taskkill /PID {Process.GetCurrentProcess.Id} & start {QuoteStr("")} {QuoteStr(GetCurrentProgramFile.FullName)} {Command()}",
            .WorkingDirectory = Directory.GetCurrentDirectory}
        If admin Then
            info.Verb = "runas"
        End If
        Process.Start(info)
        Environment.Exit(0)
    End Sub

    ''' <summary>
    ''' 保存这个reg字符串到临时文件，然后管理员运行regedit，然后删除这个临时文件
    ''' reg开头不用写 Windows Registry Editor Version 5.00
    ''' 如果用户拒绝管理员运行，那么会throw
    ''' </summary>
    Public Function RunRegEdit(reg As String) As Task
        Dim filename = Path.Combine(My.Application.Info.DirectoryPath, $"reg_{Now.GetHashCode}.reg")
        Using writer = File.CreateText(filename)
            With writer
                .AutoFlush = True
                .WriteLine("Windows Registry Editor Version 5.00")
                .Write(reg)
                .Close()
            End With
        End Using
        Dim info As New ProcessStartInfo With {
            .FileName = "regedit.exe",
            .Arguments = QuoteStr(filename),
            .WorkingDirectory = Directory.GetCurrentDirectory,
            .Verb = "runas",
            .CreateNoWindow = False
        }
        Dim t1 As New Task(Sub()
                               Dim ps = Process.Start(info)
                               ps.WaitForExit()
                               If File.Exists(filename) Then File.Delete(filename)
                           End Sub)
        t1.Start()
        Return t1
    End Function

    ''' <summary>
    ''' 把文件路径转换为 file:/// 开头的文件URL，注意：输入的path必须为完整的path
    ''' </summary>
    ''' <returns></returns>
    Public Function ConvertPathToFileURL(path As String) As String
        If path Is Nothing OrElse path.Length < 1 Then
            Throw New NullReferenceException("path is null or empty.")
        End If
        If path.StartsWith("http") OrElse path.StartsWith("ftp") OrElse path.StartsWith("file:///") Then
            Return path
        End If
        path = "file:///" + path.Replace("\", "/")
        Return path
    End Function

    ''' <summary>
    ''' 渐变色获取类
    ''' </summary>
    Public Class GradientColor

        ''' <summary>
        ''' 获取c1到c2两个颜色中间的颜色，类似渐变色，pos是0-1的一个小数
        ''' </summary>
        Public Shared Function GetMiddleColor(c1 As Color, c2 As Color, pos As Single) As Color
            If pos <= 0 Then Return c1
            If pos >= 1 Then Return c2
            Return Color.FromArgb(CInt(c1.R) + (CInt(c2.R) - CInt(c1.R)) * pos, CInt(c1.G) + (CInt(c2.G) - CInt(c1.G)) * pos, CInt(c1.B) + (CInt(c2.B) - CInt(c1.B)) * pos)
        End Function

        Public Sub New()
        End Sub

        Private _min As Single = 1, _max As Single = 0

        Public ReadOnly Property MinColor As Color
            Get
                Return Colors.Item(_min)
            End Get
        End Property

        Public ReadOnly Property MaxColor As Color
            Get
                Return Colors.Item(_max)
            End Get
        End Property

        Private _list As New List(Of Single)
        Public Property Colors As New Dictionary(Of Single, Color)

        Public Sub SetColor(pos As Single, c As Color)
            If pos > 1 OrElse pos < 0 Then
                Throw New ArgumentOutOfRangeException(paramName:="pos")
            End If
            If Colors.ContainsKey(pos) Then
                Colors.Item(pos) = c
            Else
                Colors.Add(pos, c)
                _list.Add(pos)
                If pos > _max Then
                    _max = pos
                ElseIf pos < _min Then
                    _min = pos
                End If
                _list.Sort()
            End If
        End Sub

        Public Function GetColor(pos As Single) As Color
            If Colors.Count < 1 Then
                Throw New Exception("No color here.")
            End If
            If Colors.Count = 1 Then
                Return Colors.Values.First
            End If
            If Colors.ContainsKey(pos) Then
                Return Colors.Item(pos)
            End If
            If pos > _max Then
                Return MaxColor
            ElseIf pos < _min Then
                Return MinColor
            End If
            Dim pos1 As Single = 0
            Dim pos2 As Single = 0
            For i As Integer = 0 To _list.Count - 2
                pos1 = _list(i)
                pos2 = _list(i + 1)
                If pos1 < pos AndAlso pos2 > pos Then
                    Exit For
                End If
            Next
            Dim c1 = Colors.Item(pos1)
            Dim c2 = Colors.Item(pos2)
            pos = (pos - pos1) / (pos2 - pos1)
            Return GetMiddleColor(c1, c2, pos)
        End Function

    End Class

    ''' <summary>
    ''' 打开这个链接
    ''' </summary>
    Public Function OpenBrower(link As String) As Boolean
        Try
            Process.Start("cmd.exe", $"/c start """" ""{link}""")
            Return True
        Catch ex As Exception
            Clipboard.SetText(link)
            MsgBox($"打开链接失败，链接已经复制到您的剪贴板里面，您可以自行打开。{vbCrLf}{link}", MsgBoxStyle.Critical)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' 输出错误信息到 errors.txt 里，如果输出失败就失败了
    ''' </summary>
    Public Sub DoBGLog(m As String)
        Static logfile As New FileInfo(Path.Combine(GetCurrentProgramFile.DirectoryName, "errors.txt"))
        m = $"{Now} | {m}{vbCrLf}"
        Try
            File.AppendAllText(logfile.FullName, m)
        Catch ex As Exception
            Debug.WriteLine(ex)
        End Try
    End Sub

    ''' <summary>
    ''' 获取 github release 的最新版本
    ''' </summary>
    ''' <returns></returns>
    Public Function TryGetNewsetReleaseVersion(ByRef newVer As Version, ByRef errors As String) As Boolean
        errors = ""
        newVer = Nothing
        Dim h = WebRequest.CreateHttp("https://api.github.com/repos/gordonwalkedby/fdzsdw/releases/latest")
        With h
            .Method = "GET"
            .Timeout = 8000
            .Accept = "application/vnd.github.v3+json"
            .UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:78.0) Gecko/20100101 Firefox/78.0"
            Try
                Using r As HttpWebResponse = h.GetResponse
                    If r.StatusCode = 200 Then
                        Using reader As New StreamReader(r.GetResponseStream)
                            Dim jj As String = reader.ReadToEnd
                            jj = jj.Replace("""", "")
                            Dim m As Match = Regex.Match(jj, "tag_name: v([0-9\.]+)")
                            If m.Success Then
                                Dim str = m.Result("$1")
                                Dim vv As Version = Nothing
                                If Version.TryParse(str, vv) Then
                                    newVer = vv
                                    Return True
                                Else
                                    errors = $"tag_name 字符串里无法读取版本号 {m}"
                                End If
                            Else
                                errors = $"返回的 json 里无法读取 tag_name "
                            End If
                        End Using
                    Else
                        errors = $"http 返回 {r.StatusCode}"
                    End If
                End Using
            Catch ex As Exception
                errors = $"http 请求出错 {ex}"
            End Try
        End With
        If errors.Length > 0 Then
            errors = "检测更新失败：" + errors
            DoBGLog(errors)
        End If
        Return False
    End Function

End Module
