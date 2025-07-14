using Microsoft.AspNetCore.Components;
using OP_Shared_Library;
using OP_Shared_Library.Struct;
using System.Text;

public class TranslationsService
{
    private readonly HttpClient _http;
    private readonly NavigationManager _navigation;

    private string _language = "cs";

    public List<ChangeTextItem> ChangeTextItemList { get; private set; } = new();

    public TranslationsService(HttpClient http, NavigationManager navigation)
    {
        _http = http;
        _navigation = navigation;
    }

    public async Task LoadAsync()
    {
        var file = $"{_language}.yaml";
        var filePath = $"/data/{file}";
        var url = $"{_navigation.BaseUri}{filePath}";
        using var stream = await _http.GetStreamAsync(url);

        ChangeTextItemList = Serializing.LoadYamlList<ChangeTextItem>(url, stream);
    }

    public string GetText(string name)
    {
        return ChangeTextItemList.FirstOrDefault(x => x.Id == name)?.Translation ?? $"[{name}]";
    }

    public async Task ChnageLanguageAsync(string language)
    {
        _language = language;
        await LoadAsync();
    }
    public async Task UpdateTranslationAsync(string id, string newText, string webRootPath)
    {
        // Find the translation item with the specified ID from the list
        var item = ChangeTextItemList.FirstOrDefault(x => x.Id == id);

        if (item != null)
        {
            // Update the translation text in list
            item.Translation = newText;

            // Serialize the updated list to YAML format
            var yaml = Serializing.SerializeYaml(ChangeTextItemList);

            // Build the full path to wwwroot/data/{language}.yaml
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", $"{_language}.yaml");

            // Ensure the target directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            // Save the YAML content to the file
            await File.WriteAllTextAsync(path, yaml, Encoding.UTF8);

            // Reload the updated list from disk to ensure it's in sync
            await LoadAsync();
        }
    }

}