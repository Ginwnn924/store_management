using Microsoft.AspNetCore.Components;
using BlazorApp.Models;

namespace BlazorApp.Components.Shared;

/// <summary>
/// Logic layer: Chỉ chứa Parameters
/// UI logic (format, animation) nằm trong file .razor
/// </summary>
public partial class ProductCard
{
    [Parameter]
    public ProductResponse? Product { get; set; }

    [Parameter]
    public double AnimationDelay { get; set; }
}
