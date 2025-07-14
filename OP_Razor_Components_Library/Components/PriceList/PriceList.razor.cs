using Microsoft.AspNetCore.Components;
using OP_Shared_Library;

namespace OP_Razor_Components_Library.Components.PriceList;

public partial class PriceList
{
    [Inject]
    private HttpClient Http { get; set; }
    [Inject]
    private NavigationManager Navigation { get; set; }
    [Inject]
    private TranslationsService Translations { get; set; }

    protected List<PricelistItem> PricelistItems { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        await Translations.LoadAsync();

        var filePath = $"_content/OP_Razor_Components_Library/data/pricelist.yaml";

        var url = $"{Navigation.BaseUri}{filePath}";
        using var stream = await Http.GetStreamAsync(url);
        PricelistItems = Serializing.LoadYamlList<PricelistItem>(url,stream);
    }

    // This is serialization for *.json
    //    private async Task LoadPricelistData()
    //    {
    //        try
    //        {
    //            // https://https://localhost:44321/
    //            var baseUri = Navigation.BaseUri;

    //            // https://https://localhost:44321/_content/OP_Razor_Components_Library/data/pricelist.json
    //            var url = $"{baseUri}_content/OP_Razor_Components_Library/data/pricelist.json";

    //            PricelistItems = await Http.GetFromJsonAsync<List<PricelistItem>>(url)
    //                            ?? new List<PricelistItem>();
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Error loading JSON: {ex.Message}");
    //        }
    //    }
}

public class PricelistItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Price { get; set; }
}