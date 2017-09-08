Imports System.IO
Imports System.IO.Ports
Imports System.Text

Public Class Form1
    Dim ArduinoSerialPort As New SerialPort
    Dim COMSetupComplete As Boolean = False
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If COMSetupComplete = False Then
            Try
                With ArduinoSerialPort
                    .PortName = "COM" & NumericUpDown2.Value
                    .BaudRate = 38400
                    .Parity = Parity.None
                    .DataBits = 8
                    .StopBits = 1
                    .DtrEnable = True
                    .ReadTimeout = 30000
                    .Open()
                    Threading.Thread.Sleep(1000)
                End With
                COMSetupComplete = True
            Catch ex As Exception
                COMSetupComplete = False
            End Try
        End If
        If COMSetupComplete = True Then
            GetFollowersAndSend()
            Timer1.Interval = 60000
            Timer1.Start()
        Else
            MsgBox("Another program is already using COM" & NumericUpDown2.Value & "." & vbCrLf & vbCrLf &
                     "Please try again later", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "COM" & NumericUpDown2.Value & " Not Available")
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        GetFollowersAndSend()
    End Sub
    Private Sub GetFollowersAndSend()
        Dim myWebRequest As Net.WebRequest = Net.WebRequest.Create("https://moisescardona.me/steem/getFollowersCount/?a=" & TextBox1.Text)
        Dim myWebResponse As Net.WebResponse = myWebRequest.GetResponse()
        Dim ReceiveStream As Stream = myWebResponse.GetResponseStream()
        Dim encode As Encoding = System.Text.Encoding.GetEncoding("utf-8")
        Dim readStream As New StreamReader(ReceiveStream, encode)
        If ArduinoSerialPort.IsOpen = False Then ArduinoSerialPort.Open()
        ArduinoSerialPort.Write(TextBox1.Text & ":|" & readStream.ReadLine & " followers")
    End Sub
End Class
