Imports System.ComponentModel
Imports NAudio.Wave

Public Class MainForm

    Private ReadOnly GameTitle As String = "服毒自杀的我"

    Const FormWidth As Integer = 1280

    Private isHelpPage As Boolean = False
    Private isGaming As Boolean = False
    Private isPause As Boolean = False

    Private ReadOnly tui As New TypeUI
    Private ReadOnly powerBar As New ColorfulProgressBar
    Private ReadOnly powerLabel As New Label
    Private ReadOnly subLabel As New Label
    Private ReadOnly pauseLabel As New Label
    Private ReadOnly FakeClock As New Label

    Private ReadOnly audioPly As New WaveOutEvent
    Private audioReader As AudioFileReader = Nothing
    Private ReadOnly gameSW As New Stopwatch
    Private ReadOnly ieply As New IEMediaPlayer

    Private sets As Settings = Nothing

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Icon = My.Resources._001
        LoadArgs()
        Me.DoubleBuffered = True
        Pn.Controls.Add(ieply)
        ieply.Visible = False
        ieply.LoadIE()
        Me.Width = FormWidth
        TopMenu.Font = Me.Font
        Dim less = FormWidth / 16 * 9 - Pn.Height
        Me.Height += less
        Pn.Height += less
        Pn.BackColor = Me.BackColor
        Me.BackColor = TopMenu.BackColor
        SkipCGButton.Visible = False
        ShowBuyPage()
        LoadTypeUnitsFromLocal()
        GenDefaultSubtitleFile()
        LoadSets(Settings.LoadFromLocal)
    End Sub

    Private Sub LoadSets(s As Settings)
        audioPly.Volume = s.VoiceVolume / 100
        If IEplayTimer.Enabled Then
            ieply.SetVolume(s.BGMVolume / 100)
        End If
        If s.SkipCG = True Then
            skipCG = True
        End If
        Me.sets = s
    End Sub

    Private Sub ShowBuyPage()
        Dim pic As New PictureBox
        Pn.Controls.Add(pic)
        With pic
            .Dock = DockStyle.Fill
            .Image = My.Resources.shop
            .SizeMode = PictureBoxSizeMode.Normal
        End With
        Dim but As New Button
        pic.Controls.Add(but)
        With but
            .Text = "立即购买"
            .Width = 164
            .Height = 42
            .Left = 531
            .Top = 608
            .BringToFront()
            AddHandler .Click, Sub()
                                   pic.Dispose()
                                   If skipCG Then
                                       ShowHelpPage()
                                   Else
                                       PlayIntro()
                                   End If
                               End Sub
        End With
    End Sub

    Private Sub PlayIntro()
        Dim mp4 As New FileInfo(Path.Combine(GetCurrentProgramFile.DirectoryName, "media", "intro.mp4"))
        If mp4.Exists = False Then
            Throw New FileNotFoundException($"文件不存在： {mp4.FullName}")
        End If
        SkipCGButton.Visible = True
        If ieply.LoadIE = False Then
            MsgBox("对不起，你系统中没有 IE 11 或更高版本，无法播放视频。此处帮您直接跳过。", MsgBoxStyle.Critical, "服毒自杀的我")
            SkipCGButton_Click(Nothing, Nothing)
            Exit Sub
        End If
        ieply.Dock = DockStyle.Fill
        ieply.Visible = True
        ieply.SetContent(mp4.FullName, False, True, sets.BGMVolume / 100, False)
        IEplayTimer.Interval = 48400
        IEplayTimer.Enabled = True
    End Sub

    Private Sub IEplayTimer_Tick(sender As Object, e As EventArgs) Handles IEplayTimer.Tick
        SkipCGButton_Click(Nothing, Nothing)
    End Sub

    Private Sub ShowHelpPage()
        isHelpPage = True
        Pn.BackgroundImage = My.Resources.help
    End Sub

    Private Sub LoadGame()
        Pn.BackgroundImageLayout = ImageLayout.Stretch
        Pn.Controls.Add(tui)
        With tui
            .Dock = DockStyle.Bottom
            .Height = 90
        End With
        Pn.Controls.Add(powerLabel)
        With powerLabel
            .Left = 10
            .Top = 10
            .Text = $"死亡欲望：{StayingPower.ToString.PadLeft(2, "0")}"
            .Width = 10
            .Height = 10
            .AutoSize = True
        End With
        Pn.Controls.Add(powerBar)
        With powerBar
            .BackColor = Color.White
            .Left = powerLabel.Right + 10
            .Top = powerLabel.Top
            .Width = Pn.Width - .Left * 2
            .Height = powerLabel.Height
            .BorderColor = Color.White
            .Min = 0
            .Max = MaxStayingPower
            .Value = StayingPower
            .GradientColor.SetColor(0, Color.FromArgb(72, 34, 72))
            .GradientColor.SetColor(0.5, Color.DarkGreen)
            .GradientColor.SetColor(1, Color.YellowGreen)
        End With
        Pn.Controls.Add(subLabel)
        With subLabel
            .Left = 100
            .AutoSize = True
            .Font = New Font(Me.Font.Name, 20)
            .Text = ""
            .Top = tui.Top - .Font.Height - 10
        End With
        Pn.Controls.Add(pauseLabel)
        With pauseLabel
            .AutoSize = True
            .Text = "暂停！"
            .Font = New Font(Me.Font.Name, 60)
            .Left = (FormWidth - .Width) / 2
            .Top = powerBar.Bottom + 30
            .Visible = False
        End With
        Pn.Controls.Add(FakeClock)
        With FakeClock
            .Left = powerLabel.Left
            .Top = powerLabel.Bottom + 4
            .Font = New Font(Me.Font.Name, 25)
            .AutoSize = True
            .Text = "12:00"
            .BorderStyle = BorderStyle.FixedSingle
        End With
        FakeClockTimer.Enabled = True
        gameSW.Start()
        TopMenu.Visible = False
        isGaming = True
        Me.Text = GameTitle + " （按ESC可暂停或继续游戏）"
        AddHandler LevelManager.StayingPowerChanged, Sub()
                                                         powerBar.SetValueWithAnimation(StayingPower)
                                                         powerLabel.Text = $"死亡欲望：{StayingPower.ToString.PadLeft(2, "0")}"
                                                     End Sub
        AddHandler tui.TypeUnitOver, AddressOf MoveToNextLevel
        MoveToNextLevel()
    End Sub

    Private Sub MoveToNextLevel()
        If audioPly.PlaybackState = PlaybackState.Playing Then
            audioPly.Stop()
        End If
        If audioReader IsNot Nothing Then
            audioReader.Close()
            audioReader = Nothing
        End If
        Dim last = tui.CurrentTypeUnit
        If last IsNot Nothing Then
            If tui.IsPass = False Then
                StayingPower -= 1
                If StayingPower = 0 Then
                    GameOver()
                    Exit Sub
                End If
            End If
        End If
        If Units.Count > 0 Then
            Dim t = Units.Item(0)
            Select Case t.Id
                Case "01_01"
                    Pn.BackgroundImage = My.Resources.l1
                Case "02_01"
                    StayingPower += 8
                    Pn.BackgroundImage = My.Resources.l2
                Case "02_18"
                    NowDifficulty = Difficulty.Middle
                Case "03_Marker 01"
                    StayingPower += 1
                    Pn.BackgroundImage = My.Resources.l3
                Case "04_Marker 02"
                    StayingPower += 1
                    Pn.BackgroundImage = My.Resources.l4
                Case "05_Marker 01"
                    StayingPower += 1
                    Pn.BackgroundImage = My.Resources.l3
                Case "06_Marker 01"
                    StayingPower += 2
                    Pn.BackgroundImage = My.Resources.l5
                    NowDifficulty = Difficulty.Hard
                Case "06_Marker 30"
                    StayingPower += 1
                    Pn.BackgroundImage = My.Resources.l6
            End Select
            tui.CurrentTypeUnit = t
            audioReader = New AudioFileReader(t.AudioFileName)
            audioPly.Init(audioReader)
            audioPly.Play()
            Units.RemoveAt(0)
            subLabel.Visible = True
            subLabel.Text = t.Subtitle
            subLabel.Left = (FormWidth - subLabel.Width) / 2
        Else
            GameOver()
            Exit Sub
        End If
    End Sub

    Private Sub MainForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If isHelpPage Then
            If e.KeyCode = Keys.Enter Then
                Pn.BackgroundImage = Nothing
                isHelpPage = False
                LoadGame()
            End If
        ElseIf IEplayTimer.Enabled = True Then
            If e.KeyCode = Keys.Escape Then
                SkipCGButton_Click(Nothing, Nothing)
            End If
        ElseIf isGaming Then
            If e.KeyCode = Keys.Escape Then
                isPause = Not isPause
                tui.Pause(isPause)
                TopMenu.Visible = isPause
                pauseLabel.Visible = isPause
                If isPause Then
                    gameSW.Stop()
                Else
                    gameSW.Start()
                End If
                If audioPly.PlaybackState <> PlaybackState.Stopped Then
                    If isPause Then
                        audioPly.Pause()
                    Else
                        audioPly.Play()
                    End If
                End If
                Pn.Focus()
            Else
                If isPause = False Then
                    Dim index As Integer = e.KeyCode
                    If index < 128 Then
                        tui.InputKey(Char.ToLower(Chr(index)))
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub GameOver()
        Me.Text = GameTitle
        FakeClockTimer.Enabled = False
        tui.CurrentTypeUnit = Nothing
        subLabel.Visible = False
        tui.Visible = False
        SaveGameLog()
        gameSW.Stop()
        TopMenu.Visible = True
        isPause = False
        isGaming = False
        If StayingPower > 0 Then
            Pn.BackgroundImage = My.Resources.end2
        Else
            Pn.BackgroundImage = My.Resources.end1
        End If
    End Sub

    Private Sub SaveGameLog()
        If gameSW.IsRunning = False Then Exit Sub
        Dim mins = gameSW.Elapsed.TotalMinutes
        If mins < 5 / 60 Then
            Exit Sub
        End If
        Dim logFile As New FileInfo(Path.Combine(GetCurrentProgramFile.DirectoryName, "ends.txt"))
        Dim sb As New StringBuilder
        sb.AppendLine("======================")
        sb.AppendLine($"日期：{Now:yyyy/MM/dd HH:mm:ss}")
        sb.AppendLine($"坚持：{Math.Round(mins, 2)} 分钟")
        sb.AppendLine($"最终欲望：{StayingPower} ")
        sb.AppendLine($"打字速度：{Math.Round(tui.TypeSpeed, 1)} 字/秒 ")
        sb.AppendLine($"参数：{Command()}")
        File.AppendAllText(logFile.FullName, sb.ToString, New UTF8Encoding(False))
    End Sub

    Private Sub SkipCGButton_Click(sender As Object, e As EventArgs) Handles SkipCGButton.Click
        If Not SkipCGButton.Visible Then
            Exit Sub
        End If
        SkipCGButton.Visible = False
        IEplayTimer.Enabled = False
        ieply.Dispose()
        ShowHelpPage()
    End Sub

    Private Sub FakeClockTimer_Tick(sender As Object, e As EventArgs) Handles FakeClockTimer.Tick
        Static tm As New Date(2020, 6, 22, 12, 0, 0)
        If isPause = False AndAlso isGaming Then
            tm = tm.AddSeconds(6)
            FakeClock.Text = tm.ToString("HH:mm")
        End If
    End Sub

    Private Sub SettingsButton_Click(sender As Object, e As EventArgs) Handles SettingsButton.Click
        Using f = New SettingsForm
            With f
                .LoadSettings(sets)
                .Icon = Me.Icon
                Dim r = .ShowDialog()
                If r = DialogResult.OK Then
                    LoadSets(f.OutputSettings)
                    sets.SaveToLocal()
                End If
            End With
        End Using
    End Sub

    Private Sub AboutButton_Click(sender As Object, e As EventArgs) Handles AboutButton.Click
        Using f = New AboutForm
            With f
                .Icon = Me.Icon
                .ShowDialog()
            End With
        End Using
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveGameLog()
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case &H112 'WM_SYSCOMMAND
                If m.WParam.ToInt32() = &HF100 Then '玩家不小心按下ALT，阻止事件继续发生
                    Exit Sub
                End If
        End Select
        MyBase.WndProc(m)
    End Sub

End Class
