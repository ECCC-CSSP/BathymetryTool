namespace MainViewModels;

public partial class MainViewModel
{
    public async Task CreateSubGroupOfXYZFilesFromLocationPolygonKMLAsync()
    {
        FileInfo fi = new FileInfo($"{StartDir}\\kml\\Location Polygons.kml");

        if (!fi.Exists)
        {
            Log = $"File does not exist [{fi.FullName}]";

            return;
        }

        List<LatLngFileCount> latLngFileCountList = GetLatLngFileCountListForXYZ().OrderBy(c => c.FileName).ToList();

        int count = (from c in latLngFileCountList
                     select c.ValCount).Sum();

        XDocument doc = XDocument.Load(fi.FullName);

        if (doc.Root != null)
        {

            foreach (XElement e1 in doc.Root.Descendants().Where(c => c.Name.ToString().EndsWith("Placemark")))
            {
                string DirectoryName = string.Empty;

                List<Coord> poly = new List<Coord>();

                XElement? elem_name = e1.Descendants().Where(c => c.Name.ToString().EndsWith("name")).FirstOrDefault();
                if (elem_name != null)
                {
                    DirectoryName = elem_name.Value;
                    DirectoryInfo di = new DirectoryInfo($"{StartDir}\\sub_xyz\\{DirectoryName}");
                    if (!di.Exists)
                    {
                        try
                        {
                            di.Create();
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    }
                }

                if (DirectoryName == string.Empty)
                {
                    continue;
                }

                if (DirectoryName.Length > 0) //.Contains("Vancouver")) // == "South of Vancouver Island")
                {
                    XElement? elem_Coord = e1.Descendants().Where(c => c.Name.ToString().EndsWith("coordinates")).FirstOrDefault();
                    if (elem_Coord != null)
                    {
                        string CoordText = elem_Coord.Value;
                        List<string> CoordListText = CoordText.Split(" ", StringSplitOptions.None).ToList();
                        foreach (string CoordText2 in CoordListText)
                        {
                            List<string> CoordListText2 = CoordText2.Trim().Split(",").ToList();
                            if (CoordListText2.Count < 3)
                            {
                                continue;
                            }
                            float lat = float.Parse(CoordListText2[1]);
                            float lng = float.Parse(CoordListText2[0]);

                            poly.Add(new Coord() { Lat = lat, Lng = lng });
                        }

                        if (poly[0].Lng < -90.0f)
                        {


                            foreach (LatLngFileCount latLngFileCount in latLngFileCountList)
                            {
                                List<Coord> fileCoordList = new List<Coord>()
                                {
                                    new Coord() { Lat = latLngFileCount.Lat1, Lng = latLngFileCount.Lng1 },
                                    new Coord() { Lat = latLngFileCount.Lat2, Lng = latLngFileCount.Lng1 },
                                    new Coord() { Lat = latLngFileCount.Lat1, Lng = latLngFileCount.Lng2 },
                                    new Coord() { Lat = latLngFileCount.Lat2, Lng = latLngFileCount.Lng2 }
                                };

                                foreach (Coord fileCoord in fileCoordList)
                                {
                                    if (CoordInPolygon(poly, fileCoord))
                                    {
                                        FileInfo fiExist = new FileInfo($"{StartDir}\\sub_xyz\\{DirectoryName}\\{latLngFileCount.FileName}");
                                        if (fiExist.Exists)
                                        {
                                            try
                                            {
                                                fiExist.Delete();
                                            }
                                            catch (Exception)
                                            {

                                                throw;
                                            }
                                        }

                                        File.Copy($"{StartDir}\\xyz\\{latLngFileCount.FileName}", $"{StartDir}\\sub_xyz\\{DirectoryName}\\{latLngFileCount.FileName}");
                                        fiExist = new FileInfo($"{StartDir}\\sub_xyz\\{DirectoryName}\\{latLngFileCount.FileName}");
                                        if (fiExist.Exists)
                                        {
                                            FileInfo fiXYZ = new FileInfo($"{StartDir}\\sub_xyz\\{DirectoryName}\\{latLngFileCount.FileName}");

                                            await ReduceXYZFileToContainOnlyDataInLocationPolygonAsync(fiXYZ, poly);
                                        }
                                        break;
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
    }
}