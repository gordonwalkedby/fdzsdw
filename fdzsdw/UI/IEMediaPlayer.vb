''' <summary>
''' WebBrowser为基础的媒体播放器，x64编译的情况下，Visual Studio的winform设计器会报错
''' </summary>
Public Class IEMediaPlayer
    Inherits Control

    Private ReadOnly ie As New WebBrowser

    Public Sub New()
        Me.BackColor = Color.Blue
    End Sub

    ''' <summary>
    ''' 初始化IE，必须在使用之前调用这个函数，如果IE版本为11或更高，就返回 true
    ''' </summary>
    Public Function LoadIE() As Boolean
        Static vv As Integer = 0
        If vv < 1 Then
            Me.Controls.Add(ie)
            With ie
                .Dock = DockStyle.Fill
                .IsWebBrowserContextMenuEnabled = False
                .ScriptErrorsSuppressed = True
                ClearPage()
                vv = Math.Max(.Version.Major, 5)
            End With
        End If
        Return vv >= 11
    End Function

    ''' <summary>
    ''' 清理网页上的全部内容
    ''' </summary>
    Public Sub ClearPage()
        Static p As String = Nothing
        If p Is Nothing Then
            Dim files = Directory.GetFiles(AppContext.BaseDirectory, "IEMediaPlayerBase.html", SearchOption.AllDirectories)
            If files.Length > 0 Then
                p = ConvertPathToFileURL(files(0))
            Else
                Throw New FileNotFoundException("I cannot find IEMediaPlayerBase.html")
            End If
        End If
        ie.Navigate(p)
    End Sub

    ''' <summary>
    ''' 尽量不要使用，直接获取内部的webbrowser实例
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IEinside As WebBrowser
        Get
            Return ie
        End Get
    End Property

#Region "shared"

    ''' <summary>
    ''' 获取当前系统的 webbrowser 控件表现版本的注册表键的路径字符串，是HKEY_LOCAL_MACHINE/之后的部分
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetIEModeRegistryKeyPath(is64bit As Boolean) As String
        Static p32 = "SOFTWARE\"
        Static p64 = "SOFTWARE\"
        If p32.Length < 20 Then
            p64 += "Wow6432Node\"
            Dim p = "Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"
            p64 += p
            p32 += p
        End If
        If is64bit Then
            Return p64
        End If
        Return p32
    End Function

    ''' <summary>
    ''' 检测当前进程的WebBrowser是否已经是IE11模式
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function IsIE11Mode() As Boolean
        Dim name = My.Application.Info.AssemblyName + ".exe"
        Using k = Registry.LocalMachine.OpenSubKey(GetIEModeRegistryKeyPath(Environment.Is64BitProcess))
            If k Is Nothing Then Return False
            Dim strs = k.GetValueNames()
            Dim s As Object = k.GetValue(name, 18)
            If s Is Nothing Then Return False
            Return s.Equals(11001)
        End Using
    End Function

    ''' <summary>
    ''' 获取REG文件
    ''' </summary>
    ''' <param name="head"></param>
    ''' <param name="v"></param>
    ''' <returns></returns>
    Public Shared Function GetRegFileContent(head As Boolean, v As String) As String
        Dim name = My.Application.Info.AssemblyName + ".exe"
        Dim sb As New StringBuilder
        If head Then
            sb.AppendLine("Windows Registry Editor Version 5.00")
        End If
        Dim f1 = $"{QuoteStr(name)}={v}"
        sb.AppendLine($"[HKEY_LOCAL_MACHINE\{GetIEModeRegistryKeyPath(False)}]")
        sb.AppendLine(f1)
        sb.AppendLine($"[HKEY_LOCAL_MACHINE\{GetIEModeRegistryKeyPath(True)}]")
        sb.AppendLine(f1)
        Return sb.ToString
    End Function

    ''' <summary>
    ''' 试着把当前进程设置为IE11模式，以管理员权限运行这个reg文件，用户会得到一个弹窗，询问是否注册。
    ''' </summary>
    Public Shared Function SetIE11Mode() As Task
        Return RunRegEdit(GetRegFileContent(False, "dword:00002af9"))
    End Function

    ''' <summary>
    ''' 试着把当前进程的webbrowser的模式设置清除，以管理员权限运行这个reg文件，用户会得到一个弹窗，询问是否注册。
    ''' </summary>
    Public Shared Function ClearIEMode() As Task
        Return RunRegEdit(GetRegFileContent(False, "-"))
    End Function

#End Region

#Region "Media"
    ''' <summary>
    ''' 设置视频或音频的内容，可以初始化音量，音量是 0-1之间的一个浮点数，不要在loadie之后立马使用这个，可能需要等1秒
    ''' </summary>
    Public Sub SetContent(src As String, controls As Boolean, autoplay As Boolean, volume As Single, IsLoop As Boolean)
        src = ConvertPathToFileURL(src)
        ie.Document.InvokeScript("SetContent", {src, controls, autoplay, volume, IsLoop})
    End Sub

    ''' <summary>
    ''' 设置背景颜色，指的是比如视频两侧的或者是音频页面的背景色
    ''' </summary>
    Public Sub SetBackColor(c As Color)
        Dim s As String = $"rgb({c.R},{c.G},{c.B})"
        ie.Document.InvokeScript("SetBackColor", {s})
    End Sub

    ''' <summary>
    ''' 设置音量，是 0-1之间的一个浮点数
    ''' </summary>
    Public Sub SetVolume(v As Single)
        If v < 0 OrElse v > 1 Then
            Throw New ArgumentOutOfRangeException(paramName:="v")
        End If
        ie.Document.InvokeScript("SetVolume", {v})
    End Sub

    ''' <summary>
    ''' 返回是否已经暂停播放 
    ''' </summary>
    Public Function IsPaused() As Boolean
        Dim obj = ie.Document.InvokeScript("IsPaused")
        If obj Is Nothing Then
            Throw New Exception("cant read ply.paused from webbrowser.")
        End If
        Return obj
    End Function

    ''' <summary>
    ''' 继续播放或暂停，如果已经播放完成了，且没有loop ，那继续播放是不会回到开头重新播放的
    ''' </summary>
    Public Sub PlayOrPause(b As Boolean)
        ie.Document.InvokeScript("PlayOrPause", {b})
    End Sub

    ''' <summary>
    ''' 修改播放速度，最小不能比0.1小，最快不能比5.0快
    ''' </summary>
    Public Sub SetPlayRate(v As Single)
        If v < 0.1 OrElse v > 5 Then
            Throw New ArgumentOutOfRangeException(paramName:="v")
        End If
        ie.Document.InvokeScript("SetPlayRate", {v})
    End Sub

    ''' <summary>
    ''' 设置是否要循环播放
    ''' </summary>
    Public Sub SetLoop(b As Boolean)
        ie.Document.InvokeScript("Setloop", {b})
    End Sub

    ''' <summary>
    ''' 设置播放的位置，单位是秒
    ''' </summary>
    Public Sub SetTimePos(v As Single)
        If v < 0 Then
            Throw New ArgumentOutOfRangeException(paramName:="v")
        End If
        ie.Document.InvokeScript("SetTimePos", {v})
    End Sub

    ''' <summary>
    ''' 获取播放的当前位置，单位是秒
    ''' </summary>
    ''' <returns></returns>
    Public Function GetTimePos() As Single
        Dim obj = ie.Document.InvokeScript("GetTimePos")
        If obj Is Nothing Then
            Throw New Exception("cant read ply.currentTime from webbrowser.")
        End If
        Return obj
    End Function

    ''' <summary>
    ''' 获取当前媒体的总时间长度，单位是秒
    ''' </summary>
    ''' <returns></returns>
    Public Function GetMediaTimeLength() As Single
        Dim obj = ie.Document.InvokeScript("GetMediaTimeLength")
        If obj Is Nothing OrElse Single.IsNaN(obj) OrElse Double.IsNaN(obj) Then
            Throw New Exception("cant read ply.duration from webbrowser.")
        End If
        Return obj
    End Function

    ''' <summary>
    ''' 设置字幕文件，vtt格式的
    ''' </summary>
    Public Sub SetSubtitleSrc(src As String)
        src = ConvertPathToFileURL(src)
        ie.Document.InvokeScript("SetSubtitleSrc", {src})
    End Sub

#End Region

End Class

