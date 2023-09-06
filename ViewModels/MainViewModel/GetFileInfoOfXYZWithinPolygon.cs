namespace MainViewModels;

public partial class MainViewModel
{
    public List<LatLngFileCount> GetFileInfoOfXYZWithinPolygon(List<Coord> poly)
    {
        List<LatLngFileCount> latLngFileCountList = GetLatLngFileCountListForXYZ();

        List<LatLngFileCount> latLngFileCountListSubset = new List<LatLngFileCount>();

        // need to read the .kml or text file to get the polygon
        // then need to read all the .xyz files names to see if the lat and lng are within the polygon

        // need to check what lat, lng are within the polygon and keep the file info


        StringBuilder sb = new StringBuilder();
        foreach (LatLngFileCount latLng in latLngFileCountList)
        {
            sb.AppendLine(latLng.ToString());
        }

        Log = sb.ToString();

        return latLngFileCountListSubset;
    }
}
