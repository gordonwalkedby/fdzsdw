<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TopMenu = New System.Windows.Forms.MenuStrip()
        Me.SettingsButton = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutButton = New System.Windows.Forms.ToolStripMenuItem()
        Me.SkipCGButton = New System.Windows.Forms.ToolStripMenuItem()
        Me.Pn = New System.Windows.Forms.Panel()
        Me.IEplayTimer = New System.Windows.Forms.Timer(Me.components)
        Me.FakeClockTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TopMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'TopMenu
        '
        Me.TopMenu.BackColor = System.Drawing.Color.Gray
        Me.TopMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingsButton, Me.AboutButton, Me.SkipCGButton})
        Me.TopMenu.Location = New System.Drawing.Point(0, 0)
        Me.TopMenu.Name = "TopMenu"
        Me.TopMenu.Size = New System.Drawing.Size(1264, 24)
        Me.TopMenu.TabIndex = 0
        Me.TopMenu.Text = "MenuStrip1"
        '
        'SettingsButton
        '
        Me.SettingsButton.Name = "SettingsButton"
        Me.SettingsButton.Size = New System.Drawing.Size(45, 20)
        Me.SettingsButton.Text = "设置"
        '
        'AboutButton
        '
        Me.AboutButton.Name = "AboutButton"
        Me.AboutButton.Size = New System.Drawing.Size(45, 20)
        Me.AboutButton.Text = "关于"
        '
        'SkipCGButton
        '
        Me.SkipCGButton.Name = "SkipCGButton"
        Me.SkipCGButton.Size = New System.Drawing.Size(61, 20)
        Me.SkipCGButton.Text = "跳过CG"
        '
        'Pn
        '
        Me.Pn.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Pn.Location = New System.Drawing.Point(0, 24)
        Me.Pn.Name = "Pn"
        Me.Pn.Size = New System.Drawing.Size(1264, 667)
        Me.Pn.TabIndex = 1
        '
        'IEplayTimer
        '
        Me.IEplayTimer.Interval = 1000
        '
        'FakeClockTimer
        '
        Me.FakeClockTimer.Interval = 1000
        '
        'MainForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(1264, 691)
        Me.Controls.Add(Me.Pn)
        Me.Controls.Add(Me.TopMenu)
        Me.Font = New System.Drawing.Font("Microsoft YaHei", 10.0!)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.TopMenu
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "服毒自杀的我"
        Me.TopMenu.ResumeLayout(False)
        Me.TopMenu.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TopMenu As MenuStrip
    Friend WithEvents SettingsButton As ToolStripMenuItem
    Friend WithEvents AboutButton As ToolStripMenuItem
    Friend WithEvents Pn As Panel
    Friend WithEvents IEplayTimer As Windows.Forms.Timer
    Friend WithEvents SkipCGButton As ToolStripMenuItem
    Friend WithEvents FakeClockTimer As Windows.Forms.Timer
End Class
