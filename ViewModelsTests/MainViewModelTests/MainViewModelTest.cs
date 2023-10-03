
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MainViewModelTests;

public partial class MainViewModelTest
{
    private MainViewModel vm { get; set; }

    public MainViewModelTest()
    {
        vm = new MainViewModel();
    }

    [Fact]
    public void Check_Setup()
    {
        SetupStart();

        FileInfo fi = new FileInfo($"{vm.StartDir}{fileName}");
        Assert.True(fi.Exists);

        SetupEnd();
        FileInfo fi2 = new FileInfo($"{vm.StartDir}{fileName}");
        Assert.False(fi2.Exists);
    }

    [Fact]
    public void Subdivide_OK()
    {
        SetupStart();
        vm.Subdivide();

        Assert.NotEmpty(vm.Log);
        Assert.Equal("Done...", vm.Status);

        SetupEnd();
    }

    [Fact]
    public void Subdivide_Error_Directory_Not_Exist_OK()
    {
        SetupStart();

        vm.StartDir = "E:\\Bathymetry\\CHS_High_Res\\\\Data\\NotThere\\";
        vm.Subdivide();

        Assert.NotEmpty(vm.Log);
        Assert.StartsWith("Directory does not exist [", vm.Log);

        SetupEnd();

    }

    [Fact]
    public void GetLatLngFileCountList_OK()
    {
        SetupStart();

        List<LatLngFileCount> latLngFileCountList = vm.GetLatLngFileCountListForXYZ();

        Assert.Empty(vm.Log);
        Assert.NotEmpty(vm.Status);
        Assert.Equal("Done...", vm.Status);
        Assert.NotEmpty(latLngFileCountList);
        Assert.Equal(40.0f, latLngFileCountList[0].Lat1);
        Assert.Equal(40.1f, latLngFileCountList[0].Lat2);
        Assert.Equal(-60.3f, latLngFileCountList[0].Lng1);
        Assert.Equal(-60.2f, latLngFileCountList[0].Lng2);
        Assert.Equal(11926, latLngFileCountList.Count);
        int count = latLngFileCountList.Sum(c => c.ValCount);
        Assert.True(count > 916000000);

        SetupEnd();

    }

    [Fact]
    public void GetLatLngFileCountList_Error_Directory_Does_Not_Exist_OK()
    {
        SetupStart();

        vm.StartDir = "E:\\Bathymetry\\CHS_High_Res\\\\Data\\NotThere\\";

        List<LatLngFileCount> latLngFileCountList = vm.GetLatLngFileCountListForXYZ();

        Assert.NotEmpty(vm.Log);
        Assert.StartsWith("Directory does not exist [", vm.Log);
        Assert.Empty(vm.Status);
        Assert.Empty(latLngFileCountList);

        SetupEnd();

    }

    [Fact]
    public void CreateSubGroupOfXYZFilesFromLocationPolygonKML_OK()
    {
        SetupStart();

        vm.CreateSubGroupOfXYZFilesFromLocationPolygonKML();

        Assert.Empty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public void CreateKMLToShowXYZExtentsFromLocationPolygonKML_OK()
    {
        SetupStart();

        vm.CreateKMLFileToShowXYZExtentsFromLocationPolygonKML();

        Assert.Empty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public void CreateKMLFileFromXYZFile_OK()
    {
        SetupStart();

        FileInfo fiXYZ = new FileInfo("E:\\CSSP\\Modelling\\Mike21\\Quebec\\IlesDeLaMadeleine\\External Data\\IDM_CAM_EDN.xyz");

        vm.CreateKMLFileFromXYZFile(fiXYZ);

        Assert.Empty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public void CreateXYZFileFromKMLFile_OK()
    {
        SetupStart();

        FileInfo fiXYZ = new FileInfo("E:\\CSSP\\Modelling\\Mike21\\Quebec\\IlesDeLaMadeleine\\External Data\\IDM_CAM_EDN2.kml");

        vm.CreateXYZFileFromKMLFile(fiXYZ);

        Assert.Empty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public void CreateOneXYZFileFromManyXYZFileWithinAPolygon_OK()
    {
        SetupStart();

        DirectoryInfo startDir = new DirectoryInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\");

        string fileNameToCreate = "Gaspe.xyz";
        string kmlOfPolygon = "";

        StringBuilder sb = new StringBuilder();

        sb.AppendLine($@"<?xml version=""1.0"" encoding=""UTF-8""?>");
        sb.AppendLine($@"<kml xmlns=""http://www.opengis.net/kml/2.2"" xmlns:gx=""http://www.google.com/kml/ext/2.2"" xmlns:kml=""http://www.opengis.net/kml/2.2"" xmlns:atom=""http://www.w3.org/2005/Atom"">");
        sb.AppendLine($@"<Document>");
        sb.AppendLine($@"	<name>KmlFile</name>");
        sb.AppendLine($@"	<Placemark>");
        sb.AppendLine($@"		<name>Untitled Polygon</name>");
        sb.AppendLine($@"		<styleUrl>#m_ylw-pushpin</styleUrl>");
        sb.AppendLine($@"		<Polygon>");
        sb.AppendLine($@"			<tessellate>1</tessellate>");
        sb.AppendLine($@"			<outerBoundaryIs>");
        sb.AppendLine($@"				<LinearRing>");
        sb.AppendLine($@"					<coordinates>");
        sb.AppendLine($@"						-64.34905561301321,48.75298180532126,0 -64.25490923616539,48.81474992065815,0 -64.50106254480318,48.91842418973383,0 -64.62543857093259,48.90719801751963,0 -64.61494894824321,48.81723049009093,0 -64.46339565091488,48.75257287267696,0 -64.34905561301321,48.75298180532126,0 ");
        sb.AppendLine($@"					</coordinates>");
        sb.AppendLine($@"				</LinearRing>");
        sb.AppendLine($@"			</outerBoundaryIs>");
        sb.AppendLine($@"		</Polygon>");
        sb.AppendLine($@"	</Placemark>");
        sb.AppendLine($@"</Document>");
        sb.AppendLine($@"</kml>");

        kmlOfPolygon = sb.ToString();

        vm.CreateOneXYZFileFromManyXYZFileWithinAPolygon(startDir, fileNameToCreate, kmlOfPolygon);

        Assert.Empty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public void FixKMLFileByRemovingAbsoluteLine_OK()
    {
        SetupStart();

        DirectoryInfo startDir = new DirectoryInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\");

        vm.FixKMLFileByRemovingAbsoluteLine(startDir);

        Assert.Empty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public void RemoveHighValuesFromXYZFile_OK()
    {
        SetupStart();

        FileInfo fiXYZ = new FileInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\Gaspé - Paspébiac\\xyz\\48_8-64_5_6083.xyz");

        double highValue = 2.0;

        vm.RemoveHighValuesFromXYZFile(fiXYZ, highValue);

        Assert.Empty(vm.Log);

        SetupEnd();

    }

}