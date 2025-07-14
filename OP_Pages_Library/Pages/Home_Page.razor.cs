using Microsoft.AspNetCore.Components;
using OP_Shared_Library.Struct;

namespace OP_Pages_Library.Pages;

public partial class Home_Page
{
    #region Private Properties
    [Inject]
    private TranslationsService Translations { get; set; }
    #endregion

    #region Private Properties
    private bool isLoaded = false;
    #endregion

    protected List<ChangeTextItem> ChangeTextItemList { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        await Translations.LoadAsync();

        EditModeService.OnEditModeChanged += HandleEditModeChanged;

        isLoaded = true;
    }

    #region Handle
    private void HandleEditModeChanged()
    {
        // Redraw the component when the edit mode changes
        InvokeAsync(StateHasChanged);
    }
    #endregion
}