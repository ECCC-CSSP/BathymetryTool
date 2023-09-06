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
    }

    public MainViewModel()
    {
        Status = "Bathymetry Tools";
        StartDir = "E:\\Bathymetry\\CHS_High_Res\\\\Data\\";
        Log = "";
    }
}
