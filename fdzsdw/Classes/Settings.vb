Public Class Settings

    Public Shared xs As New XmlSerializer(GetType(Settings))
    Public Shared mainfile As New FileInfo(Path.Combine(GetCurrentProgramFile.DirectoryName, "XML", "settings.xml"))

    Public Sub New()
    End Sub

    Private _VoiceVolume As Integer = 100
    Public Property VoiceVolume As Integer
        Get
            Return _VoiceVolume
        End Get
        Set(value As Integer)
            _VoiceVolume = Math.Min(Math.Max(value, 0), 100)
        End Set
    End Property

    Private _BGMVolume As Integer = 100
    Public Property BGMVolume As Integer
        Get
            Return _BGMVolume
        End Get
        Set(value As Integer)
            _BGMVolume = Math.Min(Math.Max(value, 0), 100)
        End Set
    End Property

    Public Property SkipCG As Boolean = False

    Public Shared Function LoadFromLocal() As Settings
        mainfile.Refresh()
        If mainfile.Exists Then
            Dim s As Settings = Nothing
            Using ff = mainfile.OpenRead
                s = xs.Deserialize(ff)
            End Using
            Return s
        Else
            Return New Settings
        End If
    End Function

    Public Sub SaveToLocal()
        Using ff = mainfile.Create
            xs.Serialize(ff, Me)
        End Using
    End Sub

End Class
