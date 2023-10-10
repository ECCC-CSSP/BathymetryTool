namespace MainViewModels;

public partial class MainViewModel
{
    public async Task FixKMLFileByRemovingAbsoluteLineAsync(DirectoryInfo startDir)
    {
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
        #endregion checking function parameters

        #region correcting kml files by removing the absolute line
        List<DirectoryInfo> diList = startDir.GetDirectories().ToList();

        foreach (DirectoryInfo di in diList)
        {
            DirectoryInfo diKML = new DirectoryInfo(di.FullName + "\\kml\\");
            if (!diKML.Exists)
            {
                continue;
            }

            List<FileInfo> fiList = diKML.GetFiles().Where(c => c.Extension == ".kml").OrderBy(c => c.Name).ToList();

            foreach (FileInfo fiKMLBlock in fiList)
            {
                StringBuilder sb = new StringBuilder();
                Status = fiKMLBlock.Name;

                using (StreamReader sr = new StreamReader(fiKMLBlock.FullName))
                {
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim().Length > 0)
                        {
                            if (line.Contains("<altitudeMode>absolute</altitudeMode>"))
                            {
                                continue;
                            }
                            sb.AppendLine(line);
                        }
                    }
                }

                FileInfo fi = new FileInfo(fiKMLBlock.FullName);
                StreamWriter sw = fi.CreateText();
                await sw.WriteAsync(sb.ToString());
                sw.Close();
            }
        }
        #endregion correcting kml files by removing the absolute line

        Status = "Done...";
    }
}
