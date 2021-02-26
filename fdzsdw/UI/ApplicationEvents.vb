Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
            If IEMediaPlayer.IsIE11Mode = False Then
                Dim r = MsgBox("安装本游戏需要在您的注册表里登记一小条内容，表示我们要使用您的 IE 11 内核。
如果您同意，请按OK，并请在管理员权限下同意注册表注入。
游戏目录里有一个 clear.reg ，运行那个REG会清除我们之前的注册表内容。", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, "服毒自杀的我")
                If r = MsgBoxResult.Ok Then
                    IEMediaPlayer.SetIE11Mode.Wait()
                    RestartProgram()
                Else
                    Environment.Exit(0)
                End If
            End If
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MsgBox($"{e.Exception}", MsgBoxStyle.Critical, "出错，请截图给戈登走過去看")
            Environment.Exit(-1)
        End Sub

    End Class
End Namespace
