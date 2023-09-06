using System.Text;

namespace BathymetryToolApp;

public partial class BathymetryTool
{
    public List<LatLngFile> GetLatLngFileList(TextBox textBoxStartDir, RichTextBox richTextBoxStatus, Label lblStatus)
    {
        List<LatLngFile> LatLngFileList = new List<LatLngFile>();

        DirectoryInfo di = new DirectoryInfo(textBoxStartDir.Text + ".xyz\\");

        if (!di.Exists)
        {
            richTextBoxStatus.Text = "Directory does not exist [" + di.FullName + "";
            return new List<LatLngFile>();
        }

        List<FileInfo> fiList = di.GetFiles().Where(c => c.Extension == ".xyz").OrderBy(c => c.Name).ToList();

        foreach (FileInfo fi in fiList)
        {
            richTextBoxStatus.AppendText(fi.Name + "\r\n");

            float lat1 = float.Parse(fi.Name.Substring(0, fi.Name.IndexOf("-")).Replace("_", "."));
            float lng1 = float.Parse(fi.Name.Substring(fi.Name.IndexOf("-"), fi.Name.IndexOf("_", fi.Name.IndexOf("-")) + 1).Replace("_", "."));
            int valCount = int.Parse(fi.Name.Substring(fi.Name.IndexOf("_", fi.Name.IndexOf("-")) + 1).Replace(".xyz", ""));

            LatLngFile latLngFile = new LatLngFile()
            {
                Lat1 = lat1,
                Lat2 = lat1 + 0.1f,
                Lng1 = lng1,
                Lng2 = lng1 + 0.1f,
                FileName = fi.Name,
                ValCount = valCount, 
            };

            LatLngFileList.Add(latLngFile);
        }

        lblStatus.Text = "Done...";
        
        return LatLngFileList;

    }
}