using Microsoft.AspNetCore.Components;
using Serilog;
using System.Threading.Tasks;

namespace OP_Pages_Library.Shared;
public partial class MainLayout
{
    #region Parameters
    [Parameter] public string MainTitle { get; set; } = "OP Template";
    #endregion

    private string currentLang = "cs"; // nebo si to vezmi z TranslationsService

    #region Ctor
    //protected override Task OnInitializedAsync()
    //{
    //    return base.OnInitializedAsync();
    //}
    #endregion

    #region Public
    public void OnlineStatusChanged(bool isOnline)
    {
        // Save state to Globals
        Globals.Instance.IsOnlineGlobal = isOnline;
    }
    public void OnLanguageChanged(string newLang)
    {
        currentLang = newLang;
        // zavolej svou metodu zde
        // napø. TranslationsService.SetLanguage(newLang);
        // NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true); // pøípadnì refresh
        Log.Information($"Language changed to {newLang}");
    }
    public void ToggleEditMode()
    {
        EditModeService.ToggleEditMode();
    }
    #endregion
}