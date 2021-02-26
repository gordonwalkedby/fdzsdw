''' <summary>
''' 一个有渐变色的进度条
''' </summary>
Public Class ColorfulProgressBar
    Inherits Control

    Private _timer As New System.Windows.Forms.Timer

    Public Sub New()
        DoubleBuffered = True
        _timer.Enabled = False
        _timer.Interval = 1000 / 40
        AddHandler _timer.Tick, Sub()
                                    Dim v = _paintValue + _paintDropSpeed
                                    If _paintDropSpeed > 0 Then
                                        _paintValue = Math.Min(v, _value)
                                    Else
                                        _paintValue = Math.Max(v, _value)
                                    End If
                                    Invalidate()
                                    If _paintValue = _value Then
                                        _timer.Enabled = False
                                    End If
                                End Sub
    End Sub

    Public Property Min As Single = 0

    Public Property Max As Single = 100

    Private _paintDropSpeed As Single = 1
    Private _paintValue As Single = 0
    Private _value As Single = 0
    Public Property Value As Single
        Get
            Return _value
        End Get
        Set(value As Single)
            value = Math.Min(Max, Math.Max(value, Min))
            If Not _value.Equals(value) Then
                _value = value
                _paintValue = value
                Invalidate()
            End If
        End Set
    End Property

    ''' <summary>
    ''' 设置值，但不是立马让进度条显示到那个值，会产生一个动画，0.5秒的情况下到达那个值，BUG是频繁呼叫会导致卡住
    ''' </summary>
    Public Sub SetValueWithAnimation(value As Single)
        _timer.Enabled = False
        value = Math.Min(Max, Math.Max(value, Min))
        If Not _value.Equals(value) Then
            Dim dis = value - _value
            _paintDropSpeed = dis / (1000 / _timer.Interval) / 0.5
            _timer.Enabled = True
            _paintValue = _value
            _value = value
        End If
    End Sub

    ''' <summary>
    ''' 渐变色在这里调整
    ''' </summary>
    ''' <returns></returns>
    <ComponentModel.Browsable(False)>
    Public Property GradientColor As New GradientColor

    Public Property BorderColor As Color = Color.BlueViolet
    Public Property BorderWidth As Integer = 2

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If Min < Max AndAlso GradientColor.Colors.Count > 0 Then
            Dim g = e.Graphics
            Dim pos As Single = _paintValue / (Max - Min)
            Using bs = New SolidBrush(GradientColor.GetColor(pos))
                g.FillRectangle(bs, New Rectangle(0, 0, pos * Width, Height))
            End Using
            If BorderWidth > 0 Then
                Using pp = New Pen(BorderColor, BorderWidth)
                    Dim rs As Single = BorderWidth / 2
                    ' g.DrawRectangle(pp, ClientRectangle)
                    g.DrawRectangle(pp, rs, rs, Width - rs * 2, Height - rs * 2)
                End Using
            End If
        End If
        MyBase.OnPaint(e)
    End Sub

End Class
