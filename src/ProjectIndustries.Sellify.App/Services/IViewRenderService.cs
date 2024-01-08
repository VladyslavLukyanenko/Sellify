using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.App.Services
{
  public interface IViewRenderService
  {
    Task<string> RenderAsync(string viewName, object model);
  }
}