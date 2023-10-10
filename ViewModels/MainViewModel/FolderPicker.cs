using CommunityToolkit.Maui.Core.Primitives;

namespace MainViewModels;

public partial class MainViewModel
{
    async Task<FolderPickerResult> PickFolder(string startDir, CancellationToken cancellationToken)
    {
        return await FolderPicker.Default.PickAsync(startDir, cancellationToken);
    }
}
