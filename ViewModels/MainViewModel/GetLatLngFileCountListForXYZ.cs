namespace MainViewModels;

public partial class MainViewModel
{
    public List<LatLngFileCount> GetLatLngFileCountListForXYZ()
    {
        StringBuilder sb = new StringBuilder();

        List<LatLngFileCount> latLngFileCountList = new List<LatLngFileCount>();

        DirectoryInfo di = new DirectoryInfo($"{StartDir}\\xyz\\");

        if (!di.Exists)
        {
            Log = $"Directory does not exist [{di.FullName}]";

            return new List<LatLngFileCount>();
        }

        List<FileInfo> fiList = di.GetFiles().Where(c => c.Extension == ".xyz").OrderBy(c => c.Name).ToList();

        foreach (FileInfo fi in fiList)
        {
            Status = fi.Name;

            LatLngFileCount latLngFileCount = GetLatLngFileCountFromFileInfo(fi);

            latLngFileCountList.Add(latLngFileCount);
        }

        Status = "Done...";

        return latLngFileCountList;

    }
}