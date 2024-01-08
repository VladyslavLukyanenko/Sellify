using System.IO;
using ProjectIndustries.Sellify.Core.FileStorage.Image.ResizeStrategies;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem.Image
{
  public class OptimizedImageData
    : BaseBinaryData
  {
    public OptimizedImageData(string fileName, string contentType, ImageSize size, byte[] content)
    {
      Name = fileName;
      ContentType = contentType;
      Size = size;
      Content = content;
      Length = content.Length;
    }

    public ImageSize Size { get; }
    public byte[] Content { get; }

    public override Stream OpenReadStream()
    {
      return new MemoryStream(Content);
    }
  }
}