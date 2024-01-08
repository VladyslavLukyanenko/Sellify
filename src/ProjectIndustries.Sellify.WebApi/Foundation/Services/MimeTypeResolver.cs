﻿using Microsoft.AspNetCore.StaticFiles;
using MimeKit;
using ProjectIndustries.Sellify.Infra.Services.FileSystem;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Services
{
  public class MimeTypeResolver : IMimeTypeResolver
  {
    private readonly IContentTypeProvider _provider;

    public MimeTypeResolver(IContentTypeProvider provider)
    {
      _provider = provider;
    }

    public bool TryGetFileExtension(string mimeType, out string extension)
    {
      return MimeTypes.TryGetExtension(mimeType, out extension);
    }

    public bool TryGetMimeTypeByFilePath(string fullPath, out string contentType)
    {
      return _provider.TryGetContentType(fullPath, out contentType);
    }

    public bool IsImage(string contentType)
    {
      return !string.IsNullOrEmpty(contentType) && contentType.StartsWith("image/");
    }
  }
}