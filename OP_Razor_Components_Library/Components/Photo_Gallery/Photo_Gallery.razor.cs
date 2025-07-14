using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace OP_Razor_Components_Library.Components.Photo_Gallery;
public partial class Photo_Gallery
{
    #region Protected Properties
    protected List<GalleryImage> ImagePaths = new List<GalleryImage>();
    protected bool IsModalOpen = false;
    protected GalleryImage SelectedImage;
    protected double startX;
    #endregion

    #region Inject
    [Inject]
    private IJSRuntime JSRuntime { get; set; }
    [Inject]
    private TranslationsService Translations { get; set; }
    #endregion

    #region Ctor
    protected override void OnInitialized()
    {
        GetImagesFromFolderToList();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // Focus on modal content (for @onkeydown event)
        if (IsModalOpen)
        {
            await JSRuntime.InvokeVoidAsync("eval", "document.querySelector('.modal-content').focus();");
        }
    }
    protected override async Task OnInitializedAsync()
    {
        await Translations.LoadAsync();
    }
    #endregion

    #region Private Methods
    private void GetImagesFromFolderToList()
    {
        // Folder path
        var folderPath = "wwwroot/Images/Photo_Gallery/";

        if (System.IO.Directory.Exists(folderPath))
        {
            ImagePaths = System.IO.Directory.GetFiles(folderPath, "*.*")
                                  .Where(file => file.EndsWith(".jpg")
                                              || file.EndsWith(".png")
                                              || file.EndsWith(".jpeg")
                                              || file.EndsWith(".webp")
                                              || file.EndsWith(".JPG")
                                              || file.EndsWith(".PNG")
                                              || file.EndsWith(".JPEG")
                                              || file.EndsWith(".WEBP")
                                              )
                                  .Select(file => new GalleryImage
                                  {
                                      ImagePath = $"/Images/Photo_Gallery/{Path.GetFileName(file)}",
                                      ImageTitle = Path.GetFileNameWithoutExtension(file),
                                      ImageAlt = GetCommentFromMetadata(file)
                                  })
                                  .ToList();
        }
        else
        {
            Console.WriteLine($"Složka neexistuje: {folderPath}");
        }

    }
    private string GetCommentFromMetadata(string file)
    {
        string result = string.Empty;

        // Read metadata from image
        var directories = ImageMetadataReader.ReadMetadata(file);

        // Get comment from metadata
        var exifIfd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();

        // 0x9C9C = Windows XP Comment (Comment in Windows Folder View)
        if (exifIfd0Directory != null)
        {
            var comment = exifIfd0Directory.GetDescription(0x9C9C); // 0x9C9C = Windows XP Comment
            if (!string.IsNullOrEmpty(comment))
            {
                result = comment;
            }
        }

        return result;
    }
    #endregion

    #region Protected Methods
    // Open image in modal
    protected async Task OpenImage(GalleryImage galleryImage)
    {
        SelectedImage = galleryImage;
        IsModalOpen = true;

        await JSRuntime.InvokeVoidAsync("document.body.classList.add", "modal-open");
    }
    // Close modal
    protected async Task CloseModal()
    {
        IsModalOpen = false;

        await JSRuntime.InvokeVoidAsync("document.body.classList.remove", "modal-open");

    }
    // Handle keyboard events
    protected void Move(KeyboardEventArgs e)
    {
        if (e.Key == "ArrowRight" || e.Key == "ArrowLeft")
        {
            // Get the current index of the selected image
            int currentIndex = ImagePaths.IndexOf(SelectedImage);

            // Next Move
            if (e.Key == "ArrowRight")
            {
                MoveNext();
            }
            // Prev Move
            else if (e.Key == "ArrowLeft")
            {
                MovePrevious();
            }
        }
    }
    // Move to the next image
    private void MoveNext()
    {
        // Get the current index of the selected image
        int currentIndex = ImagePaths.IndexOf(SelectedImage);
        // Next Move
        int nextIndex = (currentIndex + 1) % ImagePaths.Count; // Loop to the start if at the end
        SelectedImage = ImagePaths[nextIndex];
    }
    // Move to the previous image
    private void MovePrevious()
    {
        // Get the current index of the selected image
        int currentIndex = ImagePaths.IndexOf(SelectedImage);
        // Prev Move
        int prevIndex = (currentIndex - 1 + ImagePaths.Count) % ImagePaths.Count; // Loop to the end if at the start
        SelectedImage = ImagePaths[prevIndex];
    }
    // Handle start touch event
    protected void HandleTouchStart(TouchEventArgs e)
    {
        // Save start position of touch on X axis
        startX = e.Touches[0].ClientX;
    }
    // Handle end touch event
    protected void HandleTouchEnd(TouchEventArgs e)
    {
        // Get the end position of touch on X axis
        double endX = e.ChangedTouches[0].ClientX;

        // We evaluate the direction of the gesture
        if (startX - endX > 50)
        {
            // Swipe left
            MoveNext();
        }
        else if (endX - startX > 50)
        {            
            // Swipe right
            MovePrevious();
        }
    }
    #endregion
}

#region Classes
// Class for gallery image
public class GalleryImage()
{
    public string ImagePath { get; set; }
    public string ImageAlt { get; set; }
    public string ImageTitle { get; set; }
}
#endregion