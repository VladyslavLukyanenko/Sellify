using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectIndustries.Sellify.App.Orders.Model;
using ProjectIndustries.Sellify.App.Orders.Services;
using ProjectIndustries.Sellify.Core.Collections;
using ProjectIndustries.Sellify.Core.Orders;
using ProjectIndustries.Sellify.Core.Orders.Services;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers;

namespace ProjectIndustries.Sellify.WebApi.Orders
{
  public class OrdersController : SecuredStoreBoundControllerBase
  {
    private readonly IOrderProvider _orderProvider;
    private readonly IOrderManager _orderManager;
    private readonly IOrderRepository _orderRepository;

    public OrdersController(IServiceProvider provider, IOrderProvider orderProvider, IOrderManager orderManager,
      IOrderRepository orderRepository)
      : base(provider)
    {
      _orderProvider = orderProvider;
      _orderManager = orderManager;
      _orderRepository = orderRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<IPagedList<OrderRowData>>))]
    public async ValueTask<IActionResult> GetRowsPageAsync([FromQuery] OrderRowsPageRequest pageRequest,
      CancellationToken ct)
    {
      var page = await _orderProvider.GetRowsPageAsync(CurrentStoreId, pageRequest, ct);
      return Ok(page);
    }

    [HttpGet("Purchases")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<IPagedList<PurchasedProductData>>))]
    public async ValueTask<IActionResult> GetPurchasesPageAsync([FromQuery] CustomerPurchasesPageRequest pageRequest,
      CancellationToken ct)
    {
      var page = await _orderProvider.GetCustomerPurchasesPageAsync(CurrentStoreId, pageRequest, ct);
      return Ok(page);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<string>))]
    public async ValueTask<IActionResult> CreateOrderAsync([FromBody] CreateOrderCommand cmd,
      CancellationToken ct)
    {
      Result<Order?> createOrderResult = await _orderManager.CreateAsync(cmd, ct);
      if (createOrderResult.IsFailure)
      {
        return BadRequest(createOrderResult.Error.ToApiError());
      }

      var createdOrder = createOrderResult.Value;
      if (createdOrder == null)
      {
        return NotFound();
      }

      return Ok(createdOrder.Id);
    }

    [HttpPatch("{orderId:guid}/Status")]
    public async ValueTask<IActionResult> ChangeStatus(Guid orderId, OrderStatus status, CancellationToken ct)
    {
      Order? order = await _orderRepository.GetByIdAsync(orderId, ct);
      if (order == null)
      {
        return NotFound();
      }

      if (order.IsCompleted())
      {
        return BadRequest();
      }

      order.ManuallyChangeStatus(status);
      _orderRepository.Update(order);

      return NoContent();
    }

    [HttpPut("{orderId:guid}/Cancel")]
    public async ValueTask<IActionResult> CancelOrderAsync(Guid orderId, CancellationToken ct)
    {
      Order? order = await _orderRepository.GetByIdAsync(orderId, ct);
      if (order == null)
      {
        return NotFound();
      }

      order.Cancel();
      _orderRepository.Update(order);

      return NoContent();
    }
  }
}