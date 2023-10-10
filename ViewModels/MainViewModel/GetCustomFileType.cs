namespace MainViewModels;

public partial class MainViewModel
{
    async Task<PickOptions> GetCustomFileType(string extension)
    {
        if (extension.StartsWith("."))
        {
            extension = extension.Substring(1);
        }

        var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { $"public.my.{extension}.extension" } }, // UTType values
                    { DevicePlatform.Android, new[] { $"application/{extension}" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { $".{extension}", $".{extension}" } }, // file extension
                    { DevicePlatform.Tizen, new[] { "*/*" } },
                    { DevicePlatform.macOS, new[] { $"{extension}", $"{extension}" } }, // UTType values
                });

        PickOptions options = new()
        {
            PickerTitle = $"Please select a .{extension} file",
            FileTypes = customFileType,
        };

        return await Task.FromResult(options);
    }
}
