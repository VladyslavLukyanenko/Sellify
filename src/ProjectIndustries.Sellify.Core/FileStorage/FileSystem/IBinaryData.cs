using System.IO;

namespace ProjectIndustries.Sellify.Core.FileStorage.FileSystem
{
  public interface IBinaryData
  {
    string Name { get; }
    string ContentType { get; }
    long Length { get; }
    string GetNameWithoutExtension();
    string GetExtension();
    Stream OpenReadStream();
  }
}