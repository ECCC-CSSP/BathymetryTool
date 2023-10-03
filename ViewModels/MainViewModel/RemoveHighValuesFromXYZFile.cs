namespace MainViewModels;

public partial class MainViewModel
{
    public void RemoveHighValuesFromXYZFile(FileInfo fiXYZ, double highValue)
    {
        if (!fiXYZ.Exists)
        {
            Log = $"File does not exist [{fiXYZ.FullName}]";

            return;
        }

        StringBuilder sb = new StringBuilder();

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

                if (z > highValue) continue;

                sb.AppendLine(line);
            }
        }

        using (StreamWriter sw = new StreamWriter($"{fiXYZ.FullName.Replace(".xyz", $"_HighValue_{highValue}_Removed.xyz")}"))
        {
            sw.Write(sb.ToString());
        }
    }
}
