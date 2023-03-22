namespace BotModule.Extensions.ImageManipulation.Core;

public class Image
{

	public Image(string fileName, string extension, string folder)
	{
		FileName = fileName;
		Extension = extension;
		Folder = folder;
	}
	
	public string FileName { get; set; }
	public string Extension { get; set; }
	public string Folder { get; set; }

	public bool IsAnimated => Extension == "gif";

	public string SourcePath => Path.Combine(Folder, (FileName + "." + Extension));

	public string TargetPath => Path.Combine(Folder, (FileName + "0." + Extension));
}