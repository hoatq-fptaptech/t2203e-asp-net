using System;
using System.Collections.Generic;

namespace T2203E_API.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product>? Products { get; set; } = new List<Product>();
}
