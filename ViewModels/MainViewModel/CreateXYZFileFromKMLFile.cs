namespace MainViewModels;

public partial class MainViewModel
{
    public void CreateXYZFileFromKMLFile(FileInfo fiKML)
    {
        StringBuilder sb = new StringBuilder();

        if (!fiKML.Exists)
        {
            Log = $"File does not exist [{fiKML.FullName}]";

            return;
        }

        XElement root = XElement.Load(fiKML.FullName);

        foreach (XElement elem in root.Descendants())
        {
            if (elem.Name.LocalName == "Placemark")
            {
                float depth = -999;
                foreach (XElement elem2 in elem.Descendants())
                {
                    if (elem2.Name.LocalName == "name")
                    {
                        if (elem2.Value.StartsWith("--"))
                        {
                            depth = float.Parse(elem2.Value.Substring(1));
                        }
                        else
                        {
                            depth = float.Parse(elem2.Value);
                        }
                    }
                    if (elem2.Name.LocalName == "coordinates")
                    {
                        string elemTxt = elem2.Value;
                        List<string> coordsTxtList = elemTxt.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                        if (coordsTxtList.Count == 5)
                        {
                            List<float> latList = new List<float>();
                            List<float> lngList = new List<float>();

                            foreach (string s in coordsTxtList.Skip(1))
                            {
                                List<string> pointTxtList = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                                if (pointTxtList.Count != 3)
                                {
                                    Log = $"Error: pointTxtList.Count != 3";
                                    Status = "Error: pointTxtList.Count != 3";
                                    return;
                                }

                                latList.Add(float.Parse(pointTxtList[1]));
                                lngList.Add(float.Parse(pointTxtList[0]));

                            }

                            float lat = latList.Average();
                            float lng = lngList.Average();

                            sb.AppendLine(lng.ToString("F6") + " " + lat.ToString("F6") + " " + depth.ToString("F2"));
                        }
                        else
                        {
                            foreach (string s in coordsTxtList)
                            {
                                List<string> pointTxtList = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                                if (pointTxtList.Count != 3)
                                {
                                    Log = "Error: pointTxtList.Count != 3";
                                    Status = "Error: pointTxtList.Count != 3";
                                    return;
                                }

                                sb.AppendLine(float.Parse(pointTxtList[0]).ToString("F6") + " " + float.Parse(pointTxtList[1]).ToString("F6") + " " + depth.ToString("F2"));
                            }
                        }
                    }
                }
            }
        }
        FileInfo fi = new FileInfo(fiKML.FullName.Replace(".kml", ".xyz"));
        StreamWriter sw = fi.CreateText();
        sw.Write(sb.ToString());
        sw.Close();

        Status = "Done...";
    }
}
