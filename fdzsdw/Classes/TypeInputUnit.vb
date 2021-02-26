''' <summary>
''' 一句话就是一个 TypeInputUnit
''' </summary>
Public Class TypeInputUnit

    Private Shared ReadOnly r As New Random()

    Private _txtlen As Integer
    Private _txt As String

    Public Sub New()
    End Sub

    Public Sub New(id As String, text As String)
        Me.Id = id
        Me.Text = text
    End Sub

    ''' <summary>
    ''' 唯一ID
    ''' </summary>
    Public Property Id As String

    ''' <summary>
    ''' 字幕
    ''' </summary>
    Public Property Subtitle As String

    ''' <summary>
    ''' 打字内容，原始内容，不是字幕
    ''' </summary>
    Public Property Text As String
        Get
            Dim value = _txt
            If value.Contains(" ") Then
                Dim strs = value.Split(" ")
                Dim sb As New StringBuilder
                Dim addv As Integer = 0
                For Each i As String In strs
                    i = i.Trim.ToLower
                    If i.Length > 0 Then
                        If sb.Length > 0 Then
                            sb.Append(" "c)
                        End If
                        Dim v As Integer = Val(i)
                        If v > 0 Then
                            addv += v
                            For k As Integer = 1 To v
                                sb.Append(Chr(r.Next(97, 122)))
                            Next
                        Else
                            If NowDifficulty = Difficulty.Hard Then
                                If r.NextDouble <= 0.1 Then
                                    i = StrReverse(i)
                                End If
                            End If
                            sb.Append(i)
                        End If
                    End If
                Next
                _txtlen += addv * 1.5
                value = sb.ToString
            End If
            Return value
        End Get
        Set(value As String)
            If value Is Nothing OrElse value.Length < 1 OrElse value.Trim.Length < 1 Then
                Throw New Exception("value cant be null!")
            End If
            _txtlen = value.Length
            _txt = value
        End Set
    End Property

    ''' <summary>
    ''' 获取当前难度下的时间限制，单位：秒
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property TimeLimit As Single
        Get
            Dim s As String = Text + "d"
            Return GetTimeLimit(_txtlen, LevelManager.NowDifficulty)
        End Get
    End Property

    ''' <summary>
    ''' 获取对应的配音文件名
    ''' </summary>
    Public ReadOnly Property AudioFileName As String
        Get
            If Id.Length < 1 Then Return ""
            Static p As String = Path.Combine(GetCurrentProgramFile.DirectoryName, "audios", Id + ".mp3")
            Return p
        End Get
    End Property

End Class
