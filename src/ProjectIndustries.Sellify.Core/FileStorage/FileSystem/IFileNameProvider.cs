using ProjectIndustries.Sellify.Core.FileStorage.Config;

namespace ProjectIndustries.Sellify.Core.FileStorage.FileSystem
{
  public interface IFileNameProvider
  {
    string? OldFileName { get; }
    string GetDstFileName(FileUploadsConfig cfg, IBinaryData data, string? computedHash = null);
  }
}