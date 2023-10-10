namespace MainViewModels;

public partial class MainViewModel
{
    public async Task CreateXYZFileFromKMLFileAsync(FileInfo fiKML)
    {
        StringBuilder sb = new ();

        if (!fiKML.Exists)
        {
            Status = $"File does not exist [{fiKML.FullName}]";
            Log = Status;

            return;
        }

        FileInfo fiXYZ = new (fiKML.FullName.Replace(".kml", ".xyz"));
        if (fiXYZ.Exists)
        {
            Status = $"File already exist [{fiXYZ.FullName}]";
            Log = Status;

            return;
        }

        XElement root = XElement.Load(fiKML.FullName);

        foreach (XElement elem in root.Descendants())
        {
            if (elem.Name.LocalName == "Placemark")
            {
                float depth = -9999;
                foreach (XElement elem2 in elem.Descendants())
                {
                    if (elem2.Name.LocalName == "name")
                    {
                        if (elem2.Value.StartsWith("--"))
                        {
                            float depthTemp = -9999;
                            if (float.TryParse(elem2.Value[1..], out depthTemp))
                            {
                                depth = depthTemp;
                            }
                        }
                        else
                        {
                            float depthTemp = -9999;
                            if (float.TryParse(elem2.Value, out depthTemp))
                            {
                                depth = depthTemp;
                            }
                        }
                    }
                    if (elem2.Name.LocalName == "coordinates")
                    {
                        string elemTxt = elem2.Value;
                        List<string> coordsTxtList = elemTxt.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                        if (coordsTxtList.Count == 5)
                        {
                            List<float> latList = new ();
                            List<float> lngList = new ();

                            foreach (string s in coordsTxtList.Skip(1))
                            {
                                List<string> pointTxtList = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                                if (pointTxtList.Count != 3)
                                {
                                    Status = "Error: pointTxtList.Count != 3";
                                    Log = Status;

                                    return;
                                }

                                latList.Add(float.Parse(pointTxtList[1]));
                                lngList.Add(float.Parse(pointTxtList[0]));

                            }

                            float lat = latList.Average();
                            float lng = lngList.Average();

                            if (depth != -9999)
                            {
                                sb.AppendLine(lng.ToString("F6") + "," + lat.ToString("F6") + "," + depth.ToString("F2"));
                            }
                        }
                        else
                        {
                            foreach (string s in coordsTxtList)
                            {
                                List<string> pointTxtList = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                                if (pointTxtList.Count != 3)
                                {
                                    Status = "Error: pointTxtList.Count != 3";
                                    Log = Status;

                                    return;
                                }

                                if (depth != -9999)
                                {
                                    sb.AppendLine(float.Parse(pointTxtList[0]).ToString("F6") + "," + float.Parse(pointTxtList[1]).ToString("F6") + "," + depth.ToString("F2"));
                                }
                            }
                        }
                    }
                }
            }
        }

        //FileInfo fi = new FileInfo(fiKML.FullName.Replace(".kml", ".xyz"));
        StreamWriter sw = fiXYZ.CreateText();
        await sw.WriteAsync(sb.ToString());
        sw.Close();

        Status = "";
    }
}
