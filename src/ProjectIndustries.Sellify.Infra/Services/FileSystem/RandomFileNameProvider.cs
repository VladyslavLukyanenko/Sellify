using System;
using ProjectIndustries.Sellify.Core.FileStorage.Config;
using ProjectIndustries.Sellify.Core.FileStorage.FileSystem;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem
{
  public class RandomFileNameProvider : FileNameProvider
  {
    public RandomFileNameProvider(string? oldFileName = null)
      : base(oldFileName)
    {
    }

    protected override string GetDstFileNameWithoutExt(FileUploadsConfig cfg, IBinaryData data)
    {
      if (!cfg.GenerateRandomFileName)
      {
        throw new InvalidOperationException("Only random name generation supported. "
                                            + $"You should provide another implementation of {nameof(FileNameProvider)}");
      }

      return Guid.NewGuid().ToString("N");
    }
  }
}