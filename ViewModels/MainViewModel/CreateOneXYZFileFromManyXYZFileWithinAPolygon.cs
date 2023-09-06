namespace MainViewModels;

public partial class MainViewModel
{
    public void CreateOneXYZFileFromManyXYZFileWithinAPolygon(DirectoryInfo startDir, string fileNameToCreate, string kmlOfPolygon)
    {
        StringBuilder sb = new StringBuilder();
        List<Coord> poly = new List<Coord>();

        #region checking function parameters
        if (startDir == null)
        {
            Log = "Error: startDir == null";
            Status = "Error: startDir == null";
            return;
        }

        if (!startDir.Exists)
        {
            Log = $"Error: startDir does not exist [{startDir.FullName}]";
            Status = $"Error: startDir does not exist [{startDir.FullName}]";
            return;
        }

        FileInfo fiXYZ = new FileInfo(startDir.FullName + "\\" + fileNameToCreate);

        if (fiXYZ.Exists)
        {
            try
            {
                fiXYZ.Delete();
            }
            catch (Exception ex)
            {
                Log = $"Error: Could not delete File [{fiXYZ.FullName}] -- exception: {ex.Message}";
                Status = $"Error: Could not delete File [{fiXYZ.FullName}] -- exception: {ex.Message}";
                return;
            }
        }
        #endregion checking function parameters

        #region  reading the xyz files coordinates
        List<LatLngFileCount> latLngFileCountList = new List<LatLngFileCount>();

        List<DirectoryInfo> diList = startDir.GetDirectories().ToList();

        foreach (DirectoryInfo di in diList)
        {
            DirectoryInfo diXYZ = new DirectoryInfo(di.FullName + "\\xyz\\");
            if (!diXYZ.Exists)
            {
                continue;
            }

            List<FileInfo> fiList = diXYZ.GetFiles().Where(c => c.Extension == ".xyz").OrderBy(c => c.Name).ToList();

            foreach (FileInfo fiXYZBlock in fiList)
            {
                Status = fiXYZBlock.Name;

                LatLngFileCount latLngFileCount = GetLatLngFileCountFromFileInfo(fiXYZBlock);

                latLngFileCount.FileName = fiXYZBlock.FullName;

                latLngFileCountList.Add(latLngFileCount);
            }
        }
        #endregion  reading the xyz files coordinates

        #region getting the polygon coordinates
        XDocument doc = XDocument.Parse(kmlOfPolygon);

        if (doc.Root != null)
        {
            foreach (XElement e1 in doc.Root.Descendants().Where(c => c.Name.ToString().EndsWith("Placemark")))
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
                }
            }
        }
        #endregion getting the polygon coordinates

        if (poly.Count > 2)
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

                int CountCoordInPoly = 0;
                foreach (Coord fileCoord in fileCoordList)
                {
                    if (CoordInPolygon(poly, fileCoord))
                    {
                        CountCoordInPoly++;
                    }
                }

                if (CountCoordInPoly == 4)
                {
                    // should take the whole file and read transfer all points to sb
                    FileInfo fiXYZBlock = new FileInfo(latLngFileCount.FileName);
                    if (!fiXYZBlock.Exists)
                    {
                        Log = $"Error: fiXYZBlock does not exist [{fiXYZBlock.FullName}]";
                        Status = $"Error: fiXYZBlock does not exist [{fiXYZBlock.FullName}]";
                        return;
                    }

                    using (StreamReader sr = new StreamReader(fiXYZBlock.FullName))
                    {
                        string? line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Trim().Length > 0)
                            {
                                sb.AppendLine(line);
                            }
                        }
                    }

                }
                else if (CountCoordInPoly > 0)
                {
                    // should take the only the points that falls within poly transfer these points to sb
                    FileInfo fiXYZBlock = new FileInfo(latLngFileCount.FileName);
                    if (!fiXYZBlock.Exists)
                    {
                        Log = $"Error: fiXYZBlock does not exist [{fiXYZBlock.FullName}]";
                        Status = $"Error: fiXYZBlock does not exist [{fiXYZBlock.FullName}]";
                        return;
                    }

                    using (StreamReader sr = new StreamReader(fiXYZBlock.FullName))
                    {
                        string? line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.Trim().Length > 0)
                            {
                                List<string> lineList = line.Trim().Split(",").ToList();
                                if (lineList.Count < 3)
                                {
                                    continue;
                                }
                                Coord coord = new Coord() { Lat = float.Parse(lineList[1]), Lng = float.Parse(lineList[0]) };

                                if (CoordInPolygon(poly, coord))
                                {
                                    sb.AppendLine(line);
                                }
                            }
                        }
                    }
                }
                else
                {
                    // nothing here as all 4 points fall outside the polygon
                }
            }
        }

        FileInfo fi = new FileInfo(startDir.FullName + "\\" + fileNameToCreate);
        StreamWriter sw = fi.CreateText();
        sw.Write(sb.ToString());
        sw.Close();

        Status = "Done...";
    }
}
