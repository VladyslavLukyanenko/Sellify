using System;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.Core.Orders;
using ProjectIndustries.Sellify.Core.Orders.Services;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Infra.Repositories;

namespace ProjectIndustries.Sellify.Infra.Orders.Services
{
  public class EfOrderRepository : EfSoftRemovableCrudRepository<Order, Guid>, IOrderRepository
  {
    public EfOrderRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
  }
}