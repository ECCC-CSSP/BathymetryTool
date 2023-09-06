namespace MainViewModels;

public partial class MainViewModel
{
    public List<LatLngFileCount> GetLatLngFileCountListForKML(DirectoryInfo diKML)
    {
        StringBuilder sb = new StringBuilder();

        List<LatLngFileCount> latLngFileCountList = new List<LatLngFileCount>();

        if (!diKML.Exists)
        {
            Log = $"Directory does not exist [{diKML.FullName}]";

            return new List<LatLngFileCount>();
        }

        List<FileInfo> fiList = diKML.GetFiles().Where(c => c.Extension == ".kml").OrderBy(c => c.Name).ToList();

        foreach (FileInfo fi in fiList)
        {
            if (fi.Name.StartsWith("_")) continue;

            Status = fi.Name;

            LatLngFileCount latLngFileCount = GetLatLngFileCountFromFileInfo(fi);

            latLngFileCountList.Add(latLngFileCount);
        }

        Status = "Done...";

        return latLngFileCountList;

    }
}