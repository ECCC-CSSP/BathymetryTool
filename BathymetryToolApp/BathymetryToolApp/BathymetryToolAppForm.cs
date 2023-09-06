namespace BathymetryToolApp;

public partial class BathymetryToolAppForm : Form
{
    BathymetryTool bathymetryTool = new BathymetryTool();

    public BathymetryToolAppForm()
    {
        InitializeComponent();
        bathymetryTool = new BathymetryTool();

    }

    #region Events
    private void butOnlyUseXYZWithinPolygon_Click(object sender, EventArgs e)
    {
       bathymetryTool.OnlyUseXYZWithinPolygon(this);
    }
    private void butSubdivide_Click(object sender, EventArgs e)
    {
       bathymetryTool.Subdivide(this);
    }
    #endregion Events
}