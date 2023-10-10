namespace MainViewModelTests;

public partial class MainViewModelTest
{
    [Fact]
    public async Task CreateKMLFileFromXYZFileAsync_OK()
    {
        SetupStart();

        FileInfo fiXYZ = new FileInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\Grand Manan\\xyz\\44_5-66_7_422.xyz");

        FileInfo fiKML = new FileInfo(fiXYZ.FullName.Replace(".xyz", ".kml"));
        if (fiKML.Exists)
        {
            try
            {
                fiKML.Delete();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Error deleting {fiKML.FullName}: {ex.Message}");
            }
        }

        await vm.CreateKMLFileFromXYZFileAsync(fiXYZ);

        Assert.Empty(vm.Log);

        fiKML = new FileInfo(fiXYZ.FullName.Replace(".xyz", ".kml"));
        Assert.True(fiKML.Exists);

        SetupEnd();
    }

    [Fact]
    public async Task CreateKMLFileFromXYZFileAsync_Error_File_Does_Not_Exist()
    {
        SetupStart();

        FileInfo fiXYZ = new FileInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\Grand Manan\\xyz\\44_5-66_7_422_Does_Not_Exist.xyz");

        await vm.CreateKMLFileFromXYZFileAsync(fiXYZ);

        Assert.NotEmpty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public async Task CreateKMLFileFromXYZFileAsync_Error_File_Already_Exist()
    {
        SetupStart();

        FileInfo fiXYZ = new FileInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\Grand Manan\\xyz\\44_5-66_7_422.xyz");

        FileInfo fiKML = new FileInfo(fiXYZ.FullName.Replace(".xyz", ".kml"));
        if (fiKML.Exists)
        {
            try
            {
                fiKML.Delete();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Error deleting {fiKML.FullName}: {ex.Message}");
            }
        }

        await vm.CreateKMLFileFromXYZFileAsync(fiXYZ);
        await vm.CreateKMLFileFromXYZFileAsync(fiXYZ);

        Assert.NotEmpty(vm.Log);

        SetupEnd();

    }

}