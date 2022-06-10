using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Renci.SshNet;

public class UploadHelper
{
    public static void UploadBuild(string path)
    {
        using var client =
            new SftpClient(
                Credentials.Host,
                Credentials.Port,
                Credentials.Username,
                Credentials.Password);

        client.Connect();
        Console.WriteLine("Connected to {0}", Credentials.Host);

        client.ChangeDirectory(Credentials.WorkingDirectory);
        Console.WriteLine("Changed directory to {0}", Credentials.WorkingDirectory);

        // get all directory files
        var dir = client.ListDirectory(Credentials.WorkingDirectory).ToArray();
        var files = Directory.GetFiles(path);

        var toUpload = new List<string>();

        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            var match = dir.SingleOrDefault(x => x.Name == fileName);

            if (match != null)
            {
                var info = new FileInfo(file);
                if (DateTime.Compare(info.LastWriteTimeUtc, match.LastWriteTimeUtc) > 0) toUpload.Add(file);
                Console.WriteLine($"{fileName} was newer. Uploading.");
            }
            else
            {
                Console.WriteLine($"{fileName} does not yet exist on server. Uploading.");
                toUpload.Add(file);
            }
        }

        foreach (var file in toUpload)
        {
            using var fileStream = new FileStream(file, FileMode.Open);

            Console.WriteLine("Uploading {0} ({1:N0} bytes)", file, fileStream.Length);
            client.BufferSize = 4 * 1024; // bypass Payload error large files
            client.UploadFile(fileStream, Path.GetFileName(file));
        }

        Console.WriteLine($"Uploaded {toUpload.Count} files");
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