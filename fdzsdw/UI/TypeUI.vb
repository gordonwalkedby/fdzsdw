''' <summary>
''' 打字UI
''' </summary>
Public Class TypeUI
    Inherits Control

    Private WithEvents _timer As New System.Windows.Forms.Timer
    Private _gColor As New GradientColor
    Private totalinputLength As Integer = 0
    Private lastTypeTimeRecord As Integer = 0
    Private totalPassedTime As Integer = 0

    Public Sub New()
        DoubleBuffered = True
        Me.Font = New Font("Consolas", 12)
        _timer.Enabled = False
        _timer.Interval = 1000 / 40
        AddHandler _timer.Tick, AddressOf TimerTick
        _gColor.SetColor(0, Color.Red)
        _gColor.SetColor(1, Color.Green)
        _gColor.SetColor(0.5, Color.Blue)
    End Sub

    Private Sub TimerTick()
        If Me.Disposing OrElse Me.IsDisposed Then Exit Sub
        Dim v = PassedMS + _timer.Interval
        If v >= TimeLimit Then
            PassedMS = TimeLimit
            IsOver = True
        Else
            PassedMS = v
        End If
        totalPassedTime += _timer.Interval
        Me.Invalidate()
    End Sub

    ''' <summary>
    ''' 输入一个字符
    ''' </summary>
    Public Sub InputKey(c As Char)
        Dim a = Asc(c)
        If (a < 97 AndAlso a <> 8) OrElse a > 122 Then
            Exit Sub
        End If
        If TimeLimit < 1 OrElse PassedMS > TimeLimit Then Exit Sub
        If IsOnError Then
            If c = vbBack Then
                IsOnError = False
            End If
        Else
            If c = vbBack Then
                Exit Sub
            End If
            Dim pp = InputIndex + 1
            If pp >= Text.Length Then
                Exit Sub
            End If
            Dim good As String = Text.Chars(pp)
            If good.Equals(c, StringComparison.CurrentCultureIgnoreCase) OrElse cheatMode.Equals("fast") Then
                InputIndex = pp
                totalinputLength += 1
                lastTypeTimeRecord = totalPassedTime
                If InputIndex = Text.Length - 1 Then
                    IsPass = True
                Else
                    If Text.Chars(pp + 1) = " " Then
                        InputIndex += 1
                    End If
                End If
            Else
                IsOnError = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' 获取玩家每秒能打的字数
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property TypeSpeed As Single
        Get
            If lastTypeTimeRecord < 1 Then Return 0
            Return totalinputLength / lastTypeTimeRecord * 1000
        End Get
    End Property

    ''' <summary>
    ''' 过去的毫秒数
    ''' </summary>
    ''' <returns></returns>
    <ComponentModel.Browsable(False)>
    Public Property PassedMS As Integer = 0

    ''' <summary>
    ''' 时间限制，毫秒
    ''' </summary>
    <ComponentModel.Browsable(False)>
    Public Property TimeLimit As Integer = 0

    ''' <summary>
    ''' 输入到第几个字了，还没开始输入就是第-1个字
    ''' </summary>
    <ComponentModel.Browsable(False)>
    Public Property InputIndex As Integer = -1

    ''' <summary>
    ''' 当前字符输入错误
    ''' </summary>
    <ComponentModel.Browsable(False)>
    Public Property IsOnError As Boolean

    Private _isover As Boolean = False
    ''' <summary>
    ''' 当前的任务是否已经结束，包括超时或者成功
    ''' </summary>
    Public Property IsOver As Boolean
        Get
            Return _isover
        End Get
        Set(value As Boolean)
            If value AndAlso _isover.Equals(value) = False Then
                _isover = True
                RaiseEvent TypeUnitOver(Me, Nothing)
            End If
        End Set
    End Property

    Private _ispass As Boolean = False
    ''' <summary>
    ''' 当前的任务是否成功
    ''' </summary>
    Public Property IsPass As Boolean
        Get
            Return _ispass
        End Get
        Set(value As Boolean)
            If value AndAlso _ispass.Equals(value) = False Then
                _ispass = True
                IsOver = True
            End If
        End Set
    End Property

    ''' <summary>
    ''' 暂停游戏
    ''' </summary>
    Public Sub Pause(stops As Boolean)
        If stops Then
            _timer.Enabled = False
        Else
            _timer.Enabled = True
        End If
    End Sub

    Public Event TypeUnitOver As EventHandler

    Private _CurrentTypeUnit As TypeInputUnit
    ''' <summary>
    ''' 当前输入任务，一旦set这个值，计时器就会开始工作
    ''' </summary>
    <ComponentModel.Browsable(False)>
    Public Property CurrentTypeUnit As TypeInputUnit
        Get
            Return _CurrentTypeUnit
        End Get
        Set(value As TypeInputUnit)
            _ispass = False
            _isover = False
            _timer.Stop()
            PassedMS = 0
            IsOnError = False
            InputIndex = -1
            _CurrentTypeUnit = value
            If value Is Nothing Then
                Text = ""
                Invalidate()
            Else
                TimeLimit = value.TimeLimit * 1000
                If cheatMode.Equals("slow") Then
                    TimeLimit = 1000 * 60 * 10
                End If
                _timer.Start()
                Text = value.Text
            End If
        End Set
    End Property

    Public ReadOnly UntypedCharBrush As New SolidBrush(Color.White)
    Public ReadOnly TypedCharBrush As New SolidBrush(Color.FromArgb(60, 255, 78))
    Public ReadOnly BadCharBrush As New SolidBrush(Color.FromArgb(255, 60, 70))
    Public ReadOnly UntypedCharPen As New Pen(UntypedCharBrush, 2)
    Public ReadOnly TypedCharPen As New Pen(TypedCharBrush, 2)
    Public ReadOnly BadCharPen As New Pen(BadCharBrush, 2)

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Static fontSize As SizeF = SizeF.Empty
        Dim len = Text.Length
        If len > 0 Then
            Dim g = e.Graphics
            Dim linew = Me.Width * 0.9
            Dim paddleft = Me.Width * 0.05
            Dim rightside = Me.Width * 0.95
            Dim y As Integer = 5
            Dim h As Integer = 18
            Dim c1 = Color.Green
            Dim c2 = Color.Red
            Dim pos = If(TimeLimit > 0, PassedMS / TimeLimit, 0)
            g.FillRectangle(UntypedCharBrush, New Rectangle(paddleft, y, linew, y + h))
            If pos > 0 Then
                Using b1 = New SolidBrush(_gColor.GetColor(1 - pos))
                    g.FillRectangle(b1, New Rectangle(paddleft, y, linew * (1 - pos), y + h))
                End Using
            End If
            y += h + 3
            If fontSize.IsEmpty Then
                fontSize = g.MeasureString("A", Font)
            End If
            Dim bs = TypedCharBrush
            Dim x As Single = paddleft
            Dim fontbottom As Integer = y + fontSize.Height
            Dim fontwidth As Integer = fontSize.Width * 0.8
            Dim typedchars As Integer = 0
            Dim untypedchars As Integer = 0
            Dim needshowError As Boolean = False
            For i As Integer = 0 To len - 1
                If i > InputIndex Then
                    untypedchars += 1
                    bs = UntypedCharBrush
                    If i = InputIndex + 1 Then
                        If IsOnError Then
                            bs = BadCharBrush
                            needshowError = True
                        End If
                    End If
                Else
                    typedchars += 1
                End If
                Dim c = Text.Chars(i)
                g.DrawString(c, Font, bs, New PointF(x, y))
                x += fontwidth
                If x > rightside OrElse i = len - 1 Then
                    Dim linestart As Integer = paddleft
                    If InputIndex >= 0 Then
                        linestart = paddleft + fontwidth * typedchars
                        g.DrawLine(TypedCharPen, New Point(paddleft, fontbottom), New Point(linestart, fontbottom))
                    End If
                    If needshowError Then
                        g.DrawLine(BadCharPen, New Point(linestart, fontbottom), New Point(linestart + fontwidth, fontbottom))
                        linestart += fontwidth
                        untypedchars -= 1
                    End If
                    If untypedchars > 0 Then
                        g.DrawLine(UntypedCharPen, New Point(linestart, fontbottom), New Point(linestart + fontwidth * untypedchars, fontbottom))
                    End If
                    typedchars = 0
                    untypedchars = 0
                    x = paddleft
                    Dim addy As Integer = fontSize.Height * 1.2
                    y += addy
                    fontbottom += addy
                    needshowError = False
                End If
            Next
        End If
        MyBase.OnPaint(e)
    End Sub

End Class
