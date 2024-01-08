using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ProjectIndustries.Sellify.App.Products.Config;
using ProjectIndustries.Sellify.Core.FileStorage.FileSystem;
using ProjectIndustries.Sellify.Core.Products;

namespace ProjectIndustries.Sellify.App.Products.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _productRepository;
    private readonly IFileUploadService _fileUploadService;
    private readonly ProductConfig _config;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IFileUploadService fileUploadService,
      ProductConfig config, IMapper mapper)
    {
      _productRepository = productRepository;
      _fileUploadService = fileUploadService;
      _config = config;
      _mapper = mapper;
    }

    public async ValueTask<long> CreateAsync(Guid storeId, SaveProductCommand cmd, CancellationToken ct = default)
    {
      var pic = await _fileUploadService.UploadFileOrDefaultAsync(cmd.UploadedPicture, _config.PictureUpload, ct);
      var product = new Product(storeId, cmd.SKU, cmd.Title, cmd.Content, cmd.Excerpt, cmd.Type, cmd.Price,
        cmd.CategoryId, pic, cmd.Stock, cmd.Attributes);

      var created = await _productRepository.CreateAsync(product, ct);
      return created.Id;
    }

    public async ValueTask UpdateAsync(Product product, SaveProductCommand cmd, CancellationToken ct = default)
    {
      product.Picture = await _fileUploadService.UploadFileOrDefaultAsync(cmd.UploadedPicture, _config.PictureUpload,
        product.Picture, ct);
      product.ReplaceAttributes(cmd.Attributes);

      _mapper.Map(cmd, product);

      _productRepository.Update(product);
    }
  }
}