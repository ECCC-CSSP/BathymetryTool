using System.Globalization;
using System.Xml;
using WinRT;

namespace MainViewModels;

public partial class MainViewModel
{
    public void ReduceXYZFileDensity(FileInfo fi)
    {
        if (!fi.Exists)
        {
            Log = $"File does not exist [{fi.FullName}]";

            return;
        }

        LatLngFileCount latLngFileCount = GetLatLngFileCountFromFileInfo(fi);

        List<XYZ> xyzList = new List<XYZ>();

        List<ColorVal> ColorValList = new List<ColorVal>();
        ColorValList = FillColorValues();

        if (fi.DirectoryName != null)
        {
            DirectoryInfo di = new DirectoryInfo(fi.DirectoryName + "\\xyz\\");
            if (!di.Exists)
            {
                try
                {
                    di.Create();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            di = new DirectoryInfo(fi.DirectoryName + "\\kml\\");
            if (!di.Exists)
            {
                try
                {
                    di.Create();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        StringBuilder sb = new StringBuilder();
        StringBuilder sbHtml = new StringBuilder();

        using (StreamReader sr = new StreamReader($"{fi.FullName}"))
        {
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                // Split the line by spaces, assuming that's what separates the coordinates
                string[] parts = line.Split(',');
                if (parts.Length < 3) continue; // Skip if not enough coordinates

                // Parse coordinates, assuming that they're in floating-point format
                float x = float.Parse(parts[0], CultureInfo.InvariantCulture);
                float y = float.Parse(parts[1], CultureInfo.InvariantCulture);
                float z = float.Parse(parts[2], CultureInfo.InvariantCulture);

                if (z < 100 && z > -3000)
                {
                    xyzList.Add(new XYZ() { x = (float)x, y = (float)y, z = (float)z });
                }
            }
        }

        float subSize = 0.005f;

        int count = 0;
        for (float lat = latLngFileCount.Lat1; lat <= latLngFileCount.Lat2; lat = lat + subSize)
        {
            for (float lng = latLngFileCount.Lng1; lng <= latLngFileCount.Lng2; lng = lng + subSize)
            {
                List<XYZ> xyzListSub = (from c in xyzList
                                        where (c.y >= lat
                                        && c.y <= lat + subSize)
                                        && (c.x >= lng
                                        && c.x <= lng + subSize)
                                        select c).ToList();

                if (xyzListSub.Count > 0)
                {
                    float zMinSub = xyzListSub.Min(c => c.z);
                    float zMaxSub = xyzListSub.Max(c => c.z);
                    float zAvgSub = xyzListSub.Average(c => c.z);
                    float xAvgSub = xyzListSub.Average(c => c.x);
                    float yAvgSub = xyzListSub.Average(c => c.y);

                    if (zMaxSub < -5 && ((zAvgSub - zMinSub) / zAvgSub) < 0.1f && ((zMaxSub - zAvgSub) / zAvgSub) < 0.1f)
                    {
                        count++;

                        sb.AppendLine($"{(xAvgSub).ToString("F6")},{(yAvgSub).ToString("F6")},{zAvgSub.ToString("F2")}");

                        sbHtml.AppendLine(@"		<Placemark>");
                        sbHtml.AppendLine($@"			<name>{zAvgSub.ToString("F2")}</name>");
                        sbHtml.AppendLine(@"			<styleUrl>#" + GetColorStyleID((double)zAvgSub * -1, ColorValList) + "</styleUrl>");
                        sbHtml.AppendLine(@"			<Polygon>");
                        //sbHtml.AppendLine(@"");
                        sbHtml.AppendLine(@"				<outerBoundaryIs>");
                        sbHtml.AppendLine(@"				    <LinearRing>");
                        sbHtml.AppendLine(@"				        <coordinates>");
                        sbHtml.Append($"{(xAvgSub - 0.0005f).ToString("F6")},{(yAvgSub - 0.0005f).ToString("F6")},0 ");
                        sbHtml.Append($"{(xAvgSub + 0.0005f).ToString("F6")},{(yAvgSub - 0.0005f).ToString("F6")},0 ");
                        sbHtml.Append($"{(xAvgSub + 0.0005f).ToString("F6")},{(yAvgSub + 0.0005f).ToString("F6")},0 ");
                        sbHtml.Append($"{(xAvgSub - 0.0005f).ToString("F6")},{(yAvgSub + 0.0005f).ToString("F6")},0 ");
                        sbHtml.Append($"{(xAvgSub - 0.0005f).ToString("F6")},{(yAvgSub - 0.0005f).ToString("F6")},0 ");
                        sbHtml.AppendLine();
                        sbHtml.AppendLine(@"			          	</coordinates>");
                        sbHtml.AppendLine(@"				    </LinearRing>");
                        sbHtml.AppendLine(@"				</outerBoundaryIs>");
                        sbHtml.AppendLine(@"			</Polygon>");
                        sbHtml.AppendLine(@"		</Placemark>");


                    }
                    else
                    {
                        float subSubSize = 0.001f;

                        for (float lat2 = lat; lat2 <= lat + subSize; lat2 = lat2 + subSubSize)
                        {
                            for (float lng2 = lng; lng2 <= lng + subSize; lng2 = lng2 + subSubSize)
                            {
                                List<XYZ> xyzListSubSub = (from c in xyzListSub
                                                           where (c.y >= lat2
                                                           && c.y <= lat2 + subSubSize)
                                                           && (c.x >= lng2
                                                           && c.x <= lng2 + subSubSize)
                                                           select c).ToList();

                                if (xyzListSubSub.Count > 0)
                                {
                                    float zMinSubSub = xyzListSubSub.Min(c => c.z);
                                    float zMaxSubSub = xyzListSubSub.Max(c => c.z);
                                    float zAvgSubSub = xyzListSubSub.Average(c => c.z);
                                    float xAvgSubSub = xyzListSubSub.Average(c => c.x);
                                    float yAvgSubSub = xyzListSubSub.Average(c => c.y);

                                    if (zMaxSubSub < -2 && ((zAvgSubSub - zMinSubSub) / zAvgSubSub) < 0.1f && ((zMaxSubSub - zAvgSubSub) / zAvgSubSub) < 0.1f)
                                    {
                                        count++;

                                        sb.AppendLine($"{(xAvgSubSub).ToString("F6")},{(yAvgSubSub).ToString("F6")},{zAvgSubSub.ToString("F2")}");

                                        sbHtml.AppendLine(@"		<Placemark>");
                                        sbHtml.AppendLine($@"			<name>{zAvgSubSub.ToString("F2")}</name>");
                                        sbHtml.AppendLine(@"			<styleUrl>#" + GetColorStyleID((double)zAvgSubSub * -1, ColorValList) + "</styleUrl>");
                                        sbHtml.AppendLine(@"			<Polygon>");
                                        //sbHtml.AppendLine(@"");
                                        sbHtml.AppendLine(@"				<outerBoundaryIs>");
                                        sbHtml.AppendLine(@"				    <LinearRing>");
                                        sbHtml.AppendLine(@"				        <coordinates>");
                                        sbHtml.Append($"{(xAvgSubSub - 0.0001f).ToString("F6")},{(yAvgSubSub - 0.0001f).ToString("F6")},0 ");
                                        sbHtml.Append($"{(xAvgSubSub + 0.0001f).ToString("F6")},{(yAvgSubSub - 0.0001f).ToString("F6")},0 ");
                                        sbHtml.Append($"{(xAvgSubSub + 0.0001f).ToString("F6")},{(yAvgSubSub + 0.0001f).ToString("F6")},0 ");
                                        sbHtml.Append($"{(xAvgSubSub - 0.0001f).ToString("F6")},{(yAvgSubSub + 0.0001f).ToString("F6")},0 ");
                                        sbHtml.Append($"{(xAvgSubSub - 0.0001f).ToString("F6")},{(yAvgSubSub - 0.0001f).ToString("F6")},0 ");
                                        sbHtml.AppendLine();
                                        sbHtml.AppendLine(@"			          	</coordinates>");
                                        sbHtml.AppendLine(@"				    </LinearRing>");
                                        sbHtml.AppendLine(@"				</outerBoundaryIs>");
                                        sbHtml.AppendLine(@"			</Polygon>");
                                        sbHtml.AppendLine(@"		</Placemark>");
                                    }
                                    else
                                    {
                                        float subSubSubSize = 0.0005f;

                                        for (float lat3 = lat2; lat3 <= lat2 + subSubSize; lat3 = lat3 + subSubSubSize)
                                        {
                                            for (float lng3 = lng2; lng3 <= lng2 + subSubSize; lng3 = lng3 + subSubSubSize)
                                            {
                                                List<XYZ> xyzListSubSubSub = (from c in xyzListSubSub
                                                                              where (c.y >= lat3
                                                                              && c.y <= lat3 + subSubSubSize)
                                                                              && (c.x >= lng3
                                                                              && c.x <= lng3 + subSubSubSize)
                                                                              select c).ToList();

                                                if (xyzListSubSubSub.Count > 0)
                                                {
                                                    float zMinSubSubSub = xyzListSubSubSub.Min(c => c.z);
                                                    float zMaxSubSubSub = xyzListSubSubSub.Max(c => c.z);
                                                    float zAvgSubSubSub = xyzListSubSubSub.Average(c => c.z);
                                                    float xAvgSubSubSub = xyzListSubSubSub.Average(c => c.x);
                                                    float yAvgSubSubSub = xyzListSubSubSub.Average(c => c.y);

                                                    count++;

                                                    sb.AppendLine($"{(xAvgSubSubSub).ToString("F6")},{(yAvgSubSubSub).ToString("F6")},{zAvgSubSubSub.ToString("F2")}");

                                                    sbHtml.AppendLine(@"		<Placemark>");
                                                    sbHtml.AppendLine($@"			<name>{zAvgSubSubSub.ToString("F2")}</name>");
                                                    sbHtml.AppendLine(@"			<styleUrl>#" + GetColorStyleID((double)zAvgSubSubSub * -1, ColorValList) + "</styleUrl>");
                                                    sbHtml.AppendLine(@"			<Polygon>");
                                                    //sbHtml.AppendLine(@"");
                                                    sbHtml.AppendLine(@"				<outerBoundaryIs>");
                                                    sbHtml.AppendLine(@"				    <LinearRing>");
                                                    sbHtml.AppendLine(@"				        <coordinates>");
                                                    sbHtml.Append($"{(xAvgSubSubSub - 0.00005f).ToString("F6")},{(yAvgSubSubSub - 0.00005f).ToString("F6")},0 ");
                                                    sbHtml.Append($"{(xAvgSubSubSub + 0.00005f).ToString("F6")},{(yAvgSubSubSub - 0.00005f).ToString("F6")},0 ");
                                                    sbHtml.Append($"{(xAvgSubSubSub + 0.00005f).ToString("F6")},{(yAvgSubSubSub + 0.00005f).ToString("F6")},0 ");
                                                    sbHtml.Append($"{(xAvgSubSubSub - 0.00005f).ToString("F6")},{(yAvgSubSubSub + 0.00005f).ToString("F6")},0 ");
                                                    sbHtml.Append($"{(xAvgSubSubSub - 0.00005f).ToString("F6")},{(yAvgSubSubSub - 0.00005f).ToString("F6")},0 ");
                                                    sbHtml.AppendLine();
                                                    sbHtml.AppendLine(@"			          	</coordinates>");
                                                    sbHtml.AppendLine(@"				    </LinearRing>");
                                                    sbHtml.AppendLine(@"				</outerBoundaryIs>");
                                                    sbHtml.AppendLine(@"			</Polygon>");
                                                    sbHtml.AppendLine(@"		</Placemark>");

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //sb.AppendLine($"{(lng + subSize).ToString("F6")},{(lat + subSize).ToString("F6")},{999}");
                }
            }
        }

        string fileName = fi.Name.Substring(0, fi.Name.LastIndexOf("_")) + $"_{count}.xyz";

        try
        {
            fi.Delete();
        }
        catch (Exception)
        {
            throw;
        }


        sbHtml.AppendLine($"	</Folder>");
        sbHtml.AppendLine($"</Document>");
        sbHtml.AppendLine($"</kml>");

        string fullFileName = fi.DirectoryName + "\\xyz\\" + fileName;

        StringBuilder sbFinal = new StringBuilder();
        CreateTopKMLPart(sbFinal, fileName.Replace("xyz", "kml"));
        sbFinal.AppendLine($"	<Folder>");
        sbFinal.AppendLine($"		<name>{fileName.Replace("xyz", "kml")}</name>");
        sbFinal.AppendLine(sbHtml.ToString());


        using (StreamWriter sw = new StreamWriter(fullFileName))
        {
            sw.Write(sb.ToString());
        }

        fullFileName = fi.DirectoryName + "\\kml\\" + fileName.Replace("xyz", "kml");

        using (StreamWriter sw = new StreamWriter(fullFileName))
        {
            sw.Write(sbFinal.ToString());
        }
    }
}
