﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ProjectIndustries.Sellify.Infra;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjectIndustries.Sellify.WebApi.Foundation.SwaggerSupport.Swashbuckle
{
  public class FileUploadOperationFilter : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      var fileUploadOperations = context.MethodInfo.GetParameters()
        .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(FromFormAttribute)))
        .SelectMany(GetUploadFilePropNames)
        .ToArray();
      if (!fileUploadOperations.Any())
      {
        return;
      }

      var uploadFileMediaType = new OpenApiMediaType
      {
        Schema = new OpenApiSchema
        {
          Type = "object",
          Properties = fileUploadOperations.ToDictionary(name => name, name => new OpenApiSchema
          {
            Description = "Upload File",
            Type = "string",
            Format = "binary"
          }),
          Required = new HashSet<string>(fileUploadOperations)
        }
      };
      operation.RequestBody = new OpenApiRequestBody
      {
        Content =
        {
          ["multipart/form-data"] = uploadFileMediaType
        }
      };
    }

    private static string[] GetUploadFilePropNames(ParameterInfo parameterInfo)
    {
      return typeof(IFormFile).IsAssignableFrom(parameterInfo.ParameterType)
        ? new[] {parameterInfo.Name!}
        : parameterInfo.ParameterType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .Where(p => typeof(IFormFile).IsAssignableFrom(p.PropertyType))
          .Select(_ => _.Name.ToCamelCase())
          .ToArray();
    }
  }
}