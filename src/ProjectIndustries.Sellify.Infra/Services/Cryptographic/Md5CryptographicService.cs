﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Infra.Services.Cryptographic
{
  public class Md5CryptographicService : ICryptographicService
  {
    public Task<string> ComputeHashAsync(Stream stream)
    {
      if (stream.CanSeek)
      {
        stream.Seek(0, SeekOrigin.Begin);
      }

      using (var md5 = MD5.Create())
      {
        var hash = md5.ComputeHash(stream);
        return Task.FromResult(BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant());
      }
    }
  }
}