using System;
using Renci.SshNet;
using WinSCP;
using Session = WinSCP.Session;

public class UploadHelper
{
    public static void UploadBuild(string path)
    {
        var sessionOptions = new SessionOptions
        {
            Protocol = Protocol.Sftp,
            HostName = Credentials.Host,
            UserName = Credentials.Username,
            Password = Credentials.Password,
            SshHostKeyPolicy = SshHostKeyPolicy.GiveUpSecurityAndAcceptAny,
        };

        using var session = new Session();

        session.Open(sessionOptions);
        Console.WriteLine("Connected to {0}", Credentials.Host);

        var res = session.PutFilesToDirectory(
            path,
            Credentials.WorkingDirectory);

        Console.WriteLine($"Uploaded {res.Transfers.Count} files {res.IsSuccess}");
        Console.WriteLine(new string('#', 50));
    }

    public static void RestartApp()
    {
        using var client = new SshClient(Credentials.Host,
            Credentials.Port,
            Credentials.Username,
            Credentials.Password);

        client.Connect();
        Console.WriteLine("Connected to {0}", Credentials.Host);

        var killScreen = client.CreateCommand(Credentials.KillScreen);
        var restart = client.CreateCommand(Credentials.RestartScreen);

        killScreen.Execute();

        Console.WriteLine($"Executed Kill Screen with result: {killScreen.Result}");

        restart.Execute();

        Console.WriteLine($"Executed Restart with result: {restart.Result}");
    }
}