using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectIndustries.Sellify.App.Products.Model
{
  public class CategoryGraphData : CategoryData
  {
    private readonly List<CategoryGraphData> _children = new();
    private CategoryGraphData? _parent;

    public string FullPath => _parent != null ? _parent.FullPath + " / " + Name : Name;

    public IEnumerable<CategoryGraphData> Children => _children.OrderBy(_ => _.Position).ThenBy(_ => _.Name);

    public void Add(CategoryGraphData child)
    {
      if (child == this)
      {
        throw new InvalidOperationException("Can't add self as child");
      }
      
      _children.Add(child);
      child._parent = this;
    }
  }
}