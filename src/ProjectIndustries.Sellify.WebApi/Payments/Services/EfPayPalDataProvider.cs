using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.App.Identity.Domain;
using ProjectIndustries.Sellify.App.Orders.Model;
using ProjectIndustries.Sellify.Core.Orders;
using ProjectIndustries.Sellify.Core.Products;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.Infra.Services;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services
{
  public class EfPayPalDataProvider : DataProvider, IPayPalDataProvider
  {
    private readonly IQueryable<Order> _aliveOrders;
    private readonly IQueryable<Product> _products;
    private readonly IQueryable<ApplicationUser> _users;
    private readonly IQueryable<Category> _categories;
    private readonly IQueryable<Store> _stores;

    public EfPayPalDataProvider(DbContext context) : base(context)
    {
      _aliveOrders = GetAliveDataSource<Order>();
      _products = GetDataSource<Product>();
      _users = GetDataSource<ApplicationUser>();
      _categories = GetDataSource<Category>();
      _stores = GetDataSource<Store>();
    }

    public async ValueTask<CreatePayPayOrderCommand?> GetCreatePayPayOrderCommandAsync(Guid orderId,
      CancellationToken ct = default)
    {
      var query = from o in _aliveOrders
        join s in _stores on o.StoreId equals s.Id
        join u in _users on s.OwnerId equals u.Id
        join p in _products on o.Product.Id equals p.Id
        join c in _categories on p.CategoryId equals c.Id
        where o.Id == orderId
        select new CreatePayPayOrderCommand
        {
          Quantity = (string) (object) o.Product.Quantity,
          PricePerItem = (string) (object) o.Product.Price,
          TotalPrice = (string) (object) (o.Product.Quantity * o.Product.Price),
          OrderId = (string) (object) o.Id,
          ProductTitle = o.Product.Title,
          ProductDesc = p.Excerpt,
          ProductSKU = p.SKU,
          CategoryName = c.Name,
          StoreTitle = s.Title!,
          PayeeEmail = s.PaymentGatewayConfigs.PayPal.Email!,
          PayerEmail = o.InvoiceEmail
        };

      return await query.FirstOrDefaultAsync(ct);
    }
  }
}