Public Class SettingsForm

    Public Sub LoadSettings(s As Settings)
        BarVoiceVolume.Value = s.VoiceVolume
        BarBGMVolume.Value = s.BGMVolume
        CheckSkipCG.Checked = s.SkipCG
    End Sub

    Private Sub BarBGMVolume_Scroll(sender As Object, e As EventArgs) Handles BarBGMVolume.ValueChanged
        LabBGMVv.Text = BarBGMVolume.Value.ToString
    End Sub

    Private Sub BarVoiceVolume_Scroll(sender As Object, e As EventArgs) Handles BarVoiceVolume.ValueChanged
        LabVoiceVv.Text = BarVoiceVolume.Value.ToString
    End Sub

    Private Sub ButOK_Click(sender As Object, e As EventArgs) Handles ButOK.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Public Function OutputSettings() As Settings
        Dim n As New Settings
        With n
            .BGMVolume = BarBGMVolume.Value
            .VoiceVolume = BarVoiceVolume.Value
            .SkipCG = CheckSkipCG.Checked
        End With
        Return n
    End Function

End Class