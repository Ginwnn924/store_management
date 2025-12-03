using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using static BlazorApp.Components.Pages.Home;

namespace BlazorApp.Components.Shared;

public partial class Sidebar
{
    [Inject]
    private IProductService ProductService { get; set; } = default!;

    [Parameter]
    public List<CategoryResponse>? Categories { get; set; }

    [Parameter]
    public FilterState? Filter { get; set; }

    [Parameter]
    public EventCallback<FilterState> OnFilterChanged { get; set; }

    // Local filter state for binding
    private string SearchText { get; set; } = "";
    private HashSet<int> SelectedCategories { get; set; } = [];
    private decimal? MinPrice { get; set; }
    private decimal? MaxPrice { get; set; }

    protected override void OnParametersSet()
    {
        // Sync from parent filter
        if (Filter != null)
        {
            SearchText = Filter.Search ?? "";
            SelectedCategories = [.. Filter.SelectedCategoryIds];
            MinPrice = Filter.MinPrice;
            MaxPrice = Filter.MaxPrice;
        }
    }
    [Parameter]
    public EventCallback<PagedResponse<ProductResponse>?> OnProductsLoaded { get; set; }


    private void ToggleCategory(int categoryId)
    {
        if (!SelectedCategories.Add(categoryId))
        {
            SelectedCategories.Remove(categoryId);
        }
    }

    private bool IsCategorySelected(int categoryId) => SelectedCategories.Contains(categoryId);

    private void SetPriceRange(decimal? min, decimal? max)
    {
        MinPrice = min;
        MaxPrice = max;
    }

    private async Task ApplyFilter()
    {
        Console.WriteLine("Applying filter:");

        // Gọi FilterProductsAsync
        var result = await ProductService.FilterProductsAsync(
            pageNumber: Filter?.CurrentPage ?? 1,
            pageSize: Filter?.PageSize ?? 10,
            productName: string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
            categoryIds: SelectedCategories.Count > 0 ? [.. SelectedCategories] : null,
            minPrice: MinPrice,
            maxPrice: MaxPrice
        );
        if (OnProductsLoaded.HasDelegate)
        {
            await OnProductsLoaded.InvokeAsync(result);
        }
        var newFilter = new FilterState
        {
            Search = string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
            SelectedCategoryIds = [.. SelectedCategories],
            MinPrice = MinPrice,
            MaxPrice = MaxPrice,
            CurrentPage = 1
        };
        await OnFilterChanged.InvokeAsync(newFilter);
    }

    private async Task ClearFilter()
    {
        Console.WriteLine("Clearing filter:");
        SearchText = "";
        SelectedCategories.Clear();
        MinPrice = null;
        MaxPrice = null;
        await ApplyFilter();
    }
}

