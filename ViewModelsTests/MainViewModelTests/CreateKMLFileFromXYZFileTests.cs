namespace MainViewModelTests;

public partial class MainViewModelTest
{
    [Fact]
    public void CreateKMLFileFromXYZFile_OK()
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

        vm.CreateKMLFileFromXYZFile(fiXYZ);

        Assert.Empty(vm.Log);

        fiKML = new FileInfo(fiXYZ.FullName.Replace(".xyz", ".kml"));
        Assert.True(fiKML.Exists);

        SetupEnd();
    }

    [Fact]
    public void CreateKMLFileFromXYZFile_Error_File_Does_Not_Exist()
    {
        SetupStart();

        FileInfo fiXYZ = new FileInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\Grand Manan\\xyz\\44_5-66_7_422_Does_Not_Exist.xyz");

        vm.CreateKMLFileFromXYZFile(fiXYZ);

        Assert.NotEmpty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public void CreateKMLFileFromXYZFile_Error_File_Already_Exist()
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

        vm.CreateKMLFileFromXYZFile(fiXYZ);
        vm.CreateKMLFileFromXYZFile(fiXYZ);

        Assert.NotEmpty(vm.Log);

        SetupEnd();

    }

}