using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.FileStorage.Config;

namespace ProjectIndustries.Sellify.Core.FileStorage.FileSystem
{
  // ReSharper disable once InconsistentNaming
  public static class IFileUploadServiceExtensions
  {
    public static ValueTask<string> UploadFileOrDefaultAsync(this IFileUploadService fileUploadService,
      IBinaryData? binaryData, FileUploadsConfig cfg, CancellationToken ct = default) =>
      UploadFileOrDefaultAsync(fileUploadService, binaryData, cfg, null, ct);

    public static async ValueTask<string> UploadFileOrDefaultAsync(this IFileUploadService fileUploadService,
      IBinaryData? binaryData, FileUploadsConfig cfg, string? defaultValue, CancellationToken ct = default)
    {
      if (binaryData == null || binaryData.Length == 0)
      {
        return defaultValue!;
      }

      var oldFileName = string.IsNullOrEmpty(defaultValue) ? null : Path.GetFileName(defaultValue);
      var result = await fileUploadService.StoreAsync(binaryData, new[] {cfg}, oldFileName, ct);
      return result.StoreFileResult.RelativeFilePath;
    }
  }
}