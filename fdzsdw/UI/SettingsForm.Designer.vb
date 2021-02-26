<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SettingsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.LabVoiceVolume = New System.Windows.Forms.Label()
        Me.BarVoiceVolume = New System.Windows.Forms.TrackBar()
        Me.LabVoiceVv = New System.Windows.Forms.Label()
        Me.LabBGMVolume = New System.Windows.Forms.Label()
        Me.BarBGMVolume = New System.Windows.Forms.TrackBar()
        Me.LabBGMVv = New System.Windows.Forms.Label()
        Me.ButOK = New System.Windows.Forms.Button()
        Me.CheckSkipCG = New System.Windows.Forms.CheckBox()
        CType(Me.BarVoiceVolume, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarBGMVolume, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabVoiceVolume
        '
        Me.LabVoiceVolume.AutoSize = True
        Me.LabVoiceVolume.Location = New System.Drawing.Point(12, 20)
        Me.LabVoiceVolume.Name = "LabVoiceVolume"
        Me.LabVoiceVolume.Size = New System.Drawing.Size(84, 20)
        Me.LabVoiceVolume.TabIndex = 0
        Me.LabVoiceVolume.Text = "语音音量："
        '
        'BarVoiceVolume
        '
        Me.BarVoiceVolume.Location = New System.Drawing.Point(87, 43)
        Me.BarVoiceVolume.Maximum = 100
        Me.BarVoiceVolume.Name = "BarVoiceVolume"
        Me.BarVoiceVolume.Size = New System.Drawing.Size(251, 45)
        Me.BarVoiceVolume.TabIndex = 2
        Me.BarVoiceVolume.TickFrequency = 10
        Me.BarVoiceVolume.Value = 100
        '
        'LabVoiceVv
        '
        Me.LabVoiceVv.AutoSize = True
        Me.LabVoiceVv.Location = New System.Drawing.Point(302, 20)
        Me.LabVoiceVv.Name = "LabVoiceVv"
        Me.LabVoiceVv.Size = New System.Drawing.Size(36, 20)
        Me.LabVoiceVv.TabIndex = 3
        Me.LabVoiceVv.Text = "100"
        '
        'LabBGMVolume
        '
        Me.LabBGMVolume.AutoSize = True
        Me.LabBGMVolume.Location = New System.Drawing.Point(12, 91)
        Me.LabBGMVolume.Name = "LabBGMVolume"
        Me.LabBGMVolume.Size = New System.Drawing.Size(114, 20)
        Me.LabBGMVolume.TabIndex = 4
        Me.LabBGMVolume.Text = "背景音乐音量："
        '
        'BarBGMVolume
        '
        Me.BarBGMVolume.Location = New System.Drawing.Point(87, 127)
        Me.BarBGMVolume.Maximum = 100
        Me.BarBGMVolume.Name = "BarBGMVolume"
        Me.BarBGMVolume.Size = New System.Drawing.Size(251, 45)
        Me.BarBGMVolume.TabIndex = 5
        Me.BarBGMVolume.TickFrequency = 10
        Me.BarBGMVolume.Value = 100
        '
        'LabBGMVv
        '
        Me.LabBGMVv.AutoSize = True
        Me.LabBGMVv.Location = New System.Drawing.Point(302, 91)
        Me.LabBGMVv.Name = "LabBGMVv"
        Me.LabBGMVv.Size = New System.Drawing.Size(36, 20)
        Me.LabBGMVv.TabIndex = 6
        Me.LabBGMVv.Text = "100"
        '
        'ButOK
        '
        Me.ButOK.BackColor = System.Drawing.SystemColors.Control
        Me.ButOK.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ButOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButOK.ForeColor = System.Drawing.Color.Black
        Me.ButOK.Location = New System.Drawing.Point(0, 518)
        Me.ButOK.Name = "ButOK"
        Me.ButOK.Size = New System.Drawing.Size(390, 45)
        Me.ButOK.TabIndex = 7
        Me.ButOK.Text = "点我确认，点右上角取消"
        Me.ButOK.UseVisualStyleBackColor = False
        '
        'CheckSkipCG
        '
        Me.CheckSkipCG.AutoSize = True
        Me.CheckSkipCG.Location = New System.Drawing.Point(18, 178)
        Me.CheckSkipCG.Name = "CheckSkipCG"
        Me.CheckSkipCG.Size = New System.Drawing.Size(109, 24)
        Me.CheckSkipCG.TabIndex = 8
        Me.CheckSkipCG.Text = "总是跳过CG"
        Me.CheckSkipCG.UseVisualStyleBackColor = True
        '
        'SettingsForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(390, 563)
        Me.Controls.Add(Me.CheckSkipCG)
        Me.Controls.Add(Me.ButOK)
        Me.Controls.Add(Me.LabBGMVv)
        Me.Controls.Add(Me.BarBGMVolume)
        Me.Controls.Add(Me.LabBGMVolume)
        Me.Controls.Add(Me.LabVoiceVv)
        Me.Controls.Add(Me.BarVoiceVolume)
        Me.Controls.Add(Me.LabVoiceVolume)
        Me.Font = New System.Drawing.Font("Microsoft YaHei", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SettingsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "设置菜单"
        CType(Me.BarVoiceVolume, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarBGMVolume, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabVoiceVolume As Label
    Friend WithEvents BarVoiceVolume As TrackBar
    Friend WithEvents LabVoiceVv As Label
    Friend WithEvents LabBGMVolume As Label
    Friend WithEvents BarBGMVolume As TrackBar
    Friend WithEvents LabBGMVv As Label
    Friend WithEvents ButOK As Button
    Friend WithEvents CheckSkipCG As CheckBox
End Class
