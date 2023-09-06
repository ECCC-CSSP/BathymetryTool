using System.Text;

namespace BathymetryToolApp;

public partial class BathymetryTool
{
    public void OnlyUseXYZWithinPolygon(Form form)
    {
        List<LatLngFile> latLngFileList = GetLatLngFileList(form);

        StringBuilder sb = new StringBuilder();
        foreach (LatLngFile latLng in latLngFileList)
        {
            sb.AppendLine(latLng.ToString());
        }

        (BathymetryToolAppForm)form.richTextBoxStatus.Text = sb.ToString();
    }
}