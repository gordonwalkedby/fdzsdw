Module LevelManager

    Public Property SkipToStart As String = ""
    Public Property cheatMode As String = ""
    Public Property skipCG As Boolean = False

    Public Sub LoadArgs()
        Dim args = Environment.GetCommandLineArgs
        If args.Length > 1 Then
            For i As Integer = 1 To args.Length - 1
                Dim v As String = args(i)
                Dim nv As String = If(i + 1 < args.Length, args(i + 1), "")
                Select Case v
                    Case "-cheat"
                        If nv.Length > 0 Then
                            cheatMode = nv.ToLower
                        End If
                    Case "-skip"
                        If nv.Length > 0 Then
                            SkipToStart = nv
                            i += 1
                        End If
                    Case "-df"
                        If nv.Length > 0 Then
                            Dim num = -1
                            If Integer.TryParse(nv, num) Then
                                If num > 0 Then
                                    NowDifficulty = num
                                    i += 1
                                End If
                            End If
                        End If
                    Case "-hp"
                        If nv.Length > 0 Then
                            Dim num = -1
                            If Integer.TryParse(nv, num) Then
                                StayingPower = num
                                i += 1
                            End If
                        End If
                    Case "-novid"
                        skipCG = True
                End Select
            Next
        End If
    End Sub

    ''' <summary>
    ''' 难度
    ''' </summary>
    Public Enum Difficulty
        Easy
        Middle
        Hard
    End Enum

    ''' <summary>
    ''' 所有的关卡
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Units As New List(Of TypeInputUnit)

    ''' <summary>
    ''' 当前的难度
    ''' </summary>
    ''' <returns></returns>
    Public Property NowDifficulty As Difficulty = Difficulty.Easy

    Private _stayingpower As Byte = 3

    Public Event StayingPowerChanged As EventHandler

    Public ReadOnly Property MaxStayingPower As Byte = 10

    ''' <summary>
    ''' 玩家的耐力
    ''' </summary>
    ''' <returns></returns>
    Public Property StayingPower As Byte
        Get
            Return _stayingpower
        End Get
        Set(value As Byte)
            value = Math.Min(Math.Max(value, 0), MaxStayingPower)
            If value.Equals(_stayingpower) = False Then
                _stayingpower = value
                RaiseEvent StayingPowerChanged(Nothing, Nothing)
            End If
        End Set
    End Property

    ''' <summary>
    ''' 获取一个关卡的时间限制
    ''' </summary>
    ''' <returns></returns>
    Public Function GetTimeLimit(len As Integer, df As Difficulty) As Single
        If len < 1 Then
            Throw New ArgumentException("len can't be smaller than 1.")
        End If
        Dim sp As Single = 0 '中等难度下，一秒至少打的字母数量
        Select Case df
            Case Difficulty.Easy
                sp = 2
            Case Difficulty.Middle
                sp = 4
            Case Difficulty.Hard
                sp = 6
        End Select
        If sp > 0 Then
            Return len / sp
        End If
        Throw New ArgumentException("bad Difficulty value.")
    End Function

    ''' <summary>
    ''' 从本地文件 XML/TypeUnits.xml 读取 units
    ''' </summary>
    Public Sub LoadTypeUnitsFromLocal()
        Units.Clear()
        Dim xs As New XmlSerializer(GetType(TypeInputUnit()))
        Dim s = File.OpenRead(Path.Combine(GetCurrentProgramFile.DirectoryName, "XML", "TypeUnits.xml"))
        Dim array As TypeInputUnit() = xs.Deserialize(s)
        Dim usedID As New List(Of String)
        Dim textlen As Integer = 0
        Dim start As Boolean = SkipToStart.Length < 1
        For Each i In array
            If i.Id.Length > 0 AndAlso usedID.Contains(i.Id) = False Then
                If start = False AndAlso i.Id.Equals(SkipToStart, StringComparison.CurrentCultureIgnoreCase) Then
                    start = True
                End If
                If start = False Then
                    Continue For
                End If
                If File.Exists(i.AudioFileName) Then
                    usedID.Add(i.Id)
                    Units.Add(i)
                    textlen += i.Text.Length
                Else
                    Throw New FileNotFoundException($"音频文件不存在： {i.AudioFileName}")
                End If
            Else
                Throw New Exception($"LoadTypeUnitsFromLocal ID已经存在： {i.Id}")
            End If
        Next
        If Units.Count < 1 Then
            Throw New Exception("没有一个unit被加载！")
        End If
        Debug.WriteLine($"读取XML完成，总字数：{textlen} 预计总时长： {GetTimeLimit(textlen, Difficulty.Middle) / 60} 分钟")
        s.Close()
    End Sub

    ''' <summary>
    ''' 从本地文件 XML/sub_xx.xml 读取 字幕文件
    ''' </summary>
    Public Sub LoadUnitSubtitlesFromLocal(lang As String)
        Units.Clear()
        Dim xs As New XmlSerializer(GetType(UnitSubtitle()))
        Dim s = File.OpenRead(Path.Combine(GetCurrentProgramFile.DirectoryName, "XML", $"sub_{lang}.xml"))
        Dim array As UnitSubtitle() = xs.Deserialize(s)
        For Each i In array
            If i.Id.Length > 0 AndAlso i.Subtitle.Length > 0 Then
                For Each u In Units
                    If u.Id.Equals(i.Id, StringComparison.CurrentCultureIgnoreCase) Then
                        u.Subtitle = i.Subtitle
                        Exit For
                    End If
                Next
            End If
        Next
        s.Close()
    End Sub

    Public Sub GenDefaultSubtitleFile()
        Dim li As New List(Of UnitSubtitle)
        For Each u In Units
            Dim t As New UnitSubtitle With {
                .Id = u.Id, .Subtitle = u.Subtitle}
            li.Add(t)
        Next
        Dim xs As New XmlSerializer(GetType(UnitSubtitle()))
        Dim s = File.Create(Path.Combine(GetCurrentProgramFile.DirectoryName, "XML", $"sub_zh-cn.xml"))
        xs.Serialize(s, li.ToArray)
        s.Close()
    End Sub

End Module
