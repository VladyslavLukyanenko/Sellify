using ProjectIndustries.Sellify.Core.Collections;

namespace ProjectIndustries.Sellify.App.Model
{
  public class FilteredPageRequest : PageRequest
  {
    private const int SearchTermMinLen = 3;
    public FilteredPageRequest()
    {
    }

    public FilteredPageRequest(FilteredPageRequest request)
      : base(request)
    {
      SearchTerm = request.SearchTerm;
    }

    public string? SearchTerm { get; set; }

    public string NormalizeSearchTerm()
    {
      return SearchTerm?.ToUpperInvariant() ?? string.Empty;
    }

    public bool IsSearchTermEmpty()
    {
      return string.IsNullOrWhiteSpace(SearchTerm) || SearchTerm.Trim().Length < SearchTermMinLen;
    }
  }
}