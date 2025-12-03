using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components.Pages;

/// <summary>
/// Logic layer: Chỉ xử lý DI, call API và quản lý data
/// UI logic (format, animation, mapping) nằm trong file .razor
/// </summary>
public partial class Home
{
    [Inject]
    private IProductService ProductService { get; set; } = default!;

    [Inject]
    private ICategoryService CategoryService { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    // State
    private string? ErrorMessage { get; set; }

    // Data - null = loading state
    private List<ProductResponse>? Products { get; set; }
    private List<CategoryResponse>? Categories { get; set; }

    // Pagination & Filter state
    private FilterState Filter { get; } = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        ErrorMessage = null;
        Products = null; // null = loading

        try
        {
            var (productsResult, categoriesResult) = await FetchDataAsync();

            Products = productsResult?.Items ?? [];

            // Chỉ load categories lần đầu
            if (Categories == null)
            {
                Categories = categoriesResult?.Items ?? [];
            }

            if (productsResult != null)
            {
                Filter.TotalPages = productsResult.TotalPages;
                Filter.TotalItems = productsResult.TotalItems;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Không thể tải dữ liệu: {ex.Message}";
            Products = [];
        }
    }

    private async Task<(PagedResponse<ProductResponse>?, PagedResponse<CategoryResponse>?)> FetchDataAsync()
    {
        // TODO: Truyền filter params vào API khi backend hỗ trợ
        bool hasActiveFilter = !string.IsNullOrWhiteSpace(Filter.Search) ||
                              Filter.SelectedCategoryIds.Count > 0 ||
                              Filter.MinPrice.HasValue ||
                              Filter.MaxPrice.HasValue;
        //var productsTask = ProductService.GetProductsAsync(Filter.CurrentPage, Filter.PageSize);
        var productsTask = hasActiveFilter
            ? ProductService.FilterProductsAsync(
                pageNumber: Filter.CurrentPage,
                pageSize: Filter.PageSize,
                productName: Filter.Search,
                categoryIds: Filter.SelectedCategoryIds.Count > 0 ? Filter.SelectedCategoryIds : null,
                minPrice: Filter.MinPrice,
                maxPrice: Filter.MaxPrice)
            : ProductService.GetProductsAsync(Filter.CurrentPage, Filter.PageSize);
        var categoriesTask = Categories == null
            ? CategoryService.GetCategoriesAsync(1, 50)
            : Task.FromResult<PagedResponse<CategoryResponse>?>(null);

        await Task.WhenAll(productsTask, categoriesTask);

        return (await productsTask, await categoriesTask);
    }

    // ============== FILTER & PAGINATION METHODS ==============
    private async Task ApplyFilterAsync()
    {
        UpdateUrl();
        await LoadDataAsync();
    }

    private void UpdateUrl()
    {
        var queryParams = new List<string>();

        if (Filter.CurrentPage > 1)
            queryParams.Add($"page={Filter.CurrentPage}");

        if (!string.IsNullOrWhiteSpace(Filter.Search))
            queryParams.Add($"search={Uri.EscapeDataString(Filter.Search)}");

        if (Filter.SelectedCategoryIds.Count > 0)
            queryParams.Add($"categories={string.Join(",", Filter.SelectedCategoryIds)}");

        if (Filter.MinPrice.HasValue)
            queryParams.Add($"minPrice={Filter.MinPrice.Value}");

        if (Filter.MaxPrice.HasValue)
            queryParams.Add($"maxPrice={Filter.MaxPrice.Value}");

        var url = queryParams.Count > 0 ? $"/?{string.Join("&", queryParams)}" : "/";
        Navigation.NavigateTo(url, forceLoad: false, replace: true);
    }

    private async Task GoToPageAsync(int page)
    {
        if (page < 1 || page > Filter.TotalPages || page == Filter.CurrentPage) return;
        Filter.CurrentPage = page;
        await ApplyFilterAsync();
    }

    private async Task PreviousPageAsync() => await GoToPageAsync(Filter.CurrentPage - 1);
    private async Task NextPageAsync() => await GoToPageAsync(Filter.CurrentPage + 1);

    // Callback from Sidebar
    private async Task OnFilterChanged(FilterState newFilter)
    {
        Filter.Search = newFilter.Search;
        Filter.SelectedCategoryIds = newFilter.SelectedCategoryIds;
        Filter.MinPrice = newFilter.MinPrice;
        Filter.MaxPrice = newFilter.MaxPrice;
        Filter.CurrentPage = 1; // Reset về trang 1 khi filter thay đổi
        await ApplyFilterAsync();
    }

    // Filter state object
    public class FilterState
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; } = 9;
        public int TotalPages { get; set; } = 1;
        public int TotalItems { get; set; }

        public string? Search { get; set; }
        public List<int> SelectedCategoryIds { get; set; } = [];
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
