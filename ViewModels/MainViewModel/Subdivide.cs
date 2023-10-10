namespace MainViewModels;

public partial class MainViewModel
{
    public async Task SubdivideAsync()
    {
        DirectoryInfo di = new DirectoryInfo(StartDir);
        StringBuilder sbLog = new StringBuilder();

        if (!di.Exists)
        {
            sbLog.AppendLine($"Directory does not exist [{di.FullName}]");
            Log = sbLog.ToString();
            return;
        }

        List<FileInfo> fiList = di.GetFiles().Where(c => c.Extension == ".txt").OrderBy(c => c.Name).ToList();

        foreach (FileInfo fi in fiList)
        {
            sbLog.AppendLine($"{fi.Name}");
        }

        foreach (FileInfo fi in fiList)
        {
            if (fi.Name.StartsWith("N")) // ONNAP10_4600N06600W"))
            {
                int pos = fi.Name.IndexOf("_") + 1;
                int pos2 = fi.Name.IndexOf("N", pos);
                string fnLat = fi.Name.Substring(pos, pos2 - pos - 2);
                pos = fi.Name.IndexOf("N", pos) + 1;
                pos2 = fi.Name.IndexOf("W");
                string fnLng = fi.Name.Substring(pos, pos2 - pos - 2);

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        float lat1 = float.Parse(fnLat) + i / 10.0f;
                        float lat2 = lat1 + 0.1f;
                        float lng1 = (float.Parse(fnLng) - j / 10.0f) * -1;
                        float lng2 = lng1 + 0.1f;

                        string lngText = lng1.ToString("F1").Replace(".", "_");
                        string latText = lat1.ToString("F1").Replace(".", "_");

                        string newFileName = latText + lngText;

                        Status = $"doing --- {di.FullName}\\xyz\\{newFileName}____.xyz";

                        StringBuilder sb = new StringBuilder();

                        TextReader r = new StreamReader(fi.FullName);

                        string? Line = r.ReadLine();
                        if (Line == null)
                        {
                            sbLog.AppendLine($"Error: could not read first line");
                            Log = sbLog.ToString();
                            return;
                        }

                        int countVal = 0;
                        while (Line != null)
                        {
                            Line = r.ReadLine();
                            if (Line != null)
                            {
                                float lat = float.Parse(Line.Substring(0, 2)) + float.Parse(Line.Substring(3, 2)) / 60 + float.Parse(Line.Substring(6, 6)) / 3600;
                                float lng = -1 * (float.Parse(Line.Substring(14, 3)) + float.Parse(Line.Substring(18, 2)) / 60 + float.Parse(Line.Substring(21, 6)) / 3600);
                                string depthText = Line.Substring(29, 10);

                                if (depthText.Contains("\t"))
                                {
                                    depthText = depthText.Substring(0, depthText.IndexOf("\t"));
                                }

                                if (depthText != "N/A")
                                {
                                    float depth = -1 * float.Parse(depthText);

                                    if (lat >= lat1 && lng >= lng1 && lat <= lat2 && lng <= lng2)
                                    {
                                        countVal++;
                                        sb.AppendLine(lng.ToString("F6") + "," + lat.ToString("F6") + "," + depth.ToString("F2"));
                                    }
                                }
                            }
                        }

                        r.Close();

                        if (countVal > 0)
                        {
                            FileInfo fiNew = new FileInfo($"{di.FullName}\\xyz\\{newFileName}_{countVal}.xyz");
                            if (fiNew.Exists)
                            {
                                fiNew.Delete();
                                fiNew = new FileInfo($"{di.FullName}\\xyz\\{newFileName}_{countVal}.xyz");
                            }

                            TextWriter w = fiNew.CreateText();
                            await w.WriteLineAsync(sb.ToString());
                            w.Close();

                            sbLog.AppendLine($"{fiNew.FullName}");
                            Log = sbLog.ToString();

                        }
                    }
                }

                Status = "Done...";

            }
        }
    }
}