using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectIndustries.Sellify.App.Customers.Model;
using ProjectIndustries.Sellify.App.Customers.Services;
using ProjectIndustries.Sellify.Core.Collections;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers;

namespace ProjectIndustries.Sellify.WebApi.Customers
{
  public class CustomersController : SecuredStoreBoundControllerBase
  {
    private readonly ICustomerProvider _customerProvider;
    public CustomersController(IServiceProvider provider, ICustomerProvider customerProvider)
      : base(provider)
    {
      _customerProvider = customerProvider;
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<IPagedList<CustomerRowData>>))]
    public async ValueTask<IActionResult> GetCustomerRowsPageAsync([FromQuery] CustomerRowPageRequest pageRequest,
      CancellationToken ct)
    {
      var page = await _customerProvider.GetCustomerRowsPageAsync(CurrentStoreId, pageRequest, ct);
      return Ok(page);
    }
  }
}