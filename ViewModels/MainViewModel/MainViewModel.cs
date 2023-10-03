namespace MainViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    public string status;

    [ObservableProperty]
    public string startDir;

    [ObservableProperty]
    public string log;

    [RelayCommand]
    void SayHello()
    {
        Status = "Hello";
        Log = "Hello";

        FileInfo fiKML = new FileInfo("E:\\Bathymetry\\CHS_High_Res\\Data\\sub_xyz\\Grand Manan\\kml\\44_5-66_7_422.kml");

        CreateXYZFileFromKMLFile(fiKML);
    }

    public MainViewModel()
    {
        Status = "Bathymetry Tools";
        StartDir = "E:\\Bathymetry\\CHS_High_Res\\\\Data\\";
        Log = "";
    }
}
