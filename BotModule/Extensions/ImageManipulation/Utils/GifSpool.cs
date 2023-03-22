using System.Net.Mime;
using BotModule.Extensions.ImageManipulation.Core;
using OpenCvSharp;

namespace BotModule.Extensions.ImageManipulation.Utils;

public class GifSpool
{
	public static void Unroll(Image image)
	{
		var gif = Cv2.CreateFrameSource_Video(image.SourcePath);
	}

	public static void Roll(Image image)
	{
		
	}
}