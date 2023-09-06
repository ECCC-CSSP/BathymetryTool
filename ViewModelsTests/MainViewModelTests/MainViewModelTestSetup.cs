namespace MainViewModelTests;

public partial class MainViewModelTest
{
    private string fileName { get; set; } = "NONNAP10_4000N05900W.txt";
    private string startDir { get; set; } = "E:\\Bathymetry\\CHS_High_Res\\\\Data\\";

    private void SetupStart()
    {
        vm.StartDir = startDir;
        vm.Log = "";
        vm.Status = "";
       
        //FileInfo fiShouldNotExist = new FileInfo($"{vm.StartDir}{fileName}");
        //try
        //{
        //    fiShouldNotExist.Delete();
        //}
        //catch (Exception)
        //{
        //    Assert.True(true);
        //}

        //fiShouldNotExist = new FileInfo($"{vm.StartDir}{fileName}");
        //Assert.False(fiShouldNotExist.Exists);

        //FileInfo fi = new FileInfo($"{vm.StartDir}Done\\{fileName}");
        //Assert.True(fi.Exists);

        //try
        //{
        //    File.Copy($"{vm.StartDir}Done\\{fileName}", $"{vm.StartDir}{fileName}");
        //}
        //catch (Exception ex)
        //{
        //    Assert.NotNull(ex.Message);
        //    Assert.Fail(ex.Message);
        //}
    }

    private void SetupEnd()
    {
        vm.StartDir = startDir;
        vm.Log = "";
        vm.Status = "";

        //FileInfo fi = new FileInfo($"{vm.StartDir}{fileName}");
        //Assert.True(fi.Exists);

        //try
        //{
        //    fi.Delete();
        //}
        //catch (Exception ex)
        //{
        //    Assert.NotNull(ex.Message);
        //    Assert.Fail(ex.Message);
        //}
    }
}