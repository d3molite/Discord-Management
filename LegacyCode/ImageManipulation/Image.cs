namespace DiscordApi.DiscordHost.Extensions.ImageManipulation
{
    public class Image
    {
        public string Filename { get; set; }

        public string Extension { get; set; }

        public string Folder { get; set; }

        public string SourcePath { get => Path.Combine(Folder, (Filename + "." + Extension)); }

        public string TargetPath { get => Path.Combine(Folder, (Filename + "0." + Extension)); }

    }
}
