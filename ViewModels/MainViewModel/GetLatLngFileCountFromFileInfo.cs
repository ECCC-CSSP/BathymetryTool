namespace MainViewModels;

public partial class MainViewModel
{
    public LatLngFileCount GetLatLngFileCountFromFileInfo(FileInfo fi)
    {
        int posNegative = fi.Name.IndexOf("-");
        float lat1 = float.Parse(fi.Name.Substring(0, posNegative).Replace("_", "."));
        float lng1 = float.Parse(fi.Name.Substring(posNegative, fi.Name.IndexOf("_", posNegative) - posNegative + 2).Replace("_", "."));
        int valCount = int.Parse(fi.Name.Substring(fi.Name.LastIndexOf("_") + 1).Replace(".kml", "").Replace(".xyz", "").Replace("a", "").Replace("b", ""));

        LatLngFileCount latLngFileCount = new LatLngFileCount()
        {
            Lat1 = lat1,
            Lat2 = lat1 + 0.1f,
            Lng1 = lng1,
            Lng2 = lng1 + 0.1f,
            FileName = fi.Name,
            ValCount = valCount,
        };

        return latLngFileCount;
    }
}
