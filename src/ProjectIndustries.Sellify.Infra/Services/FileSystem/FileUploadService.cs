using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.Services;
using ProjectIndustries.Sellify.Core.FileStorage.Config;
using ProjectIndustries.Sellify.Core.FileStorage.FileSystem;
using ProjectIndustries.Sellify.Infra.Services.Cryptographic;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem
{
  public class FileUploadService : IFileUploadService
  {
    private readonly ICryptographicService _cryptographicService;
    private readonly IFileSystemService _fileSystemService;
    private readonly IPathsService _pathsService;
    private readonly IBinaryDataProcessingPipelineProvider _processingPipelineProvider;

    public FileUploadService(IPathsService pathsService,
      IBinaryDataProcessingPipelineProvider processingPipelineProvider,
      ICryptographicService cryptographicService, IFileSystemService fileSystemService)
    {
      _pathsService = pathsService;
      _processingPipelineProvider = processingPipelineProvider;
      _cryptographicService = cryptographicService;
      _fileSystemService = fileSystemService;
    }

    public Task<StoredBinaryData> StoreAsync(IBinaryData data, IEnumerable<FileUploadsConfig> configs,
      string? defaultValue, CancellationToken ct = default)
    {
      return StoreAsync(data, configs, new RandomFileNameProvider(defaultValue), ct);
    }

    public async Task<StoredBinaryData> StoreAsync(IBinaryData data, IEnumerable<FileUploadsConfig> configs,
      IFileNameProvider fileNameProvider, CancellationToken ct = default)
    {
      var config = configs.FirstOrDefault(cfg => cfg.IsFileTypeSupported(data.ContentType));
      if (config == null)
      {
        throw new NotSupportedException("NotSupportedUploadFileType");
      }

      await TryRemoveFileAsync(config, fileNameProvider);

      var storeRoot = _pathsService.GetStoreAbsolutePath(config.StoreName);

      var pipeline = _processingPipelineProvider.GetPipeline(config, data);
      if (pipeline != null)
      {
        data = await pipeline.ProcessAsync(config, data, ct);
      }

      using (var fileStream = data.OpenReadStream())
      {
        var computedHash = config.AppendHashToFileName
          ? await _cryptographicService.ComputeHashAsync(fileStream)
          : string.Empty;

        var destFileName = fileNameProvider.GetDstFileName(config, data, computedHash);
        var storeFullPath =
          await _fileSystemService.SaveBinaryAsync(storeRoot, destFileName, fileStream, ct);
        var relativePath = _pathsService.ToServerRelative(storeFullPath);
        var extension = data.GetExtension();

        var storeResult = new StoreFileResult(storeFullPath, relativePath, computedHash, destFileName,
          extension);
        return new StoredBinaryData(data, storeResult);
      }
    }

    private async Task TryRemoveFileAsync(FileUploadsConfig config, IFileNameProvider fileNameProvider)
    {
      if (string.IsNullOrEmpty(fileNameProvider.OldFileName))
      {
        return;
      }

      var fullPath = _pathsService.GetStoreAbsolutePath(config.StoreName, fileNameProvider.OldFileName);
      if (File.Exists(fullPath))
      {
        await Task.Run(() => File.Delete(fullPath));
      }
    }
  }
}