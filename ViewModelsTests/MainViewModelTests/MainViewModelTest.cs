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
        Assert.NotEmpty(vm.StartDir);

        SetupEnd();
        Assert.Empty(vm.StartDir);
    }

    [Fact]
    public async Task SubdivideAsync_OK()
    {
        SetupStart();
        await vm.SubdivideAsync();

        Assert.NotEmpty(vm.Log);
        Assert.Equal("Done...", vm.Status);

        SetupEnd();
    }

    [Fact]
    public async Task SubdivideAsync_Error_Directory_Not_Exist_OK()
    {
        SetupStart();

        vm.StartDir = "E:\\Bathymetry\\CHS_High_Res\\\\Data\\NotThere\\";
        await vm.SubdivideAsync();

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
    public async Task CreateSubGroupOfXYZFilesFromLocationPolygonKMLAsync_OK()
    {
        SetupStart();

        await vm.CreateSubGroupOfXYZFilesFromLocationPolygonKMLAsync();

        Assert.Empty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public async Task CreateKMLToShowXYZExtentsFromLocationPolygonKMLAsync_OK()
    {
        SetupStart();

        await vm.CreateKMLFileToShowXYZExtentsFromLocationPolygonKMLAsync();

        Assert.Empty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public async Task CreateOneXYZFileFromManyXYZFileWithinAPolygonTask_OK()
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

        await vm.CreateOneXYZFileFromManyXYZFileWithinAPolygonAsync(startDir, fileNameToCreate, kmlOfPolygon);

        Assert.Empty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public async Task FixKMLFileByRemovingAbsoluteLineAsync_OK()
    {
        SetupStart();

        DirectoryInfo startDir = new DirectoryInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\");

        await vm.FixKMLFileByRemovingAbsoluteLineAsync(startDir);

        Assert.Empty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public async Task RemoveHighValuesFromXYZFileAsync_OK()
    {
        SetupStart();

        FileInfo fiXYZ = new FileInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\Gaspé - Paspébiac\\xyz\\48_8-64_5_6083.xyz");

        double highValue = 2.0;

        await vm.RemoveHighValuesFromXYZFileAsync(fiXYZ, highValue);

        Assert.Empty(vm.Log);

        SetupEnd();

    }

}