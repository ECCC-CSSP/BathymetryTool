namespace MainViewModels;

public partial class MainViewModel
{
    public void CreateKMLFileFromXYZFile(FileInfo fiXYZ)
    {
        if (!fiXYZ.Exists)
        {
            Status = $"File does not exist [{fiXYZ.FullName}]";
            Log = Status;

            return;
        }

        FileInfo fiKML = new FileInfo(fiXYZ.FullName.Replace(".xyz", ".kml"));
        if (fiKML.Exists)
        {
            Status = $"File already exist [{fiKML.FullName}]";
            Log = Status;

            return;
        }


        List<ColorVal> ColorValList = new List<ColorVal>();
        ColorValList = FillColorValues();

        StringBuilder sb = new StringBuilder();

        CreateTopKMLPart(sb, fiXYZ.DirectoryName + "");

        using (StreamReader sr = new StreamReader($"{fiXYZ.FullName}"))
        {

            string? line;
            int count = 0;
            while ((line = sr.ReadLine()) != null)
            {
                count++;
                string[] parts = line.Split(',');
                if (parts.Length < 3) continue; // Skip if not enough coordinates

                double x = double.Parse(parts[0], CultureInfo.InvariantCulture);
                double y = double.Parse(parts[1], CultureInfo.InvariantCulture);
                double z = double.Parse(parts[2], CultureInfo.InvariantCulture);

                float factor = 0.0001f;

                if (z > -2)
                {
                    factor = 0.00005f;
                }

                sb.AppendLine(@"		<Placemark>");
                sb.AppendLine($@"			<name>{z.ToString("F2")}</name>");
                sb.AppendLine(@"			<styleUrl>#" + GetColorStyleID((double)z * -1, ColorValList) + "</styleUrl>");
                sb.AppendLine(@"			<Polygon>");
                sb.AppendLine(@"				<outerBoundaryIs>");
                sb.AppendLine(@"				    <LinearRing>");
                sb.AppendLine(@"				        <coordinates>");
                sb.Append($"{(x - factor).ToString("F6")},{(y - factor).ToString("F6")},0 ");
                sb.Append($"{(x + factor).ToString("F6")},{(y - factor).ToString("F6")},0 ");
                sb.Append($"{(x + factor).ToString("F6")},{(y + factor).ToString("F6")},0 ");
                sb.Append($"{(x - factor).ToString("F6")},{(y + factor).ToString("F6")},0 ");
                sb.Append($"{(x - factor).ToString("F6")},{(y - factor).ToString("F6")},0 ");
                sb.AppendLine();
                sb.AppendLine(@"			          	</coordinates>");
                sb.AppendLine(@"				    </LinearRing>");
                sb.AppendLine(@"				</outerBoundaryIs>");
                sb.AppendLine(@"			</Polygon>");
                sb.AppendLine(@"		</Placemark>");

            }
        }

        sb.AppendLine($"</Document>");
        sb.AppendLine($"</kml>");

        using (StreamWriter sw = new StreamWriter($"{fiXYZ.FullName.Replace(".xyz", ".kml")}"))
        {
            sw.Write(sb.ToString());
        }
    }
}
