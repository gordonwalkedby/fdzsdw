<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutForm
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
        Me.LabInfo = New System.Windows.Forms.Label()
        Me.Pn = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        'LabInfo
        '
        Me.LabInfo.AutoSize = True
        Me.LabInfo.Location = New System.Drawing.Point(12, 9)
        Me.LabInfo.Name = "LabInfo"
        Me.LabInfo.Size = New System.Drawing.Size(57, 20)
        Me.LabInfo.TabIndex = 0
        Me.LabInfo.Text = "Label1"
        '
        'Pn
        '
        Me.Pn.Dock = System.Windows.Forms.DockStyle.Right
        Me.Pn.Location = New System.Drawing.Point(438, 0)
        Me.Pn.Name = "Pn"
        Me.Pn.Padding = New System.Windows.Forms.Padding(7)
        Me.Pn.Size = New System.Drawing.Size(198, 330)
        Me.Pn.TabIndex = 1
        '
        'AboutForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(636, 330)
        Me.Controls.Add(Me.Pn)
        Me.Controls.Add(Me.LabInfo)
        Me.Font = New System.Drawing.Font("Microsoft YaHei", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AboutForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "关于"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabInfo As Label
    Friend WithEvents Pn As Panel
End Class
