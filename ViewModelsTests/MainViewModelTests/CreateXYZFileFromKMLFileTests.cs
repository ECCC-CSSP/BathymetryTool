namespace MainViewModelTests;

public partial class MainViewModelTest
{
    [Fact]
    public void CreateXYZFileFromKMLFile_OK()
    {
        SetupStart();

        FileInfo fiKML = new FileInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\Grand Manan\\kml\\44_5-66_7_422.kml");

        FileInfo fiXYZ = new FileInfo(fiKML.FullName.Replace(".kml", ".xyz"));
        if (fiXYZ.Exists)
        {
            try
            {
                fiXYZ.Delete();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Error deleting {fiXYZ.FullName}: {ex.Message}");
            }
        }

        vm.CreateXYZFileFromKMLFile(fiKML);

        Assert.Empty(vm.Log);

        fiXYZ = new FileInfo(fiKML.FullName.Replace(".kml", ".xyz"));
        Assert.True(fiXYZ.Exists);

        SetupEnd();
    }

    [Fact]
    public void CreateXYZFileFromKMLFile_Error_File_Does_Not_Exist()
    {
        SetupStart();

        FileInfo fiKML = new FileInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\Grand Manan\\kml\\44_5-66_7_422_Does_Not_Exist.kml");

        vm.CreateXYZFileFromKMLFile(fiKML);

        Assert.NotEmpty(vm.Log);

        SetupEnd();

    }

    [Fact]
    public void CreateXYZFileFromKMLFile_Error_File_Already_Exist()
    {
        SetupStart();

        FileInfo fiKML = new FileInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\Grand Manan\\kml\\44_5-66_7_422.kml");

        FileInfo fiXYZ = new FileInfo(fiKML.FullName.Replace(".kml", ".xyz"));
        if (fiXYZ.Exists)
        {
            try
            {
                fiXYZ.Delete();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Error deleting {fiXYZ.FullName}: {ex.Message}");
            }
        }

        vm.CreateXYZFileFromKMLFile(fiKML);
        vm.CreateXYZFileFromKMLFile(fiKML);

        Assert.NotEmpty(vm.Log);

        SetupEnd();

    }

}