using System.Globalization;
using System.Xml;

namespace MainViewModels;

public partial class MainViewModel
{
    public async Task ReduceXYZFileToContainOnlyDataInLocationPolygonAsync(FileInfo fi, List<Coord> poly)
    {
        if (!fi.Exists)
        {
            Log = $"File does not exist [{fi.FullName}]";

            return;
        }

        LatLngFileCount latLngFileCount = GetLatLngFileCountFromFileInfo(fi);

        StringBuilder sb = new StringBuilder();

        int count = 0;
        using (StreamReader sr = new StreamReader($"{fi.FullName}"))
        {
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                // Split the line by spaces, assuming that's what separates the coordinates
                string[] parts = line.Split(',');
                if (parts.Length < 3) continue; // Skip if not enough coordinates

                // Parse coordinates, assuming that they're in floating-point format
                double x = double.Parse(parts[0], CultureInfo.InvariantCulture);
                double y = double.Parse(parts[1], CultureInfo.InvariantCulture);
                double z = double.Parse(parts[2], CultureInfo.InvariantCulture);

                Coord coord = new Coord() { Lat = (float)y, Lng = (float)x };
                if (CoordInPolygon(poly, coord))
                {
                    count++;
                    sb.AppendLine(line);
                }
            }
        }

        FileInfo fiCompact = new FileInfo(fi.FullName.Replace(".xyz", "a.xyz"));
        using (StreamWriter sw = new StreamWriter(fiCompact.FullName))
        {
            sw.Write(sb.ToString());
        }

        try
        {
            fi.Delete();
        }
        catch (Exception)
        {
            throw;
        }

        FileInfo fiXYZ = new FileInfo(fiCompact.FullName);
        await ReduceXYZFileDensityAsync(fiXYZ);

    }
}
