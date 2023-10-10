namespace MainViewModels;

public partial class MainViewModel
{
    public async Task<FileResult?> PickFile(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);

            return result;
        }
        catch (Exception ex)
        {
            Status = $"File does not exist: {ex.Message}";
        }

        return null;
    }
}
