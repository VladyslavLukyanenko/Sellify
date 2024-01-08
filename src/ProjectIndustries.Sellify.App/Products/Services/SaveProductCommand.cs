using ProjectIndustries.Sellify.App.Products.Model;
using ProjectIndustries.Sellify.Core.FileStorage.FileSystem;

namespace ProjectIndustries.Sellify.App.Products.Services
{
  public class SaveProductCommand : ProductData
  {
    public IBinaryData? UploadedPicture { get; set; }
  }
}