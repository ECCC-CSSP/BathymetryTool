namespace MainViewModelTests;

public partial class MainViewModelTest
{
    private string startDir { get; set; } = "E:\\Bathymetry\\CHS_High_Res\\\\Data\\";

    private void SetupStart()
    {
        vm.StartDir = startDir;
        vm.Log = "";
        vm.Status = "";      
    }

    private void SetupEnd()
    {
        vm.StartDir = "";
        vm.Log = "";
        vm.Status = "";
    }
}