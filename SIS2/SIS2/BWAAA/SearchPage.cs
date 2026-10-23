using Microsoft.Playwright;

namespace BigEcommerceApp.Tests.Models;

public class SearchPage
{
    //private readonly IPage _page;
    //private readonly ILocator _searchTermInput;

    //public SearchPage(IPage page)
    //{
    //    _page = page;
    //    _searchTermInput = page.GetByTestId("sb_form_q");
    //}

    //public async Task GotoAsync()
    //{
    //    await _page.GotoAsync("https://bing.com");
    //}

    //public async Task SearchAsync(string text)
    //{
    //    await _searchTermInput.FillAsync(text);
    //    await _searchTermInput.PressAsync("Enter");
    //}
    private readonly IPage _page;
    private readonly ILocator _searchTermInput;

    public SearchPage(IPage page)
    {
        _page = page;
        _searchTermInput = page.Locator("[aria-label='Enter your search term']");
    }

    public async Task GotoAsync()
    {
        await _page.GotoAsync("https://bing.com");
    }

    public async Task SearchAsync(string text)
    {
        await _searchTermInput.FillAsync(text);
        await _searchTermInput.PressAsync("Enter");
    }
}