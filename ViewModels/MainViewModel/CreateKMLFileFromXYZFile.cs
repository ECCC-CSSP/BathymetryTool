namespace MainViewModels;

public partial class MainViewModel
{
    public void CreateKMLFileFromXYZFile(FileInfo fiXYZ)
    {
        if (!fiXYZ.Exists)
        {
            Log = $"File does not exist [{fiXYZ.FullName}]";

            return;
        }

        LatLngFileCount latLngFileCount = GetLatLngFileCountFromFileInfo(fiXYZ);

        StringBuilder sb = new StringBuilder();

        CreateTopKMLPart(sb, fiXYZ.DirectoryName + "");
        sb.AppendLine($"	<Folder>");
        sb.AppendLine($"		<name>{fiXYZ.DirectoryName + ""}</name>");


        sb.AppendLine($"		<Placemark>");
        sb.AppendLine($"			<name>{latLngFileCount.ValCount}</name>");
        sb.AppendLine($"			<styleUrl>#m_ylw-pushpin</styleUrl>");
        sb.AppendLine($"			<Point>");
        sb.AppendLine($"				<coordinates>{latLngFileCount.Lng1},{latLngFileCount.Lat1},0</coordinates>");
        sb.AppendLine($"			</Point>");
        sb.AppendLine($"		</Placemark>");

        sb.AppendLine($"		<Placemark>");
        sb.AppendLine($"			<name>Untitled Polygon</name>");
        sb.AppendLine($"			<styleUrl>#m_ylw-pushpin</styleUrl>");
        sb.AppendLine($"			<Polygon>");
        sb.AppendLine($"				<outerBoundaryIs>");
        sb.AppendLine($"					<LinearRing>");
        sb.AppendLine($"						<coordinates>");
        sb.AppendLine($"							{latLngFileCount.Lng1},{latLngFileCount.Lat1},0 {latLngFileCount.Lng2},{latLngFileCount.Lat1},0 {latLngFileCount.Lng2},{latLngFileCount.Lat2},0 {latLngFileCount.Lng1},{latLngFileCount.Lat2},0 {latLngFileCount.Lng1},{latLngFileCount.Lat1},0 ");
        sb.AppendLine($"						</coordinates>");
        sb.AppendLine($"					</LinearRing>");
        sb.AppendLine($"				</outerBoundaryIs>");
        sb.AppendLine($"			</Polygon>");
        sb.AppendLine($"		</Placemark>");

        using (StreamReader sr = new StreamReader($"{fiXYZ.FullName}"))
        {

            string? line;
            int count = 0;
            while ((line = sr.ReadLine()) != null)
            {
                count++;
                //if (count % 1000 != 0) continue;

                // Split the line by spaces, assuming that's what separates the coordinates
                string[] parts = line.Split(',');
                if (parts.Length < 3) continue; // Skip if not enough coordinates

                // Parse coordinates, assuming that they're in floating-point format
                double x = double.Parse(parts[0], CultureInfo.InvariantCulture);
                double y = double.Parse(parts[1], CultureInfo.InvariantCulture);
                double z = double.Parse(parts[2], CultureInfo.InvariantCulture);

                sb.AppendLine($"		<Placemark>");
                sb.AppendLine($"			<name>{z}</name>");
                sb.AppendLine($"			<styleUrl>#m_ylw-pushpin</styleUrl>");
                sb.AppendLine($"			<Point>");
                sb.AppendLine($"				<coordinates>{x},{y},{0}</coordinates>");
                sb.AppendLine($"			</Point>");
                sb.AppendLine($"		</Placemark>");

            }
        }

        sb.AppendLine($"	</Folder>");
        sb.AppendLine($"</Document>");
        sb.AppendLine($"</kml>");

        using (StreamWriter sw = new StreamWriter($"{fiXYZ.FullName.Replace(".xyz", ".kml")}"))
        {
            sw.Write(sb.ToString());
        }
    }
}
