using System.Threading.Tasks;
using MassTransit;
using ProjectIndustries.Sellify.App.Identity.Domain;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Core.Stores;

namespace ProjectIndustries.Sellify.Infra.Stores.EventHandlers
{
  public class CreateStoreOnApplicationUserCreated : IConsumer<ApplicationUserCreated>
  {
    private readonly IStoreRepository _storeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateStoreOnApplicationUserCreated(IStoreRepository storeRepository, IUnitOfWork unitOfWork)
    {
      _storeRepository = storeRepository;
      _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<ApplicationUserCreated> context)
    {
      var store = new Store(context.Message.Id);
      var ct = context.CancellationToken;
      await _storeRepository.CreateAsync(store, ct);
      await _unitOfWork.SaveEntitiesAsync(ct);
    }
  }
}