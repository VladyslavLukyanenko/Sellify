using System;
using System.Collections;
using System.Collections.Generic;

namespace ProjectIndustries.Sellify.Core.Collections
{
  public interface IPagedList<T>
  {
    bool IsFirst { get; }

    bool IsLast { get; }

    int PageIndex { get; }

    int Limit { get; }

    int Count { get; }

    int TotalElements { get; }

    int TotalPages { get; }

    bool IsEmpty { get; }

    IEnumerable ToEnumerable();
    
    
    IReadOnlyCollection<T> Content { get; }

    IPagedList<TOther> CopyWith<TOther>(IEnumerable<TOther> content);
    IPagedList<TOther> ProjectTo<TOther>(Func<T, TOther> projection);
  }
}